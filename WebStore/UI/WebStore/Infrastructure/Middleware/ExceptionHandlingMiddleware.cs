using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace WebStore.Infrastructure.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _Next;
        private readonly ILogger<ExceptionHandlingMiddleware> _Logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _Next = next;
            _Logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _Next(context);
            }
            catch (Exception error)
            {

                HandleException(context, error);
                throw;
            }
        }

        private void HandleException(HttpContext context, Exception error)
        {
            _Logger.LogError(error, "Ошибка при выполнении запроса {0}", context.Request.Path);
        }
    }
}
