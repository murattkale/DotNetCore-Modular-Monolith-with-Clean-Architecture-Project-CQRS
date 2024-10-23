using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace UI.Middleware;

public class ResponseCacheMiddleware(RequestDelegate next, IMemoryCache cache)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // Check if cache should be bypassed
        if (context.Request.Query.TryGetValue("cache", out var cacheControl) && cacheControl == "false")
        {
            var cacheKeyClear = GenerateCacheKeyFromRequest(context);
            cache.Remove(cacheKeyClear);
            context.Response.Headers["X-Cache"] = "BYPASS";
        }

        
        var cacheKey = GenerateCacheKeyFromRequest(context);
        if (cache.TryGetValue(cacheKey, out string cachedResponse))
        {
            context.Response.Headers["X-Cache"] = "HIT";
            context.Response.ContentType = "text/html";
            await context.Response.WriteAsync(cachedResponse);
            return;
        }

        var originalBodyStream = context.Response.Body;

        using (var responseBody = new MemoryStream())
        {
            context.Response.Body = responseBody;

            await next(context);

            context.Response.Body = originalBodyStream;
            context.Response.Headers["X-Cache"] = "MISS";

            if (context.Response.StatusCode == 200)
            {
                responseBody.Seek(0, SeekOrigin.Begin);
                var responseText = new StreamReader(responseBody).ReadToEnd();
                cache.Set(cacheKey, responseText, TimeSpan.FromMinutes(1));
                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
    }

    private string GenerateCacheKeyFromRequest(HttpContext context)
    {
        var request = context.Request;
        var keyBuilder = new StringBuilder();
        keyBuilder.Append($"{request.Path}");

       
        // Session'dan LangId değeri alınıyor
        var langId = context.Session.GetInt32("LangId");
        keyBuilder.Append($"|LangId-{langId}");


        foreach (var (key, value) in request.Query.Where(o => o.Key != "cache").OrderBy(x => x.Key))
            keyBuilder.Append($"|{key}-{value}");

        return keyBuilder.ToString();
    }
}

public static class ResponseCacheMiddlewareExtensions
{
    public static IApplicationBuilder UseResponseCacheMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ResponseCacheMiddleware>();
    }
}