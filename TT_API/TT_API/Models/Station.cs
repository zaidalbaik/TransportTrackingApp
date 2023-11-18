namespace TTSAPI.Models
{
    public class Station
    {
        public int StationID { get; set; }

        public string? StationName { get; set; }

        public Nullable<double> Lat { get; set; }

        public Nullable<double> Lon { get; set; }
    }
}
