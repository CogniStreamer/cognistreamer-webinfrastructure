using System.Security.Principal;
using Microsoft.Owin;
// ReSharper disable UnusedMember.Global

namespace Cognistreamer.WebInfrastructure.Extractors
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class IdentityExtractor
    {
        private readonly IIdentity _identity;

        public IdentityExtractor(IOwinContext context)
        {
            _identity = context?.Authentication?.User?.Identity;
        }

        public virtual IIdentity GetIdentity() => _identity;
    }
}
