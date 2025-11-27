using System.Text;
using CsharpTags.Core.Interface;

namespace CsharpTags.Core.Types
{
    /// <summary>
    /// Represents an HTML list element that aggregates multiple <see cref="HtmlElement"/> instances.
    /// </summary>
    public record HtmlList : HtmlElement
    {
        /// <summary>
        /// Gets or sets the collection of HTML elements that comprise the list.
        /// </summary>
        public required IEnumerable<HtmlElement> Value { get; set; }

        /// <inheritdoc/>
        public override string Render()
        {
            return Value.Aggregate(
                    new StringBuilder(),
                    (acc, it) => acc.Append(it.Render()))
                .ToString();
        }
    }

    public static partial class Prelude
    {
        /// <summary>
        /// Constructor of an Array of elements.
        /// </summary>
        public static HtmlElement HList(params ReadOnlySpan<HtmlElement> element)
            // Is more efficient to use a seq because in the implementation of Tag,
            // using List gets converted to Seq, so the conversion doesn't happen.
            => Seq(element).ToHtml();

        /// <summary>
        /// Convert <see cref="IEnumerable{T}"/> to List HtmlElement.
        /// </summary>
        public static HtmlElement ToHtml(this IEnumerable<HtmlElement> self)
            => new HtmlList()
            {
                Value = self
            };
    }
}
