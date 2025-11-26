using System.Net;
using CsharpTags.Core.Interface;

namespace CsharpTags.Core.Types
{
    public record List : HtmlElement
    {
        public required IEnumerable<HtmlElement> Value { get; set; }

        public override string Render()
        {
            return Value.Aggregate("" , (acc, it) => acc + it.Render());
        }
    }

    public static partial class Prelude
    {
        /// <summary>
        /// Constructor of an Array of elements.
        /// </summary>
        public static HtmlElement HList(params ReadOnlySpan<HtmlElement> element)
            => Seq( element).ToHtml();
        public static HtmlElement ToHtml(this IEnumerable<HtmlElement> self)
            => new List()
            {
                Value = self
            };
    }
}
