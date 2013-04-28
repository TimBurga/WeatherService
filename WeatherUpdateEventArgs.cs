using System;
using System.Collections.Generic;

namespace WeatherNotifierService
{
    public class WeatherUpdateEventArgs : EventArgs
    {
        protected List<WeatherMessage> Messages;

        public WeatherMessage LatestDiscussionMessage { get; protected set; }
        public bool ContainsNewDiscussion { get; protected set; }
        public bool ContainsUpdate { get; protected set; }

        public IEnumerable<WeatherMessage> WeatherMessages
        {
            get { return Messages.AsReadOnly(); }
        }

        public WeatherUpdateEventArgs(List<WeatherMessage> messages, bool hasNewDiscussion, bool containsUpdate)
        {
            Messages = messages;
            ContainsNewDiscussion = hasNewDiscussion;
            ContainsUpdate = containsUpdate;
            LatestDiscussionMessage = Messages.Find(message => message.IsDiscussion);
        }
    }
}
