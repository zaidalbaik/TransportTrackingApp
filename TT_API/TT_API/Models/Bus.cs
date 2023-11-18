using System.ComponentModel.DataAnnotations.Schema;

namespace TTSAPI.Models
{
    public class Bus
    {
        public int BusID { get; set; }

        public Nullable<double> Lat { get; set; }

        public Nullable<double> Lon { get; set; }

        public Nullable<int> LineId { get; set; }
        [ForeignKey("LineId")]
        public Line? Line { get; set; }

        public bool? IsActive { get; set; }
    }
}
