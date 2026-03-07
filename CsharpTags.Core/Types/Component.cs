using CsharpTags.Core.Interface;

namespace CsharpTags.Core.Types;

public abstract record Component
    : IHtml, IWrapHtmlElement
{
    protected abstract IHtml Build();

    protected abstract string TagName { get; }


    public Seq<HtmlAttribute> Attributes => Split.Item2;
    public Seq<HtmlElement> Children => Split.Item1;

    private (Seq<HtmlElement>, Seq<HtmlAttribute>)? _split;

#if NET10_0_OR_GREATER
    public Component(params ReadOnlySpan<IHtml> content)
#else
    public Component(params IHtml[] content)
#endif
    {
#if NET10_0_OR_GREATER
        Content = Seq(content);
#else
        Content = Seq<IHtml>(content);
#endif
    }

    /// <summary>
    /// Get the split of children and attributes
    /// </summary>
    private (Seq<HtmlElement>, Seq<HtmlAttribute>) Split => _split ??= Splitter.CalculateSplit(Content);

    /// <summary>
    /// Collection of Attributes and Child elements
    /// </summary>
    public Seq<IHtml> Content
    {
        get;
        init
        {
            // Invalid cache when new content is set
            _split = null;
            field = value;
        }
    }

    /// <inheritdoc/>
    public HtmlElement Simplify() => new Tag()
    {
        TagName = TagName,
        Content = Seq(Build()),
        IsVoid = false,
    };
}
