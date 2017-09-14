using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ASX.Market.Jobs.Core.Entities
{
    public class StockDetailAggregatedLatestMovementEntity
    {
        [JsonProperty(PropertyName = "id", Order = 1, DefaultValueHandling = DefaultValueHandling.Include)]
        public long Id { get; set; }

        [JsonProperty(PropertyName = "stockId", Order = 2, DefaultValueHandling = DefaultValueHandling.Include)]
        public long StockId { get; set; }

        [JsonProperty(PropertyName = "movementDays", Order = 4, DefaultValueHandling = DefaultValueHandling.Include)]
        public int MovementDays { get; set; }

        [JsonProperty(PropertyName = "movementDirection", Order = 5, DefaultValueHandling = DefaultValueHandling.Include)]
        public string MovementDirection { get; set; }

        [JsonProperty(PropertyName = "startDate", Order = 6, DefaultValueHandling = DefaultValueHandling.Include)]
        public long StartDate { get; set; }

        [JsonProperty(PropertyName = "startDay", Order = 7, DefaultValueHandling = DefaultValueHandling.Include)]
        public string StartDay { get; set; }

        [JsonProperty(PropertyName = "startPrice", Order = 8, DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal? StartPrice { get; set; }

        [JsonProperty(PropertyName = "startIndicator", Order = 9, DefaultValueHandling = DefaultValueHandling.Include)]
        public bool StartIndicator { get; set; }

        [JsonProperty(PropertyName = "endDate", Order = 10, DefaultValueHandling = DefaultValueHandling.Include)]
        public long EndDate { get; set; }

        [JsonProperty(PropertyName = "endDay", Order = 11, DefaultValueHandling = DefaultValueHandling.Include)]
        public string EndDay { get; set; }

        [JsonProperty(PropertyName = "endPrice", Order = 12, DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal? EndPrice { get; set; }

        [JsonProperty(PropertyName = "startIndicator", Order = 13, DefaultValueHandling = DefaultValueHandling.Include)]
        public bool EndIndicator { get; set; }

        [JsonProperty(PropertyName = "changed", Order = 14, DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal? Changed { get; set; }

        [JsonProperty(PropertyName = "changedPercent", Order = 15, DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal? ChangedPercent { get; set; }

        [JsonProperty(PropertyName = "overallChanged", Order = 16, DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal? OverallChanged { get; set; }

        [JsonProperty(PropertyName = "overallChangedPercent", Order = 17, DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal? OverallChangedPercent { get; set; }

        [JsonProperty(PropertyName = "maxPrice", Order = 18, DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal? MaxPrice { get; set; }

        [JsonProperty(PropertyName = "minPrice", Order = 19, DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal? MinPrice { get; set; }

        [JsonProperty(PropertyName = "overallMaxPrice", Order = 20, DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal? OverallMaxPrice { get; set; }

        [JsonProperty(PropertyName = "overallMinPrice", Order = 21, DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal? OverallMinPrice { get; set; }

        [JsonIgnore]
        public DateTime? DateTimeCreated { get; set; }

        [JsonIgnore]
        public DateTime? DateTimeLastModified { get; set; }
    }
}