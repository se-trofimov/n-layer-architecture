using Ocelot.Middleware;

namespace OcelotApiGateway;

public class AuthMiddleware
{
    public static Task AuthorizationMiddleware(HttpContext context, Func<Task> next)
    {
        var downstreamRoute = context.Items.DownstreamRoute();
        var requirements = downstreamRoute.RouteClaimsRequirement;

        if (requirements.TryGetValue("IsInRole", out var role))
        {
           var roles = context.User.Claims.Where(x => x.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                .Select(x=>x.Value)
                .ToList();
           if (!roles.Contains(role))
               context.Response.StatusCode = 403;
        }
        
        return next();
    }
}