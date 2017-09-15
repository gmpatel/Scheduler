using Insureme.Core.v1.Entities;
using Newtonsoft.Json;

namespace Insureme.Core.v1.Objects.Responses
{
    public class EmailResponse
    {
        [JsonProperty("sent", Order = 1)]
        public bool Sent { get; set; }

        [JsonProperty("user", Order = 2, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public UserEntity User { get; set; }
    }
}