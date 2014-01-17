using System;
using System.IO;

namespace InfernoEmu
{
    public static class MyLogger
    {
        /// <summary>
        /// Writes specified string into log file
        /// </summary>
        public static void WriteLog(string log)
        {
            try
            {
                using (var sw = new StreamWriter(Config.LogName, true))
                {
                    sw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy h:mm:ss tt") + " : " + log);
                }
            }
            catch { }
        }

        public static void WriteLoginServerLog(string log)
        {
            var name = "LoginServer_" + Config.LogName.Split('_')[1];
            try
            {
                using (var sw = new StreamWriter(name, true))
                {
                    sw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy h:mm:ss tt") + " : " + log);
                }
            }
            catch { }
        }

        public static void WriteGameServerLog(string log)
        {
            var name = "GameServer_" + Config.LogName.Split('_')[1];
            try
            {
                using (var sw = new StreamWriter(name, true))
                {
                    sw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy h:mm:ss tt") + " : " + log);
                }
            }
            catch { }
        }
    }
}
