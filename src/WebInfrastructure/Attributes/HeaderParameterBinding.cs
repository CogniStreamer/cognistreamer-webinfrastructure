using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;

namespace Cognistreamer.WebInfrastructure.Attributes
{
    public class HeaderParameterBinding<TResult> : HttpParameterBinding
    {
        private readonly string _headerName;
        private readonly Func<string, TResult> _valueConverter;
        private readonly Func<TResult> _getDefaultValue;

        public HeaderParameterBinding(HttpParameterDescriptor descriptor, string headerName, Func<string, TResult> valueConverter, Func<TResult> getDefaultValue = null)
            : base(descriptor)
        {
            if (string.IsNullOrWhiteSpace(headerName)) throw new ArgumentNullException(nameof(headerName));
            if (valueConverter == null) throw new ArgumentNullException(nameof(valueConverter));
            _headerName = headerName;
            _valueConverter = valueConverter;
            _getDefaultValue = getDefaultValue ?? (() => default(TResult));
        }

        public override Task ExecuteBindingAsync(ModelMetadataProvider metadataProvider, HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            IEnumerable<string> headerValues;
            if (actionContext.Request.Headers.TryGetValues(_headerName, out headerValues))
            {
                actionContext.ActionArguments[Descriptor.ParameterName] = headerValues.Select(_valueConverter).FirstOrDefault();
            }
            else actionContext.ActionArguments[Descriptor.ParameterName] = _getDefaultValue();
            return Task.FromResult(0);
        }
    }
}