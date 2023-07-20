namespace MotorsportAPI.Business
{
    public class RaceEvent
    {
        public RaceEvent(string eventName, string trackName, string dates)
        {
            EventName = eventName;
            TrackName = trackName;
            Dates = dates;
        }
        public string EventName { get; set; }
        public string TrackName { get; set; }
        public string Dates { get; set; }
    }
}
