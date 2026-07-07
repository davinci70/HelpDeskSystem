using HelpDesk.Application.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.API.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToProblem(this Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Cannot convert a successful result to a problem response.");
        }

        var problem = Results.Problem(statusCode: result.Error.StatusCode);
        var problemDetails = problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem) as ProblemDetails;

        problemDetails!.Extensions = new Dictionary<string, object?>
        {
                {
                    "errors", new[]
                    {
                        result.Error.Code,
                        result.Error.Description
                    }
                }
        };

        return new ObjectResult(problemDetails);
    }

    //public static IActionResult ToProblem<TValue>(this Result<TValue> result, ControllerBase controller)
    //{
    //    if (result.IsSuccess)
    //    {
    //        throw new InvalidOperationException("Cannot convert a successful result to a problem response.");
    //    }

    //    return controller.Problem(
    //        title: result.Error.Code,
    //        detail: result.Error.Description,
    //        statusCode: result.Error.StatusCode ?? StatusCodes.Status400BadRequest);
    //}
}