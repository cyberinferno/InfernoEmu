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
        public static void Main(string[] args)
        {
            try
            {
                int port = 3550;
                int gsPort = 9856;
                string serverName = "InfernoEmu";
                IPAddress loginServerIp = IPAddress.Any;
                IPAddress gameServerIp = IPAddress.Parse("127.0.0.1");
                _isRunning = true;
                /*if(!File.Exists("LoginServer.ini"))
                    throw new FileNotFoundException("LoginServer.ini not found");
                using (var streamReader = new StreamReader("LoginServer.ini", true))
                {
                    var readLine = streamReader.ReadLine();
                }*/ //Config file support will be done in future release
                Console.WriteLine(".:: Starting InfernoEmu Login Server ::.");
                Console.Title = "InfernoEmu Login Server";
                Console.WriteLine("Press ENTER key to activate command interface!");
                _loginServer = new Server(port, gsPort, loginServerIp, gameServerIp, serverName);
                _loginServer.Start();
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
            _isRunning = false;
            _loginServer.Stop();
            _loginServer = null;
            Environment.Exit(0);
        }
    }
}