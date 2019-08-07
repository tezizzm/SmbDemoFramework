using System;
using Autofac;
using Autofac.Integration.Mvc;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Steeltoe.Common.Configuration.Autofac;
using Steeltoe.Common.Options.Autofac;
using Steeltoe.Extensions.Configuration.CloudFoundry;

namespace LighthouseWeb
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            Console.WriteLine("Application Start");
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ApplicationConfig.RegisterConfig();

            var builder = new ContainerBuilder();

            // Register all the controllers with Autofac
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Add Options service to Autofac container
            builder.RegisterOptions();

            // Adds Options to Autofac container
            builder.RegisterCloudFoundryOptions(ApplicationConfig.Configuration);

            // Adds IConfiguration and IConfigurationRoot to Autofac container
            builder.RegisterConfiguration(ApplicationConfig.Configuration);

            builder.RegisterType<SmbService>().As<ISmbService>().InstancePerRequest();
            builder.RegisterType<SmbClientFactory>().As<ISmbClientFactory>().InstancePerRequest();

            // Create the Autofac container
            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            Console.WriteLine("Finished Declaring Dependencies");
        }

        private void Application_Error(object sender, EventArgs e)
        {
            var lastError = Server.GetLastError();
            Console.WriteLine("Unhandled exception: " + lastError.Message + lastError.StackTrace);
        }
    }
}
