using System.Net;
using Newtonsoft.Json;

namespace Market.Authentication.Core.Objects.Responses.Common
{
    public class Error
    {
        [JsonProperty("response", Order = 1, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Response => string.Format("{0} ({1})", ResponseCode, (int)ResponseCode);

        [JsonProperty("responseCode", Order = 2, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public HttpStatusCode ResponseCode { get; set; }

        [JsonProperty("code", Order = 3, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Code { get; set; }

        [JsonProperty("message", Order = 4, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Message { get; set; }
    }
}