using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Cors;
using Autofac;
using Autofac.Integration.WebApi;
using Cognistreamer.WebInfrastructure.Extractors;
using Cognistreamer.WebInfrastructure.Startup;
using Microsoft.Owin.Cors;
using Newtonsoft.Json.Serialization;
using Owin;

namespace Cognistreamer.WebInfrastructure
{
    public abstract class StartupBase
    {
        public void Configuration(IAppBuilder app)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.Builder.RegisterApiControllers(GetApiControllerAssembliesForAutofacRegistration());
            serviceCollection.Builder.RegisterType<RequestHeaderExtractor>().AsSelf().InstancePerLifetimeScope();
            serviceCollection.Builder.RegisterType<IdentityExtractor>().AsSelf().InstancePerLifetimeScope();
            ConfigureServices(serviceCollection);

            // TODO We'll probably have to map this to /signalr as WebApi uses a different CORS technique (see below)
            app.UseCors(CorsOptions.AllowAll);

            var config = new HttpConfiguration();
            var applicationBuilder = new ApplicationBuilder(app, serviceCollection, config);
            app.UseAutofacMiddleware(applicationBuilder.Services);
            Configure(applicationBuilder);

            config.EnableCors(new EnableCorsAttribute("*", "*", "GET, POST, OPTIONS, PUT, DELETE"));
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;
            config.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Include;

            applicationBuilder.MapHttpAttributeRoutesForRegisteredApiControllers(config);
            applicationBuilder.MapRegisteredApiRoutes(config);
            app.UseAutofacWebApi(config);

            app.UseWebApi(config);
        }

        protected abstract void ConfigureServices(IServiceCollection services);

        protected abstract void Configure(IApplicationBuilder app);

        private static Assembly[] GetApiControllerAssembliesForAutofacRegistration()
        {
            return
                AppDomain.CurrentDomain.GetAssemblies()
                    .Where(x => x.FullName.StartsWith("CogniStreamer", StringComparison.OrdinalIgnoreCase))
                    .ToArray();
        }
    }
}
