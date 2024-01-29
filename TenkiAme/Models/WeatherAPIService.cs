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
        public static HttpClient httpClient { get; private set; }
        private HttpContext? _context;



        public WeatherAPIService()
        {
            httpClient = new HttpClient();
            //httpClient.BaseAddress = new Uri("https://forecast-v2.metoceanapi.com/");
            //httpClient.DefaultRequestHeaders.Accept.Clear();
            //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("x-api-key", "WUcPrDqoG9SLAbx5QQcWGM");
        }
        [HttpPost]
        public async Task<PointResponseData> GetPointTimeSeries()
        {
            //Build list of variables we want from the API - see https://forecast-docs.metoceanapi.com/docs/#/variables?id=variables
            List<String> variables = new List<String>();
            variables.Add("air.temperature.at-2m");

            //Create the timeseries we want the data over
            WeatherTimeSeries weatherTimeSeries = new WeatherTimeSeries(DateTime.Now,"3h",3);

            PointPostData pointPostData = new PointPostData(-41.291, 174.777, variables, weatherTimeSeries);

            var jsonSettings = new JsonSerializerSettings { 
                                                            DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ", 
                                                            NullValueHandling = NullValueHandling.Ignore
                                                            };
            var jsonData = JsonConvert.SerializeObject(pointPostData, jsonSettings);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            using (var response = await httpClient.PostAsync(new Uri("https://forecast-v2.metoceanapi.com/point/time"), content))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    System.Diagnostics.Debug.WriteLine(result);

                    
                    PointResponseData pointResponseData = JsonConvert.DeserializeObject<PointResponseData>(result);

                    foreach (KeyValuePair<string, VariableDetails> weatherVar in pointResponseData.Variables)
                    {
                        System.Diagnostics.Debug.WriteLine("Variable: " + weatherVar.Key + ", Values: " + weatherVar.Value.Data[0]);
                    }

                    Util.PrintD(pointResponseData.Variables.GetValueOrDefault("air.temperature.at-2m").ToString());

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
