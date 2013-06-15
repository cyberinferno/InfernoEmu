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
using System.IO;
using System.Net;
using System.Security.Permissions;

#endregion

namespace Login_Server
{
    public static class Program
    {
        private static bool _isRunning;
        private static Server _loginServer;

        /// <summary>
        /// Returns server status.
        /// </summary>
        public static bool Running
        {
            get { return (!Environment.HasShutdownStarted && _isRunning); }
        }

        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        public static void Main()
        {
            try
            {
                //Initializing default configuration values
                Config.LoginServerPort = 3550;
                Config.GameServerPort = 3300;
                Config.ServerName = "InfernoEmu";
                Config.LoginServerIp = IPAddress.Any;
                Config.GameServerIp = IPAddress.Parse("127.0.0.1");
                Config.DbServerHost = "localhost";
                Config.DbUsername = "root";
                Config.DbPassword = "";
                Config.DbName = "a3";

                if(!File.Exists("LoginServer.ini"))
                    throw new FileNotFoundException("LoginServer.ini not found");
                using (var streamReader = new StreamReader("LoginServer.ini", true))
                {
                    string readLine;
                    while ((readLine = streamReader.ReadLine()) != null)
                    {
                        var config = readLine.Split(':');
                        if(config.Length != 2)
                            continue;
                        switch (config[0])
                        {
                            case "LoginServerIp":
                                Config.LoginServerIp = IPAddress.Parse(config[1].Trim());
                                break;
                            case "LoginServerPort":
                                Config.LoginServerPort = Convert.ToInt32(config[1].Trim());
                                break;
                            case "GameServerIp":
                                Config.GameServerIp = IPAddress.Parse(config[1].Trim());
                                break;
                            case "GameServerPort":
                                Config.GameServerPort = Convert.ToInt32(config[1].Trim());
                                break;
                            case "ServerName":
                                Config.ServerName = config[1].Trim();
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
                            case "DatabaseName":
                                Config.DbName = config[1].Trim();
                                break;
                        }
                    }
                }
                _loginServer = new Server();
                _loginServer.Start();
                _isRunning = true;
                Console.WriteLine(".:: Starting InfernoEmu Login Server ::.");
                Console.Title = "InfernoEmu Login Server";
                Console.WriteLine("Server started with IP {0} , port {1} and server name {2}!", Config.LoginServerIp, Config.LoginServerPort, Config.ServerName);
                Console.WriteLine("Game server IP is {0} and port is {1}", Config.GameServerIp, Config.GameServerPort);
                Console.WriteLine("Press ENTER key to activate command interface!");
                Console.Beep();
                Command.Listen();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
                Logger.WriteLog("ERROR: " + ex.Message);
                Console.ReadKey(true);
                Stop();
            }
        }

        /// <summary>
        /// Gracefully stops server.
        /// </summary>
        public static void Stop()
        {
            try
            {
                _isRunning = false;
                _loginServer.Stop();
                _loginServer = null;
                Environment.Exit(0);
            }
            catch
            {
                _isRunning = false;
                Environment.Exit(0);
            }
        }
    }
}