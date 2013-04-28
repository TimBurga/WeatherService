namespace WeatherNotifierService
{
    public interface IWeatherMessageFactory
    {
        WeatherMessage Create(string rawText);
    }
}
