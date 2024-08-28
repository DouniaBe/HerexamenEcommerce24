using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace HerexamenEcommerce2024.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Voorbeeld: lees cookie-informatie
            var cookieValue = context.Request.Cookies["SessionCookie"];

            if (!string.IsNullOrEmpty(cookieValue))
            {
                // Hier kun je logica toevoegen om met sessie-informatie te werken
            }

            // Roep de volgende middleware aan in de pipeline
            await _next(context);
        }
    }
}
