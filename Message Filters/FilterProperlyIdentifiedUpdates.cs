namespace WeatherNotifierService.Filters
{
    public class FilterProperlyIdentifiedUpdates : IFilter
    {
        public bool Matches(WeatherMessage message)
        {
            return message.IsUpdate;
        }
    }
}
