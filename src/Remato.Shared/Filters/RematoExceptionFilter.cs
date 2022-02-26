using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Remato.Shared
{
    public sealed class RematoExceptionFilter : ExceptionFilterAttribute
    {
        private readonly RematoMediaType _mediaType;

        public RematoExceptionFilter(RematoMediaType mediaType)
        {
            _mediaType = mediaType;
        }
        
        public override void OnException(ExceptionContext context)
        {
            var storagrError = context.Exception is RematoError error
                ? error
                : new InternalServerError(context.Exception);
            
            context.Result = new ObjectResult(storagrError)
            {
                StatusCode = storagrError.Code
            };
            context.HttpContext.Response.StatusCode = storagrError.Code;
            context.HttpContext.Response.ContentType = _mediaType.MediaType.Value;
        }
    }
}