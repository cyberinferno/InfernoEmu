using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace InfernoEmu
{
    public static class Packet
    {
        /// <summary>
        /// Returns packet for welcome message of the server
        /// </summary>
        public static byte[] CreateWelcomeMessage()
        {
            var serverName = Config.ServerName;
            var headPacket = new byte[] { 0x5c, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0xe3, 0x01 };
            var midPacket = new byte[]
                                {
                                    0x4c, 0x27, 0xd3, 0x77, 0xe4, 0x03, 0x01, 0xf5, 0x21, 0x00, 0x00, 0x00, 0x14, 0x00,
                                    0x00, 0x00, 0xe4, 0x03, 0x6f, 0x00, 0x00, 0x00, 0x01, 0x4f, 0x00, 0x00, 0x01, 0xe1,
                                    0x01, 0x00, 0x00
                                };
            var interPacket = new byte[] { 0x68, 0x00, 0x00, 0x00 };
            string welcomeMsg = Config.WelcomeMsg;
            if (welcomeMsg.Length > 61)
                serverName = serverName.Substring(0, 60);
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
            string ip = Config.ServerIp.ToString();
            int port = Config.GameServerPort;
            var headPacket = new byte[] { 0x22, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0xe2, 0x11, 0x38, 0x54, 0x00 };
            byte[] packet = CombineByteArray(headPacket, GetBytesFrom(ip + GetNullString(16 - ip.Length)));
            packet = CombineByteArray(packet, CreateReverseHexPacket(port));
            return CombineByteArray(packet, GetBytesFrom(GetNullString(2)));
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
            var credentials = new[] { username, password };
            return credentials;
        }

        /// <summary>
        /// Returns packet containing custom message in bytes
        /// </summary>
        public static byte[] CreateMessage(string msg)
        {
            var headPacket = new byte[] { 0x5c, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0xe0, 0x01 };
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
        public static byte[] CombineByteArray(byte[] a, byte[] b)
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
        /// Returns packet with server IP and port
        /// </summary>
        public static byte[] CreateZoneAgentIdPacket()
        {
            var packet = CombineByteArray(new byte[]{0x20}, GetBytesFrom(GetNullString(7)));
            packet = CombineByteArray(packet, new byte[] {0x02, 0xe0});
            string aid = string.Format("{0:x}", 0);
            string sid = string.Format("{0:x}", 0);
            var tempByte = new[] { Convert.ToByte(sid, 16) };
            packet = CombineByteArray(packet, new[] { tempByte[0] });
            tempByte = new[] { Convert.ToByte(aid, 16) };
            packet = CombineByteArray(packet, new[] { tempByte[0] });
            packet = CombineByteArray(packet, GetBytesFrom(Config.ServerIp.ToString()));
            packet = CombineByteArray(packet, GetBytesFrom(GetNullString(16 - Config.GameServerPort.ToString().Length)));
            packet = CombineByteArray(packet, CreateReverseHexPacket(Config.GameServerPort));
            return CombineByteArray(packet, GetBytesFrom(GetNullString(2)));
        }

        /// <summary>
        /// Returns packet with server IP and port
        /// </summary>
        public static byte[] CreateGateOkPacket()
        {
            return new byte[] { 0x0b, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0xe0, 0x00};
        }

        /// <summary>
        /// Returns packet with server port
        /// </summary>
        private static byte[] CreateReverseHexPacket(int port)
        {
            string hexPort = string.Format("{0:x}", port);
            while (hexPort.Length < 4)
                hexPort = "0" + hexPort;
            string temp = hexPort[2] + hexPort[3].ToString();
            string temp1 = hexPort[0] + hexPort[1].ToString();
            var tempByte = new[] { Convert.ToByte(temp, 16), Convert.ToByte(temp1, 16) };
            return tempByte;
        }

        /// <summary>
        /// Removes extra null bytes from the end of a packet by finding its actual length
        /// </summary>
        public static byte[] TrimPacket(byte[] packet)
        {
            var length = GetIntFromHex(new[] {packet[0], packet[1]});
            MyLogger.WriteLoginServerLog("Length is " + length);
            var newPacket = new byte[] { 0x00 };
            for (int i = 0; i < length; i++)
            {
                if (i == 0)
                    newPacket[i] = packet[i];
                else
                {
                    var temp = new[] { packet[i] };
                    newPacket = CombineByteArray(newPacket, temp);
                }
            }
            return newPacket;
        }


        public static byte[] CreatePaymentPacket(int cindex, string charName)
        {
            var packet = new byte[] {0x00, 0x00};
            packet = CombineByteArray(packet, CreateReverseHexPacket(cindex));
            var temp = new byte[] {0x03, 0xff, 0x00, 0x18, 0xf5, 0x58, 0x2b, 0x00, 0x00};
            packet = CombineByteArray(packet, temp);
            packet = CombineByteArray(packet, GetBytesFrom(charName));
            packet = CombineByteArray(packet, GetBytesFrom(GetNullString(21 - charName.Length)));
            packet = CombineByteArray(packet, GetBytesFrom("Fuck off!"));
            packet = CombineByteArray(packet, GetBytesFrom(GetNullString(13)));
            packet = CombineByteArray(CreateReverseHexPacket(packet.Length + 2), packet);
            //Crypt.encrypt_acl(Encoding.UTF8.GetString(packet), 952, 12);
            return packet;
        }

        public static byte[] AlterCharacterPacket(byte[] packet)
        {
            for (int i = 0; i < packet.Length; i++)
            {
                if((i + 2) < packet.Length)
                {
                    if (packet[i] == 0x9f && packet[i + 1] == 0x37 && packet[i + 2] == 0x1a)
                    {
                        packet[i] = 0x9c;
                        packet[i + 1] = 0xe4;
                        packet[i + 2] = 0x13;
                    }
                    else if (packet[i] == 0x9c && packet[i + 1] == 0xe6 && packet[i + 2] == 0x49)
                    {
                        packet[i] = 0x9c;
                        packet[i + 1] = 0xe7;
                        packet[i + 2] = 0x64;
                    }
                    else if (packet[i] == 0x9e && packet[i + 1] == 0x1c && packet[i + 2] == 0x88)
                    {
                        packet[i] = 0x9c;
                        packet[i + 1] = 0xe5;
                        packet[i + 2] = 0x2e;
                    }
                    else if (packet[i] == 0x9d && packet[i + 1] == 0x01 && packet[i + 2] == 0xf5)
                    {
                        packet[i] = 0x9c;
                        packet[i + 1] = 0xe6;
                        packet[i + 2] = 0x49;
                    }
                    else if (packet[i] == 0x01 && packet[i + 1] == 0x9e && packet[i + 2] == 0xa2)
                    {
                        packet[i] = 0x9c;
                        packet[i + 1] = 0xe5;
                        packet[i + 2] = 0x2e;
                    }
                }
            }
            return packet;
        }

        public static byte[] CreateZoneDisconnectPacket(int cindex)
        {
            var packet = new byte[] { 0x00, 0x00 };
            packet = CombineByteArray(packet, CreateReverseHexPacket(cindex));
            var temp = new byte[] { 0x03, 0xff, 0x08, 0x11 };
            packet = CombineByteArray(packet, temp);
            return CombineByteArray(CreateReverseHexPacket(packet.Length + 2), packet);
        }

        public static byte[] CreateLoginDisconnectPacket(int cindex)
        {
            // TODO: Create DC packet
            return new byte[] {0x00};
        }

        public static int GetIntFromHex(byte[] data)
        {
            /*if (BitConverter.IsLittleEndian)
                Array.Reverse(data);
            return BitConverter.ToInt16(data, 0);*/
            return (data[1] << 8) | data[0];
        }

        public static int GetClientId(byte[] data)
        {
            if(BitConverter.IsLittleEndian)
                Array.Reverse(data);
            return data.Aggregate(0, (current, t) => (current << 8) | t);
        }

        public static byte[] CreateClientStatusPacket(int clientId, string accountId)
        {
            var packet = new byte[] { 0x00, 0x00 };
            packet = CombineByteArray(packet, CreateReverseHexPacket(clientId));
            var temp = new byte[] { 0x02, 0xe3 };
            packet = CombineByteArray(packet, CombineByteArray(temp, GetBytesFrom(accountId)));
            packet = CombineByteArray(packet, GetBytesFrom(GetNullString(21 - accountId.Length)));
            return CombineByteArray(CreateReverseHexPacket(packet.Length + 2), packet);
        }

        public static string GetCharName(byte[] packet)
        {
            try
            {
                IntPtr unmanagedPointer = Marshal.AllocHGlobal(packet.Length);
                Marshal.Copy(packet, 0, unmanagedPointer, packet.Length);
                Crypt.decrypt_acl(unmanagedPointer, 37, 0);
                Marshal.Copy(unmanagedPointer, packet, 0, 37);
                Marshal.FreeHGlobal(unmanagedPointer);
                return Encoding.Default.GetString(packet).Substring(12, 12).Trim().TrimEnd('\0');
            }
            catch(Exception e)
            {
                MyLogger.WriteLog("Exception in GetCharName : " + e.Message);
                return " ";
            }
        }

        public static byte[] CreateCharDetailPacket(string charName, int level, int type)
        {
            // name + (20 - name)*0 + 0 + type + town + 0 + level + 0 + 0 + 0 
            var returnByte = CombineByteArray(GetBytesFrom(charName), GetBytesFrom(GetNullString(20 - charName.Length)));
            byte typeByte = 0x00;
            switch(type)
            {
                case 1:
                    typeByte = 0x01;
                    break;
                case 2:
                    typeByte = 0x02;
                    break;
                case 3:
                    typeByte = 0x03;
                    break;
            }
            returnByte = CombineByteArray(returnByte, new byte[]{0x00, typeByte, 0x00, 0x00});
            returnByte = CombineByteArray(returnByte, new byte[] { Convert.ToByte(string.Format("{0:x}", level), 16), 0x00, 0x00, 0x00 });
            return CombineByteArray(returnByte, GetBytesFrom(GetNullString(188 - returnByte.Length)));
        }

        public static byte[] CreateCharacterPacket(string[] chars, string[] levels, string[] types)
        {
            if (chars[0] == " ")
                return CreateNewAccountPacket();
            var toReturn = new byte[] { 0x00 };
            for (int i = 0; i < chars.Length; i++)
                toReturn = i == 0 ? CreateCharDetailPacket(chars[i], Convert.ToInt32(levels[i]), Convert.ToInt32(types[i])) : CombineByteArray(toReturn, chars[i] != " " ? CreateCharDetailPacket(chars[i], Convert.ToInt32(levels[i]), Convert.ToInt32(types[i])) : CreateEmptyCharSlot());
            toReturn = AlterCharacterPacket(Encrypt(toReturn));
            return CombineByteArray(new byte[] {0xb8, 0x03, 0x00, 0x00, 0x00, 0x00, 0x0b, 0x00, 0x03, 0xff, 0x05, 0x11}, toReturn);
        }

        private static byte[] CreateNewAccountPacket()
        {
            var header = new byte[] { 0xb8, 0x03, 0x00, 0x00, 0x00, 0x00, 0x0b, 0x00, 0x03, 0xff, 0x05, 0x11 };
            var toReturn = new byte[] {0x00};
            for (int i = 0; i < 5; i++)
                toReturn = i == 0 ? CreateEmptyCharSlot() : CombineByteArray(toReturn, CreateEmptyCharSlot());
            return CombineByteArray(header, Encrypt(toReturn));
        }

        private static byte[] CreateEmptyCharSlot()
        {
            var toReturn = CombineByteArray(GetBytesFrom(GetNullString(21)), new byte[] { 0xff });
            return CombineByteArray(toReturn, GetBytesFrom(GetNullString(188 - 22)));
        }

        public static byte[] CreatePopup(string msg)
        {
            var header = new byte[] { 0x4e, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0b, 0x00, 0x03, 0xff, 0xff, 0x0f, 0x7e, 0x2f, 0x6e, 0x33};
            if (msg.Length > 56)
                msg = msg.Substring(0, 56);
            var toReturn = CombineByteArray(GetBytesFrom(msg), GetBytesFrom(GetNullString(60 - msg.Length)));
            return CombineByteArray(header, CombineByteArray(Encrypt(toReturn), new byte[] { 0x00, 0x00 }));
        }

        public static byte[] Decrypt(byte[] packet)
        {
            var length = packet.Length;
            IntPtr unmanagedPointer = Marshal.AllocHGlobal(length);
            Marshal.Copy(packet, 0, unmanagedPointer, length);
            Crypt.decrypt_acl(unmanagedPointer, length, 0);
            Marshal.Copy(unmanagedPointer, packet, 0, length);
            Marshal.FreeHGlobal(unmanagedPointer);
            return packet;
        }

        public static byte[] Encrypt(byte[] packet)
        {
            var length = packet.Length;
            IntPtr unmanagedPointer = Marshal.AllocHGlobal(length);
            Marshal.Copy(packet, 0, unmanagedPointer, length);
            Crypt.encrypt_acl(unmanagedPointer, length, 0);
            Marshal.Copy(unmanagedPointer, packet, 0, length);
            Marshal.FreeHGlobal(unmanagedPointer);
            return packet;
        }
    }
}
