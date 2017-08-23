using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BET.Market.Jobs.Core.Entities
{
    public class VenueEntity
    {
        [JsonProperty(PropertyName = "id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long Id { get; set; }

        [JsonProperty(PropertyName = "name", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "name2", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Name2 { get; set; }

        [JsonProperty(PropertyName = "name3", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Name3 { get; set; }

        [JsonProperty(PropertyName = "province", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Province { get; set; }

        [JsonIgnore]
        public DateTime DateTimeCreated { get; set; }

        [JsonIgnore]
        public DateTime? DateTimeLastModified { get; set; }

        [JsonIgnore]
        public virtual ICollection<MeetingEntity> Meetings { get; set; }
    }
}