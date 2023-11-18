namespace TT_App.Models
{
    public class Bus
    {
        public int BusID { get; set; }

        public double Lat { get; set; }

        public double Lon { get; set; }

        public int LineId { get; set; }

        public bool IsActive { get; set; }
    }
}
