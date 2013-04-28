namespace WeatherNotifierService
{
    public interface IMessageParser
    {
        string StartToken { get; set; }
        WeatherMessage Parse(string rawText);
    }
}