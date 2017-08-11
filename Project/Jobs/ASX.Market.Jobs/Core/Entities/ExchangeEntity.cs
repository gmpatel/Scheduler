using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ASX.Market.Jobs.Core.Entities
{
    public class ExchangeEntity
    {
        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; }

        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonIgnore]
        public DateTime DateTimeCreated { get; set; }

        [JsonIgnore]
        public DateTime? DateTimeLastModified { get; set; }

        [JsonIgnore]
        public virtual ICollection<IndexEntity> Indices { get; set; }
    }
}