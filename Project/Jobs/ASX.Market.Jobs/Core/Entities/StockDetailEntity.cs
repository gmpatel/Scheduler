using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ASX.Market.Jobs.Core.Entities
{
    public class StockDetailEntity
    {
        [JsonProperty(PropertyName = "id", Order = 1, DefaultValueHandling = DefaultValueHandling.Include)]
        public long Id { get; set; }

        [JsonProperty(PropertyName = "date", Order = 2, DefaultValueHandling = DefaultValueHandling.Include)]
        public long Date { get; set; }

        [JsonProperty(PropertyName = "stockId", Order = 3, DefaultValueHandling = DefaultValueHandling.Include)]
        public long StockId { get; set; }

        [JsonProperty(PropertyName = "price", Order = 5, DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal? Price { get; set; }

        [JsonProperty(PropertyName = "change", Order = 6, DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal? Change { get; set; }

        [JsonProperty(PropertyName = "changePercent", Order = 7, DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal? ChangePercent { get; set; }

        [JsonProperty(PropertyName = "high", Order = 8, DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal? High { get; set; }

        [JsonProperty(PropertyName = "low", Order = 9, DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal? Low { get; set; }

        [JsonProperty(PropertyName = "volume", Order = 10, DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal? Volume { get; set; }

        [JsonProperty(PropertyName = "marketCapital", Order = 11, DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal? MarketCapital { get; set; }

        [JsonProperty(PropertyName = "oneYearChange", Order = 12, DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal? OneYearChange { get; set; }

        [JsonProperty(PropertyName = "flag1", Order = 4, DefaultValueHandling = DefaultValueHandling.Include)]
        public bool Flag1 { get; set; }

        [JsonIgnore]
        public DateTime DateTimeCreated { get; set; }

        [JsonIgnore]
        public DateTime? DateTimeLastModified { get; set; }

        [JsonIgnore]
        public virtual StockEntity Stock { get; set; }
    }
}