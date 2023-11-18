using System.Collections.Generic;

namespace TT_App.Models
{
    public class Direction
    {
        // Direction myDeserializedClass = JsonConvert.DeserializeObject<Direction>(myJsonResponse);

        public List<GeocodedWaypoint> Geocoded_waypoints { get; set; }
        public List<Route> Routes { get; set; }
        public string Status { get; set; }

    }

    public class Bounds
    {
        public Northeast Northeast { get; set; }
        public Southwest Southwest { get; set; }
    }

    public class Distance
    {
        public string Text { get; set; }
        public int Value { get; set; }
    }

    public class Duration
    {
        public string Text { get; set; }
        public int Value { get; set; }
    }

    public class EndLocation
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public class GeocodedWaypoint
    {
        public string Geocoder_status { get; set; }
        public string Place_id { get; set; }
        public List<string> Types { get; set; }
    }

    public class Leg
    {
        public Distance Distance { get; set; }
        public Duration Duration { get; set; }
        public string End_address { get; set; }
        public EndLocation End_location { get; set; }
        public string Start_address { get; set; }
        public StartLocation Start_location { get; set; }
        public List<Step> Steps { get; set; }
        public List<object> Traffic_speed_entry { get; set; }
        public List<object> Via_waypoint { get; set; }
    }

    public class Northeast
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public class OverviewPolyline
    {
        public string Points { get; set; }
    }

    public class Polyline
    {
        public string Points { get; set; }
    }



    public class Route
    {
        public Bounds Bounds { get; set; }
        public string Copyrights { get; set; }
        public List<Leg> Legs { get; set; }
        public OverviewPolyline Overview_polyline { get; set; }
        public string Summary { get; set; }
        public List<object> Warnings { get; set; }
        public List<object> Waypoint_order { get; set; }
    }

    public class Southwest
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public class StartLocation
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public class Step
    {
        public Distance Distance { get; set; }
        public Duration Duration { get; set; }
        public EndLocation End_location { get; set; }
        public string Html_instructions { get; set; }
        public Polyline Polyline { get; set; }
        public StartLocation Start_location { get; set; }
        public string Travel_mode { get; set; }
        public string Maneuver { get; set; }
    }



}