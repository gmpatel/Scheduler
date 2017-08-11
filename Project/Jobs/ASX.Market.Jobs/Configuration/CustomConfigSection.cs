using System.Configuration;
using ASX.Market.Jobs.Configuration.ElementCollections;

namespace ASX.Market.Jobs.Configuration
{
    public class CustomConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("assemblies")]
        public AssemblyElementCollection Assemblies
        {
            get { return ((AssemblyElementCollection)(base["assemblies"])); }
            set { base["assemblies"] = value; }
        }
    }
}