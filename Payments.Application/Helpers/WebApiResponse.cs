using Microsoft.AspNetCore.Mvc;
using Payments.Application.Result;
using System.Net;

namespace Payments.Application.Helpers
{
    public class WebApiResponse
    {
        /// <summary>
        /// Creates an ActionResult from a service Result
        /// </summary>
        /// <returns>The action result.</returns>
        /// <param name="result">Service Result.</param>
        /// <typeparam name="T">The data type of the Result.</typeparam>
        public static ActionResult GetErrorResponse<T>(Result<T> result) where T : class
        {
            return result.ResultType switch
            {
                ResultType.Unexpected => new StatusCodeResult((int)HttpStatusCode.InternalServerError),
                ResultType.NotFound => new NotFoundObjectResult(new { Success = false, result.Errors }),
                ResultType.Unauthorized => new UnauthorizedObjectResult(new { Success = false, result.Errors }),
                ResultType.Invalid => new BadRequestObjectResult(new { Success = false, result.Errors }),
                _ => throw new Exception("An unhandled result has occurred as a result of a service call."),
            };
        }
    }
}
