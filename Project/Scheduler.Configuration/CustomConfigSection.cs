using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scheduler.Configuration.ElementCollections;

namespace Scheduler.Configuration
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