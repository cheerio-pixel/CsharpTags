using System.Runtime.CompilerServices;
using CsharpTags.Core.Types;

namespace CsharpTags.Core.Interface
{
    /// <summary>
    /// "interface" Representing something that can be represented as html
    /// </summary>
    public abstract record HtmlElement
    {
        /// <summary>
        /// Convert this Virtual DOM element into its string representation.
        /// </summary>
        public abstract string Render();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HtmlElement Transform(Func<HtmlElement, Option<HtmlElement>> map)
        {
            return Zipper<HtmlZipperOps, Tag, HtmlElement>.Transform(this, map);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HtmlElement Transform(IEnumerable<Func<HtmlElement, Option<HtmlElement>>> mappers)
        {
            return Zipper<HtmlZipperOps, Tag, HtmlElement>.Transform(this, mappers);
        }

        /// <summary>
        /// Use Str to convert to HtmlElement
        /// </summary>
        public static implicit operator HtmlElement(string v) => new Str()
        {
            Value = v
        };

        /// <summary>
        /// Use Str to convert to HtmlElement
        /// </summary>
        public static implicit operator HtmlElement(int v) => new Str()
        {
            Value = v.ToString()
        };

        /// <summary>
        /// Use Str to convert to HtmlElement
        /// </summary>
        public static implicit operator HtmlElement(long v) => new Str()
        {
            Value = v.ToString()
        };

        /// <summary>
        /// Use Str to convert to HtmlElement
        /// </summary>
        public static implicit operator HtmlElement(double v) => new Str()
        {
            Value = v.ToString()
        };

        /// <summary>
        /// Use Str to convert to HtmlElement
        /// </summary>
        public static implicit operator HtmlElement(float v) => new Str()
        {
            Value = v.ToString()
        };

        /// <summary>
        /// Use Str to convert to HtmlElement
        /// </summary>
        public static implicit operator HtmlElement(decimal v) => new Str()
        {
            Value = v.ToString()
        };

        /// <summary>
        /// Use Str to convert to HtmlElement
        /// </summary>
        public static implicit operator HtmlElement(DateTime v) => new Str()
        {
            Value = v.ToString()
        };

        /// <summary>
        /// Use Str to convert to HtmlElement
        /// </summary>
        public static implicit operator HtmlElement(Guid v) => new Str()
        {
            Value = v.ToString()
        };

        /// <summary>
        /// Convert seq to element
        /// </summary>
        public static implicit operator HtmlElement(Seq<HtmlElement> v)
            => new HtmlList()
            {
                Value = v
            };

        /// <summary>
        /// Convert list to element
        /// </summary>
        public static implicit operator HtmlElement(List<HtmlElement> v)
            => new HtmlList()
            {
                Value = v
            };
    }
}
