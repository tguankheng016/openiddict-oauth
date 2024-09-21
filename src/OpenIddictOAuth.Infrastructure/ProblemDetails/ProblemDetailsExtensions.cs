using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace OpenIddictOAuth.Infrastructure.ProblemDetails;

public static class ProblemDetailsExtensions
{
    public static WebApplication UseCustomProblemDetails(this WebApplication app)
    {
        app.UseStatusCodePages(statusCodeHandlerApp =>
        {
            statusCodeHandlerApp.Run(async context =>
            {
                context.Response.ContentType = "application/problem+json";

                if (context.RequestServices.GetService<IProblemDetailsService>() is { } problemDetailsService)
                {
                    await problemDetailsService.WriteAsync(new ProblemDetailsContext
                    {
                        HttpContext = context,
                        ProblemDetails =
                        {
                            Detail = ReasonPhrases.GetReasonPhrase(context.Response.StatusCode),
                            Status = context.Response.StatusCode
                        }
                    });
                }
            });
        });

        app.UseExceptionHandler(exceptionHandlerApp =>
        {
            exceptionHandlerApp.Run(async context =>
            {
                context.Response.ContentType = "application/problem+json";

                if (context.RequestServices.GetService<IProblemDetailsService>() is { } problemDetailsService)
                {
                    var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var exceptionType = exceptionHandlerFeature?.Error;

                    if (exceptionType is not null)
                    {
                        (string Detail, string Title, int StatusCode) details = exceptionType switch
                        {
                            // ValidationException validationException =>
                            // (
                            //     exceptionType.Message,
                            //     exceptionType.GetType().Name,
                            //     context.Response.StatusCode = (int)validationException.StatusCode
                            // ),
                            // UnAuthorizedException => 
                            // (
                            //     exceptionType.Message,
                            //     exceptionType.GetType().Name,
                            //     context.Response.StatusCode = StatusCodes.Status401Unauthorized
                            // ),
                            // ForbiddenException => 
                            // (
                            //     exceptionType.Message,
                            //     exceptionType.GetType().Name,
                            //     context.Response.StatusCode = StatusCodes.Status403Forbidden
                            // ),
                            // BadRequestException =>
                            // (
                            //     exceptionType.Message,
                            //     exceptionType.GetType().Name,
                            //     context.Response.StatusCode = StatusCodes.Status400BadRequest
                            // ),
                            // NotFoundException =>
                            // (
                            //     exceptionType.Message,
                            //     exceptionType.GetType().Name,
                            //     context.Response.StatusCode = StatusCodes.Status404NotFound
                            // ),
                            DbUpdateConcurrencyException =>
                            (
                                exceptionType.Message,
                                exceptionType.GetType().Name,
                                context.Response.StatusCode = StatusCodes.Status409Conflict
                            ),
                            _ =>
                            (
                                exceptionType.Message,
                                exceptionType.GetType().Name,
                                context.Response.StatusCode = StatusCodes.Status500InternalServerError
                            )
                        };

                        var problem = new ProblemDetailsContext
                        {
                            HttpContext = context,
                            ProblemDetails =
                            {
                                Title = details.Title,
                                Detail = details.Detail,
                                Status = details.StatusCode
                            }
                        };

                        if (app.Environment.IsDevelopment())
                        {
                            problem.ProblemDetails.Extensions.Add("exception", exceptionHandlerFeature?.Error.ToString());
                        }

                        await problemDetailsService.WriteAsync(problem);
                    }
                }
            });
        });

        return app;
    }
}