using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace WeatherNotifierService.Services
{
    public class WeatherService : IWeatherService
    {
        public event Action<WeatherUpdateEventArgs> WeatherUpdated;

        protected static readonly ILog Logger = LogManager.GetLogger(typeof(WeatherService));
        protected const int MaxMessagesToInclude = 5;
        protected const string MessageSeparatorToken = "$$";
        protected List<WeatherMessage> MessageCollection = new List<WeatherMessage>();
        protected WeatherMessage PreviousLatestMessage;
        protected WeatherMessage LastNotifiedMessage;
        protected IDataService DataService;
        protected IWeatherMessageFactory MessageBuilder;

        public WeatherService(IDataService dataService, IWeatherMessageFactory messageFactory)
        {
            DataService = dataService;
            MessageBuilder = messageFactory;
        }

        public string CurrentForecastDiscussionText
        {
            get
            {
                var counter = 0;
                var builder = new StringBuilder();

                foreach (var message in MessageCollection)
                {
                    builder.Append(message.HeaderText);
                    builder.Append(Environment.NewLine);
                    builder.Append(Environment.NewLine);
                    builder.Append(message.MessageText);
                    builder.Append(Environment.NewLine);
                    builder.Append(Environment.NewLine);
                    builder.Append(MessageSeparatorToken);
                    builder.Append(Environment.NewLine);
                    builder.Append(Environment.NewLine);
                    builder.Append(Environment.NewLine);

                    if (++counter >= MaxMessagesToInclude)
                        break;
                }

                return builder.ToString();
            }
        }

        public void Poll()
        {
            Task.Factory.StartNew(AsyncPoll);
        }


        protected List<string> ParseForecastItems(string forecast)
        {
            var updatesArray = forecast.Split(new[] {MessageSeparatorToken}, StringSplitOptions.RemoveEmptyEntries);
            return new List<string>(updatesArray);
        }

        protected void RebuildMessageCollection(string forecast)
        {
            var weatherUpdates = ParseForecastItems(forecast);

            MessageCollection.Clear();
            weatherUpdates.ForEach(text =>
                                       {
                                           var message = MessageBuilder.Create(text);

                                           if (message != null)
                                               MessageCollection.Add(message);
                                       });
            MessageCollection.Sort();
        }

        protected void NotifyNewMessage(WeatherMessage message)
        {
            if (WeatherUpdated != null)
            {
                var args = new WeatherUpdateEventArgs(MessageCollection, 
                                                      message.IsDiscussion,
                                                      !MessageCollection[0].Equals(PreviousLatestMessage));
                WeatherUpdated(args);
            }

            LastNotifiedMessage = message;
        }

        protected void AsyncPoll()
        {
            string forecast;

            try
            {
                forecast = DataService.RetrieveData();
            }
            catch (Exception ex)
            {
                Logger.Warn("NWS data could not be retrieved", ex);
                return;
            }

            RebuildMessageCollection(forecast);

            if (!MessageCollection[0].Equals(PreviousLatestMessage)) 
                PreviousLatestMessage = MessageCollection[0];

            NotifyNewMessage(MessageCollection[0]);
        }
    }
}
