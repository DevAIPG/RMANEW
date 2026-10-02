using Amphenol.RMA.Controllers;
using Amphenol.RMA.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

public class GlobalErrorHandlerTests
{
    private static ErrorController CreateController(bool failed = true)
    {
        var context = new DefaultHttpContext { TraceIdentifier = "test-reference" };
        context.Request.Method = "POST";
        if (failed)
        {
            context.Features.Set<IExceptionHandlerPathFeature>(new ExceptionHandlerFeature
            {
                Error = new InvalidOperationException("Private database information"),
                Path = "/Client/Rma/Aprobar"
            });
        }
        return new ErrorController(NullLogger<ErrorController>.Instance)
        {
            ControllerContext = new ControllerContext { HttpContext = context }
        };
    }

    [Fact]
    public void FailedFormPostRendersErrorViewWithReferenceAndPreservesFailureStatus()
    {
        var controller = CreateController();
        var result = Assert.IsType<ViewResult>(controller.Index());
        Assert.Equal("~/Views/Error.cshtml", result.ViewName);
        Assert.Equal("test-reference", Assert.IsType<ErrorViewModel>(result.Model).RequestId);
        Assert.Equal(500, controller.Response.StatusCode);
        Assert.Equal(ErrorController.ErrorMessage, controller.ViewData["ErrorMessage"]);
    }

    [Theory]
    [InlineData("X-Requested-With", "XMLHttpRequest")]
    [InlineData("Accept", "application/json")]
    [InlineData("Sec-Fetch-Dest", "empty")]
    public void BackgroundRequestReturnsSafeProblemDetails(string header, string value)
    {
        var controller = CreateController();
        controller.Request.Headers[header] = value;
        var result = Assert.IsType<ObjectResult>(controller.Index());
        var problem = Assert.IsType<ProblemDetails>(result.Value);
        Assert.Equal(500, result.StatusCode);
        Assert.Contains("application/problem+json", result.ContentTypes);
        Assert.Equal("test-reference", problem.Extensions["traceId"]);
        Assert.Equal(ErrorController.ErrorMessage, problem.Detail);
        Assert.DoesNotContain("Private database", problem.Detail);
    }

    [Fact]
    public void DirectErrorUrlDoesNotCreateAFakeFailure()
    {
        Assert.IsType<NotFoundResult>(CreateController(false).Index());
    }
}
