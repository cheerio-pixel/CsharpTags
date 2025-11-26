using CsharpTags.AspNetCore.Filters;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Builder
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class RouteHandlerBuilderExtensions
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        /// <summary>
        /// Adds HTML content support to the endpoint
        /// </summary>
        public static RouteHandlerBuilder ProducesHtml(this RouteHandlerBuilder builder)
        {
            return builder
                .AddEndpointFilter<HtmlElementEndpointFilter>()
                .Produces<string>(StatusCodes.Status200OK, "text/html");
        }

        /// <summary>
        /// Adds HTML content support with specific status code
        /// </summary>
        public static RouteHandlerBuilder ProducesHtml(this RouteHandlerBuilder builder, int statusCode)
        {
            return builder
                .AddEndpointFilter<HtmlElementEndpointFilter>()
                .Produces<string>(statusCode, "text/html");
        }
    }
}
