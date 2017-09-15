using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Market.Authentication.Core.Entities
{
    public class TokenEntity
    {
        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; }

        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }

        [JsonIgnore]
        public long UserId { get; set; }

        [JsonIgnore]
        public DateTime DateTimeCreated { get; set; }

        [JsonIgnore]
        public DateTime DateTimeExpire { get; set; }

        [JsonIgnore]
        public virtual UserEntity User { get; set; }
    }
}