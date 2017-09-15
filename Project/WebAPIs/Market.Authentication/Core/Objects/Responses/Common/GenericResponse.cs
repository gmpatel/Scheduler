using Newtonsoft.Json;

namespace Market.Authentication.Core.Objects.Responses.Common
{
    public class GenericResponse<T>
    {
        [JsonProperty(PropertyName = "error", Order = 1, DefaultValueHandling = DefaultValueHandling.Include)]
        public Error Error { get; set; }

        [JsonProperty(PropertyName = "result", Order = 2, DefaultValueHandling = DefaultValueHandling.Include)]
        public T Result { get; set; }
    }
}