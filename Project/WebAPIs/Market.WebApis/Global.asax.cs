using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ASX.Market.Jobs.DataAccess.EF.Defaults;
using ASX.Market.Jobs.DataAccess.EF.Interfaces;
using Autofac;
using Autofac.Integration.WebApi;
using BET.Market.Jobs.DataAccess.EF.Defaults;
using BET.Market.Jobs.DataAccess.EF.Interfaces;
using Market.Authentication.DataAccess.EF.Defaults;
using Market.Authentication.DataAccess.EF.Interfaces;
using DataContext = Market.Authentication.DataAccess.EF.Defaults.DataContext;
using IDataContext = Market.Authentication.DataAccess.EF.Interfaces.IDataContext;
using IUnitOfWork = Market.Authentication.DataAccess.EF.Interfaces.IUnitOfWork;
using UnitOfWork = Market.Authentication.DataAccess.EF.Defaults.UnitOfWork;

namespace Market.WebApis
{
    public static class WebSystem
    {
        public static IContainer Container { get; set; }
        public static string BackEndKey { get; set; }
        public static string ApiKey { get; set; }
        public static bool ApiKeyEnabled { get; set; }
        public static string HostRootUrl { get; set; }
        public static double TokenLifeSpanMinutes { get; set; }
    }
    
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(WebSystem.Container = ConfigureContainer());
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GetConfiguration(WebSystem.Container);
        }

        public static IContainer ConfigureContainer()
        {
            var cb = new ContainerBuilder();

            cb.RegisterApiControllers(Assembly.GetExecutingAssembly());
            cb.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);

            cb.RegisterType<DataContext>().As<IDataContext>().InstancePerLifetimeScope();
            cb.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            
            cb.RegisterType<DataServiceAUTH>().As<IDataServiceAUTH>().InstancePerLifetimeScope();
            cb.RegisterType<DataServiceASX>().As<IDataServiceASX>().InstancePerLifetimeScope();
            cb.RegisterType<DataServiceBET>().As<IDataServiceBET>().InstancePerLifetimeScope();

            cb.RegisterGeneric(typeof(Authentication.DataAccess.EF.Defaults.Repository<>)).As(typeof(Authentication.DataAccess.EF.Interfaces.IRepository<>)).InstancePerLifetimeScope();

            return cb.Build();
        }

        public static void GetConfiguration(IContainer container)
        {
            WebSystem.ApiKey = Authentication.Properties.Resources.ApiKey;
            WebSystem.BackEndKey = Authentication.Properties.Resources.BackEndKey;
            WebSystem.ApiKeyEnabled = true;
            WebSystem.HostRootUrl = default(string);
            WebSystem.TokenLifeSpanMinutes = double.Parse(Authentication.Properties.Resources.TokenLifeSpanMinutes);
        }
    }
}
