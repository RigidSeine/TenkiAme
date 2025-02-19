using TenkiAme.DataTransferObjects;
using TenkiAme.UtilityObjects;
using System.Linq;
using System.Runtime.InteropServices;

namespace TenkiAme.Models
{
    public class WeatherModel
    {
        private WeatherAPIService _weatherAPIService;
        public Dictionary<string, VariableDetails> WeatherVariables { get; set; }
        
        public List<WeatherDayData> WeatherDays { get; set; }

        public List<SunriseSunsetResponseData> SunriseSunsets { get; set; }

        public List<UVResponseProduct> UVs { get; set; }


        public WeatherModel() 
        {
            _weatherAPIService = new WeatherAPIService();
        }

        public async Task InitializeAsync()
        {
            try
            {
                await Task.WhenAll(GetVariablesAsync());
            }
            catch (Exception ex)
            {
                DevUtil.PrintD("Exception encountered during InitializeAsync - Calling GetVariables(): " + ex.Message);
            }

            try
            { 
                await Task.Run(CreateDailyTimeSeries);
            } 
            catch (Exception ex) 
            {
                DevUtil.PrintD("Exception encountered during InitializeAsync - Calling CreateDailyTimeSeries(): " + ex.Message);
            }

            try
            {
                await Task.Run(StoreSunriseSet);
            }
            catch ( Exception ex)
            {
                DevUtil.PrintD("Exception encountered during InitializeAsync - Calling StoreSunriseSet(): " + ex.Message);
            }

            try
            {
                await Task.Run(StoreUVData);
            }
            catch (Exception ex)
            {
                DevUtil.PrintD("Exception encountered InitializeAsync - Calling StoreUVData(): " + ex.Message);
            }
        }

        //Get weather data from the MetOcean API
        private async Task GetVariablesAsync()
        {
            var weatherResponse = await _weatherAPIService.GetPointTimeSeriesAsync();
            WeatherVariables = weatherResponse.Variables;
            //PrintNoDataReasons(WeatherVariables);
        }

        //Breakdown the API Response data and recombine it into a form suitable for use in a webpage.
        private void CreateDailyTimeSeries()
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
                    var weatherHour = new WeatherHourData(day.ElementAt(i), WeatherUtil.RoundPrecipitationRate(rainByDay[i]), WeatherUtil.KelvinToCelsius(tempByDay[i]));
                    weatherDay.WeatherHours.Add(weatherHour);
                }

                //Add to the list of weatherdays under the WeatherModel
                WeatherDays.Add(weatherDay);
            }

        }
        //Get Twilight info and store it
        private async Task StoreSunriseSet()
        {
            var sunriseSunsetResponse = await _weatherAPIService.GetSunriseSunset("Wellington");
            SunriseSunsets = sunriseSunsetResponse.Results;
        }

        //Get UV info and store it
        private async Task StoreUVData()
        {
            
            var uVresponse = await _weatherAPIService.GetUVDataFromNiwa("Wellington");
            UVs = uVresponse.products;
        }

        private void PrintNoDataReasons(Dictionary<string, VariableDetails> weatherVariables)
        {
            foreach(var weatherVariable in weatherVariables)
            {
                if (weatherVariable.Value.Data == null)
                {
                    DevUtil.PrintD("No data for " + weatherVariable.Key);
                }
                else
                {
                    DevUtil.PrintD("Data for " + weatherVariable.Key + ": " );
                    foreach (var data in weatherVariable.Value.NoData)
                    {
                        DevUtil.PrintD(data.ToString());
                    }
                }

            }
        }
    }
}