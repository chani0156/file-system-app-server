
using Microsoft.AspNetCore.Mvc.Filters;

namespace files.Middleware
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<CustomExceptionFilter> _logger;

        public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            string errorMessage = "files-Error in Controler {0} Function Name {1} InnerException {2} Message {3}";
            _logger.LogError(string.Format(errorMessage, ((object[])context.RouteData.Values.Values)[1], ((object[])context.RouteData.Values.Values)[0], context.Exception.InnerException, context.Exception.Message));
        }
    }



}
