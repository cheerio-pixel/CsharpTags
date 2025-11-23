using System.Net;
using CsharpTags.Core.Interface;

namespace CsharpTags.Core.Types
{
    public record Str : HtmlElement
    {
        public required string Value { get; set; }

        public override string Render()
        {
            return WebUtility.HtmlEncode(Value);
        }
    }
}
