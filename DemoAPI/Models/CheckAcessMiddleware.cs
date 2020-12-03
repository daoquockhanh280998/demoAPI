using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoAPI.Models
{
    public class CheckAcessMiddleware
    {
        private readonly RequestDelegate _next;

        public CheckAcessMiddleware(RequestDelegate next) /*=> _next = next;*/
        {
            _next = next;
        }

        private ImportResponse ImportResponse = new ImportResponse();

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Response.StatusCode != 200)
            {
                Console.WriteLine("lỗi user");
                await Task.Run(
                  async () =>
                  {
                      string link = ImportResponse.Result.ToString();
                      httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                      await httpContext.Response.WriteAsync(link);
                  }
                );
            }
            else
            {
                // Thiết lập Header cho HttpResponse
                httpContext.Response.Headers.Add("throughCheckAcessMiddleware", new[] { DateTime.Now.ToString() });

                Console.WriteLine("CheckAcessMiddleware: Cho truy cập");

                // Chuyển Middleware tiếp theo trong pipeline
                await _next(httpContext);
            }
        }
    }
}