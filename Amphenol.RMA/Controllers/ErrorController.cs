using Amphenol.RMA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Amphenol.RMA.Controllers
{
    [AllowAnonymous]
    [IgnoreAntiforgeryToken]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : Controller
    {
        public const string ErrorMessage = "An error occurred while processing your request. Check the request status before trying again. If the problem continues, contact support with the error reference below.";
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        // Accept every HTTP method: exception handling preserves the failed request's method.
        [Route("/Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index()
        {
            var failure = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (failure == null)
                return NotFound();

            var reference = HttpContext.TraceIdentifier;
            _logger.LogError(failure.Error, "Request failed at {Path}. Error reference: {Reference}", failure.Path, reference);
            Response.StatusCode = StatusCodes.Status500InternalServerError;

            if (string.Equals(Request.Headers["X-Requested-With"], "XMLHttpRequest", StringComparison.OrdinalIgnoreCase)
                || string.Equals(Request.Headers["Sec-Fetch-Dest"], "empty", StringComparison.OrdinalIgnoreCase)
                || Request.Headers.Accept.ToString().Contains("json", StringComparison.OrdinalIgnoreCase))
            {
                var problem = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Unable to complete the request",
                    Detail = ErrorMessage
                };
                problem.Extensions["traceId"] = reference;
                var result = new ObjectResult(problem) { StatusCode = StatusCodes.Status500InternalServerError };
                result.ContentTypes.Add("application/problem+json");
                return result;
            }

            ViewData["ErrorMessage"] = ErrorMessage;
            return View("~/Views/Error.cshtml", new ErrorViewModel { RequestId = reference });
        }
    }
}
