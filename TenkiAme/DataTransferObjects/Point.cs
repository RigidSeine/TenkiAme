using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace TenkiAme.DataTransferObjects
{
    public class Point
    {
        [JsonProperty("lat")]
        public double Lat { get; set; }
        [JsonProperty("lon")]
        public double Lon { get; set; }

        public Point (double lat, double lon)
        {
            this.Lat = lat;
            this.Lon = lon;
        }
    }
}
