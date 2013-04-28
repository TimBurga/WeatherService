using WeatherNotifierService;

namespace WeatherNotifierService.Filters
{
    public interface IFilter
    {
        /// <summary>
        /// Returns true if the message meets filtering criteria
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        bool Matches(WeatherMessage message);
    }
}
