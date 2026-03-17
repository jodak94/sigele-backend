using System.Diagnostics;

namespace Api.Middlewares;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.Request.Path;
        var method = context.Request.Method;
        var stopWatch = Stopwatch.StartNew();

        await _next(context);
        
        stopWatch.Stop();
        var elapses = stopWatch.ElapsedMilliseconds;
        
        Console.WriteLine($"Method: {method} {endpoint} - {elapses}ms");
    }
}