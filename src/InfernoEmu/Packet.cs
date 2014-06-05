using System;
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
        /// Returns packet with server IP and num
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
            foreach (var c in temp2)
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
        /// Returns packet with byte equivalent of int
        /// </summary>
        private static byte[] CreateReverseHexPacket(int num)
        {
            if (num == 0)
                return new byte[] { 0x00, 0x00 };
            string hexPort = string.Format("{0:x}", num);
            while (hexPort.Length < 4)
                hexPort = "0" + hexPort;
            string temp = hexPort[2] + hexPort[3].ToString();
            string temp1 = hexPort[0] + hexPort[1].ToString();
            var tempByte = new[] { Convert.ToByte(temp, 16), Convert.ToByte(temp1, 16) };
            return tempByte;
        }

        /// <summary>
        /// Returns packet with byte equivalent of int for wear
        /// </summary>
        private static byte[] WearCreateReverseHexPacket(long num)
        {
            if(num == 0)
                return new byte[] {0x00, 0x00, 0x00, 0x00};
            var toReturn = new byte[4];
            toReturn[0] = (byte)num;
            toReturn[1] = (byte)(((uint)num >> 8) & 0xFF);
            toReturn[2] = (byte)(((uint)num >> 16) & 0xFF);
            toReturn[3] = (byte)(((uint)num >> 24) & 0xFF);
            return toReturn;
        }

        /// <summary>
        /// Removes extra null bytes from the end of a packet by finding its actual length
        /// </summary>
        public static byte[] TrimPacket(byte[] packet)
        {
            var length = GetIntFromHex(new[] {packet[0], packet[1]});
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

        /// <summary>
        /// Creates payment info packet
        /// </summary>
        public static byte[] CreatePaymentPacket(int cindex, string charName)
        {
            var packet = new byte[] {0x00, 0x00};
            packet = CombineByteArray(packet, CreateReverseHexPacket(cindex));
            var temp = new byte[] {0x03, 0xff, 0x00, 0x18, 0xf5, 0x58, 0x2b, 0x00, 0x00};
            packet = CombineByteArray(packet, temp);
            packet = CombineByteArray(packet, GetBytesFrom(charName));
            packet = CombineByteArray(packet, GetBytesFrom(GetNullString(21 - charName.Length)));
            packet = CombineByteArray(packet, GetBytesFrom(Config.ServerName + " is a free server!"));
            packet = CombineByteArray(packet, GetBytesFrom(GetNullString(13)));
            packet = CombineByteArray(CreateReverseHexPacket(packet.Length + 2), packet);
            return Crypt.Encrypt(packet);
        }

        /// <summary>
        /// Returns hex value of a byte data
        /// </summary>
        public static int GetIntFromHex(byte[] data)
        {
            /*if (BitConverter.IsLittleEndian)
                Array.Reverse(data);
            return BitConverter.ToInt16(data, 0);*/
            return (data[1] << 8) | data[0];
        }

        /// <summary>
        /// Returns character name from an encrypted packet
        /// </summary>
        public static string GetCharName(byte[] packet)
        {
            return Encoding.Default.GetString(Crypt.Decrypt(packet)).Substring(12, 12).Trim().TrimEnd('\0');
        }

        /// <summary>
        /// Creates character details packet
        /// </summary>
        public static byte[] CreateCharDetailPacket(string charName, int level, int type, string wear)
        {
            // name + (20 - name)*0 + 0 + type + town + 0 + level + 0 + 0 + 0 + 0 + 0 + 0 + 0 + wear + (188 - wear)*0 
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
            returnByte = CombineByteArray(returnByte, new byte[] { 0x00, 0x01, typeByte, 0x00 });
            returnByte = CombineByteArray(returnByte, new byte[] { Convert.ToByte(string.Format("{0:x}", level), 16), 0x00, 0x00, 0x00 });
            if (wear != " ")
            {
                returnByte = CombineByteArray(returnByte, new byte[] {0x00, 0x00, 0x00, 0x00});
                var temp = wear.Split(';');
                var itemPosition = 0;
                //Convert wear into reverse hex packet and append it to packet byte array
                for (var i = 0; i < temp.Length; i++)
                {
                    if (i != 0 && (i+1) % 3 == 0)
                    {
                        returnByte = CombineByteArray(returnByte, WearCreateReverseHexPacket(DecideWear(itemPosition, type)));
                        returnByte = CombineByteArray(returnByte, new byte[]{0x00, 0x00, 0x00, 0x00});
                        itemPosition++;
                        continue;
                    }
                    returnByte = CombineByteArray(returnByte, WearCreateReverseHexPacket(Convert.ToInt64(temp[i])));
                }
            }
            return CombineByteArray(returnByte, GetBytesFrom(GetNullString(188 - returnByte.Length)));
        }

        /// <summary>
        /// Decides where the item has to go while displaying in client side
        /// </summary>
        private static long DecideWear(int itemPosition, int type)
        {
            if (itemPosition == 0)
            {
                switch (type)
                {
                    case 1:
                    case 3:
                        return 0;
                    default:
                        return 2;
                }
            }
            if(itemPosition == 1)
            {
                switch (type)
                {
                    case 1:
                    case 3:
                        return 1;
                    default:
                        return 3;
                }
            }
            if (type == 0 || type == 2)
                return itemPosition + 2;
            return itemPosition + 1;
        }

        /// <summary>
        /// Creates all character display packet
        /// </summary>
        public static byte[] CreateCharacterPacket(string[] chars, string[] levels, string[] types, string[] wears)
        {
            if (chars[0] == " ")
                return CreateNewAccountPacket();
            var toReturn = new byte[] { 0x00 };
            for (var i = 0; i < chars.Length; i++)
                toReturn = i == 0 ? CreateCharDetailPacket(chars[i], Convert.ToInt32(levels[i]), Convert.ToInt32(types[i]), wears[i]) : CombineByteArray(toReturn, chars[i] != " " ? CreateCharDetailPacket(chars[i], Convert.ToInt32(levels[i]), Convert.ToInt32(types[i]), wears[i]) : CreateEmptyCharSlot());
            return CombineByteArray(new byte[] {0xb8, 0x03, 0x00, 0x00, 0x00, 0x00, 0x0b, 0x00, 0x03, 0xff, 0x05, 0x11}, Crypt.Encrypt(toReturn));
        }

        /// <summary>
        /// Creates packet for account without characters
        /// </summary>
        private static byte[] CreateNewAccountPacket()
        {
            var header = new byte[] { 0xb8, 0x03, 0x00, 0x00, 0x00, 0x00, 0x0b, 0x00, 0x03, 0xff, 0x05, 0x11 };
            var toReturn = new byte[] {0x00};
            for (var i = 0; i < 5; i++)
                toReturn = i == 0 ? CreateEmptyCharSlot() : CombineByteArray(toReturn, CreateEmptyCharSlot());
            return CombineByteArray(header, Crypt.Encrypt(toReturn));
        }

        /// <summary>
        /// Creates empty slot of each character packet
        /// </summary>
        private static byte[] CreateEmptyCharSlot()
        {
            var toReturn = CombineByteArray(GetBytesFrom(GetNullString(22)), new byte[] { 0xff });
            return CombineByteArray(toReturn, GetBytesFrom(GetNullString(188 - 23)));
        }

        /// <summary>
        /// Creates a client side dialog box with a 'OK' button and custom message which exits the game on response from player
        /// </summary>
        public static byte[] CreatePopup(string msg)
        {
            var header = new byte[] { 0x4e, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0b, 0x00, 0x03, 0xff, 0xff, 0x0f, 0x7e, 0x2f, 0x6e, 0x33};
            if (msg.Length > 56)
                msg = msg.Substring(0, 56);
            var toReturn = CombineByteArray(GetBytesFrom(msg), GetBytesFrom(GetNullString(60 - msg.Length)));
            return CombineByteArray(header, CombineByteArray(Crypt.Encrypt(toReturn), new byte[] { 0x00, 0x00 }));
        }

        /// <summary>
        /// Checks whether the packet is character deletion request packet
        /// </summary>
        public static bool CheckCharDeletePacket(byte[] packet)
        {
            return packet[8] == 0x03 && packet[9] == 0xff && packet[10] == 0x02 && packet[11] == 0xa0;
        }
    }
}
