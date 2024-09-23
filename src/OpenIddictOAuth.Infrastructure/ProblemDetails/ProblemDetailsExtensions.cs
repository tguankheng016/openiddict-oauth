using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddictOAuth.Infrastructure.Exceptions;

namespace OpenIddictOAuth.Infrastructure.ProblemDetails;

public static class ProblemDetailsExtensions
{
    public static WebApplication UseCustomProblemDetails(this WebApplication app)
    {
        app.UseStatusCodePages(statusCodeHandlerApp =>
        {
            statusCodeHandlerApp.Run(context =>
            {
                context.Response.Redirect($"/Error?statusCode={context.Response.StatusCode}");
                return Task.CompletedTask;
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
                    var openIddictResponse = context.GetOpenIddictServerResponse();

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
                            BadRequestException =>
                            (
                                exceptionType.Message,
                                exceptionType.GetType().Name,
                                context.Response.StatusCode = StatusCodes.Status400BadRequest
                            ),
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

                        var problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails()
                        {
                            Title = details.Title,
                            Detail = details.Detail,
                            Status = details.StatusCode
                        };

                        var problem = new ProblemDetailsContext
                        {
                            HttpContext = context,
                            ProblemDetails = problemDetails
                        };

                        if (app.Environment.IsDevelopment())
                        {
                            problem.ProblemDetails.Extensions.Add("exception", exceptionHandlerFeature?.Error.ToString());
                        }
                        
                        if (context.Request.Path.StartsWithSegments("/connect/authorize"))
                        {
                            context.Response.Redirect($"/Error?error={openIddictResponse?.Error ?? details.Title}&error_description={details.Detail}");
                        }
                        else if (exceptionType is UserFriendlyException)
                        {
                            var userFriendlyException = (UserFriendlyException)exceptionType;
                            context.Response.Redirect($"/Error?error={userFriendlyException.Message}&error_description={userFriendlyException.Details}");
                        }
                        else
                        {
                            await problemDetailsService.WriteAsync(problem);
                        }
                    }
                }
            });
        });

        return app;
    }
}