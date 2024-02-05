using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
//using System.Text.Json;
using Newtonsoft.Json;
using TenkiAme.DataTransferObjects;
using TenkiAme.wwwroot;

namespace TenkiAme.Models
{
    public class WeatherAPIService
    {
        public  HttpClient HttpClient { get; private set; }

        public WeatherTimeSeries WeatherTimeSeries { get; private set; }

        public WeatherAPIService()
        {
            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.Add("x-api-key", "WUcPrDqoG9SLAbx5QQcWGM");
        }

        [HttpPost]
        public async Task<PointResponseData> GetPointTimeSeries()
        {
            //Build list of variables we want from the API - see https://forecast-docs.metoceanapi.com/docs/#/variables?id=variables
            List<String> variables = new List<String>();
            variables.Add("air.temperature.at-2m");
            variables.Add("precipitation.rate");

            //Create the timeseries we want the data over
            WeatherTimeSeries = new WeatherTimeSeries(DateTime.Now,"3h",47); //TODO Replace hard-coded values with declared constants

            PointPostData pointPostData = new PointPostData(-41.291, 174.777, variables, WeatherTimeSeries);

            var jsonSettings = new JsonSerializerSettings { 
                                                            DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ", 
                                                            NullValueHandling = NullValueHandling.Ignore
                                                            };
            var jsonData = JsonConvert.SerializeObject(pointPostData, jsonSettings);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            using (var response = await HttpClient.PostAsync(new Uri("https://forecast-v2.metoceanapi.com/point/time"), content))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
       
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
