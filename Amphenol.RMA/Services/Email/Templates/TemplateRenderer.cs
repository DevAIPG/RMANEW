using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Amphenol.RMA.Services.Email.Templates
{
    public interface IEmailTemplateRenderer
    {
        Task<string> RenderAsync<TModel>(string viewPath, TModel model);
    }
    public class EmailTemplateRenderer(IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider, IServiceProvider serviceProvider) : IEmailTemplateRenderer
    {
        public async Task<string> RenderAsync<TModel>(string viewPath, TModel model)
        {
            var httpContext = new DefaultHttpContext
            {
                RequestServices = serviceProvider
            };

            var actionContext = new ActionContext(
            httpContext,
            new RouteData(),
            new ActionDescriptor());

            await using var writer = new StringWriter();

            var viewResult = viewEngine.GetView(
            executingFilePath: null,
            viewPath: viewPath,
            isMainPage: true);

            if (!viewResult.Success)
            {
                throw new InvalidOperationException(
                $"View '{viewPath}' was not found.");
            }

            var viewData = new ViewDataDictionary<TModel>(
            new EmptyModelMetadataProvider(),
            new ModelStateDictionary())
            {
                Model = model
            };

            var viewContext = new ViewContext(
            actionContext,
            viewResult.View,
            viewData,
            new TempDataDictionary(
            httpContext,
            tempDataProvider),
            writer,
            new HtmlHelperOptions());

            await viewResult.View.RenderAsync(viewContext);

            return writer.ToString();
        }
    }
}
