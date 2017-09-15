using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Market.Authentication.Core.Entities
{
    public class IncomeRangeEntity
    {
        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonIgnore]
        public bool Enabled { get; set; }

        [JsonIgnore]
        public DateTime DateTimeCreated { get; set; }

        [JsonIgnore]
        public DateTime? DateTimeLastModified { get; set; }

        [JsonIgnore]
        public virtual ICollection<UserEntity> Users { get; set; }
    }
}