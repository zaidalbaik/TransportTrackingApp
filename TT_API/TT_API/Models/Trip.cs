using System.ComponentModel.DataAnnotations.Schema;

namespace TTSAPI.Models
{
    public class Trip
    {
        public int Id { get; set; }

        public Nullable<int> LineId { get; set; }
        [ForeignKey("LineId")]
        public Line? Line { get; set; }

        public Nullable<int> DriverId { get; set; }
        [ForeignKey("DriverId")]
        public Driver? Driver { get; set; }

        public DateTime? Departure_time { get; set; }

        public DateTime? Arrival_time { get; set; }




    }
}
