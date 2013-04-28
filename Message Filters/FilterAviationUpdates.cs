namespace WeatherNotifierService.Filters
{
    public class FilterAviationUpdates : IFilter
    {
        public bool Matches(WeatherMessage message)
        {
            return (message.MessageText.Substring(0, 20).IndexOf("AVIATION") > -1);
        }
    }
}
