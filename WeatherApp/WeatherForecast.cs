namespace WeatherApp;

public class WeatherForecast
{
    public string? WeatherType { get; set; }
    
    public DateTime Date { get; set; }
    public double TemperatureC { get; set; }

    public string? Location { get; set; }
    
    // Direction the wind is coming from in degrees
    public double? WindFromDirection { get; set; }
    
    // Velocity of the wind in m/s
    public double? WindVelocity { get; set; }
    
    // mm of precipitation in the next 6 hours
    public double? PrecipitationNext6Hours { get; set; }

    // In-memory cache for result from yr, use lock before usage, the data should always be ordered by date
    public static List<WeatherForecast> WeatherForecasts = new();

    public static string? WeatherTypeFromString(string? str)
    {
        // Check is a weather type string is one of the supported weathers
        switch (str)
        {
            case "clearsky_day":
            case "clearsky_night":
            case "clearsky_polartwilight":
            case "cloudy":
            case "fair_day":
            case "fair_night":
            case "fair_polartwilight":
            case "fog":
            case "heavyrain":
            case "heavyrainandthunder":
            case "heavyrainshowersandthunder_day":
            case "heavyrainshowersandthunder_night":
            case "heavyrainshowersandthunder_polartwilight":
            case "heavyrainshowers_day":
            case "heavyrainshowers_night":
            case "heavyrainshowers_polartwilight":
            case "heavysleet":
            case "heavysleetandthunder":
            case "heavysleetshowersandthunder_day":
            case "heavysleetshowersandthunder_night":
            case "heavysleetshowersandthunder_polartwilight":
            case "heavysnow":
            case "heavysnowandthunder":
            case "heavysnowshowers_day":
            case "heavysnowshowers_night":
            case "heavysnowshowers_polartwilight":
            case "heavysnowshowersandthunder_day":
            case "heavysnowshowersandthunder_night":
            case "heavysnowshowersandthunder_polartwilight":
            case "lightrain":
            case "lightrainandthunder":
            case "lightrainshowersandthunder_day":
            case "lightrainshowersandthunder_night":
            case "lightrainshowersandthunder_polartwilight":
            case "lightrainshowers_day":
            case "lightrainshowers_night":
            case "lightrainshowers_polartwilight":
            case "lightsleet":
            case "lightsleetandthunder":
            case "lightsleetshowersandthunder_day":
            case "lightsleetshowersandthunder_night":
            case "lightsleetshowersandthunder_polartwilight":
            case "lightsnow":
            case "lightsnowandthunder":
            case "lightsnowshowers_day":
            case "lightsnowshowers_night":
            case "lightsnowshowers_polartwilight":
            case "lightsnowshowersandthunder_day":
            case "lightsnowshowersandthunder_night":
            case "lightsnowshowersandthunder_polartwilight":
            case "partlycloudy_day":
            case "partlycloudy_night":
            case "partlycloudy_polartwilight":
            case "rain":
            case "rainandthunder":
            case "rainshowers_day":
            case "rainshowers_night":
            case "rainshowers_polartwilight":
            case "sleet":
            case "sleetandthunder":
            case "sleetshowers_day":
            case "sleetshowers_night":
            case "sleetshowers_polartwilight":
            case "sleetshowersandthunder_day":
            case "sleetshowersandthunder_night":
            case "sleetshowersandthunder_polartwilight":
            case "snow":
            case "snowandthunder":
            case "snowshowers_day":
            case "snowshowers_night":
            case "snowshowers_polartwilight":
            case "snowshowersandthunder_day":
            case "snowshowersandthunder_night":
            case "snowshowersandthunder_polartwilight":
                return str;
            default:
                return null;
        }
    }
}