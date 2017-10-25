using Autofac;
using AutoMapper.Configuration;

namespace Cognistreamer.WebInfrastructure.Services
{
    public interface IServiceCollection
    {
        ContainerBuilder Builder { get; }

        MapperConfigurationExpression MapperConfiguration { get; }
    }
}
