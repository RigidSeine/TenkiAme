using Microsoft.VisualStudio.TextTemplating;

namespace TenkiAme.DataTransferObjects
{
    public class WeatherHourData
    {
        
        public DateTime Time { get; set; }
        public double? Rainfall {  get; set; }
        public double? Temperature { get; set; }

        public WeatherHourData(DateTime time, double? rainfall, double? temperature) 
        { 
            Time = time;
            Rainfall = rainfall;
            Temperature = temperature;
        }

        public string PrintToPage()
        {
            return "Time: " + Time.ToString() + "; Rain: " + Rainfall.ToString() + "; Temperature: " + Temperature.ToString();
        }
    }
}
