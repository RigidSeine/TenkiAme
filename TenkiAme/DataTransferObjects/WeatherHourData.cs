
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

        //Return the value formatted to one decimal point if it's not null otherwise return N/A
        public string GetFormattedRainfall()
        {
            return Rainfall.HasValue ? ((double)Rainfall).ToString("F1") : "N/A";
        }

        //Return the value formatted to one decimal point if it's not null otherwise return N/A
        public string GetFormattedTemperature()
        {
            return Temperature.HasValue ? ((double)Temperature).ToString("F1") : "N/A";
        }


        public string PrintToPage()
        {
            return "Time: " + Time.ToString("htt") + "; Rain: " + Rainfall.ToString() + "mm; Temperature: " + Temperature.ToString() + "°C";
        }
    }
}
