namespace TT_App.Models
{
    public class Line
    {
        public int LineID { get; set; }

        public string LineName { get; set; }

        public int Dep_StationID { get; set; }

        public int Arr_StationID { get; set; }
    }
}
