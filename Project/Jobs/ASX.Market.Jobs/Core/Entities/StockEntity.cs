using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ASX.Market.Jobs.Core.Entities
{
    public class StockEntity
    {
        [JsonProperty(PropertyName = "id", Order = 1, DefaultValueHandling = DefaultValueHandling.Include)]
        public long Id { get; set; }

        [JsonProperty(PropertyName = "code", Order = 2, DefaultValueHandling = DefaultValueHandling.Include)]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "name", Order = 3, DefaultValueHandling = DefaultValueHandling.Include)]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "flag1", Order = 4, DefaultValueHandling = DefaultValueHandling.Include)]
        public bool Flag1 { get; set; }

        [JsonIgnore]
        public DateTime DateTimeCreated { get; set; }

        [JsonIgnore]
        public DateTime? DateTimeLastModified { get; set; }

        [JsonIgnore]
        public virtual ICollection<IndexEntity> Indices { get; set; }

        [JsonIgnore]
        public virtual ICollection<StockDetailEntity> StockDetails { get; set; }

        [JsonIgnore]
        public virtual ICollection<StockDetailAggregatedEntity> StockDetailAggregated { get; set; }
    }
}
