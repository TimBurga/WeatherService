using System;
using System.Net;
using System.IO;

namespace WeatherNotifierService.Services
{
    public class WebDataService : IDataService
    {
        protected Uri Uri;
        protected IDataCleaningStrategy CleaningStrategy;

        public WebDataService(IDataCleaningStrategy cleaningStrategy, Uri location)
        {
            CleaningStrategy = cleaningStrategy;
            Uri = location;
        }

        public string RetrieveData()
        {
            return RetrieveWebsite(Uri);
        }

        public string RetrieveWebsite(Uri uri)
        {
            var result = String.Empty;
            var request = WebRequest.Create(uri);

            using (var response = request.GetResponse())
            {
                if (response == null) return result;
                using (var stream = response.GetResponseStream())
                {
                    if (stream == null) return result;
                    using (var reader = new StreamReader(stream))
                        result = reader.ReadToEnd();
                }
            }

            return CleaningStrategy.Clean(result);
        }
    }
}
