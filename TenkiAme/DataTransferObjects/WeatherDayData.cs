using Microsoft.Data.SqlClient;
using System;

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

        //Uses a non-nullable double and returns a string to get consistent numeric rounding
        public string? GetCurrentTemperature()
        {
            if (WeatherHours == null) { return "N/A"; }

            double? currentTemp = WeatherHours.Find(weatherHour => weatherHour.Time.Hour == DateTime.Now.Hour).Temperature;
            
            return currentTemp.HasValue ? ((double)currentTemp).ToString("F1") : "N/A";
       
        }

        //Uses a non-nullable double and returns a string to get consistent numeric rounding
        public string? GetMaxTemperature()
        {
            if (WeatherHours == null) { return "N/A"; }

            double? maxTemp = WeatherHours.Max(weatherHour => weatherHour.Temperature);
            return maxTemp.HasValue ? ((double)maxTemp).ToString("F1") : "N/A";
        }

        //Uses a non-nullable double and returns a string to get consistent numeric rounding
        public string GetMinTemperature()
        {
            if (WeatherHours == null) { return "N/A"; }

            double? minTemp = WeatherHours.Min(weatherHour => weatherHour.Temperature);

            return minTemp.HasValue ? ((double)minTemp).ToString("F1") : "N/A";
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

        //Debug function
        public string PrintToPage()
        {
            var str = "Date: " + CalendarDate.Date.ToString("dd MMM") + 
                       "; Temperature High: " + GetMaxTemperature() +
                       "°C; Temperature Low: " + GetMinTemperature() +
                       "°C";

            return str;

        }

        //Debug function
        public string PrintAllValues()
        {
            var str = "";

            foreach (var hour in WeatherHours)
            {
                str += "Time: " + hour.Time + "; Temp: " + hour.Temperature + "; Rain: " + hour.Rainfall;
                if(hour.Temperature == Convert.ToDouble(GetMaxTemperature()))
                {
                    str += " !!MAX TEMP!!";
                }
                else if (hour.Temperature == Convert.ToDouble(GetMaxTemperature()))
                {
                    str += " !!MIN TEMP!!";
                }

                    str += '\n';
            }

            return str;
        }
    }
}
