using System.Collections.Generic;
using System.Net;

namespace InfernoEmu
{
    /// <summary>
    /// Holds players preapred by login server
    /// </summary>
    public static class PreparedPlayers
    {
        private static readonly Dictionary<string, IPAddress> PlayerUserid = new Dictionary<string, IPAddress>();

        public static bool PreparePlayer(string userid, IPAddress ip)
        {
            if (PlayerUserid.ContainsKey(userid))
                return false;
            PlayerUserid.Add(userid, ip);
            return true;
        }

        public static bool IsPrepared(string userid)
        {
            if (PlayerUserid.ContainsKey(userid))
                return true;
            return false;
        }

        public static void UnPrepare(string userid)
        {
            if (PlayerUserid.ContainsKey(userid))
                PlayerUserid.Remove(userid);
        }
    }
}
