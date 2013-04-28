using System;

namespace WeatherNotifierService
{
    public class WeatherMessage : IComparable<WeatherMessage>
    {
        public DateTime IssuedAt { get; set; }
        public string MessageType { get; set; }
        public string RegionCode { get; set; }
        public string OfficeCode { get; set; }
        public string IssuedAtGmt { get; set; }
        public string UpdateCode { get; set; }
        public bool IsDiscussion { get; set; }
        public bool IsUpdate { get; set; }
        public string MessageText { get; set; }
        public string HeaderText { get; set; }
        public string RawText { get; set; }

        public WeatherMessage(string rawText)
        {
            RawText = rawText;
        }

        public int CompareTo(WeatherMessage other)
        {
            // descending sort by date (newest message first)
            return -IssuedAt.CompareTo(other.IssuedAt);
        }

        public override bool Equals(object other)
        {
            if (other == null) return false;

            var otherMsg = other as WeatherMessage;
            if (otherMsg == null) throw new ArgumentException();

            return IssuedAt == otherMsg.IssuedAt;
        }

        public override int GetHashCode()
        {
            return IssuedAt.GetHashCode();
        }
    }
}
