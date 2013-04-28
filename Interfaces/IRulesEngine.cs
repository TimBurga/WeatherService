namespace WeatherNotifierService
{
    public interface IRulesEngine
    {
        /// <summary>
        /// Returns true if the message passes all rules/filters
        /// </summary>
        /// <param name="message">Forecast message to examine</param>
        /// <returns>boolean</returns>
        bool ApplyRules(WeatherMessage message);
    }
}
