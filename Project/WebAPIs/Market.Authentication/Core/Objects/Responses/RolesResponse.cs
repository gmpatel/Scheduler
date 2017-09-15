using System.Collections.Generic;
using Insureme.Core.v1.Entities;
using Newtonsoft.Json;

namespace Insureme.Core.v1.Objects.Responses
{
    public class RolesResponse
    {
        [JsonProperty("roles", Order = 1, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IList<RoleEntity> Roles { get; set; }
    }
}