using CsharpTags.Core.Interface;

namespace CsharpTags.Core.Types;

/// <summary>
/// Base class for creating reusable HTML components with a fluent, functional API.
/// Components encapsulate HTML structure and logic, providing a type-safe way to 
/// build complex UI elements that can be composed together.
/// </summary>
public abstract record Component
    : IHtml, IWrapHtmlElement
{
    /// <summary>
    /// Builds the internal HTML structure of this component.
    /// Override this method to define the component's rendered output.
    /// </summary>
    /// <returns>An IHtml element representing this component's content.</returns>
    protected abstract IHtml Build();

    /// <summary>
    /// Gets the HTML tag name used when this component is simplified.
    /// This tag will wrap the result of <see cref="Build()"/>.
    /// </summary>
    protected abstract string TagName { get; }


    /// <summary>
    /// Gets the sequence of HTML attributes associated with this component.
    /// Attributes are lazily calculated from the component's content.
    /// </summary>
    public Seq<HtmlAttribute> Attributes => Split.Item2;
    
    /// <summary>
    /// Gets the sequence of child HTML elements associated with this component.
    /// Children are lazily calculated from the component's content.
    /// </summary>
    public Seq<HtmlElement> Children => Split.Item1;

    private (Seq<HtmlElement>, Seq<HtmlAttribute>)? _split;

#if NET10_0_OR_GREATER
    /// <summary>
    /// Initializes a new instance of the <see cref="Component"/> class with the specified content.
    /// </summary>
    /// <param name="content">The initial content (attributes and child elements) for this component.</param>
    public Component(params ReadOnlySpan<IHtml> content)
#else
    /// <summary>
    /// Initializes a new instance of the <see cref="Component"/> class with the specified content.
    /// </summary>
    /// <param name="content">The initial content (attributes and child elements) for this component.</param>
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
