using System;
using System.Data;
using System.Data.SqlClient;

namespace InfernoEmu
{
    /// <summary>
    /// Class for database operations
    /// </summary>
    public class Database
    {
        private readonly SqlConnection _myConnection;
        private bool _isConnected;

        public Database()
        {
            _isConnected = false;
            _myConnection = new SqlConnection("user id=" + Config.DbUsername + ";password=" + Config.DbPassword + ";server=" + Config.DbServerHost + ";Trusted_Connection=yes;database=ASD;connection timeout=30");
            Connect();
        }

        /// <summary>
        /// Tries to connects to Database and updates connection status
        /// </summary>
        private void Connect()
        {
            try
            {
                _myConnection.Open();
                _isConnected = true;
            }
            catch (Exception e)
            {
                MyLogger.WriteLog("ERROR in connecting to MSSQL. Error details :::::::::: " + e);
                _isConnected = false;
            }
        }
        
        /// <summary>
        /// Disconnects from database
        /// </summary>
        private void Disconnect()
        {
            _myConnection.Close();
            _isConnected = false;
        }

        /// <summary>
        /// Validates user credentials
        /// </summary>
        public bool IsValidUser(string username, string passwd)
        {
            if(!_isConnected)
                return false;
            var cmd = new SqlCommand("select c_id from account where c_id = '" + username + "' and c_headera = '" + passwd + "'", _myConnection);
            var dataReader = cmd.ExecuteReader();
            var toReturn = dataReader.HasRows;
            dataReader.Close();
            return toReturn;
        }

        /// <summary>
        /// check for banned user
        /// </summary>
        public bool IsBanned(string username)
        {
            if (!_isConnected)
                return false;
            var cmd = new SqlCommand("select c_id from account where c_id = '" + username + "' and c_status != 'A'", _myConnection);
            var dataReader = cmd.ExecuteReader();
            var toReturn = dataReader.HasRows;
            dataReader.Close();
            return toReturn;
        }

        /// <summary>
        /// check if character exists in the account
        /// </summary>
        public bool CharacterExists(string username, string character)
        {
            if (!_isConnected)
                return false;
            var cmd = new SqlCommand("select c_id from charac0 where c_sheadera = '" + username + "' and c_id = '" + character + "'", _myConnection);
            var dataReader = cmd.ExecuteReader();
            var toReturn = dataReader.HasRows;
            dataReader.Close();
            return toReturn;
        }

        public void GetCharacters(string username, ref string[] chars, ref string[] levels, ref string[] types )
        {
            if (!_isConnected)
            {
                chars = new[] {" "};
                levels = new[] { " " };
                types = new[] { " " };
            }
            var cmd = new SqlCommand("select c_id, c_sheaderc, c_sheaderb from charac0 where c_sheadera = '" + username + "' and c_status = 'A' order by d_udate desc", _myConnection);
            var dataReader = cmd.ExecuteReader();
            if (!dataReader.HasRows)
            {
                chars = new[] { " " };
                levels = new[] { " " };
                types = new[] { " " };
            }
            else
            {
                int i = 0;
                for (int j = 0; j < 5; j++ )
                {
                    chars[j] = " ";
                    levels[j] = " ";
                    types[j] = " ";
                }
                while (dataReader.Read())
                {
                    var row = (IDataRecord)dataReader;
                    chars[i] = row[0].ToString().Trim();
                    levels[i] = row[1].ToString().Trim();
                    types[i] = row[2].ToString().Trim();
                    i++;
                    if (i >= 5)
                        break;
                }
                dataReader.Close();
            }
        }
    }
}
