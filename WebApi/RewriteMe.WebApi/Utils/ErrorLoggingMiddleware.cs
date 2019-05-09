using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RewriteMe.Domain.Interfaces.Services;

namespace RewriteMe.WebApi.Utils
{
    public class ErrorLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IApplicationLogService _applicationLogService;

        public ErrorLoggingMiddleware(
            RequestDelegate next,
            IApplicationLogService applicationLogService)
        {
            _next = next;
            _applicationLogService = applicationLogService;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var message = JsonConvert.SerializeObject(ex);
                await _applicationLogService.ErrorAsync(message).ConfigureAwait(false);

                throw;
            }
        }
    }
}
