using Insureme.Core.v1.Entities;
using Newtonsoft.Json;

namespace Insureme.Core.v1.Objects.Responses
{
    public class VerifyUserResponse
    {
        [JsonProperty("attemptToVerify", Order = 1)]
        public bool AttemptToVerify { get; set; }

        [JsonProperty("user", Order = 2, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public UserEntity User { get; set; }
    }
}