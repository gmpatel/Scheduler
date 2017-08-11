using System.Configuration;
using ASX.Market.Jobs.Configuration.ElementCollections;

namespace ASX.Market.Jobs.Configuration.Elements
{
    public class AssemblyElement : ConfigurationElement
    {
        [ConfigurationProperty("name", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }
        
        [ConfigurationProperty("jobs")]
        public JobElementCollection Jobs
        {
            get { return ((JobElementCollection)(base["jobs"])); }
            set { base["jobs"] = value; }
        }
    }
}