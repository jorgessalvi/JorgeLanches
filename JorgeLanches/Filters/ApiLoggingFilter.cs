using Microsoft.AspNetCore.Mvc.Filters;

namespace JorgeLanches.Filters
{
    public class ApiLoggingFilter : IActionFilter
    {
        private readonly ILogger<ApiLoggingFilter> _logger;

        public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("Executando OnActionExecuted");
            _logger.LogInformation($"{DateTime.UtcNow}");
            _logger.LogInformation($"Model State is valid: {context.HttpContext.Response.StatusCode}");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("Executando OnActionExecuting");
            _logger.LogInformation($"{DateTime.UtcNow}");
            _logger.LogInformation($"Model State is valid: {context.ModelState.IsValid}");
        }
    }
}
