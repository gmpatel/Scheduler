using Insureme.Core.v1.Entities;
using Newtonsoft.Json;

namespace Insureme.Core.v1.Objects.Responses
{
    public class SingleUserResponse
    {
        [JsonProperty("user", Order = 1, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public UserEntity User { get; set; }

    }
}