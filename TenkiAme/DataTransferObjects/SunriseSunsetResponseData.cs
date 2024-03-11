namespace TenkiAme.DataTransferObjects
{
    public class SunriseSunsetResponseData
    {
        public string date { get; set; }
        public string sunrise { get; set; }
        public string sunset { get; set; }
        public string first_light { get; set; }
        public string last_light { get; set; }
        public string dawn { get; set; }
        public string dusk { get; set; }
        public string solar_noon { get; set; }
        public string golden_hour { get; set; }
        public string day_length { get; set; }
        public string timezone { get; set; }
        public int utc_offset { get; set; }

        public SunriseSunsetResponseData(string date, string sunrise, string sunset, string first_light, string last_light, string dawn, string dusk, string solar_noon, string golden_hour, string day_length, string timezone, int utc_offset)
        {
            this.date = date;
            this.sunrise = sunrise;
            this.sunset = sunset;
            this.first_light = first_light;
            this.last_light = last_light;
            this.dawn = dawn;
            this.dusk = dusk;
            this.solar_noon = solar_noon;
            this.golden_hour = golden_hour;
            this.day_length = day_length;
            this.timezone = timezone;
            this.utc_offset = utc_offset;
        }
    }
}
