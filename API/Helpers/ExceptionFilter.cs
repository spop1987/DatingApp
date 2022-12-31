
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Helpers
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ExceptionFilterConfig _exceptionFilterConfig;
        public ExceptionFilter(ExceptionFilterConfig exceptionFilterConfig)
        {
            _exceptionFilterConfig = exceptionFilterConfig;
        }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var activity = GetActivity(context.HttpContext.Request);
            var result  = new ObjectResult(DatingAppError.SystemFailure(activity)) { StatusCode = StatusCodes.Status500InternalServerError };

            if(exception is DatingAppInvalidUserException datingAppInvalidUserException)
            {
                var message = string.IsNullOrEmpty(datingAppInvalidUserException?.Error?.Message) ? exception.Message : datingAppInvalidUserException.Error.Message;
                result = new BadRequestObjectResult(DatingAppError.CreateDuplicated(activity));
            }else if(exception is DatingAppNotFoundUserException datingAppNotFoundUserException)
            {
                var message = string.IsNullOrEmpty(datingAppNotFoundUserException?.Error?.Message) ? exception.Message : datingAppNotFoundUserException.Error.Message;
                result = new BadRequestObjectResult(DatingAppError.NotFound(activity));
            }

            context.Result = result;
        }

        public ErrorActivity GetActivity(HttpRequest request)
        {
            if(_exceptionFilterConfig.GetActivityFunc != null)
                return _exceptionFilterConfig.GetActivityFunc(request);

            return request.GetActivityDefault();
        }
    }

    public class ExceptionFilterConfig
    {
        public Func<HttpRequest, ErrorActivity> GetActivityFunc {get; set;}
        public ExceptionFilterConfig(Func<HttpRequest, ErrorActivity> getActivityFunc = null)
        {
            GetActivityFunc = getActivityFunc;
        }
    }
}