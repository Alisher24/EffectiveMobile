using EffectiveMobile.Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace EffectiveMobile.Api;

public static class ResponseExtensions
{
    public static ActionResult ToResponse(this ErrorList errorList)
    {
        if (!errorList.Any())
        {
            return new ObjectResult(errorList)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        var distinctErrorTypes = errorList
            .Select(x => x.Type)
            .Distinct()
            .ToList();

        var statusCode = distinctErrorTypes.Count > 1
            ? StatusCodes.Status500InternalServerError
            : GetStatusCode(distinctErrorTypes.First());

        return new ObjectResult(errorList)
        {
            StatusCode = statusCode
        };
    }

    private static int GetStatusCode(ErrorType errorType) => errorType switch
    {
        ErrorType.Validation => StatusCodes.Status400BadRequest,
        ErrorType.Failure => StatusCodes.Status500InternalServerError,

        _ => StatusCodes.Status500InternalServerError
    };
}