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
using System.Text;

#endregion

namespace Login_Server
{
    public static class Packet
    {
        /// <summary>
        /// Returns the type of packet sent by the client by checking its data length
        /// </summary>
        public static string GetPacketType(int packetLength)
        {
            string type;
            switch (packetLength)
            {
                case 56:
                    type = "LoginRequest";
                    break;
                case 11:
                    type = "GetServerDetails";
                    break;
                default:
                    type = "Unknown";
                    break;
            }
            return type;
        }

        /// <summary>
        /// Returns packet containing custom message in bytes
        /// </summary>
        public static byte[] CreateMessage(string msg)
        {
            var headPacket = new byte[] {0x5c, 0x00, 0x00, 0x00, 0x01, 0x4f, 0x00, 0x00, 0x01, 0xe0, 0x01};
            if (msg.Length > 70)
                msg = msg.Substring(0, 69);
            int toFill = 92 - 11 - msg.Length;
            string filler = GetNullString(toFill);
            headPacket = CombineByteArray(headPacket, GetBytesFrom(msg));
            return CombineByteArray(headPacket, GetBytesFrom(filler));
        }

        /// <summary>
        /// Byte array concatination
        /// </summary>
        private static byte[] CombineByteArray(byte[] a, byte[] b)
        {
            var c = new byte[a.Length + b.Length];
            Buffer.BlockCopy(a, 0, c, 0, a.Length);
            Buffer.BlockCopy(b, 0, c, a.Length, b.Length);
            return c;
        }

        /// <summary>
        /// Returns byte equivalent of a string
        /// </summary>
        private static byte[] GetBytesFrom(string str)
        {
            var encoding = new ASCIIEncoding();
            Byte[] bytes = encoding.GetBytes(str);
            return bytes;
        }

        /// <summary>
        /// Returns a null string of specified length
        /// </summary>
        private static string GetNullString(int length)
        {
            string str = "";
            for (var i = 0; i < length; i++)
                str += char.ConvertFromUtf32(0);
            return str;
        }

        /// <summary>
        /// Returns user ID and password after parsing the login request packet
        /// </summary>
        public static string[] GetParsedCredentials(byte[] packet, int length, Encoding enc)
        {
            string temp1 = enc.GetString(packet, 10, length);
            string temp2 = enc.GetString(packet, 31, length);
            string username = "", password = "";
            foreach (char c in temp1)
            {
                int asciiInt = c;
                if (asciiInt == 0)
                    break;
                if ((asciiInt >= 48 && asciiInt <= 57) || (asciiInt >= 65 && asciiInt <= 90) ||
                    (asciiInt >= 97 && asciiInt <= 122) || asciiInt == 64)
                    username += c.ToString();
            }
            foreach (char c in temp2)
            {
                int asciiInt = c;
                if (asciiInt == 0)
                    break;
                if ((asciiInt >= 48 && asciiInt <= 57) || (asciiInt >= 65 && asciiInt <= 90) ||
                    (asciiInt >= 97 && asciiInt <= 122) || asciiInt == 64)
                    password += c.ToString();
            }
            var credentials = new[] {username, password};
            return credentials;
        }

        /// <summary>
        /// Returns packet for welcome message of the server
        /// </summary>
        public static byte[] CreateWelcomeMessage()
        {
            string serverName = Config.ServerName;
            var headPacket = new byte[] {0x5c, 0x00, 0x00, 0x00, 0x01, 0x4f, 0x00, 0x00, 0x01, 0xe3, 0x01};
            var midPacket = new byte[]
                                {
                                    0x4c, 0x27, 0xd3, 0x77, 0xe4, 0x03, 0x01, 0xf5, 0x21, 0x00, 0x00, 0x00, 0x14, 0x00,
                                    0x00, 0x00, 0xe4, 0x03, 0x6f, 0x00, 0x00, 0x00, 0x01, 0x4f, 0x00, 0x00, 0x01, 0xe1,
                                    0x01, 0x00, 0x00
                                };
            var interPacket = new byte[] {0x68, 0x00, 0x00, 0x00};
            if (serverName.Length > 12)
                serverName = serverName.Substring(0, 11);
            string welcomeMsg = "Welcome to A3 " + serverName;
            welcomeMsg += GetNullString(63 - welcomeMsg.Length);
            string serverSelect = serverName + GetNullString(13 - serverName.Length);
            byte[] packet = CombineByteArray(headPacket, GetBytesFrom(welcomeMsg));
            packet = CombineByteArray(packet, midPacket);
            packet = CombineByteArray(packet, GetBytesFrom(serverSelect));
            packet = CombineByteArray(packet, interPacket);
            packet = CombineByteArray(packet, GetBytesFrom("ONLINE"));
            packet = CombineByteArray(packet, GetBytesFrom(GetNullString(75)));
            return packet;
        }

        /// <summary>
        /// Returns packet with server IP and port
        /// </summary>
        public static byte[] CreateServerDetails()
        {
            string ip = Config.GameServerIp.ToString();
            int port = Config.GameServerPort;
            var headPacket = new byte[]
                                 {0x22, 0x00, 0x00, 0x00, 0x01, 0x4f, 0x00, 0x00, 0x01, 0xe2, 0x11, 0x38, 0x54, 0x00};
            byte[] packet = CombineByteArray(headPacket, GetBytesFrom(ip + GetNullString(16 - ip.Length)));
            packet = CombineByteArray(packet, PortPacketMaker(port));
            packet = CombineByteArray(packet, GetBytesFrom(GetNullString(2)));
            return packet;
        }

        /// <summary>
        /// Returns packet with server port
        /// </summary>
        private static byte[] PortPacketMaker(int port)
        {
            string hexPort = string.Format("{0:x}", port);
            while (hexPort.Length < 4)
                hexPort = "0" + hexPort;
            string temp = hexPort[2] + hexPort[3].ToString();
            string temp1 = hexPort[0] + hexPort[1].ToString();
            var tempByte = new[] {Convert.ToByte(temp, 16), Convert.ToByte(temp1, 16)};
            return tempByte;
        }
    }
}