using System.Text;
using CsharpTags.Core.Interface;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;

namespace CsharpTags.AspNetCore
{
    public static class Extensions
    {
        public static IActionResult ToActionResult(this HtmlElement self)
        {
            return new ContentResult()
            {
                Content = self.Render(),
                ContentType = "text/html; charset=utf-8",
                StatusCode = 200
            };
        }

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
