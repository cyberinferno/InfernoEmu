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
using MySql.Data.MySqlClient;

#endregion

namespace Login_Server
{
    public sealed class Database : IDisposable
    {
        public readonly MySqlConnection Connection;
        private bool _isDisposed;

        public Database()
        {
            string connectionString = "SERVER=" + Config.DbServerHost + ";" + "DATABASE=" + Config.DbName + ";" + "UID=" + Config.DbUsername + ";" +
                                      "PASSWORD=" + Config.DbPassword + ";";
            Connection = new MySqlConnection(connectionString);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    Connection.Dispose();
                }
            }
            _isDisposed = true;
        }

        /// <summary>
        /// Opens a new connection to the database and returns its connection result
        /// </summary>
        public bool OpenConnection()
        {
            try
            {
                Connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to MySQL server!");
                        break;
                    case 1045:
                        Console.WriteLine("Invalid MySQL server username or password!");
                        break;
                    default:
                        Console.WriteLine("Cannot connect to " + Config.DbName + " database!");
                        break;
                }
                return false;
            }
        }

        /// <summary>
        /// Closes database connection
        /// </summary>
        public bool CloseConnection()
        {
            try
            {
                Connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("MySQL disconnection error : " + ex.Message);
                return false;
            }
        }
    }
}