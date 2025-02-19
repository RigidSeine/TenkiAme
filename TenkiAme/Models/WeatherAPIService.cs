using Microsoft.AspNetCore.Mvc;
using System.Text;
using Newtonsoft.Json;
using DotNetEnv;
using TenkiAme.DataTransferObjects;
using TenkiAme.Data;
using TenkiAme.UtilityObjects;


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
            //Get the API key
            string APIKey = Environment.GetEnvironmentVariable("Tenkiame_Metservice_API_Key")!;

            //Set the API key
            HttpClient.DefaultRequestHeaders.Add("x-api-key", APIKey);

            //Build the request data
            PointPostData pointPostData = await BuildPointPostDataAsync();

            //Set the JSON settings of the request
            var jsonSettings = new JsonSerializerSettings 
            { 
                DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ", 
                NullValueHandling = NullValueHandling.Ignore
            };

            //Convert the request data to JSON
            var jsonData = JsonConvert.SerializeObject(pointPostData, jsonSettings);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");


            //Send the request to the API
            using (var response = await HttpClient.PostAsync(new Uri("https://forecast-v2.metoceanapi.com/point/time"), content))
            {
                if (response.IsSuccessStatusCode)
                {
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

        [HttpGet]
        public async Task<SunriseSunsetResponse> GetSunriseSunset(string location = "Wellington")
        {
            
            SunriseSunsetResponse sunriseSunsetResponse = new SunriseSunsetResponse();
            
            //Set the API key to nothing
            HttpClient.DefaultRequestHeaders.Add("x-api-key", "");

            //Get the coordinates of Wellington
            var locCoords = WeatherLocation.GetLocationCoordinates(location);

            //Set the JSON settings of the request
            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            int numOfDaysRetrieved = 2;

            //Send the request to the API using the provided location and today and tomorrow
            using (var response = await HttpClient.GetAsync(new Uri("https://api.sunrisesunset.io/json?lat=" + locCoords.Lat 
                                                                    + "&lng=" + locCoords.Lon 
                                                                    + "&date_start=" + DateTime.Today.ToString("yyyy-MM-dd") 
                                                                    + "&date_end=" + DateTime.Today.AddDays(numOfDaysRetrieved).ToString("yyyy-MM-dd")
                                                                    )
                                                            )
            )
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();

                    sunriseSunsetResponse = JsonConvert.DeserializeObject<SunriseSunsetResponse>(result);

                    return sunriseSunsetResponse;
                }
                else
                {
                    throw new Exception(response.StatusCode.ToString() + " " + response.Content.ToString() + " " + response.RequestMessage.ToString());
                }
            }
        }

        [HttpGet]
        public async Task<UVResponse> GetUVDataFromNiwa(string location = "Wellington")
        {
            //Create an object to deserialise the response data into
            UVResponse uVResponseData = new UVResponse();

            //Empty the API key header
            HttpClient.DefaultRequestHeaders.Add("x-api-key", "");

            var locCoords = WeatherLocation.GetLocationCoordinates(location);

            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            //Get the API key
            string APIKey = Environment.GetEnvironmentVariable("Tenkiame_Niwa_API_Key")!;

            var url = new Uri("https://api.niwa.co.nz/uv/data?lat=" + locCoords.Lat + "&long=" + locCoords.Lon + "&apikey=" + APIKey);

            using (var response = await HttpClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();

                    DevUtil.PrintD(result.ToString());

                    uVResponseData = JsonConvert.DeserializeObject<UVResponse>(result);

                    return uVResponseData;
                }
                else
                {
                    throw new Exception(response.StatusCode.ToString() + " " + response.Content.ToString());
                }
            }

        }
    }


}
