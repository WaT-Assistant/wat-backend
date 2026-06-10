using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Security.Claims;
using System.Text.Json;
using WatApi.Config;
using Microsoft.Extensions.Logging;

namespace WatApi.Middleware
{
    internal class RateLimitEntry
    {
        public int Count { get; set; }
        public DateTime WindowStart { get; set; }
    }

    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;
        private readonly ILogger<RateLimitingMiddleware> _logger;
        private readonly RateLimitOptions _options;
        private readonly ConcurrentDictionary<string, object> _locks = new();

        public RateLimitingMiddleware(RequestDelegate next, IMemoryCache cache, IOptions<RateLimitOptions> opts, ILogger<RateLimitingMiddleware> logger)
        {
            _next = next;
            _cache = cache;
            _options = opts.Value;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var clientId = GetClientIdentifier(context);
            var routePart = _options.PerRoute ? context.Request.Path.Value ?? "/" : string.Empty;
            var key = $"rl:{clientId}:{routePart}";

            var now = DateTime.UtcNow;
            var lockObj = _locks.GetOrAdd(key, _ => new object());
            RateLimitEntry? entry;

            lock (lockObj)
            {
                if (!_cache.TryGetValue(key, out entry) || entry == null)
                {
                    entry = new RateLimitEntry { Count = 1, WindowStart = now };
                    _cache.Set(key, entry, TimeSpan.FromSeconds(_options.WindowSeconds));
                }
                else
                {
                    var elapsed = now - entry.WindowStart;
                    if (elapsed.TotalSeconds >= _options.WindowSeconds)
                    {
                        entry = new RateLimitEntry { Count = 1, WindowStart = now };
                        _cache.Set(key, entry, TimeSpan.FromSeconds(_options.WindowSeconds));
                    }
                    else
                    {
                        entry.Count++;
                        // refresh remaining expiration
                        var remaining = TimeSpan.FromSeconds(Math.Max(1, _options.WindowSeconds - (int)elapsed.TotalSeconds));
                        _cache.Set(key, entry, remaining);
                    }
                }
            }

            if (entry.Count > _options.PermitLimit)
            {
                var retryAfter = Math.Max(1, _options.WindowSeconds - (int)(now - entry.WindowStart).TotalSeconds);
                context.Response.Headers["Retry-After"] = retryAfter.ToString();
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                _logger.LogWarning("Rate limit exceeded for {Client}. Path: {Path}", clientId, context.Request.Path);

                var problem = new
                {
                    type = "about:blank",
                    title = "Too many requests",
                    status = StatusCodes.Status429TooManyRequests,
                    detail = "Rate limit exceeded. Try again later."
                };
                context.Response.ContentType = "application/problem+json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
                return;
            }

            await _next(context);
        }

        private string GetClientIdentifier(HttpContext context)
        {
            if (_options.UseUserId && context.User?.Identity?.IsAuthenticated == true)
            {
                var claim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrWhiteSpace(claim))
                    return $"user:{claim}";
            }

            // X-Forwarded-For if behind proxy, otherwise remote IP
            if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var vals))
            {
                var ip = vals.FirstOrDefault()?.Split(',').FirstOrDefault()?.Trim();
                if (!string.IsNullOrEmpty(ip))
                    return $"ip:{ip}";
            }

            var remote = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            return $"ip:{remote}";
        }
    }

    public static class RateLimitingMiddlewareExtensions
    {
        public static IApplicationBuilder UseSimpleRateLimiting(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RateLimitingMiddleware>();
        }
    }
}