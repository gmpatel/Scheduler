using System.Collections.Generic;
using Newtonsoft.Json;

namespace Market.Authentication.Core.Objects.Requests
{
    public class EmailRequest
    {
        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("toEmails")]
        public IList<string> ToEmails { get; set; }
    }
}