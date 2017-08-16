using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
// ReSharper disable UnusedMember.Global

namespace Cognistreamer.WebInfrastructure.Extractors
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class RequestHeaderExtractor
    {
        private readonly IOwinRequest _request;

        public RequestHeaderExtractor(IOwinContext context)
        {
            _request = context?.Request ?? throw new ArgumentNullException(nameof(context.Request));
        }

        // ReSharper disable once MemberCanBeProtected.Global
        public virtual IEnumerable<string> GetValues(string headerName) => _request.Headers.GetValues(headerName);

        public virtual string GetFirstValue(string headerName) => GetValues(headerName)?.FirstOrDefault();
    }
}
