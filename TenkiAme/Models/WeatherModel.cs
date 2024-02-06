using TenkiAme.DataTransferObjects;
using TenkiAme.UtilityObjects;
using System.Linq;

namespace TenkiAme.Models
{
    public class WeatherModel
    {
        private const int NumOfDataPoints = 48;

        private WeatherAPIService _weatherAPIService;
        public Dictionary<string, VariableDetails> WeatherVariables { get; set; }
        
        public List<WeatherDayData> WeatherDays { get; set; }


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

            await Task.Run(CreateDailyTimeSeries);
        }

        //Get weather data from the MetOcean API
        private async Task GetVariables()
        {
            _weatherAPIService = new WeatherAPIService();
            var weatherResponse = await _weatherAPIService.GetPointTimeSeries();
            WeatherVariables = weatherResponse.Variables;
        }

        //Breakdown the API Response data and recombine it into a form suitable for use in a webpage.
        private void CreateDailyTimeSeries ()
        {
            WeatherDays = new List<WeatherDayData>();

            var weatherTimeSeries = _weatherAPIService.WeatherTimeSeries.GetTimeSeriesAsList();
            var rainData = WeatherVariables["precipitation.rate"].Data;
            var temperatureData = WeatherVariables["air.temperature.at-2m"].Data;

            //Group the timeseries by day
            var hoursByDay = weatherTimeSeries.GroupBy(x => x.Date);
            
            //Iterate over each group of hours
            foreach (var day in hoursByDay)
            {
                //Group the rain data by day - aka foreach index of rainData check if the
                //corresponding value in weatherTimeSeries matches the current group's date
                //and return the value (precipitation rate) if so
                var rainByDay = rainData.Where((value, index) => weatherTimeSeries[index].Date == day.Key).ToList();

                //Do the same for temperature
                var tempByDay = temperatureData.Where((value, index) => weatherTimeSeries[index].Date == day.Key).ToList();

                //Instantiate a new WeatherDay
                WeatherDayData weatherDay = new WeatherDayData(day.Key);

                //Now iterate over the group of hours (day) and fill in the WeatherDay
                for (int i = 0; i < day.Count(); i++)
                {
                    var weatherHour = new WeatherHourData(day.ElementAt(i), rainByDay[i], WeatherUtil.KelvinToCelsius(tempByDay[i]));
                    weatherDay.WeatherHours.Add(weatherHour);
                }

                //Add to the list of weatherdays under the WeatherModel
                WeatherDays.Add(weatherDay);
            }

        }
    }
}