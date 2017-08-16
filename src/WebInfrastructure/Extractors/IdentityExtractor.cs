using System.Security.Principal;
using Microsoft.Owin;

namespace Cognistreamer.WebInfrastructure.Extractors
{
    public class IdentityExtractor
    {
        private readonly IIdentity _identity;

        public IdentityExtractor(IOwinContext context)
        {
            _identity = context?.Authentication?.User?.Identity;
        }

        public IIdentity GetIdentity() => _identity;
    }
}
