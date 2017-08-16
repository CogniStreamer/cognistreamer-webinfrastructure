using System;
using System.Threading;
using Microsoft.Owin;
using Owin;
// ReSharper disable UnusedMember.Global

// ReSharper disable once CheckNamespace
namespace Cognistreamer.WebInfrastructure
{
    public static class AppBuilderExtensions
    {
        public static IAppBuilder RegisterOnDisposeHandler(this IAppBuilder app, Action onDisposeCallback)
        {
            var context = new OwinContext(app.Properties);
            var token = context.Get<CancellationToken>("host.OnAppDisposing");
            if (token != CancellationToken.None)
            {
                token.Register(onDisposeCallback);
            }

            return app;
        }
    }
}