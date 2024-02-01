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
            await CreateHourlyTimeSeries();
        }

        //Get weather data from the MetOcean API
        private async Task GetVariables()
        {
            _weatherAPIService = new WeatherAPIService();
            var weatherResponse = await _weatherAPIService.GetPointTimeSeries();
            WeatherVariables = weatherResponse.Variables;
        }

        //Breakdown 
        private async Task CreateHourlyTimeSeries ()
        {
            List<WeatherHourData> weatherHours = new List<WeatherHourData>();

            var weatherTimeSeries = _weatherAPIService.WeatherTimeSeries.GetTimeSeriesAsList();
            var rainData = WeatherVariables.GetValueOrDefault("precipitation.rate").Data;
            var temperatureData = WeatherVariables.GetValueOrDefault("air.temperature.at-2m").Data;

            //TODO: Solve Nullexceptions
            for(int i = 0; i < _numOfDataPoints; i++)
            {
                var weatherHour = new WeatherHourData(weatherTimeSeries[i], rainData[i], temperatureData[i]);
                weatherHours.Add(weatherHour);
            }

            WeatherHours = weatherHours;

            Util.PrintD("WEAEHTSJKGNSKGJNSJKG ==========================" + WeatherHours.ToString());
        }
    }
}