using Microsoft.AspNetCore.Mvc;
using System.Text;
using Newtonsoft.Json;
using TenkiAme.DataTransferObjects;
using TenkiAme.Data;


namespace TenkiAme.Models
{
    public class WeatherAPIService
    {
        private const int NumOfDataPoints = 214; //214 is the number of data points for 9 days. The API only returns data for 9.5 days
        
        public  HttpClient HttpClient { get; private set; }

        public WeatherTimeSeries WeatherTimeSeries { get; private set; }

        public WeatherAPIService()
        {
            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.Add("x-api-key", "WUcPrDqoG9SLAbx5QQcWGM");
        }

        public async Task<PointPostData> BuildPointPostDataAsync()
        {
            //Build list of variables we want from the API - see https://forecast-docs.metoceanapi.com/docs/#/variables?id=variables
            List<String> variables = new List<String>();
            variables.Add("air.temperature.at-2m");
            variables.Add("precipitation.rate");

            //Create the timeseries we want the data over - i.e. start from 1am today and get data every hour
            WeatherTimeSeries = new WeatherTimeSeries(TimeZoneInfo.ConvertTimeToUtc(DateTime.Now.Date.AddHours(1)),"1h", NumOfDataPoints);

            var wellingtonCoords = WeatherLocation.GetLocationCoordinates("Wellington");

            PointPostData pointPostData = new PointPostData(wellingtonCoords, variables, WeatherTimeSeries);

            return pointPostData;
        }

        [HttpPost]
        public async Task<PointResponseData> GetPointTimeSeriesAsync()
        {
            PointPostData pointPostData = await BuildPointPostDataAsync();

            var jsonSettings = new JsonSerializerSettings 
            { 
                DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ", 
                NullValueHandling = NullValueHandling.Ignore
            };

            var jsonData = JsonConvert.SerializeObject(pointPostData, jsonSettings);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            using (var response = await HttpClient.PostAsync(new Uri("https://forecast-v2.metoceanapi.com/point/time"), content))
            {
                if (response.IsSuccessStatusCode)
                {
                    //var result = response.Content.ReadAsStringAsync().Result;
                    var result = await response.Content.ReadAsStringAsync();

                    PointResponseData pointResponseData = JsonConvert.DeserializeObject<PointResponseData>(result);

                    return pointResponseData;
                }
                else
                {
                    throw new Exception(response.StatusCode.ToString() + " " + response.Content.ToString() + " " + response.RequestMessage.ToString());
                }
            }

        }
    }


}
