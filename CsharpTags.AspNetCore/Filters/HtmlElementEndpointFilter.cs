using System.Text;
using CsharpTags.Core.Interface;
using Microsoft.AspNetCore.Http;

namespace CsharpTags.AspNetCore.Filters
{
    /// <summary>
    /// An endpoint filter that automatically converts <see cref="HtmlElement"/> return values
    /// into proper HTTP responses with HTML content type.
    /// </summary>
    /// <remarks>
    /// This filter intercepts endpoint execution and checks if the return value is an <see cref="HtmlElement"/>.
    /// If so, it renders the HTML element to a string and returns an <see cref="Results.Content(string?, string?, Encoding?)"/> response
    /// with the appropriate HTML content type and UTF-8 encoding.
    ///
    /// This enables endpoints to return <see cref="HtmlElement"/> objects directly while ensuring
    /// the client receives properly formatted HTML responses.
    /// </remarks>
    public class HtmlElementEndpointFilter : IEndpointFilter
    {
        /// <summary>
        /// Invokes the endpoint filter to process the request and response.
        /// </summary>
        /// <param name="context">The context containing information about the current endpoint invocation.</param>
        /// <param name="next">The delegate representing the next filter in the pipeline or the endpoint itself.</param>
        /// <returns>
        /// A <see cref="ValueTask{T}"/> that completes with the filtered result.
        /// Returns an HTML content response if the result is an <see cref="HtmlElement"/>,
        /// otherwise returns the original result unchanged.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="context"/> or <paramref name="next"/> is null.</exception>
        public async ValueTask<object?> InvokeAsync(
            EndpointFilterInvocationContext context,
            EndpointFilterDelegate next)
        {
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(next);

            var result = await next(context);

            if (result is HtmlElement htmlElement)
            {
                return Results.Content(
                    htmlElement.Render(),
                    "text/html",
                    Encoding.UTF8
                );
            }

            return result;
        }
    }
}
