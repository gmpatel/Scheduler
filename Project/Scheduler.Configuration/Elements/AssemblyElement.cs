using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scheduler.Configuration.ElementCollections;

namespace Scheduler.Configuration.Elements
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