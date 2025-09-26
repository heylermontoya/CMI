using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using CMI.Domain.Exceptions;

namespace CMI.Api.Filters
{
    [AttributeUsage(AttributeTargets.All)]
    public sealed class AppExceptionFilterAttribute(
        ILogger<AppExceptionFilterAttribute> logger
    ) : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context != null && context.Exception != null)
            {
                HttpStatusCode statusCode;
                string errorMessage = "Ha ocurrido un error inesperado";

                switch (context.Exception)
                {
                    case AppException:
                    case ValidatorException:
                        statusCode = HttpStatusCode.BadRequest;
                        errorMessage = context.Exception.Message;
                        break;
                    case TimeoutErrorException:
                        statusCode = HttpStatusCode.RequestTimeout;
                        errorMessage = context.Exception.Message;
                        break;
                    default:
                        statusCode = HttpStatusCode.InternalServerError;
                        break;
                }

                context.HttpContext.Response.StatusCode = (int)statusCode;

                logger.LogError(context.Exception, "An error occurred: {Message}", errorMessage);

                var messageResponse = new
                {
                    Message = errorMessage
                };

                context.Result = new ObjectResult(messageResponse);
            }
        }
    }
}
