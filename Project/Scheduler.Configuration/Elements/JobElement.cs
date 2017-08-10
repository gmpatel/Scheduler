using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Configuration.Elements
{
    public class JobElement : ConfigurationElement
    {
        [ConfigurationProperty("name", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        [ConfigurationProperty("class", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Class
        {
            get { return (string)base["class"]; }
            set { base["class"] = value; }
        }

        [ConfigurationProperty("cronTrigger", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string CronTrigger
        {
            get { return (string)base["cronTrigger"]; }
            set { base["cronTrigger"] = value; }
        }
    }
}