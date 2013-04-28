namespace WeatherNotifierService
{
    public interface IDataCleaningStrategy
    {
        string Clean(string input);
    }
}
