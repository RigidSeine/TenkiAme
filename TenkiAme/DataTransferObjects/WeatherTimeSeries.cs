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

        public List<DateTime> GetTimeSeriesAsList(bool isInUTC = true)
        {
            List<DateTime> list = new List<DateTime>();

            //Take out the 'h' and convert to integer
            int interval = Int32.Parse(this.Interval.Replace("h", ""));

            //Copy the From property
            DateTime from = this.From;

            //Covert to entries to local time the argument passed in is true
            from = isInUTC ? TimeZoneInfo.ConvertTimeFromUtc(from, TimeZoneInfo.Local) : from;

            //Build timeseries
            for (int i = 0; i < this.Repeat+1; i++)
            {
                list.Add(from);
                from = from.AddHours(1);
            }

            return list;
        }
    }
}
