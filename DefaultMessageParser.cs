using System;
using System.Text.RegularExpressions;

namespace WeatherNotifierService
{
    public class DefaultMessageParser : IMessageParser
    {
        private static Regex _firstHeaderLineRegex;
        private static Regex _lastHeaderLineRegex;

        public DefaultMessageParser()
        {
            StartToken = "000";
            _firstHeaderLineRegex = new Regex(@"[A-Z]{4}\d{2} K[A-Z]{3} \d{6} {0,1}[A-Z]{0,3}"); //FXUS64 KFWD 211745 AAC
            _lastHeaderLineRegex = new Regex(@"\d{3,4} [A|P]M C[S|D]T [A-Z]{3} [A-Z]{3} \d{1,2} \d{4}"); //1105 AM CST SUN FEB 21 2010
        }

        public string StartToken { get; set; }

        public WeatherMessage Parse(string rawText)
        {
            var startPosition = rawText.IndexOf(StartToken, System.StringComparison.Ordinal);
            if (startPosition == -1 || rawText.Length <= startPosition)
                return null;

            var strippedMessage = rawText.Substring(startPosition, rawText.Length - startPosition);

            return ParseStrippedMessage(strippedMessage);
        }

        private WeatherMessage ParseStrippedMessage(string rawText)
        {
            var message = new WeatherMessage(rawText);

            var firstHeaderLineMatch = _firstHeaderLineRegex.Match(rawText.Substring(0, 100));
            var lastHeaderLineMatch = _lastHeaderLineRegex.Match(rawText);

            if (!(firstHeaderLineMatch.Success && lastHeaderLineMatch.Success))
                throw new ApplicationException("Unable to parse message header");

            var firstLineEnd = firstHeaderLineMatch.Index + firstHeaderLineMatch.Length;
            var lastLineEnds = lastHeaderLineMatch.Index + lastHeaderLineMatch.Length;

            // Parse the first line
            var vals = firstHeaderLineMatch.Value.Split(' ');
            message.RegionCode = vals[0];
            message.OfficeCode = vals[1];
            message.IssuedAtGmt = vals[2].Substring(2, 4);

            if (vals.Length > 3)
            {
                message.UpdateCode = vals[3];
                message.IsUpdate = true;
            }

            // Parse the second line for message type
            if (firstLineEnd > 0)
                message.MessageType = rawText.Substring(firstLineEnd, 8).Trim(); // Message type (ex: AFDFWD)

            // Parse third line for issued datetime
            message.IssuedAt = ParseLastLineToDate(lastHeaderLineMatch.Value);

            // Store raw header and message text
            message.HeaderText = rawText.Substring(firstHeaderLineMatch.Index, lastLineEnds).Trim();
            message.MessageText = rawText.Substring(lastLineEnds).Trim();

            return message;
        }

        private DateTime ParseLastLineToDate(string text)
        {
            DateTime dt;
            int hour, minute;

            var parts = text.Split(' ');
            var startPosition = parts[0].Length - 2;

            Int32.TryParse(parts[0].Substring(0, startPosition), out hour);
            Int32.TryParse(parts[0].Substring(startPosition, 2), out minute);
            DateTime.TryParse(String.Format("{0} {1} {2} {3}:{4} {5}",
                                            parts[4], parts[5], parts[6], hour, minute, parts[1]),
                              out dt);

            return dt;
        }
    }
}