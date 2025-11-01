// Middlewares/AuthMiddleware.cs
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using AlFarabiApi.Models; // تأكد من تعديل المسار حسب مشروعك
using Microsoft.EntityFrameworkCore;

namespace AlFarabiApi.Middlewares
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthMiddleware> _logger;

        public AuthMiddleware(RequestDelegate next, ILogger<AuthMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
    
            var path = context.Request.Path.ToString().ToLower();
            if (path.StartsWith("/api/v1/auth") || path.Contains("/api/v1/GroupUsers/get-groups-by-user-id") )
            {
                await _next(context);
                return;
            }

            // استخراج الـ Token من Header
            var token = context.Request.Headers["token"].ToString();

            if (string.IsNullOrEmpty(token) || token != Models.User.TOKEN)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized: Missing or invalid Authorization header.");
                return;
            }

      
            await _next(context);
        }
    }
}