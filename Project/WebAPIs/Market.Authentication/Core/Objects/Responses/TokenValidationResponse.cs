using Newtonsoft.Json;

namespace Insureme.Core.v1.Objects.Responses
{
    public class TokenValidationResponse
    {
        [JsonProperty("isTokenValid", Order = 1, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsTokenValid { get; set; }
    }
}