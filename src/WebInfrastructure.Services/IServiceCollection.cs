using Autofac;

namespace Cognistreamer.WebInfrastructure.Services
{
    public interface IServiceCollection
    {
        ContainerBuilder Builder { get; }
    }
}
