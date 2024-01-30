using TenkiAme.DataTransferObjects;
using TenkiAme.wwwroot;

namespace TenkiAme.Models
{
    public class WeatherModel
    {
        private const int _numOfDataPoints = 48;

        private WeatherAPIService _weatherAPIService;
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
            var _weatherAPIService = new WeatherAPIService();
            var weatherResponse = await _weatherAPIService.GetPointTimeSeries();
            WeatherVariables = weatherResponse.Variables;
        }

        //Breakdown 
        private void CreateHourlyTimeSeries ()
        {
            List<WeatherHourData> weatherHours = new List<WeatherHourData>();

            var weatherTimeSeries = _weatherAPIService.WeatherTimeSeries.GetTimeSeriesAsList();
            //TODO: Loop through WeatherVariables and build out each hour
            //Append each hour to the list
            for(int i = 0; i < _numOfDataPoints; i++)
            {
                foreach(var variable in WeatherVariables)
                {
                    return;//build out each hour
                }
            }
            WeatherHours = weatherHours;
        }
    }
}
