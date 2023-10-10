using Microsoft.AspNetCore.Mvc.Filters;

namespace files.Middleware
{
    public class LogActionFilter : IActionFilter
    {
        private readonly ILogger<LogActionFilter> _logger;
        public string logEnteringText = "files-Logs entering to function {0} at {1}";
        public string logErrorText = "files-Logs Error In Function {0} at {1} {2}";
        public string logExit = "files-Logs exists function {0} at {1}";
        public string functionOperation = "files-Logs function {0} complete successfully at {1}";
        public LogActionFilter(ILogger<LogActionFilter> logger)
        {
            _logger = logger;

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string _functionName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName;
            string _controllerName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ControllerName;
             _logger.LogInformation(logEnteringText, _functionName, _controllerName);


        }



        public void OnActionExecuted(ActionExecutedContext context)
        {

            string _functionName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName;
            string _controllerName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ControllerName;
            _logger.LogInformation(logExit, _functionName, _controllerName);
            _logger.LogInformation(functionOperation, _functionName, _controllerName);
        }


    }

}
