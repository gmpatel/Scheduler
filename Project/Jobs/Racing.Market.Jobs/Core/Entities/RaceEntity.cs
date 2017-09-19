using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BET.Market.Jobs.Core.Entities
{
    public class RaceEntity
    {
        [JsonProperty(PropertyName = "id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long Id { get; set; }

        [JsonProperty(PropertyName = "meetingId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long MeetingId { get; set; }

        [JsonProperty(PropertyName = "number", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Number { get; set; }

        [JsonProperty(PropertyName = "name", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "skyId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long? SkyId { get; set; }

        [JsonProperty(PropertyName = "time", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Time { get; set; }

        [JsonProperty(PropertyName = "weather", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Weather { get; set; }

        [JsonProperty(PropertyName = "track", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Track { get; set; }

        [JsonProperty(PropertyName = "distance", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Distance { get; set; }

        [JsonProperty(PropertyName = "class", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Class { get; set; }

        [JsonProperty(PropertyName = "prizemoney", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Prizemoney { get; set; }

        [JsonIgnore]
        public DateTime DateTimeCreated { get; set; }

        [JsonIgnore]
        public DateTime? DateTimeLastModified { get; set; }

        [JsonIgnore]
        public virtual MeetingEntity Meeting { get; set; }

        [JsonProperty(PropertyName = "runners", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public virtual ICollection<RunnerEntity> Runners { get; set; }
    }
}