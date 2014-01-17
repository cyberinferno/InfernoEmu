namespace InfernoEmu
{
    /// <summary>
    /// Holds player information
    /// </summary>
    public class PlayerInfo
    {
        public string Account { get; set; }
        public int Time { get; set; }
        public bool Prepared { get; set; }
        public bool ZoneStatus { get; set; }
        public int CurrentZone { get; set; }

        public PlayerInfo(string account, int time, bool prepared, bool zoneStatus)
        {
            Account = account;
            Time = time;
            Prepared = prepared;
            ZoneStatus = zoneStatus;
            CurrentZone = 255;
        }
    }
}
