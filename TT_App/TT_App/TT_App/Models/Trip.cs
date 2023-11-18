using System;

namespace TT_App.Models
{
    public class Trip
    {
        public int Id { get; set; }

        public int LineId { get; set; }

        public int DriverId { get; set; }

        public DateTime? Departure_time { get; set; }

        public DateTime? Arrival_time { get; set; }       
    }
}

