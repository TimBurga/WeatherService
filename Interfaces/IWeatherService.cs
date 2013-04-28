using System;

namespace WeatherNotifierService
{
    public interface IWeatherService
    {
        string CurrentForecastDiscussionText { get; }
        void Poll();
        event Action<WeatherUpdateEventArgs> WeatherUpdated;
    }
}
