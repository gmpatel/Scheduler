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

        [JsonIgnore]
        public DateTime DateTimeCreated { get; set; }

        [JsonIgnore]
        public DateTime? DateTimeLastModified { get; set; }

        [JsonProperty(PropertyName = "meeting", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public virtual MeetingEntity Meeting { get; set; }

        [JsonProperty(PropertyName = "runners", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public virtual ICollection<RunnerEntity> Runners { get; set; }
    }
}