using Autofac;

namespace Cognistreamer.WebInfrastructure
{
    public interface IServiceCollection
    {
        ContainerBuilder Builder { get; }
    }
}
