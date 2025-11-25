using CsharpTags.Core.Interface;

namespace CsharpTags.Core.Types
{
    /// <summary>
    /// The element that renders to nothihing, useful for conditional rendering
    /// </summary>
    public record NoneElement : HtmlElement
    {
        /// <inheritdoc/>
        public override string Render()
        {
            return string.Empty;
        }
    }

    public static partial class Prelude
    {
        /// <summary>
        /// Singleton for easy access of the None element.
        /// </summary>
        public static readonly HtmlElement None_ = new NoneElement();

        /// <summary>
        /// Helper for rendering the element if <paramref name="flag" /> is true
        /// </summary>
        public static HtmlElement RenderWhen(bool flag, HtmlElement element)
            => flag ? element : None_;
    }
}
