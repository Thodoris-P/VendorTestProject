using Microsoft.AspNetCore.Mvc;
using WebApi.Exceptions;

namespace WebApi.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (VendorNotFoundException ex)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            context.Response.ContentType = "application/problem+json";
            var problem = new ProblemDetails
            {
                Type   = "https://example.com/problems/missing-id",
                Title  = "Vendor not found",
                Status = StatusCodes.Status404NotFound,
                Detail = ex.Message
            };
            await context.Response.WriteAsJsonAsync(problem);
        }
        catch (BadRequestException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/problem+json";
            var problem = new ProblemDetails
            {
                Type   = "https://example.com/problems/missing-id",
                Title  = "Missing identifier",
                Status = StatusCodes.Status400BadRequest,
                Detail = ex.Message
            };
            await context.Response.WriteAsJsonAsync(problem);
        }
        catch (ConflictingIdException ex)
        {
            context.Response.StatusCode = StatusCodes.Status409Conflict;
            context.Response.ContentType = "application/problem+json";
            var problem = new ProblemDetails
            {
                Type   = "https://example.com/problems/duplicate-id",
                Title  = "Identifier already exists",
                Status = StatusCodes.Status409Conflict,
                Detail = ex.Message
            };
            await context.Response.WriteAsJsonAsync(problem);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/problem+json";
            var problem = new ProblemDetails
            {
                Type   = "https://example.com/problems/internal-error",
                Title  = "Internal Server Error",
                Status = StatusCodes.Status500InternalServerError,
                Detail = ex.Message
            };
            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}