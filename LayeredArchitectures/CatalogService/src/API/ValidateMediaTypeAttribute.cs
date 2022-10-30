﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace API;
public class ValidateMediaTypeAttribute : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var acceptHeaderPresent = context.HttpContext.Request.Headers.ContainsKey("Accept");
        if (!acceptHeaderPresent)
        {
            context.Result = new BadRequestObjectResult($"Accept header is missing.");
            return;
        }
        var mediaType = context.HttpContext.Request.Headers["Accept"].FirstOrDefault();
        if (!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue? outMediaType))
        {
            context.Result = new BadRequestObjectResult("Media type not present. Please add Accept header with the required media type.");
            return;
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}
