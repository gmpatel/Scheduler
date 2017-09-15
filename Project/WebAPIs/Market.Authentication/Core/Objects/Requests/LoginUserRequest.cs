using Newtonsoft.Json;

namespace Insureme.Core.v1.Objects.Requests
{
    public class LoginUserRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}