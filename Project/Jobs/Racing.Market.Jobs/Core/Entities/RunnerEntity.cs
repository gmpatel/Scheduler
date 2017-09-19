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

        [JsonProperty(PropertyName = "rating", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Rating { get; set; }

        [JsonProperty(PropertyName = "lastFiveRuns", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string LastFiveRuns { get; set; }

        [JsonProperty(PropertyName = "scratched", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Scratched { get; set; }

        [JsonProperty(PropertyName = "barrel", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Barrel { get; set; }

        [JsonProperty(PropertyName = "tcdw", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Tcdw { get; set; }

        [JsonProperty(PropertyName = "trainer", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Trainer { get; set; }

        [JsonProperty(PropertyName = "jockey", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Jockey { get; set; }
        
        [JsonProperty(PropertyName = "weight", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public decimal Weight { get; set; }

        [JsonProperty(PropertyName = "resultPosition", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? ResultPosition { get; set; }

        [JsonProperty(PropertyName = "formSkyRating", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool FormSkyRating { get; set; }

        [JsonProperty(PropertyName = "formSkyRatingPosition", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? FormSkyRatingPosition { get; set; }

        [JsonProperty(PropertyName = "formBest12Months", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool FormBest12Months { get; set; }

        [JsonProperty(PropertyName = "formBest12MonthsPosition", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? FormBest12MonthsPosition { get; set; }

        [JsonProperty(PropertyName = "formRecent", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool FormRecent { get; set; }

        [JsonProperty(PropertyName = "formRecentPosition", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? FormRecentPosition { get; set; }

        [JsonProperty(PropertyName = "formDistance", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool FormDistance { get; set; }

        [JsonProperty(PropertyName = "formDistancePosition", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? FormDistancePosition { get; set; }

        [JsonProperty(PropertyName = "formClass", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool FormClass { get; set; }

        [JsonProperty(PropertyName = "formClassPosition", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? FormClassPosition { get; set; }

        [JsonProperty(PropertyName = "formTimeRating", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool FormTimeRating { get; set; }

        [JsonProperty(PropertyName = "formTimeRatingPosition", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? FormTimeRatingPosition { get; set; }

        [JsonProperty(PropertyName = "formInWet", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool FormInWet { get; set; }

        [JsonProperty(PropertyName = "formInWetPosition", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? FormInWetPosition { get; set; }

        [JsonProperty(PropertyName = "formBestOverall", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool FormBestOverall { get; set; }

        [JsonProperty(PropertyName = "formBestOverallPosition", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? FormBestOverallPosition { get; set; }

        [JsonProperty(PropertyName = "tipSky", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool TipSky { get; set; }

        [JsonProperty(PropertyName = "tipSkyPosition", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? TipSkyPosition { get; set; }

        [JsonIgnore]
        public DateTime DateTimeCreated { get; set; }

        [JsonIgnore]
        public DateTime? DateTimeLastModified { get; set; }

        [JsonIgnore]
        public virtual RaceEntity Race { get; set; }
    }
}
