using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scheduler.Configuration.Elements;

namespace Scheduler.Configuration.ElementCollections
{
    [ConfigurationCollection(typeof(AssemblyElement))]
    public class JobElementCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override string ElementName
        {
            get { return "job"; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new JobElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((JobElement)element).Name;
        }

        public JobElement this[string name]
        {
            get { return ((JobElement)base.BaseGet(name)); }
        }
    }
}