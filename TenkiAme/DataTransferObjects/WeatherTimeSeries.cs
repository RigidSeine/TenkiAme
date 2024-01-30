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

        public List<DateTime> GetTimeSeriesAsList()
        {
            List<DateTime> list = new List<DateTime>();

            int interval = Int32.Parse(this.Interval.Replace("h", ""));
            DateTime from = this.From; //Copy

            for (int i = 0; i < this.Repeat; i++)
            {
                list.Append(from);
                from = from.AddHours(1);
            }

            return list;
        }
    }
}
