using CsharpTags.Core.Interface;

namespace CsharpTags.Core.Types
{
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
        public static HtmlElement RenderWhen(bool flag, HtmlElement element)
            => flag ? element : None_;
    }
}
