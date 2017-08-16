using System;
using System.Collections.Generic;
using System.Web.Http;
using Autofac;
using Owin;

namespace Cognistreamer.WebInfrastructure.Startup
{
    internal class ApplicationBuilder : IApplicationBuilder
    {
        private readonly IAppBuilder _owinAppBuilder;

        public ApplicationBuilder(IAppBuilder owinAppBuilder, IServiceCollection serviceCollection, HttpConfiguration httpConfiguration)
        {
            if (owinAppBuilder == null) throw new ArgumentNullException(nameof(owinAppBuilder));
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));
            if (httpConfiguration == null) throw new ArgumentNullException(nameof(httpConfiguration));
            _owinAppBuilder = owinAppBuilder;
            HttpConfiguration = httpConfiguration;
            Services = serviceCollection.Builder.Build();
        }

        public IContainer Services { get; }

        public HttpConfiguration HttpConfiguration { get; }

        public IAppBuilder Use(object middleware, params object[] args)
            => _owinAppBuilder.Use(middleware, args);

        public object Build(Type returnType)
            => _owinAppBuilder.Build(returnType);

        public IAppBuilder New()
            => _owinAppBuilder.New();

        public IDictionary<string, object> Properties
            => _owinAppBuilder.Properties;
    }
}
