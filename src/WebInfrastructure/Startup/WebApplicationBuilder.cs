using System;
using System.Collections.Generic;
using System.Web.Http;
using Autofac;
using Cognistreamer.WebInfrastructure.Services;
using Owin;

namespace Cognistreamer.WebInfrastructure.Startup
{
    internal class WebApplicationBuilder : IWebApplicationBuilder
    {
        private readonly IAppBuilder _owinAppBuilder;

        public WebApplicationBuilder(IAppBuilder owinAppBuilder, IServiceCollection serviceCollection, HttpConfiguration httpConfiguration)
        {
            _owinAppBuilder = owinAppBuilder ?? throw new ArgumentNullException(nameof(owinAppBuilder));
            HttpConfiguration = httpConfiguration ?? throw new ArgumentNullException(nameof(httpConfiguration));
            Services = (serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection))).Builder.Build();
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
