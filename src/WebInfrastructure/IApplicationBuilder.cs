using System.Web.Http;
using Autofac;
using Owin;

namespace Cognistreamer.WebInfrastructure
{
    public interface IApplicationBuilder : IAppBuilder
    {
        IContainer Services { get; }
        HttpConfiguration HttpConfiguration { get; }
    }
}
