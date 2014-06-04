using System.Collections.Generic;
using System.Net;

namespace InfernoEmu
{
    /// <summary>
    /// Holds player usernames authenticated by login server
    /// </summary>
    public static class PreparedPlayers
    {
        private static readonly Dictionary<string, IPAddress> PlayerUserid = new Dictionary<string, IPAddress>();

        /// <summary>
        /// Set an account as authenticated for Game Server's reference
        /// </summary>
        public static void PreparePlayer(string userid, IPAddress ip)
        {
            if (PlayerUserid.ContainsKey(userid))
                return;
            PlayerUserid.Add(userid, ip);
            return;
        }

        /// <summary>
        /// Returns authentication status of an account
        /// </summary>
        public static bool IsPrepared(string userid)
        {
            return PlayerUserid.ContainsKey(userid);
        }

        /// <summary>
        /// De-authenticates an account
        /// </summary>
        public static void UnPrepare(string userid)
        {
            if (PlayerUserid.ContainsKey(userid))
                PlayerUserid.Remove(userid);
        }
    }
}
