# Global error handling

The `feature/global-error-modal` branch adds HTTP exception handling and a shared error modal to the RMA layout. The same handler runs in development and production. Full exception details are logged on the server; users receive a general message and an error reference matching `HttpContext.TraceIdentifier`.

For a failed form submission or page request, `/Error` renders the error view with the modal open and a link back to RMA requests. The response remains HTTP 500. The original form is not submitted again. For AJAX, JSON, or browser fetch requests, the endpoint returns `application/problem+json` with a safe message and `traceId`.

`global-errors.js` shows the modal for failed jQuery AJAX and fetch requests, JavaScript errors, and unhandled promise rejections. Intentional request cancellation is ignored. Fetch responses remain available to the original caller. The modal keeps the first error reference visible if multiple requests fail together. Messages are inserted as text, never as HTML. Page-level scripts can call `window.RmaErrors.show(message, reference)` for a local error or `window.RmaErrors.showRequestError(xhr, status)` for a failed request. The latter preserves safe server messages and reference IDs and ignores intentional cancellation. Remaining application error alerts use these helpers; debug popups for IDs, filenames, and completion have been removed. After DataTables loads, the layout calls `window.RmaErrors.attachDataTablesHandler()` so its warnings use the modal and technical details remain in the browser console.

Users should check the current request status before retrying, since a failure could occur after approval commits, for example during notification delivery. This handler does not change approval transactions or notification timing.

## Verification

- Run `node tests/global-errors.test.cjs` for the modal regression scenarios.
- Run `dotnet test tests/Amphenol.RMA.ApprovalTests/Amphenol.RMA.ApprovalTests.csproj --filter FullyQualifiedName~GlobalErrorHandlerTests` for HTML and JSON error responses, reference IDs, HTTP status, and private exception protection. These tests do not connect to either database.
- In a development environment, reproduce a failing approval and verify the modal opens, the response status is 500, and the error reference appears in the server log. Dismiss the modal and use the RMA requests link to check the request status.

The implementation follows the ASP.NET Core exception handler pattern: [Microsoft documentation](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-8.0). The error endpoint accepts every HTTP method because the middleware re-executes the failed request using its original method.
