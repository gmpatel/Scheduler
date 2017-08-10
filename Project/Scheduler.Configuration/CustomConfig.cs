using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Scheduler.Configuration.Elements;

namespace Scheduler.Configuration
{
    public class CustomConfig
    {
        private readonly CustomConfigSection config;
        private static volatile CustomConfig instance;

        private static readonly object ConsturctorLock = new object();

        private IList<AssemblyElement> assemblyElements;

        private CustomConfig()
        {
            if (config == null)
            {
                config = (CustomConfigSection)ConfigurationManager.GetSection("serviceConfig");
            }
        }

        public static CustomConfig Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (ConsturctorLock)
                    {
                        if (instance == null) instance = new CustomConfig();
                    }
                }

                return instance;
            }
        }

        public IList<AssemblyElement> AssemblyElements
        {
            get
            {
                return GetAssemblyElements();
            }
        }

        public IList<AssemblyElement> GetAssemblyElements()
        {
            if (assemblyElements == null)
            {
                assemblyElements = new List<AssemblyElement>();

                foreach (var assembly in config.Assemblies)
                {
                    assemblyElements.Add((AssemblyElement)assembly);
                }
            }

            return assemblyElements;
        }
    }
}