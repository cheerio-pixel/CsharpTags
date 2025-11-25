using CsharpTags.Core.Interface;

namespace CsharpTags.Core.Types
{
    public record RawStr : HtmlElement
    {
        public required string Value { get; set; }

        /// <inheritdoc/>
        public override string Render()
        {
            return Value;
        }
    }

    public static partial class Prelude
    {
        /// <summary>
        /// Render string as pure html without encoding.
        /// </summary>
        public static HtmlElement RawStr(string v)
            => new RawStr() { Value = v };
    }
}
