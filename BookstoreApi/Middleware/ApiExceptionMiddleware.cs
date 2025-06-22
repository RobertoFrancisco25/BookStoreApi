using System.Net;
using BookstoreApi.Exceptions;
using BookstoreApi.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace BookstoreApi.Middleware;

public static class ApiExceptionMiddleware
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.ContentType = "application/json";
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                if (contextFeature != null)
                {
                    var ex = contextFeature.Error;
                    context.Response.StatusCode = ex switch
                    {
                        NotFoundException _ => StatusCodes.Status404NotFound,
                        BadRequestException _ => StatusCodes.Status400BadRequest,
                        CustomUnauthorizedAccessException _ => StatusCodes.Status401Unauthorized,
                        ConflictException _ => StatusCodes.Status409Conflict,
                        InternalServerErrorException _ => StatusCodes.Status500InternalServerError,
                        _ => StatusCodes.Status500InternalServerError
                    };
                    await context.Response.WriteAsync(new ErrorDetails
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = ex.Message
                    }.ToString());
                }
            });
        });
    }
}