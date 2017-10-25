using Autofac;
using AutoMapper.Configuration;
using Cognistreamer.WebInfrastructure.Services;

namespace Cognistreamer.WebInfrastructure.Startup
{
    internal class ServiceCollection : IServiceCollection
    {
        public ServiceCollection()
        {
            Builder = new ContainerBuilder();
            MapperConfiguration = new MapperConfigurationExpression();
        }

        public ContainerBuilder Builder { get; }

        public MapperConfigurationExpression MapperConfiguration { get; }
    }
}
