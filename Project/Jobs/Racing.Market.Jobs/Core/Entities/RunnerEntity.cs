using System;
using Newtonsoft.Json;

namespace BET.Market.Jobs.Core.Entities
{
    public class RunnerEntity
    {
        [JsonProperty(PropertyName = "id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long Id { get; set; }

        [JsonProperty(PropertyName = "raceId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long RaceId { get; set; }

        [JsonProperty(PropertyName = "number", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Number { get; set; }

        [JsonProperty(PropertyName = "name", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "isGood", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsGood { get; set; }

        [JsonProperty(PropertyName = "formSkyRating", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool FormSkyRating { get; set; }

        [JsonProperty(PropertyName = "formBest12Months", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool FormBest12Months { get; set; }

        [JsonProperty(PropertyName = "formRecent", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool FormRecent { get; set; }

        [JsonProperty(PropertyName = "formDistance", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool FormDistance { get; set; }

        [JsonProperty(PropertyName = "formClass", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool FormClass { get; set; }

        [JsonProperty(PropertyName = "formTimeRating", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool FormTimeRating { get; set; }

        [JsonProperty(PropertyName = "formInWet", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool FormInWet { get; set; }

        [JsonProperty(PropertyName = "formBestOverall", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool FormBestOverall { get; set; }

        [JsonProperty(PropertyName = "tipSky", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool TipSky { get; set; }

        [JsonIgnore]
        public DateTime DateTimeCreated { get; set; }

        [JsonIgnore]
        public DateTime? DateTimeLastModified { get; set; }

        [JsonProperty(PropertyName = "race", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public virtual RaceEntity Race { get; set; }
    }
}
