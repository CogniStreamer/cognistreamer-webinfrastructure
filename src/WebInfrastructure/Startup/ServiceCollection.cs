using Autofac;

namespace Cognistreamer.WebInfrastructure.Startup
{
    internal class ServiceCollection : IServiceCollection
    {
        public ServiceCollection()
        {
            Builder = new ContainerBuilder();
        }

        public ContainerBuilder Builder { get; }
    }
}
