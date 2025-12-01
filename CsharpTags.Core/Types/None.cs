using System.Runtime.CompilerServices;
using CsharpTags.Core.Interface;

namespace CsharpTags.Core.Types
{
    /// <summary>
    /// if-elseif-else syntax for HtmlElements
    /// </summary>
    public record HtmlConditionalRenderingBuilder : HtmlElement
    {
        /// <summary>
        /// The current element of the conditional rendering builder.
        /// </summary>
        public required HtmlElement Element { get; init; }

        /// <summary>
        /// In case the previous condition failed, check this other one.
        /// </summary>
        public HtmlConditionalRenderingBuilder ElseIf(Func<bool> flag, Func<HtmlElement> element)
            =>
            Element == Prelude.None_ ? flag() ?
            new() { Element = element() } :
            this : this;

        /// <summary>
        /// Provide a default value in case the previous conditions failed.
        /// </summary>
        public HtmlElement Else(Func<HtmlElement> element)
            => Element == Prelude.None_ ? element() : Element;

        /// <inheritdoc/>
        public override string Render()
        {
            return Element.Render();
        }
    }

    public static partial class Prelude
    {
        /// <summary>
        /// Singleton for easy access of the None element.
        /// </summary>
        public static readonly HtmlElement None_ = new RawStr()
        {
            Value = string.Empty
        };

        /// <summary>
        /// Helper for rendering the element if <paramref name="flag" /> is true
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HtmlElement WhenH(bool flag, HtmlElement element)
            => flag ? element : None_;

        /// <summary>
        /// Helper for rendering the element if <paramref name="flag" /> is false
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HtmlElement UnlessH(bool flag, HtmlElement element)
            => flag ? None_ : element;

        /// <summary>
        /// Like WhenH, but starts at conditional rendering builder.
        /// Which means that you can render the element like in a normal
        /// if-elseif-else statement
        /// </summary>
        public static HtmlConditionalRenderingBuilder IfH(bool flag, Func<HtmlElement> element)
            => new() { Element = flag ? element() : None_ };
    }
}
