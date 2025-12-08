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

        /// <summary>
        /// Transforms the HTML element tree by applying a transformation function to each element.
        /// Uses a zipper data structure to efficiently traverse and modify the tree.
        /// </summary>
        /// <param name="map">A function that takes an HtmlElement and returns an Option&lt;HtmlElement&gt;.
        /// The transformation is applied to each element in the tree. If the function returns None,
        /// the element is removed; if it returns Some, the element is replaced with the new value.</param>
        /// <returns>A new HtmlElement tree with the transformations applied.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HtmlElement Transform(Func<HtmlElement, Option<HtmlElement>> map)
        {
            return Zipper<HtmlZipperOps, Tag, HtmlElement>.Transform(this, map);
        }

        /// <summary>
        /// Transforms the HTML element tree by applying multiple transformation functions sequentially.
        /// Each transformation is applied to the result of the previous transformation.
        /// </summary>
        /// <param name="mappers">A collection of transformation functions to apply sequentially.
        /// Each function takes an HtmlElement and returns an Option&lt;HtmlElement&gt;.
        /// Transformations are applied in the order they appear in the collection.</param>
        /// <returns>A new HtmlElement tree with all transformations applied sequentially.</returns>
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
