using Application.Common.Attributes;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters;

public class PermissionFilter : IAsyncActionFilter
{
    private readonly ICurrentUserService _currentUserService;

    public PermissionFilter(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }
    
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var attribute = context.ActionDescriptor.EndpointMetadata.OfType<RequiresPermissionAttribute>()
            .FirstOrDefault();

        if (attribute is null)
        {
            await next();
            return;
        }

        if (!_currentUserService.HasPermission(attribute.Permission))
        {
            context.Result = new ForbidResult();
            return;
        }
        
        await next();
    }
}