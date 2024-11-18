using Microsoft.Data.SqlClient;

namespace TenkiAme.DataTransferObjects
{
    public class WeatherDayData
    {
        public DateTime CalendarDate { get; set; }

        public List<WeatherHourData> WeatherHours { get; set; }

        public WeatherDayData(DateTime date) 
        {
            this.CalendarDate = date;
            WeatherHours = new List<WeatherHourData>();
        }

        public double? GetCurrentTemperature()
        {
            if (WeatherHours == null) { return double.NaN; }

            try
            {
                return WeatherHours.Find(weatherHour => weatherHour.Time.Hour == DateTime.Now.Hour).Temperature;
            }
            catch 
            { return double.NaN; }
       
        }

        public double? GetMaxTemperature()
        {
            if (WeatherHours == null) { return double.NaN; }

            return WeatherHours.Max(weatherHour => weatherHour.Temperature);
        }

        public double? GetMinTemperature()
        {
            if (WeatherHours == null) { return double.NaN; }

            return WeatherHours.Min(weatherHour => weatherHour.Temperature);
        }

        public string GetFormattedCalendarDate()
        {
            return CalendarDate.Date.ToString("dd MMM");
        }

        public string GetWeatherImage()
        {
            if (WeatherHours.Any(weatherHour => weatherHour.Rainfall >= 1.0))
            {
                return "/images/Umbrella-Transparent.png";
            }
            else
            {
                return "/images/Sun-Transparent.png";
            }
        }

        public string PrintToPage()
        {
            var str = "Date: " + CalendarDate.Date.ToString("dd MMM") + 
                       "; Temperature High: " + GetMaxTemperature() +
                       "°C; Temperature Low: " + GetMinTemperature() +
                       "°C";

            return str;

        }
    }
}
