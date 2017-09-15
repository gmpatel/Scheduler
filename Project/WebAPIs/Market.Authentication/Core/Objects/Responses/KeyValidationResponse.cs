using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Insureme.Core.v1.Objects.Responses
{
    public class KeyValidationResponse
    {
        [JsonProperty("isKeyValid", Order = 1, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsKeyValid { get; set; }
    }
}