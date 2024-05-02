using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;


public class WeatherData
{
    public double FeelsLike { get; set; }
    public double Humidity { get; set; }
    public long Sunrise { get; set; }
    public double Temp { get; set; }
    public int WindDegrees { get; set; }
    public double WindSpeed { get; set; }
}


namespace CRUD_API.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Weather Page";
            ViewBag.Message = "This is THe page to display weather from backend";

                return View();
        }


        [HttpPost]
        public async Task<ActionResult> SearchCity(string city)
        {
            try { 

            string host = "weather-by-api-ninjas.p.rapidapi.com";
            string key = "a525bf3d46msh414b202c73c47fep1a595bjsn237e6d934344"; // Replace with your actual API key
            string apiUrl = $"https://{host}/v1/weather?city={city}";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-RapidAPI-Key", key);
                client.DefaultRequestHeaders.Add("X-RapidAPI-Host", host);

                HttpResponseMessage response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                WeatherData weatherData = JsonConvert.DeserializeObject<WeatherData>(responseBody);

                // Process the weather data as needed
                ViewBag.Temperature = weatherData.Temp;
                ViewBag.FeelsLike = weatherData.FeelsLike;
                ViewBag.Humidity = weatherData.Humidity;
                ViewBag.Sunrise = weatherData.Sunrise;
                ViewBag.WindDegrees = weatherData.WindDegrees;
                ViewBag.WindSpeed = weatherData.WindSpeed;
                ViewBag.Message = "Successfully Got Data from the Weather API!";
            }
        }
        catch (HttpRequestException ex)
        {
            // Log or handle the exception
            ViewBag.Error = ex.Message;
        }

            return View("Index");
        }
    }
}
