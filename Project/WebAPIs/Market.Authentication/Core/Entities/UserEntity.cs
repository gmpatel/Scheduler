using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Market.Authentication.Core.Entities
{
    public class UserEntity
    {
        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; }

        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "birthDate", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTime? BirthDate { get; set; }

        [JsonIgnore]
        public long FamilyTypeId { get; set; }

        [NotMapped]
        [JsonProperty(PropertyName = "familyType", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string FamilyTypeString => FamilyType.Id > 1 ? FamilyType.Name : null;

        [JsonIgnore]
        public long StateId { get; set; }

        [NotMapped]
        [JsonProperty(PropertyName = "state", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string StateString => State.Id > 1 ? State.Name : null;

        [JsonProperty(PropertyName = "postCode", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string PostCode { get; set; }

        [JsonProperty(PropertyName = "mobile", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Mobile { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        [JsonIgnore]
        public string Key { get; set; }

        [JsonIgnore]
        public string Code { get; set; }

        [JsonIgnore]
        public long RoleId { get; set; }

        [NotMapped]
        [JsonProperty(PropertyName = "role", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string RoleString => Role.Name;

        [JsonProperty(PropertyName = "enabled")]
        public bool Enabled { get; set; }

        [JsonProperty(PropertyName = "verified")]
        public bool Verified { get; set; }

        [JsonIgnore]
        public long IncomeRangeId { get; set; }

        [NotMapped]
        [JsonIgnore]
        [JsonProperty(PropertyName = "incomeRange", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string IncomeRangeString => IncomeRange.Id > 1 ? IncomeRange.Name : null;

        [JsonIgnore]
        public DateTime DateTimeCreated { get; set; }

        [JsonIgnore]
        public DateTime? DateTimeLastModified { get; set; }

        [JsonIgnore]
        public virtual StateEntity State { get; set; }

        [JsonIgnore]
        public virtual RoleEntity Role { get; set; }

        [JsonIgnore]
        public virtual FamilyTypeEntity FamilyType { get; set; }

        [JsonIgnore]
        public virtual IncomeRangeEntity IncomeRange { get; set; }

        [JsonIgnore]
        public virtual ICollection<TokenEntity> Tokens { get; set; }

        [JsonIgnore]
        public virtual ICollection<ClientEntity> Clients { get; set; }
    }
}