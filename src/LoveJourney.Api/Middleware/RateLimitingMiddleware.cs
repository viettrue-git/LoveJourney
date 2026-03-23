using System.Collections.Concurrent;
using System.Net;

namespace LoveJourney.Api.Middleware;

/// <summary>
/// Simple in-memory sliding window rate limiter per IP.
/// </summary>
public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly int _maxRequests;
    private readonly TimeSpan _window;
    private static readonly ConcurrentDictionary<string, SlidingWindow> _clients = new();

    public RateLimitingMiddleware(RequestDelegate next, int maxRequests = 100, int windowSeconds = 60)
    {
        _next = next;
        _maxRequests = maxRequests;
        _window = TimeSpan.FromSeconds(windowSeconds);
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var window = _clients.GetOrAdd(ip, _ => new SlidingWindow());

        if (!window.TryAdd(_maxRequests, _window))
        {
            context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync("{\"error\":\"Quá nhiều request. Vui lòng thử lại sau.\"}");
            return;
        }

        await _next(context);
    }

    private class SlidingWindow
    {
        private readonly ConcurrentQueue<DateTime> _timestamps = new();
        private readonly object _lock = new();

        public bool TryAdd(int maxRequests, TimeSpan window)
        {
            var now = DateTime.UtcNow;
            var cutoff = now - window;

            // Remove expired entries
            while (_timestamps.TryPeek(out var oldest) && oldest < cutoff)
                _timestamps.TryDequeue(out _);

            lock (_lock)
            {
                if (_timestamps.Count >= maxRequests)
                    return false;

                _timestamps.Enqueue(now);
                return true;
            }
        }
    }
}
