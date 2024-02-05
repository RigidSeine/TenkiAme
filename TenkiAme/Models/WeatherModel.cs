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
        }

        public async Task InitializeAsync()
        {
            try
            {
                await Task.WhenAll(GetVariables());


            }
            catch (Exception ex)
            {
                DevUtil.PrintD("Exception during InitializeAsync - Calling GetVariables(): " + ex.Message);
            }

            await Task.Run(CreateHourlyTimeSeries);
        }

        //Get weather data from the MetOcean API
        private async Task GetVariables()
        {
            _weatherAPIService = new WeatherAPIService();
            var weatherResponse = await _weatherAPIService.GetPointTimeSeries();
            WeatherVariables = weatherResponse.Variables;
        }

        //Breakdown 
        private void CreateHourlyTimeSeries ()
        {
            List<WeatherHourData> weatherHours = new List<WeatherHourData>();

            var weatherTimeSeries = _weatherAPIService.WeatherTimeSeries.GetTimeSeriesAsList();
            var rainData = WeatherVariables["precipitation.rate"].Data;
            var temperatureData = WeatherVariables["air.temperature.at-2m"].Data;

            for(int i = 0; i < _numOfDataPoints; i++)
            {
                var weatherHour = new WeatherHourData(weatherTimeSeries[i], rainData[i], temperatureData[i]);
                weatherHours.Add(weatherHour);
            }

            WeatherHours = weatherHours;
        }
    }
}