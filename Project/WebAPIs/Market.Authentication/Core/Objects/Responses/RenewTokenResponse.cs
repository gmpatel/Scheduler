using Insureme.Core.v1.Entities;
using Newtonsoft.Json;

namespace Insureme.Core.v1.Objects.Responses
{
    public class RenewTokenResponse
    {
        [JsonProperty("token", Order = 1, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Token { get; set; }
    }
}