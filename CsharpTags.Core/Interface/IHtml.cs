using CsharpTags.Core.Types;

namespace CsharpTags.Core.Interface
{
    /// <summary>
    /// Marker interface for html parts
    /// </summary>
    public abstract record IHtml
    {
        /// <summary>
        /// Use Str to convert to element
        /// </summary>
        public static implicit operator IHtml(string v) => new Str()
        {
            Value = v
        };

        /// <summary>
        /// Use Str to convert to element
        /// </summary>
        public static implicit operator IHtml(int v) => new Str()
        {
            Value = v.ToString()
        };

        /// <summary>
        /// Use Str to convert to element
        /// </summary>
        public static implicit operator IHtml(long v) => new Str()
        {
            Value = v.ToString()
        };

        /// <summary>
        /// Use Str to convert to element
        /// </summary>
        public static implicit operator IHtml(double v) => new Str()
        {
            Value = v.ToString()
        };

        /// <summary>
        /// Use Str to convert to element
        /// </summary>
        public static implicit operator IHtml(float v) => new Str()
        {
            Value = v.ToString()
        };

        /// <summary>
        /// Use Str to convert to element
        /// </summary>
        public static implicit operator IHtml(decimal v) => new Str()
        {
            Value = v.ToString()
        };

        /// <summary>
        /// Use Str to convert to element
        /// </summary>
        public static implicit operator IHtml(DateTime v) => new Str()
        {
            Value = v.ToString()
        };

        /// <summary>
        /// Use Str to convert to element
        /// </summary>
        public static implicit operator IHtml(Guid v) => new Str()
        {
            Value = v.ToString()
        };

        /// <summary>
        /// Convert seq to element
        /// </summary>
        public static implicit operator IHtml(Seq<HtmlElement> v)
            => new HtmlElementList()
            {
                Value = v
            };

        /// <summary>
        /// Convert list to element
        /// </summary>
        public static implicit operator IHtml(List<HtmlElement> v)
            => new HtmlElementList()
            {
                Value = Seq(v.AsEnumerable())
            };

        /// <summary>
        /// Convert seq to element
        /// </summary>
        public static implicit operator IHtml(Seq<HtmlAttribute> v)
            => new ListAttribute()
            {
                Attributes = v
            };

        /// <summary>
        /// Convert list to element
        /// </summary>
        public static implicit operator IHtml(List<HtmlAttribute> v)
            => new ListAttribute()
            {
                Attributes = Seq(v.AsEnumerable())
            };

        /// <summary>
        /// Convert seq to element
        /// </summary>
        public static implicit operator IHtml(Seq<IHtml> v)
            => new HtmlList()
            {
                Value = v
            };

        /// <summary>
        /// Convert list to element
        /// </summary>
        public static implicit operator IHtml(List<IHtml> v)
            => new HtmlList()
            {
                Value = Seq(v.AsEnumerable())
            };
    }

    /// <summary>
    /// List of IHtml element
    /// </summary>
    public record HtmlList : IHtml
    {
        /// <summary>
        /// Inner html
        /// </summary>
        public Seq<IHtml> Value { get; init; }
    }
}
