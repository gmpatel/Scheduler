using System.Collections.Generic;
using Insureme.Core.v1.Entities;
using Newtonsoft.Json;

namespace Insureme.Core.v1.Objects.Responses
{
    public class AllUsersResponse
    {
        [JsonProperty("users", Order = 1, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IList<UserEntityTrimmed> Users { get; set; }
    }
}