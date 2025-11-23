using CsharpTags.Core.Types;

namespace CsharpTags.Core.Interface
{
    public abstract record HtmlElement
    {
        /// <summary>
        /// Convert this Virtual DOM element into its string representation.
        /// </summary>
        public abstract string Render();

        public static implicit operator HtmlElement(string v) => new Str()
        {
            Value = v
        };

        public static implicit operator HtmlElement(int v) => new Str()
        {
            Value = v.ToString()
        };

        public static implicit operator HtmlElement(long v) => new Str()
        {
            Value = v.ToString()
        };

        public static implicit operator HtmlElement(double v) => new Str()
        {
            Value = v.ToString()
        };

        public static implicit operator HtmlElement(float v) => new Str()
        {
            Value = v.ToString()
        };

        public static implicit operator HtmlElement(decimal v) => new Str()
        {
            Value = v.ToString()
        };

        public static implicit operator HtmlElement(DateTime v) => new Str()
        {
            Value = v.ToString()
        };

        public static implicit operator HtmlElement(Guid v) => new Str()
        {
            Value = v.ToString()
        };
    }
}
