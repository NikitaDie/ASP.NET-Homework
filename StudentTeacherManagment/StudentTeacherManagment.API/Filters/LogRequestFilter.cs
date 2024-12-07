using Microsoft.AspNetCore.Mvc.Filters;

namespace StudentTeacherManagment.API.Filters;

public class LogRequestFilter : IAsyncActionFilter
{
    private readonly ILogger<LogRequestFilter> _logger;

    public LogRequestFilter(ILogger<LogRequestFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var request = context.HttpContext.Request;
        
        _logger.LogInformation("Request Information:");
        _logger.LogInformation($"Method: {request.Method}");
        _logger.LogInformation($"Path: {request.Path}");
        _logger.LogInformation($"Query: {request.QueryString}");
        _logger.LogInformation($"Headers: {string.Join(", ", request.Headers)}");
        _logger.LogInformation($"Body (if available): {request.ContentLength}");
        
        var result = await next();
        
        _logger.LogInformation($"Action {context.ActionDescriptor.DisplayName} executed.");
    }
}