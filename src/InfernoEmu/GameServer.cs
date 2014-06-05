using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace InfernoEmu
{
    public class GameServer
    {
        private readonly List<Client> _clients;
        private readonly TcpListener _listener;
        private readonly Database _db;

        public GameServer()
        {
            Encoding = Encoding.Default;
            _clients = new List<Client>();
            _db = new Database();
            _listener = new TcpListener(Config.ServerIp, Config.GameServerPort);
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
            get
            {
                return _clients.Select(client => client.TcpClient);
            }
        }

        /// <summary>
        /// Starts the TCP Server listening for new clients.
        /// </summary>
        public void Start()
        {
            if (Config.IsLoginServerRunning)
            {
                try
                {
                    _listener.Start();
                    _listener.BeginAcceptTcpClient(ClientHandler, null);
                    Config.IsGameServerRunning = true;
                }
                catch (Exception e)
                {
                    MyLogger.WriteLog("Game server could not be started. ERROR : " + e.Message);
                    Config.IsGameServerRunning = false;
                }
            }
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
            Config.IsGameServerRunning = false;
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
                MyLogger.WriteLog("Write call back exception : " + ex.Message);
            }
        }

        /// <summary>
        /// Callback for the accept tcp client operation.
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
                NetworkStream networkStream = newClient.NetworkStream;
                networkStream.BeginRead(newClient.Buffer, 0, newClient.Buffer.Length, OnDataRead, newClient);
                _listener.BeginAcceptTcpClient(ClientHandler, null);
            }
            catch (Exception e)
            {
                MyLogger.WriteLog("ClientHandler ERROR: " + e.Message);
            }
        }

        /// <summary>
        /// Callback for the read opertaion.
        /// </summary>
        private void OnDataRead(IAsyncResult asyncResult)
        {
            var client = asyncResult.AsyncState as Client;
            var clientUsername = "";
            try
            {
                if (client == null)
                    return;
                var networkStream = client.NetworkStream;
                var newClientEp = (IPEndPoint)client.TcpClient.Client.RemoteEndPoint;
                // Check if IP is banned
                if (!IsIpBanned(newClientEp.Address.ToString()))
                {
                    var read = networkStream.EndRead(asyncResult);
                    if (read == 0)
                    {
                        lock (_clients)
                        {
                            _clients.Remove(client);
                            return;
                        }
                    }
                    clientUsername = CleanUsername(Encoding.Default.GetString(client.Buffer).Substring(14, 14).Trim().TrimEnd('\0'));
                    switch (read)
                    {
                        case 12:
                            PreparedPlayers.UnPrepare(clientUsername);
                            break;
                        case 56:
                            if (PreparedPlayers.IsPrepared(clientUsername))
                            {
                                client.Username = clientUsername;
                                string[] chars = new string[5], levels = new string[5], types = new string[5], wears = new string[5];
                                _db.GetCharacters(client.Username, ref chars, ref levels, ref types, ref wears);
                                Write(client.TcpClient, Packet.CreateCharacterPacket(chars, levels, types, wears));
                            }
                            else
                            {
                                MyLogger.WriteGameServerLog("Unprepared user entered. Username : " +
                                                            clientUsername + ", IP : " + newClientEp.Address);
                                Write(client.TcpClient, Packet.CreatePopup("User not prepared!"));
                            }
                            break;
                        case 37:
                            var charName = Packet.GetCharName(client.Buffer);
                            if (_db.CharacterExists(client.Username, charName))
                            {
                                PreparedPlayers.UnPrepare(client.Username);
                                Write(client.TcpClient, Packet.CreatePopup("Selected " + charName));
                            }
                            else
                            {
                                PreparedPlayers.UnPrepare(client.Username);
                                Write(client.TcpClient, Packet.CreatePopup("Invalid character name!"));
                            }
                            break;
                        case 33:
                            var trimPacket33 = client.Buffer;
                            Array.Resize(ref trimPacket33, read);
                            if(Packet.CheckCharDeletePacket(trimPacket33))
                            {
                                var characterName = Packet.GetCharName(client.Buffer);
                                if (_db.CharacterExists(client.Username, characterName))
                                {
                                    if (_db.DeleteCharacter(characterName))
                                    {
                                        Write(client.TcpClient, trimPacket33);
                                    }
                                    else
                                    {
                                        MyLogger.WriteGameServerLog("Error while deleting character : " +
                                                                    characterName);
                                        Write(client.TcpClient, Packet.CreatePopup("Internal server error!"));
                                    }
                                }
                                else
                                {
                                    MyLogger.WriteGameServerLog("Hacker trying to delete " + Packet.GetCharName(trimPacket33) + ". Username : " +
                                                            client.Username + ", IP : " + newClientEp.Address);
                                    Write(client.TcpClient, Packet.CreatePopup("Invalid character name!"));
                                }
                            }
                            else
                            {
                                MyLogger.WriteGameServerLog("World enter packet got!");
                                File.WriteAllBytes("p", client.Buffer);
                            }
                            break;
                        default:
                            MyLogger.WriteGameServerLog("Got packet of length of " + read);
                            var trimPacket = client.Buffer;
                            Array.Resize(ref trimPacket, read);
                            MyLogger.WriteGameServerLog("Seems like I got something new here : " + Encoding.Default.GetString(Crypt.Decrypt(trimPacket)));
                            Write(client.TcpClient, Packet.CreatePopup("Emulator under construction!"));
                            break;
                    }
                }
                else
                    Write(client.TcpClient, Packet.CreatePopup("Your IP is banned.Contact gamemaster!"));
                networkStream.BeginRead(client.Buffer, 0, client.Buffer.Length, OnDataRead, client);
            }
            catch (Exception e)
            {
                MyLogger.WriteLog("OnDataRead error : " + e.Message);
                if (client != null)
                {
                    if (client.Username != null)
                        PreparedPlayers.UnPrepare(client.Username);
                    if(clientUsername != "")
                        PreparedPlayers.UnPrepare(clientUsername);
                    lock (_clients)
                    {
                        _clients.Remove(client);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Checks whether an IP is banned
        /// </summary>
        private static bool IsIpBanned(string ip)
        {
            if (File.Exists("ipbanlist.txt"))
            {
                try
                {
                    var ipList = new List<string>();
                    using (var streamReader = new StreamReader("ipbanlist.txt", true))
                    {
                        string readLine;
                        while ((readLine = streamReader.ReadLine()) != null)
                            ipList.Add(readLine.Trim());
                    }
                    return ipList.ToArray().Any(temp => String.Equals(temp, ip));
                }
                catch (Exception ex)
                {
                    MyLogger.WriteLog("IP ban check error : " + ex.Message);
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns string with only alphanumeric characters
        /// </summary>
        private static string CleanUsername(IEnumerable<char> str)
        {
            var toReturn = "";
            foreach (var c in str)
            {
                if(!Char.IsLetter(c) && !Char.IsNumber(c))
                    break;
                toReturn += c;
            }
            return toReturn;
        }
    }
}
