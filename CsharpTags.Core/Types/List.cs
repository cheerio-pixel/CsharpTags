using System.Text;
using CsharpTags.Core.Interface;

namespace CsharpTags.Core.Types
{
    /// <summary>
    /// Represents an HTML list element that aggregates multiple <see cref="HtmlElement"/> instances.
    /// </summary>
    public record List : HtmlElement
    {
        /// <summary>
        /// Gets or sets the collection of HTML elements that comprise the list.
        /// </summary>
        public required IEnumerable<HtmlElement> Value { get; set; }

        /// <inheritdoc/>
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
