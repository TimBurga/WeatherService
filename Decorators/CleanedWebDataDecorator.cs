using System;

namespace WeatherNotifierService
{
    public class CleanedWebDataDecorator : IDataService
    {
        private readonly IDataService _baseService;
        private readonly IDataCleaningStrategy _dataCleaningStrategy;

        public CleanedWebDataDecorator(IDataService toBeDecorated, IDataCleaningStrategy dataCleaningStrategy)
        {
            _baseService = toBeDecorated;
            _dataCleaningStrategy = dataCleaningStrategy;
        }

        public string RetrieveData()
        {
            return Clean(_baseService.RetrieveData());
        }

        public string RetrieveWebsite(Uri uri)
        {
            return Clean(_baseService.RetrieveWebsite(uri));
        }

        protected string Clean(string input)
        {
            return _dataCleaningStrategy.Clean(input);
        }
    }
}
