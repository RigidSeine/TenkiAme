using Newtonsoft.Json;

namespace TenkiAme.DataTransferObjects
{
    public class WeatherTimeSeries
    {
        [JsonProperty("from")]
        public DateTime From {  get; set; }
        
        [JsonProperty("interval")]
        public string Interval { get; set; }
        
        [JsonProperty("repeat")]
        public int Repeat {  get; set; }
        
        public WeatherTimeSeries(DateTime from, string interval, int repeat) 
        {
            this.From = from;
            this.Interval = interval;
            this.Repeat = repeat;
        }

    }
}
