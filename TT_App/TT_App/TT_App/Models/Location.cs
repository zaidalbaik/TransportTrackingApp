using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace TT_App.Models
{
    public class Location
    {
        public int BusID { get; set; }

        public Position Position { get; set; }   

        public bool IsActive { get; set; }
    }
}
