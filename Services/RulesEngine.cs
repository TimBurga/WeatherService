using System.Collections.Generic;
using System.Linq;
using WeatherNotifierService.Filters;

namespace WeatherNotifierService
{
    public class RulesEngine : IRulesEngine
    {
        protected List<IFilter> Filters = new List<IFilter>();

        public RulesEngine()
        {
            InitializeRuleSet();
        }

        public bool ApplyRules(WeatherMessage message)
        {
            return !Filters.Any((rule => rule.Matches(message)));
        }

        protected void InitializeRuleSet()
        {
            Filters.Add(new FilterProperlyIdentifiedUpdates());
            Filters.Add(new FilterAviationUpdates());
            Filters.Add(new FilterUnidentifiedUpdates());
        }
    }

}
