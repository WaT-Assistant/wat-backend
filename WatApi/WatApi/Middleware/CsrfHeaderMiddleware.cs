namespace WatApi.Middleware
{
    public class CsrfHeaderMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            // Only apply CSRF checks to mutating HTTP methods
            if (HttpMethods.IsPost(context.Request.Method) ||
                HttpMethods.IsPut(context.Request.Method) ||
                HttpMethods.IsDelete(context.Request.Method) ||
                HttpMethods.IsPatch(context.Request.Method))
            {
                // The frontend MUST send this custom header on every POST/PUT/DELETE
                if (!context.Request.Headers.TryGetValue("X-CSRF", out var value))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Missing CSRF protection header.");
                    return;
                }

                var headerValue = value.ToString();
                if (headerValue != "1") // It can be any static value, the presence of the header is what matters
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Invalid CSRF protection header.");
                    return;
                }
            }

            await _next(context);
        }
    }
}