using System.Collections.Generic;
using Insureme.Core.v1.Entities;
using Newtonsoft.Json;

namespace Insureme.Core.v1.Objects.Responses
{
    public class IncomeRangesResponse
    {
        [JsonProperty("incomeRanges", Order = 1, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IList<IncomeRangeEntity> IncomeRanges { get; set; }
    }
}