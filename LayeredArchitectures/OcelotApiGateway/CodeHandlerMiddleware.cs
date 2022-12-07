using Microsoft.Extensions.Primitives;

namespace OcelotApiGateway;

public class CodeHandlerMiddleware
{
    private readonly RequestDelegate _next;
   
    public CodeHandlerMiddleware(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("x-authentication", out var authStrings) 
            && authStrings.FirstOrDefault() is { } code)
        { 
            var httpFactory = context.RequestServices.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpFactory.CreateClient();

            var configuration = context.RequestServices.GetRequiredService<IConfiguration>();
        
            var url = $"{configuration["Jwt:Issuer"]}{configuration["Jwt:AccessTokenUrl"]}?code={code}&clientSecret={configuration["Jwt:ClientSecret"]}";
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var token = $"Bearer {await response.Content.ReadAsStringAsync()}";
                context.Request.Headers.Authorization = new StringValues(token);
                await _next(context);
            }
            else context.Response.StatusCode = 403;
        }
        else await _next(context);
    }
}