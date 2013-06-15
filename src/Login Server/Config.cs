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
using System.Net;

#endregion
namespace Login_Server
{
    /// <summary>
    /// Class for saving server configurations
    /// </summary>
    public static class Config
    {
        public static IPAddress LoginServerIp { get; set; }
        public static IPAddress GameServerIp { get; set; }
        public static int LoginServerPort { get; set; }
        public static int GameServerPort { get; set; }
        public static string DbServerHost { get; set; }
        public static string DbUsername { get; set; }
        public static string DbPassword { get; set; }
        public static string DbName { get; set; }
        public static string ServerName { get; set; }
    }
}
