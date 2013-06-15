/*
	Copyright © 2013, InfernoEmu Project
	All rights reserved.
	
	This file is part of InfernoEmu.

	InfernoEmu is free software: you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation, either version 3 of the License, or
	any later version.

	InfernoEmu is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with InfernoEmu.  If not, see <http://www.gnu.org/licenses/>.
*/

#region Includes

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using MySql.Data.MySqlClient;

#endregion

namespace Login_Server
{
    public class Server
    {
        private readonly List<Client> _clients;
        private TcpListener _listener;

        /// <summary>
        /// Constructor to intialize variables for the server
        /// </summary>
        public Server()
        {
            Encoding = Encoding.Default;
            _clients = new List<Client>();
        }

        /// <summary>
        /// The encoding to use when sending / receiving strings.
        /// </summary>
        private Encoding Encoding { get; set; }

        /// <summary>
        /// An enumerable collection of all the currently connected tcp clients
        /// </summary>
        public IEnumerable<TcpClient> TcpClients
        {
            get {
                return _clients.Select(client => client.TcpClient);
            }
        }

        /// <summary>
        /// Starts the TCP Server listening for new clients.
        /// </summary>
        public void Start()
        {
            _listener = new TcpListener(Config.LoginServerIp, Config.LoginServerPort);
            _listener.Start();
            _listener.BeginAcceptTcpClient(ClientHandler, null);
        }

        /// <summary>
        /// Stops the TCP Server listening for new clients and disconnects
        /// any currently connected clients.
        /// </summary>
        public void Stop()
        {
            _listener.Stop();
            lock (_clients)
            {
                foreach (Client client in _clients)
                {
                    client.TcpClient.Client.Disconnect(false);
                }
                _clients.Clear();
            }
        }

        /// <summary>
        /// Writes a string to a given TCP Client
        /// </summary>
        /// <param name="tcpClient">The client to write to</param>
        /// <param name="data">The string to send.</param>
        private void Write(TcpClient tcpClient, string data)
        {
            byte[] bytes = Encoding.GetBytes(data);
            Write(tcpClient, bytes);
        }

        /// <summary>
        /// Writes a string to all clients connected.
        /// </summary>
        /// <param name="data">The string to send.</param>
        public void Write(string data)
        {
            foreach (Client client in _clients)
            {
                Write(client.TcpClient, data);
            }
        }

        /// <summary>
        /// Writes a byte array to all clients connected.
        /// </summary>
        /// <param name="bytes">The bytes to send.</param>
        public void Write(byte[] bytes)
        {
            foreach (Client client in _clients)
            {
                Write(client.TcpClient, bytes);
            }
        }

        /// <summary>
        /// Writes a byte array to a given TCP Client
        /// </summary>
        /// <param name="tcpClient">The client to write to</param>
        /// <param name="bytes">The bytes to send</param>
        private static void Write(TcpClient tcpClient, byte[] bytes)
        {
            NetworkStream networkStream = tcpClient.GetStream();
            networkStream.BeginWrite(bytes, 0, bytes.Length, WriteCallback, tcpClient);
        }

        /// <summary>
        /// Callback for the write opertaion.
        /// </summary>
        private static void WriteCallback(IAsyncResult result)
        {
            try
            {
                var tcpClient = result.AsyncState as TcpClient;
                if (tcpClient != null)
                {
                    NetworkStream networkStream = tcpClient.GetStream();
                    networkStream.EndWrite(result);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("Write call back exception : " + ex.Message);
            }
        }

        /// <summary>
        /// Callback for the accept tcp client opertaion.
        /// </summary>
        private void ClientHandler(IAsyncResult asyncResult)
        {
            try
            {
                TcpClient client = _listener.EndAcceptTcpClient(asyncResult);
                var buffer = new byte[client.ReceiveBufferSize];
                var newClient = new Client(client, buffer);
                lock (_clients)
                {
                    _clients.Add(newClient);
                }
                var newClientEp = (IPEndPoint) client.Client.RemoteEndPoint;
                Logger.WriteLog("New client connected with IP address " + newClientEp.Address + " and port " +
                                newClientEp.Port + "!");
                NetworkStream networkStream = newClient.NetworkStream;
                networkStream.BeginRead(newClient.Buffer, 0, newClient.Buffer.Length, OnDataRead, newClient);
                _listener.BeginAcceptTcpClient(ClientHandler, null);
            }
            catch (Exception e)
            {
                Console.WriteLine("ClientHandler ERROR: " + e.Message);
                Logger.WriteLog("ClientHandler ERROR: " + e.Message);
            }
        }

        /// <summary>
        /// Callback for the read opertaion.
        /// </summary>
        private void OnDataRead(IAsyncResult asyncResult)
        {
            try
            {
                var client = asyncResult.AsyncState as Client;
                if (client == null)
                    return;
                NetworkStream networkStream = client.NetworkStream;
                int read = networkStream.EndRead(asyncResult);
                if (read == 0)
                {
                    lock (_clients)
                    {
                        _clients.Remove(client);
                        return;
                    }
                }
                Logger.WriteLog("Got packet of length " + read);
                byte[] replyPacket = {0x00};
                string packetType = Packet.GetPacketType(read);
                switch (packetType)
                {
                    case "LoginRequest":
                        string[] credentials = Packet.GetParsedCredentials(client.Buffer, read, Encoding);
                        if (ValidUser(credentials[0], credentials[1]))
                        {
                            replyPacket = Packet.CreateWelcomeMessage();
                            Write(client.TcpClient, replyPacket);
                        }
                        else
                        {
                            replyPacket = Packet.CreateMessage("Invalid user ID/password!");
                            Write(client.TcpClient, replyPacket);
                        }
                        break;
                    case "GetServerDetails":
                        replyPacket = Packet.CreateServerDetails();
                        Write(client.TcpClient, replyPacket);
                        break;
                    default:
                        var enc = new ASCIIEncoding();
                        Logger.WriteLog("Seems like I got something new here : " + enc.GetString(client.Buffer, 0, read));
                        Write(client.TcpClient, replyPacket);
                        break;
                }
                networkStream.BeginRead(client.Buffer, 0, client.Buffer.Length, OnDataRead, client);
            }
            catch (Exception e)
            {
                Logger.WriteLog(e.Message);
            }
        }

        /// <summary>
        /// Validates username and password got from the client
        /// </summary>
        private static bool ValidUser(string username, string password)
        {
            Logger.WriteLog("Looking up for username '" + username + "' and password '" + password + "'");
            var db = new Database();
            var query = "select id from account where username = '" + username + "' and password = '" + password +
                           "'";
            if (db.OpenConnection())
            {
                var cmd = new MySqlCommand(query, db.Connection);
                var dataReader = cmd.ExecuteReader();
                return dataReader.HasRows;
            }
            return false;
        }
    }
}