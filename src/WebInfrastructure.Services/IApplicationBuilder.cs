using Autofac;
using Owin;
// ReSharper disable UnusedMemberInSuper.Global

namespace Cognistreamer.WebInfrastructure.Services
{
    public interface IApplicationBuilder : IAppBuilder
    {
        IContainer Services { get; }
    }
}
