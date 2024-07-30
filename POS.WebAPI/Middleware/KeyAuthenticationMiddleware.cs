namespace POS.API.Middleware
{
    public class KeyAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        //private const string AuthKeyHeader = "AuthKey";
        //private const string AuthKey = "DemoTrainingKey";

        //private const string AuthKeyHeader = "Authentication:AuthKeyHeader";

        private readonly IConfiguration _configuration;


        public KeyAuthenticationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;

        }

        public async Task InvokeAsync(HttpContext context)
        {
            var AuthKeyHeader = _configuration["Authentication:AuthKeyHeader"];
            var AuthKey = _configuration["Authentication:AuthKey"];

            // Check if the AuthKey header is present
            if (!context.Request.Headers.TryGetValue(AuthKeyHeader, out var authKey))
            {
                // If the AuthKey header is missing, return a 401 Unauthorized response
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("401 Unauthorized Request");
                return;
            }

            // Check if the AuthKey value matches the expected value
            if (!authKey.Equals(AuthKey))
            {
                // If the AuthKey value is invalid, return a 401 Unauthorized response
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("401 Unauthorized Request");
                return;
            }

            // If the AuthKey is valid, proceed to the next middleware in the pipeline
            await _next(context);
        }
    }
}
