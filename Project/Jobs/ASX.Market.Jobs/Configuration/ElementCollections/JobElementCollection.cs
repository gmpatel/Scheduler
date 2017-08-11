using System.Configuration;
using ASX.Market.Jobs.Configuration.Elements;

namespace ASX.Market.Jobs.Configuration.ElementCollections
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
            return ((JobElement)element).Class;
        }

        public JobElement this[string name]
        {
            get { return ((JobElement)base.BaseGet(name)); }
        }
    }
}