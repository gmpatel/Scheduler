using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ASX.Market.Jobs.Core.Entities
{
    public class IndexEntity
    {
        [JsonProperty(PropertyName = "id", Order = 1, DefaultValueHandling = DefaultValueHandling.Include)]
        public long Id { get; set; }

        [JsonProperty(PropertyName = "code", Order = 2, DefaultValueHandling = DefaultValueHandling.Include)]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "name", Order = 3, DefaultValueHandling = DefaultValueHandling.Include)]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "url", Order = 4, DefaultValueHandling = DefaultValueHandling.Include)]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "exchangeId", Order = 5, DefaultValueHandling = DefaultValueHandling.Include)]
        public long ExchangeId { get; set; }

        [JsonIgnore]
        public DateTime DateTimeCreated { get; set; }

        [JsonIgnore]
        public DateTime? DateTimeLastModified { get; set; }

        [JsonIgnore]
        public virtual ExchangeEntity Exchange { get; set; }

        [JsonIgnore]
        public virtual ICollection<StockEntity> Stocks { get; set; }
    }
}