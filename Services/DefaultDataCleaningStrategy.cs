using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace WeatherNotifierService
{
    public class DefaultDataCleaningStrategy : IDataCleaningStrategy
    {
        public List<Action<StringBuilder>> Operations;

        public DefaultDataCleaningStrategy()
        {
            Operations = new List<Action<StringBuilder>>
                             {
                                 _highlightSectionTitles,
                                 _ellipsesToCommas,
                                 //_removeCarriageReturns,
                                 _removeEndOfMessageCharacters
                             };
        }

        public string Clean(string input)
        {
            if (String.IsNullOrEmpty(input))
                return String.Empty;

            var beginPos = input.IndexOf(@"<pre>");
            var length = input.LastIndexOf(@"</pre>") - beginPos + 6;
            
            var sb = new StringBuilder(250000);
            sb.Append(input.Substring(beginPos, length).Replace("\n", Environment.NewLine));

            Operations.ForEach(op => op(sb));

            return sb.ToString();
        }

        private readonly Action<StringBuilder> _highlightSectionTitles =
            text =>
                {
                    var titles = new Regex(@"^\.{1}.+\.{3}[ /\w/$]*",
                                           RegexOptions.Multiline);

                    foreach (Match x in titles.Matches(text.ToString()))
                    {
                        var captured = x.Value;
                        var cleaned = "=== " +
                                      captured.Replace(".", String.Empty) +
                                      " === ";

                        text.Replace(captured, cleaned);
                    }
                };

        private readonly Action<StringBuilder> _ellipsesToCommas =
            text =>
                {
                    var commas = new Regex(@"\w?[\.]{3}\w?");
                    foreach (Match x in commas.Matches(text.ToString())
                        )
                    {
                        var captured = x.Value;
                        var cleaned = captured.Replace("...", ", ");

                        text.Replace(captured, cleaned);
                    }
                };

        private readonly Action<StringBuilder> _removeEndOfMessageCharacters = text => text.Replace("&&", String.Empty);

        //private readonly Action<StringBuilder> _removeCarriageReturns =
        //    text =>
        //        {
        //            var fixedWidthCrlfs = new Regex(@"^.{25,},*\s*[\r\n]{1}\s*\w+",
        //                                            RegexOptions.Multiline);
        //            var temp = fixedWidthCrlfs.Matches(text.ToString());
        //            foreach (Match x in temp)
        //            {
        //                var captured = x.Value;
        //                var cleaned = captured.Replace("\r\n", " ");

        //                text.Replace(captured, cleaned);
        //            }
        //        };
    }
}