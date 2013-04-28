namespace WeatherNotifierService.Filters
{
    public class FilterUnidentifiedUpdates : IFilter
    {
        public bool Matches(WeatherMessage message)
        {
            return (message.MessageText.Substring(0, 20).IndexOf("UPDATE") > -1);
        }
    }
}
