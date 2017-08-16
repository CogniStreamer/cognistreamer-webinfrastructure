using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace Cognistreamer.WebInfrastructure
{
    /// <summary>
    /// Extension methods for IApplicationBuilder.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        private const string ApiRegistrationFiltersKey = "CsApiRegistrationFilters";
        private const string ApiRoutesKey = "CsApiRoutes";

        /// <summary>
        /// Registers all ApiControllers for use in RouteAttribute mapping for which the filter function returns true.
        /// </summary>
        /// <param name="app">The ApplicationBuilder.</param>
        /// <param name="filter">The filter function. Type is the type of the ApiController class.</param>
        public static void RegisterApiControllers(this IApplicationBuilder app, Func<Type, bool> filter)
        {
            if (!app.Properties.ContainsKey(ApiRegistrationFiltersKey)) app.Properties.Add(ApiRegistrationFiltersKey, new List<Func<Type, bool>>());
            var apiRegistrationFilters = app.Properties[ApiRegistrationFiltersKey] as List<Func<Type, bool>>;
            if (apiRegistrationFilters == null) throw new InvalidOperationException("Already mapped");
            apiRegistrationFilters.Add(filter);
        }

        /// <summary>
        /// Registers all ApiControllers in this namespace for RouteAttribute mapping.
        /// </summary>
        /// <param name="app">The ApplicationBuilder.</param>
        /// <param name="namespace">The namespace.</param>
        public static void RegisterApiControllersInNamespace(this IApplicationBuilder app, string @namespace)
        {
            app.RegisterApiControllers(controllerType => controllerType.Namespace == @namespace);
        }

        /// <summary>
        /// Registers all ApiControllers in the namespace of the given type for RouteAttributes mapping.
        /// </summary>
        /// <typeparam name="T">Any type in the namespace you want to address.</typeparam>
        /// <param name="app">The ApplicationBuilder.</param>
        public static void RegisterApiControllersInNamespaceOf<T>(this IApplicationBuilder app)
        {
            app.RegisterApiControllersInNamespace(typeof(T).Namespace);
        }

        /// <summary>
        /// Registers all ApiControllers in the given assembly for RouteAttribute mapping.
        /// </summary>
        /// <param name="app">The ApplicationBuilder.</param>
        /// <param name="assembly">The Assembly.</param>
        public static void RegisterApiControllersInAssembly(this IApplicationBuilder app, Assembly assembly)
        {
            app.RegisterApiControllers(controllerType => controllerType.Assembly == assembly);
        }

        /// <summary>
        /// Manually register a ApiController route.
        /// </summary>
        /// <param name="app">The ApplicationBuilder.</param>
        /// <param name="route">The route.</param>
        public static void RegisterApiControllerRoute(this IApplicationBuilder app, string name, IHttpRoute route)
        {
            if (!app.Properties.ContainsKey(ApiRoutesKey)) app.Properties.Add(ApiRoutesKey, new Dictionary<string, IHttpRoute>());
            var apiRoutes = app.Properties[ApiRoutesKey] as Dictionary<string, IHttpRoute>;
            if (apiRoutes == null) throw new InvalidOperationException("Already mapped");
            apiRoutes.Add(name, route);
        }

        /// <summary>
        /// Calls MapHttpAttributeRoutes while only mapping registered ApiControllers.
        /// </summary>
        /// <param name="app">The ApplicationBuilder.</param>
        /// <param name="config">The HttpConfiguration.</param>
        internal static void MapHttpAttributeRoutesForRegisteredApiControllers(this IApplicationBuilder app, HttpConfiguration config)
        {
            if (!app.Properties.ContainsKey(ApiRegistrationFiltersKey)) return;
            var apiRegistrationFilters = app.Properties[ApiRegistrationFiltersKey] as List<Func<Type, bool>>;
            if (apiRegistrationFilters == null) throw new InvalidOperationException("Already mapped");
            config.MapHttpAttributeRoutes(new FilteredDirectRouteProvider(apiRegistrationFilters));
            app.Properties[ApiRegistrationFiltersKey] = null;
        }

        /// <summary>
        /// Maps registered api routes.
        /// </summary>
        /// <param name="app">The ApplicationBuilder.</param>
        /// <param name="config">The HttpConfiguration.</param>
        internal static void MapRegisteredApiRoutes(this IApplicationBuilder app, HttpConfiguration config)
        {
            if (!app.Properties.ContainsKey(ApiRoutesKey)) return;
            var apiRoutes = app.Properties[ApiRoutesKey] as Dictionary<string, IHttpRoute>;
            if (apiRoutes == null) throw new InvalidOperationException("Already mapped");
            foreach (var route in apiRoutes) config.Routes.Add(route.Key, route.Value);
            app.Properties[ApiRoutesKey] = null;
        }

        /// <summary>
        /// A custom DirectRouteProvider that allows to filter routes by ControllerType using a filter function.
        /// </summary>
        private class FilteredDirectRouteProvider : DefaultDirectRouteProvider
        {
            private readonly List<Func<Type, bool>> _filters;

            public FilteredDirectRouteProvider(List<Func<Type, bool>> filters)
            {
                if (filters == null) throw new ArgumentNullException(nameof(filters));
                _filters = filters;
            }

            public override IReadOnlyList<RouteEntry> GetDirectRoutes(HttpControllerDescriptor controllerDescriptor, IReadOnlyList<HttpActionDescriptor> actionDescriptors, IInlineConstraintResolver constraintResolver)
            {
                return _filters.Any(filter => filter(controllerDescriptor.ControllerType))
                    ? base.GetDirectRoutes(controllerDescriptor, actionDescriptors, constraintResolver)
                    : new RouteEntry[0];
            }
        }
    }
}