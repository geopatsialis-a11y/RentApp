using System;
using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware;

public class ExceptionMiddleware(RequestDelegate next,
ILogger<ExceptionMiddleware> logger, 
IHostEnvironment env)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,"{message}", ex.Message);

            //returning response to client type json
            context.Response.ContentType="application/json";

            //setting status code =500
            context.Response.StatusCode= (int)HttpStatusCode.InternalServerError;//=500 error server

            //different response for development and production
             var detail = env.IsDevelopment()
                ? ex.StackTrace
                : "Internal Server Error";
            var message = ex.InnerException?.Message ?? ex.Message;
            var response = new ApiException(context.Response.StatusCode, message, detail);

            //serialize response to json και απο PascalCase που έχω στην C#  -> σε camelCase που θέλει το javascript
            var option = new JsonSerializerOptions
            {
                PropertyNamingPolicy=JsonNamingPolicy.CamelCase
            };
            var json=JsonSerializer.Serialize(response,option);

            await context.Response.WriteAsync(json);
        }
    }

}
