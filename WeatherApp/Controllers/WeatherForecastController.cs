using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.Annotations;

namespace WeatherApp.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class WeatherForecastController : ControllerBase
{
    private static List<WeatherForecast> _currentData = new();
    private static DateTime _lastUpdated { get; set; }
    private static DateTime _lastRequested { get; set; }
    // Minutes until next time data gets fetched
    private static int _untilNextUpdate = 0;
    private static Random _rand = new();
    private static HttpClient _httpClient = new();

    private readonly ILogger<WeatherForecastController> _logger;

    [ApiExplorerSettings(IgnoreApi = true)]
    [NonAction]
    public static void _fetchData(bool firstTime = false)
    {
        if (firstTime || _untilNextUpdate <= 0)
        {
            // According to the api documentation the time between requests should be randomized
            _untilNextUpdate = _rand.Next(50, 60);
            _lastRequested = DateTime.Now;
            const string sitename = "Weather demo website (https://github.com/lukz0)";
            const string url = "https://api.met.no/weatherapi/locationforecast/2.0/compact?lat=58.4618&lon=8.7724";

            string responseStr;
            HttpWebResponse? response;
            try
            {
                // WebRequest is deprecated, but allows for changing the User-Agent header
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
                request.UserAgent = sitename;
                // TODO: check if IfModifiedSince prevent the website from updating
                request.IfModifiedSince = _lastUpdated;
                response = (HttpWebResponse) request.GetResponse();
                Stream streamResponse = response.GetResponseStream();
                responseStr = new StreamReader(streamResponse).ReadToEnd();
            }
            catch (IOException)
            {
                responseStr = "";
                response = null;
            }
            catch (Exception e)
            {
                responseStr = "";
                response = null;
                // TODO: Is it possible to use the logger object without Hangfire creating a copy of the Controller?
                Console.Error.WriteLine("Error occured while fetching data: {0}", e);
            }

            if (response?.StatusCode == HttpStatusCode.NotModified)
            {
                return;
            } 
            
            string? lastModifiedStr = response?.Headers.Get("last_modified");
            if (lastModifiedStr is not null)
            {
                if (DateTime.TryParse(lastModifiedStr, out DateTime result))
                {
                    _lastUpdated = result;
                }
                else
                {
                    _lastUpdated = DateTime.Now;
                }
            }
            else
            {
                _lastUpdated = DateTime.Now;
            }
            // TODO: avoid re-fetching the data if current time is less than the last returned expires header
            string? expires = response?.Headers.Get("expires");

            if (response is not null && (int) response.StatusCode < 300 && (int) response.StatusCode >= 200)
            {
                WeatherForecast[] responseJson;
                try
                {
                    responseJson = JObject.Parse(responseStr)["properties"]["timeseries"].ToArray()
                        .Select(ts => (
                                ts["data"],
                                DateTime.Parse(ts["time"].ToObject<string>(), new CultureInfo("en-US"))
                            )
                        )
                        .Where(tuple => tuple.Item1["next_6_hours"]?["summary"]?["symbol_code"] is not null &&
                                        tuple.Item1["instant"]?["details"]?["air_temperature"] is not null)
                        .OrderBy(tuple => tuple.Item2)
                        .Select(tuple => new WeatherForecast()
                        {
                            Location = "Arendal",
                            Date = tuple.Item2,
                            TemperatureC = tuple.Item1["instant"]["details"]["air_temperature"].ToObject<double>(),
                            WeatherType = WeatherForecast.WeatherTypeFromString(
                                tuple.Item1["next_6_hours"]["summary"]["symbol_code"].ToObject<string>()
                                ),
                            WindFromDirection = tuple.Item1["instant"]["details"]["wind_from_direction"]
                                .ToObject<double?>(),
                            WindVelocity = tuple.Item1["instant"]["details"]["wind_speed"].ToObject<double?>(),
                            PrecipitationNext6Hours = tuple.Item1["next_6_hours"]["details"]?["precipitation_amount"]
                                ?.ToObject<double>()
                        }).ToArray();
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine("Failed to parse fetched data: {0}", e);
                    return;
                }

                lock (WeatherForecast.WeatherForecasts)
                {
                    WeatherForecast.WeatherForecasts.Clear();
                    WeatherForecast.WeatherForecasts.AddRange(responseJson);
                }
            }
            else
            {
                // TODO
            }
        }
        else
        {
            _untilNextUpdate--;
        }
    }

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IRecurringJobManager recurringJobMAnager)
    {
        _logger = logger;

        _logger.LogWarning("Controller constructed");
        _fetchData();
        recurringJobMAnager.AddOrUpdate("data_fetch", () => _fetchData(false), Hangfire.Cron.Minutely);
    }

    /// <summary>
    /// Get current and future weathers at a location
    /// </summary>
    /// <returns>A list of WeatherForecast</returns>
    [HttpGet(Name = "GetWeatherForecast")]
    [SwaggerResponse(200, "List of WeatherForecast returned", typeof(IEnumerable<WeatherForecast>))]
    public IEnumerable<WeatherForecast> Get()
    {
        WeatherForecast[] weatherForecasts;
        lock (WeatherForecast.WeatherForecasts)
        {
            weatherForecasts = WeatherForecast.WeatherForecasts.ToArray();
        }
        return weatherForecasts;
    }
}