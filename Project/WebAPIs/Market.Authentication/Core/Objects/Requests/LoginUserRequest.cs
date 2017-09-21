using Newtonsoft.Json;

namespace Market.Authentication.Core.Objects.Requests
{
    public class LoginUserRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}