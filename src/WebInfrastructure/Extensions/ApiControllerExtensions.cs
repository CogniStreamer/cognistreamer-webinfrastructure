using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace System.Web.Http
{
    public static class ApiControllerExtensions
    {
        public static IHttpActionResult Accepted(this ApiController controller)
        {
            return new AcceptedResult(controller.Request);
        }

        public static IHttpActionResult Accepted(this ApiController controller, Uri location)
        {
            if (location == null) throw new ArgumentNullException(nameof(location));
            return new AcceptedResult(controller.Request, location);
        }

        public static IHttpActionResult BadRequest(string message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            return new BadRequestResult(message);
        }

        private class AcceptedResult : IHttpActionResult
        {
            private readonly HttpRequestMessage _request;
            private readonly Uri _location;

            public AcceptedResult(HttpRequestMessage request, Uri location = null)
            {
                _request = request;
                _location = location;
            }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                var response = new HttpResponseMessage(HttpStatusCode.Accepted)
                {
                    RequestMessage = _request
                };
                if (_location != null) response.Headers.Location = _location;
                return Task.FromResult(response);
            }
        }

        private class BadRequestResult : IHttpActionResult
        {
            private readonly string _message;

            public BadRequestResult(string message)
            {
                _message = message;
            }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(_message)
                };
                return Task.FromResult(response);
            }
        }
    }
}
