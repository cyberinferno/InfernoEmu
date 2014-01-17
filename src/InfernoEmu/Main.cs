using System;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace InfernoEmu
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            //Default values
            Config.ServerIp = IPAddress.Any;
            Config.DbServerHost = "(local)";
            Config.DbUsername = "sa";
            Config.DbPassword = "ley";
            Config.MaintenanceMsg = "Server is down for maintenance!";
            Config.WelcomeMsg = "Welcome to Inferno Emulator";
            Config.LogName = GenerateFileName("InfernoEmu") + ".log";
            Config.IsLoginServerRunning = false;
            Config.IsGameServerRunning = false;
            Config.PlayerCount = 0;
            Config.ServerName = "Inferno";
            if (!File.Exists("config.ini"))
            {
                MessageBox.Show("config.ini not found!", "InfernoEmu Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
            if (!File.Exists("asdecr.dll"))
            {
                MessageBox.Show("asdecr.dll not found!", "InfernoEmu Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
            if (!File.Exists("msvcp100d.dll"))
            {
                MessageBox.Show("msvcp100d.dll not found!", "InfernoEmu Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
            if (!File.Exists("msvcr100d.dll"))
            {
                MessageBox.Show("msvcr100d.dll not found!", "InfernoEmu Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
            using (var streamReader = new StreamReader("config.ini", true))
            {
                string readLine;
                while ((readLine = streamReader.ReadLine()) != null)
                {
                    var config = readLine.Split(':');
                    if (config.Length != 2)
                        continue;
                    switch (config[0])
                    {
                        case "ServerIp":
                            Config.ServerIp = IPAddress.Parse(config[1].Trim());
                            break;
                        case "LoginServerPort":
                            Config.LoginServerPort = Convert.ToInt32(config[1].Trim());
                            break;
                        case "GameServerPort":
                            Config.GameServerPort = Convert.ToInt32(config[1].Trim());
                            break;
                        case "DatabaseHost":
                            Config.DbServerHost = config[1].Trim();
                            break;
                        case "DatabaseUsername":
                            Config.DbUsername = config[1].Trim();
                            break;
                        case "DatabasePassword":
                            Config.DbPassword = config[1].Trim();
                            break;
                        case "MaintenanceMsg":
                            Config.MaintenanceMsg = config[1].Trim();
                            break;
                        case "WelcomeMsg":
                            Config.WelcomeMsg = config[1].Trim();
                            break;
                    }
                }
            }
            var loginServer = new LoginServer();
            loginServer.Start();
            var gameServer = new GameServer();
            gameServer.Start();
            if(!Config.IsLoginServerRunning)
            {
                MessageBox.Show("Login server could not be started. Check logs for more information", "InfernoEmu Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
            if (!Config.IsGameServerRunning)
            {
                MessageBox.Show("Game server could not be started. Check logs for more information", "InfernoEmu Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
            /*
            using (var streamReader = new StreamReader("xenosoft.acl", true))
            {
                string data = streamReader.ReadToEnd();
                MyLogger.WriteGameServerLog("Data length is " + data.Length);
                var ndata = Crypt.EncryptData(data);
                /*var sb = new StringBuilder(data);
                Crypt.encrypt_acl(data, 1880, 0);
                MyLogger.WriteGameServerLog("Ecrypted data length is " + sb.ToString().Length);
                var byteData = Encoding.Default.GetBytes(sb.ToString());
                var header = new byte[]{ 0xb8, 0x03, 0x00, 0x00, 0x00, 0x00, 0x0b, 0x00, 0x03, 0xff, 0x05, 0x11 };
                Write(client.TcpClient, Packet.CombineByteArray(header, Packet.AlterCharacterPacket(Encoding.Default.GetBytes(ndata))));
            }
            */
        }

        private void maintainanceCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Config.IsMaintainance = maintainanceCheckBox.Checked;
        }

        private static string GenerateFileName(string context)
        {
            return context + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }

        private void uiUpdater_Tick(object sender, EventArgs e)
        {
            serverIpShow.Text = Config.ServerIp.ToString();
            loginServerPortShow.Text = Config.LoginServerPort.ToString();
            gameServerPortShow.Text = Config.GameServerPort.ToString();
            playersShow.Text = Config.PlayerCount.ToString();
        }
    }
}
