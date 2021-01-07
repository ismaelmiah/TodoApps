using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace API.Todo.Middleware
{
    public class RequestMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            Log.Information($"Request Arrive Time - {DateTime.Now} - {context.Request.Method} - {context.Request.Path}");
            await _next(context);
            Log.Information($"Request Leave Time - {DateTime.Now} - {context.Request.Method} - {context.Request.Path}");
        }
    }
}