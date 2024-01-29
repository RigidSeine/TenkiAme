using Humanizer;
using Newtonsoft.Json;

namespace TenkiAme.DataTransferObjects
{
    public class PointPostData
    {
        [JsonProperty("points")]
        public List<Point> Points { get; set; }
        
        [JsonProperty("variables")]
        public List<string> Variables { get; set; }

        [JsonProperty("time")]
        public WeatherTimeSeries Time { get; set; }

        public PointPostData(double lat, double lon, List<string> postVariables, WeatherTimeSeries time) 
        {
            this.Points = new List<Point>();
            this.Points.Add(new Point(lat, lon));
            this.Variables = postVariables;
            this.Time = time;
        }

        public PointPostData(Point point, List<string> postVariables, WeatherTimeSeries time)
        {
            this.Points = new List<Point>();
            this.Points.Add(point);
            this.Variables = postVariables;
            this.Time = time;
        }
    }
}
