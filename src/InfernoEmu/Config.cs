using System.Net;

namespace InfernoEmu
{
    /// <summary>
    /// Class for saving server configurations
    /// </summary>
    public static class Config
    {
        public static IPAddress ServerIp { get; set; }
        public static int LoginServerPort { get; set; }
        public static int GameServerPort { get; set; }
        public static string DbServerHost { get; set; }
        public static string DbUsername { get; set; }
        public static string DbPassword { get; set; }
        public static string MaintenanceMsg { get; set; }
        public static string WelcomeMsg { get; set; }
        public static bool IsMaintainance { get; set; }
        public static bool IsLoginServerRunning { get; set; }
        public static bool IsGameServerRunning { get; set; }
        public static string LogName { get; set; }
        public static string ServerName { get; set; }
        public static int PlayerCount { get; set; }
    }
}
