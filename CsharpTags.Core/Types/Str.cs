using System.Net;
using CsharpTags.Core.Interface;

namespace CsharpTags.Core.Types
{
    /// <summary>
    /// A simple, html encoded string. Wrapper for all values that are not HtmlElement.
    /// </summary>
    public record Str : HtmlElement
    {
        /// <summary>
        /// Inner value of this string element.
        /// </summary>
        public required string Value { get; set; }

        /// <inheritdoc/>
        public override string Render()
        {
            return WebUtility.HtmlEncode(Value);
        }
    }
}
