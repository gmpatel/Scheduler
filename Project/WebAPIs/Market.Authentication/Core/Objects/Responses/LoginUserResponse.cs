using Market.Authentication.Core.Entities;
using Newtonsoft.Json;

namespace Market.Authentication.Core.Objects.Responses
{
    public class LoginUserResponse
    {
        [JsonProperty("user", Order = 1, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public UserEntity User { get; set; }

        [JsonProperty("token", Order = 2, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public TokenEntity Token { get; set; }
    }
}