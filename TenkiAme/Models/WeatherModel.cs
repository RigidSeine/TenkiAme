using TenkiAme.DataTransferObjects;
using TenkiAme.wwwroot;

namespace TenkiAme.Models
{
    public class WeatherModel
    {
        public Dictionary<string, VariableDetails> WeatherVariables { get; set; }
        public List<WeatherHourData> WeatherHours { get; set; }

        public WeatherModel() 
        {
            //GetVariables();
        }

        public async Task InitializeAsync()
        { 
            await GetVariables();
            //TODO: Call CreateHourlyTimeSeries();
        }

        //Get weather data from the MetOcean API
        private async Task GetVariables()
        {
            var service = new WeatherAPIService();
            var weatherResponse = await service.GetPointTimeSeries();
            WeatherVariables = weatherResponse.Variables;
        }

        //Breakdown 
        private void CreateHourlyTimeSeries ()
        {
            List<WeatherHourData> weatherHours = new List<WeatherHourData>();
            //TODO: Loop through WeatherVariables and build out each hour
            //Append each hour to the list
            WeatherHours = weatherHours;
        }
    }
}
