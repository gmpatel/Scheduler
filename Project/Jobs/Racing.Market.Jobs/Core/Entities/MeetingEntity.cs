using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BET.Market.Jobs.Core.Entities
{
    public class MeetingEntity
    {
        [JsonProperty(PropertyName = "id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long Id { get; set; }

        [JsonProperty(PropertyName = "venueId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long VenueId { get; set; }

        [JsonProperty(PropertyName = "date", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long Date { get; set; }

        [JsonProperty(PropertyName = "skyId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long? SkyId { get; set; }

        [JsonProperty(PropertyName = "tipsUrl", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string TipsUrl { get; set; }

        [JsonProperty(PropertyName = "formUrl", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string FormUrl { get; set; }

        [JsonIgnore]
        public DateTime DateTimeCreated { get; set; }

        [JsonIgnore]
        public DateTime? DateTimeLastModified { get; set; }

        [JsonIgnore]
        public virtual VenueEntity Venue { get; set; }

        [JsonProperty(PropertyName = "races", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public virtual ICollection<RaceEntity> Races { get; set; }
    }
}