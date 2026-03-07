using CsharpTags.Core.Interface;
using LanguageExt;

using static CsharpTags.Core.Types.Prelude;

namespace CsharpTags.Core.Types;

public abstract record Component
    : IHtml, IWrapHtmlElement
{
    protected abstract HtmlElement Build();

    protected abstract string TagName { get; }


    public Seq<HtmlAttribute> Attributes => Split.Item2;
    public Seq<HtmlElement> Children => Split.Item1;

    private (Seq<HtmlElement>, Seq<HtmlAttribute>)? _split;

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

    public HtmlElement Simplify() => Build();

#if NET10_0_OR_GREATER
    public Tag New(params ReadOnlySpan<IHtml> content)
#else
    public HtmlElement New(params IHtml[] content)
#endif
    {
        HtmlElement? result = content.Length == 0
            ? Build()
            : (this with {
                Content = Seq<IHtml>(content)
            }).Build();
        return new Tag()
        {
            TagName = TagName,
            Content = Seq<IHtml>(result),
            IsVoid = false,
        };
    }
}

