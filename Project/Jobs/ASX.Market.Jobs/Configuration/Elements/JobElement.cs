using System.Configuration;

namespace ASX.Market.Jobs.Configuration.Elements
{
    public class JobElement : ConfigurationElement
    {
        [ConfigurationProperty("class", DefaultValue = "", IsKey = true, IsRequired = true)]
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