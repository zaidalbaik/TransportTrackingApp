using System.ComponentModel.DataAnnotations.Schema;

namespace TTSAPI.Models
{
    public class Line
    {
        public int LineID { get; set; }
        public string? LineName { get; set; }

        public Nullable<int> Dep_StationID { get; set; }

        [ForeignKey("Dep_StationID")]
        public Station? Departure_Station { get; set; }

        public Nullable<int> Arr_StationID { get; set; }
        [ForeignKey("Arr_StationID")]
        public Station? Arrival_Station { get; set; }
    }
}
