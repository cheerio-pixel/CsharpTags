using System.Text;
using CsharpTags.Core.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CsharpTags.AspNetCore
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class Extensions
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        /// <summary>
        /// Converts an <see cref="HtmlElement"/> to an ASP.NET Core <see cref="IActionResult"/> for use in MVC controllers.
        /// </summary>
        /// <param name="self">The HTML element to convert.</param>
        /// <returns>
        /// A <see cref="ContentResult"/> with the rendered HTML content,
        /// content type set to "text/html; charset=utf-8", and HTTP status code 200.
        /// </returns>
        public static IActionResult ToActionResult(this HtmlElement self)
        {
            return new ContentResult()
            {
                Content = self.Render(),
                ContentType = "text/html; charset=utf-8",
                StatusCode = 200
            };
        }

        /// <summary>
        /// Converts an <see cref="HtmlElement"/> to an ASP.NET Core <see cref="IResult"/> for use in minimal APIs.
        /// </summary>
        /// <param name="self">The HTML element to convert.</param>
        /// <returns>
        /// An <see cref="IResult"/> with the rendered HTML content,
        /// content type set to "text/html", and UTF-8 encoding.
        /// </returns>
        public static IResult ToResult(this HtmlElement self)
        {
            return Results.Content(
                 self.Render(),
                 "text/html",
                 Encoding.UTF8
                );
        }
    }
}
