using Insureme.Core.v1.Entities;
using Newtonsoft.Json;

namespace Insureme.Core.v1.Objects.Responses
{
    public class LoginUserResponse
    {
        [JsonProperty("user", Order = 1, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public UserEntity User { get; set; }

        [JsonProperty("token", Order = 2, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Token { get; set; }
    }
}