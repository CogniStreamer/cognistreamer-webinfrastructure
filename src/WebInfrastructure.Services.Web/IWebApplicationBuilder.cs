using System.Web.Http;
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMemberInSuper.Global

// ReSharper disable once CheckNamespace
namespace Cognistreamer.WebInfrastructure.Services
{
    public interface IWebApplicationBuilder : IApplicationBuilder
    {
        HttpConfiguration HttpConfiguration { get; }
    }
}
