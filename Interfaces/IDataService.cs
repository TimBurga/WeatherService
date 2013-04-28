using System;

namespace WeatherNotifierService
{
    public interface IDataService
    {
        string RetrieveData();
        string RetrieveWebsite(Uri uri);
    }
}
