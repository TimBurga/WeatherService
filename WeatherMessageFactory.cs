namespace WeatherNotifierService
{
    public class WeatherMessageFactory : IWeatherMessageFactory
    {
        private readonly IRulesEngine _rulesEngine;
        private readonly IMessageParser _messageParser;

        public WeatherMessageFactory(IRulesEngine rulesEngine, IMessageParser messageParser)
        {
            _rulesEngine = rulesEngine;
            _messageParser = messageParser;
        }

        public WeatherMessage Create(string rawText)
        {
            var message = _messageParser.Parse(rawText);
            if (message != null)
            {
                message.IsDiscussion = _rulesEngine.ApplyRules(message);
            }

            return message;
        }
    }

}
