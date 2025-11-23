using System.Text;
using CsharpTags.Core.Interface;

namespace CsharpTags.Core.Types
{
    public record Tag : HtmlElement
    {
        public required string TagName { get; init; }
        public required bool IsVoid { get; init; }
        public Seq<IHtmlAttribute> Attributes { get; init; } = Seq<IHtmlAttribute>();
        public Seq<HtmlElement> Children { get; init; } = Seq<HtmlElement>();

        /// <summary>
        /// Set the attributes of this tag
        /// </summary>
        public Tag Attr(params IHtmlAttribute[] attrs)
         => this with
         {
             Attributes = Seq<IHtmlAttribute>(attrs)
         };

        /// <summary>
        /// Set the children of this tag
        /// </summary>
        public Tag Child(params HtmlElement[] children)
            => this with
            {
                Children = Seq<HtmlElement>(children)
            };

        /// <summary>
        /// Append the attributes to the already existing sequence of attributes of this tag
        /// </summary>
        public Tag AppendAttr(params IHtmlAttribute[] attrs)
         => this with
         {
             Attributes = Attributes.Concat(attrs)
         };

        /// <summary>
        /// Append the children to the already existing sequence of children of this tag
        /// </summary>
        public Tag AppendChild(params HtmlElement[] children)
            => this with
            {
                Children = Children.Concat(children)
            };

        public override string Render()
        {
            var sb = new StringBuilder();
            var stack = new Stack<RenderState>();
            stack.Push(new RenderState()
            {
                Tag = this,
                RemainingChildren = Children
            });

            while (stack.Count > 0)
            {
                var state = stack.Peek();

                if (state.Phase == RenderPhase.Start)
                {
                    sb.Append('<').Append(state.Tag.TagName);

                    foreach (var attribute in state.Tag.Attributes)
                    {
                        sb.Append(' ').Append(attribute.Render());
                    }

                    if (state.Tag.IsVoid)
                    {
                        sb.Append(" />");
                        stack.Pop();
                        continue;
                    }
                    else
                    {
                        sb.Append('>');
                        state.Phase = RenderPhase.Children;
                    }
                }

                if (state.Phase == RenderPhase.Children)
                {
                    if (!state.RemainingChildren.IsEmpty)
                    {
                        var child = (HtmlElement)state.RemainingChildren.Head;
                        state.RemainingChildren = state.RemainingChildren.Tail;

                        if (child is Tag childTag)
                        {
                            stack.Push(new RenderState()
                            {
                                Tag = childTag,
                                RemainingChildren = childTag.Children
                            });
                        }
                        else if (child is List childList)
                        {
                            state.RemainingChildren = Seq(childList.Value)
                                .Concat(state.RemainingChildren);
                        }
                        else
                        {
                            sb.Append(child.Render());
                        }
                    }
                    else
                    {
                        state.Phase = RenderPhase.End;
                    }
                }

                if (state.Phase == RenderPhase.End)
                {
                    sb.Append("</").Append(state.Tag.TagName).Append('>');
                    stack.Pop();
                }
            }

            return sb.ToString();
        }

        private class RenderState
        {
            public required Tag Tag { get; init; }
            public Seq<HtmlElement> RemainingChildren { get; set; } = Seq<HtmlElement>();
            public RenderPhase Phase { get; set; } = RenderPhase.Start;
        }

        private enum RenderPhase
        {
            Start,
            Children,
            End
        }
    }

    public static partial class Prelude
    {
        #region Document Metadata

        /// <summary>
        /// The &lt;html&gt; HTML element represents the root (top-level element) of an HTML document.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/html
        /// </remarks>

        public static readonly Tag Html = new() { TagName = "html", IsVoid = false };

        /// <summary>
        /// The &lt;head&gt; HTML element contains machine-readable information (metadata) about the document.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/head
        /// </remarks>

        public static readonly Tag Head = new() { TagName = "head", IsVoid = false };

        /// <summary>
        /// The &lt;title&gt; HTML element defines the document's title that is shown in a browser's title bar or a page's tab.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/title
        /// </remarks>

        public static readonly Tag Title = new() { TagName = "title", IsVoid = false };

        /// <summary>
        /// The &lt;base&gt; HTML element specifies the base URL to use for all relative URLs in a document.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/base
        /// </remarks>

        public static readonly Tag Base_ = new() { TagName = "base", IsVoid = true };

        /// <summary>
        /// The &lt;link&gt; HTML element specifies relationships between the current document and an external resource.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/link
        /// </remarks>

        public static readonly Tag Link = new() { TagName = "link", IsVoid = true };

        /// <summary>
        /// The &lt;meta&gt; HTML element represents metadata that cannot be represented by other HTML meta-related elements.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/meta
        /// </remarks>

        public static readonly Tag Meta = new() { TagName = "meta", IsVoid = true };

        /// <summary>
        /// The &lt;style&gt; HTML element contains style information for a document, or part of a document.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/style
        /// </remarks>

        public static readonly Tag Style_ = new() { TagName = "style", IsVoid = false };

        #endregion

        #region Sectioning Root

        /// <summary>
        /// The &lt;body&gt; HTML element represents the content of an HTML document.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/body
        /// </remarks>

        public static readonly Tag Body = new() { TagName = "body", IsVoid = false };

        #endregion

        #region Content Sectioning

        /// <summary>
        /// The &lt;article&gt; HTML element represents a self-contained composition in a document, page, application, or site.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/article
        /// </remarks>

        public static readonly Tag Article = new() { TagName = "article", IsVoid = false };

        /// <summary>
        /// The &lt;section&gt; HTML element represents a generic standalone section of a document.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/section
        /// </remarks>

        public static readonly Tag Section = new() { TagName = "section", IsVoid = false };

        /// <summary>
        /// The &lt;nav&gt; HTML element represents a section of a page whose purpose is to provide navigation links.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/nav
        /// </remarks>

        public static readonly Tag Nav = new() { TagName = "nav", IsVoid = false };

        /// <summary>
        /// The &lt;aside&gt; HTML element represents a portion of a document whose content is only indirectly related to the main content.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/aside
        /// </remarks>

        public static readonly Tag Aside = new() { TagName = "aside", IsVoid = false };

        /// <summary>
        /// The &lt;h1&gt; HTML element represents a level 1 section heading.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/Heading_Elements
        /// </remarks>

        public static readonly Tag H1 = new() { TagName = "h1", IsVoid = false };

        /// <summary>
        /// The &lt;h2&gt; HTML element represents a level 2 section heading.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/Heading_Elements
        /// </remarks>

        public static readonly Tag H2 = new() { TagName = "h2", IsVoid = false };

        /// <summary>
        /// The &lt;h3&gt; HTML element represents a level 3 section heading.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/Heading_Elements
        /// </remarks>

        public static readonly Tag H3 = new() { TagName = "h3", IsVoid = false };

        /// <summary>
        /// The &lt;h4&gt; HTML element represents a level 4 section heading.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/Heading_Elements
        /// </remarks>

        public static readonly Tag H4 = new() { TagName = "h4", IsVoid = false };

        /// <summary>
        /// The &lt;h5&gt; HTML element represents a level 5 section heading.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/Heading_Elements
        /// </remarks>

        public static readonly Tag H5 = new() { TagName = "h5", IsVoid = false };

        /// <summary>
        /// The &lt;h6&gt; HTML element represents a level 6 section heading.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/Heading_Elements
        /// </remarks>

        public static readonly Tag H6 = new() { TagName = "h6", IsVoid = false };

        /// <summary>
        /// The &lt;header&gt; HTML element represents introductory content, typically a group of introductory or navigational aids.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/header
        /// </remarks>

        public static readonly Tag Header = new() { TagName = "header", IsVoid = false };

        /// <summary>
        /// The &lt;footer&gt; HTML element represents a footer for its nearest ancestor sectioning content or sectioning root element.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/footer
        /// </remarks>

        public static readonly Tag Footer = new() { TagName = "footer", IsVoid = false };

        /// <summary>
        /// The &lt;address&gt; HTML element indicates that the enclosed HTML provides contact information for a person or people, or for an organization.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/address
        /// </remarks>

        public static readonly Tag Address = new() { TagName = "address", IsVoid = false };

        /// <summary>
        /// The &lt;main&gt; HTML element represents the dominant content of the body of a document.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/main
        /// </remarks>

        public static readonly Tag Main = new() { TagName = "main", IsVoid = false };

        #endregion

        #region Text Content

        /// <summary>
        /// The &lt;div&gt; HTML element is the generic container for flow content. It has no effect on the content or layout until styled in some way using CSS.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/div
        /// </remarks>

        public static readonly Tag Div = new() { TagName = "div", IsVoid = false };

        /// <summary>
        /// The &lt;p&gt; HTML element represents a paragraph.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/p
        /// </remarks>

        public static readonly Tag P = new() { TagName = "p", IsVoid = false };

        /// <summary>
        /// The &lt;hr&gt; HTML element represents a thematic break between paragraph-level elements.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/hr
        /// </remarks>

        public static readonly Tag Hr = new() { TagName = "hr", IsVoid = true };

        /// <summary>
        /// The &lt;pre&gt; HTML element represents preformatted text which is to be presented exactly as written in the HTML file.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/pre
        /// </remarks>

        public static readonly Tag Pre = new() { TagName = "pre", IsVoid = false };

        /// <summary>
        /// The &lt;blockquote&gt; HTML element indicates that the enclosed text is an extended quotation.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/blockquote
        /// </remarks>

        public static readonly Tag Blockquote = new() { TagName = "blockquote", IsVoid = false };

        /// <summary>
        /// The &lt;ol&gt; HTML element represents an ordered list of items — typically rendered as a numbered list.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/ol
        /// </remarks>

        public static readonly Tag Ol = new() { TagName = "ol", IsVoid = false };

        /// <summary>
        /// The &lt;ul&gt; HTML element represents an unordered list of items, typically rendered as a bulleted list.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/ul
        /// </remarks>

        public static readonly Tag Ul = new() { TagName = "ul", IsVoid = false };

        /// <summary>
        /// The &lt;li&gt; HTML element is used to represent an item in a list.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/li
        /// </remarks>

        public static readonly Tag Li = new() { TagName = "li", IsVoid = false };

        /// <summary>
        /// The &lt;dl&gt; HTML element represents a description list.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/dl
        /// </remarks>

        public static readonly Tag Dl = new() { TagName = "dl", IsVoid = false };

        /// <summary>
        /// The &lt;dt&gt; HTML element specifies a term in a description or definition list.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/dt
        /// </remarks>

        public static readonly Tag Dt = new() { TagName = "dt", IsVoid = false };

        /// <summary>
        /// The &lt;dd&gt; HTML element provides the description, definition, or value for the preceding term (&lt;dt&gt;) in a description list (&lt;dl&gt;).
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/dd
        /// </remarks>

        public static readonly Tag Dd = new() { TagName = "dd", IsVoid = false };

        /// <summary>
        /// The &lt;figure&gt; HTML element represents self-contained content, potentially with an optional caption, which is specified using the &lt;figcaption&gt; element.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/figure
        /// </remarks>

        public static readonly Tag Figure = new() { TagName = "figure", IsVoid = false };

        /// <summary>
        /// The &lt;figcaption&gt; HTML element represents a caption or legend describing the rest of the contents of its parent &lt;figure&gt; element.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/figcaption
        /// </remarks>

        public static readonly Tag Figcaption = new() { TagName = "figcaption", IsVoid = false };

        #endregion

        #region Inline Text

        /// <summary>
        /// The &lt;a&gt; HTML element (or anchor element), with its href attribute, creates a hyperlink to web pages, files, email addresses, locations in the same page, or anything else a URL can address.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/a
        /// </remarks>

        public static readonly Tag A = new() { TagName = "a", IsVoid = false };

        /// <summary>
        /// The &lt;em&gt; HTML element marks text that has stress emphasis.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/em
        /// </remarks>

        public static readonly Tag Em = new() { TagName = "em", IsVoid = false };

        /// <summary>
        /// The &lt;strong&gt; HTML element indicates that its contents have strong importance, seriousness, or urgency.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/strong
        /// </remarks>

        public static readonly Tag Strong = new() { TagName = "strong", IsVoid = false };

        /// <summary>
        /// The &lt;small&gt; HTML element represents side-comments and small print, like copyright and legal text.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/small
        /// </remarks>

        public static readonly Tag Small = new() { TagName = "small", IsVoid = false };

        /// <summary>
        /// The &lt;s&gt; HTML element renders text with a strikethrough, or a line through it.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/s
        /// </remarks>

        public static readonly Tag S = new() { TagName = "s", IsVoid = false };

        /// <summary>
        /// The &lt;cite&gt; HTML element is used to mark up the title of a cited creative work.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/cite
        /// </remarks>

        public static readonly Tag Cite_ = new() { TagName = "cite", IsVoid = false };

        /// <summary>
        /// The &lt;q&gt; HTML element indicates that the enclosed text is a short inline quotation.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/q
        /// </remarks>

        public static readonly Tag Q = new() { TagName = "q", IsVoid = false };

        /// <summary>
        /// The &lt;dfn&gt; HTML element is used to indicate the term being defined within the context of a definition phrase or sentence.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/dfn
        /// </remarks>

        public static readonly Tag Dfn = new() { TagName = "dfn", IsVoid = false };

        /// <summary>
        /// The &lt;abbr&gt; HTML element represents an abbreviation or acronym.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/abbr
        /// </remarks>

        public static readonly Tag Abbr = new() { TagName = "abbr", IsVoid = false };

        /// <summary>
        /// The &lt;data&gt; HTML element links a given piece of content with a machine-readable translation.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/data
        /// </remarks>

        public static readonly Tag Data_ = new() { TagName = "data", IsVoid = false };

        /// <summary>
        /// The &lt;time&gt; HTML element represents a specific period in time.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/time
        /// </remarks>

        public static readonly Tag Time = new() { TagName = "time", IsVoid = false };

        /// <summary>
        /// The &lt;code&gt; HTML element displays its contents styled in a fashion intended to indicate that the text is a short fragment of computer code.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/code
        /// </remarks>

        public static readonly Tag Code = new() { TagName = "code", IsVoid = false };

        /// <summary>
        /// The &lt;var&gt; HTML element represents the name of a variable in a mathematical expression or a programming context.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/var
        /// </remarks>

        public static readonly Tag Var = new() { TagName = "var", IsVoid = false };

        /// <summary>
        /// The &lt;samp&gt; HTML element is used to enclose inline text which represents sample (or quoted) output from a computer program.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/samp
        /// </remarks>

        public static readonly Tag Samp = new() { TagName = "samp", IsVoid = false };

        /// <summary>
        /// The &lt;kbd&gt; HTML element represents a span of inline text denoting textual user input from a keyboard, voice input, or any other text entry device.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/kbd
        /// </remarks>

        public static readonly Tag Kbd = new() { TagName = "kbd", IsVoid = false };

        /// <summary>
        /// The &lt;sub&gt; HTML element specifies inline text which should be displayed as subscript.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/sub
        /// </remarks>

        public static readonly Tag Sub = new() { TagName = "sub", IsVoid = false };

        /// <summary>
        /// The &lt;sup&gt; HTML element specifies inline text which is to be displayed as superscript.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/sup
        /// </remarks>

        public static readonly Tag Sup = new() { TagName = "sup", IsVoid = false };

        /// <summary>
        /// The &lt;i&gt; HTML element represents a range of text that is set off from the normal text for some reason, such as idiomatic text, technical terms, taxonomical designations, among others.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/i
        /// </remarks>

        public static readonly Tag I = new() { TagName = "i", IsVoid = false };

        /// <summary>
        /// The &lt;b&gt; HTML element is used to draw the reader's attention to the element's contents, which are not otherwise granted special importance.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/b
        /// </remarks>

        public static readonly Tag B = new() { TagName = "b", IsVoid = false };

        /// <summary>
        /// The &lt;u&gt; HTML element represents a span of inline text which should be rendered in a way that indicates that it has a non-textual annotation.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/u
        /// </remarks>

        public static readonly Tag U = new() { TagName = "u", IsVoid = false };

        /// <summary>
        /// The &lt;mark&gt; HTML element represents text which is marked or highlighted for reference or notation purposes.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/mark
        /// </remarks>

        public static readonly Tag Mark = new() { TagName = "mark", IsVoid = false };

        /// <summary>
        /// The &lt;ruby&gt; HTML element represents small annotations that are rendered above, below, or next to base text.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/ruby
        /// </remarks>

        public static readonly Tag Ruby = new() { TagName = "ruby", IsVoid = false };

        /// <summary>
        /// The &lt;rt&gt; HTML element specifies the ruby text component of a ruby annotation.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/rt
        /// </remarks>

        public static readonly Tag Rt = new() { TagName = "rt", IsVoid = false };

        /// <summary>
        /// The &lt;rp&gt; HTML element is used to provide fall-back parentheses for browsers that do not support display of ruby annotations.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/rp
        /// </remarks>

        public static readonly Tag Rp = new() { TagName = "rp", IsVoid = false };

        /// <summary>
        /// The &lt;bdi&gt; HTML element tells the browser's bidirectional algorithm to treat the text it contains in isolation from its surrounding text.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/bdi
        /// </remarks>

        public static readonly Tag Bdi = new() { TagName = "bdi", IsVoid = false };

        /// <summary>
        /// The &lt;bdo&gt; HTML element overrides the current directionality of text, so that the text within is rendered in a different direction.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/bdo
        /// </remarks>

        public static readonly Tag Bdo = new() { TagName = "bdo", IsVoid = false };

        /// <summary>
        /// The &lt;span&gt; HTML element is a generic inline container for phrasing content, which does not inherently represent anything.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/span
        /// </remarks>

        public static readonly Tag Span = new() { TagName = "span", IsVoid = false };

        /// <summary>
        /// The &lt;br&gt; HTML element produces a line break in text (carriage-return).
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/br
        /// </remarks>

        public static readonly Tag Br = new() { TagName = "br", IsVoid = true };

        /// <summary>
        /// The &lt;wbr&gt; HTML element represents a word break opportunity—a position within text where the browser may optionally break a line.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/wbr
        /// </remarks>

        public static readonly Tag Wbr = new() { TagName = "wbr", IsVoid = true };

        #endregion

        #region Image and Multimedia

        /// <summary>
        /// The &lt;img&gt; HTML element embeds an image into the document.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/img
        /// </remarks>

        public static readonly Tag Img = new() { TagName = "img", IsVoid = true };

        /// <summary>
        /// The &lt;picture&gt; HTML element contains zero or more &lt;source&gt; elements and one &lt;img&gt; element to offer alternative versions of an image for different display/device scenarios.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/picture
        /// </remarks>

        public static readonly Tag Picture = new() { TagName = "picture", IsVoid = false };

        /// <summary>
        /// The &lt;source&gt; HTML element specifies multiple media resources for the &lt;picture&gt;, the &lt;audio&gt; element, or the &lt;video&gt; element.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/source
        /// </remarks>

        public static readonly Tag Source = new() { TagName = "source", IsVoid = true };

        /// <summary>
        /// The &lt;audio&gt; HTML element is used to embed sound content in documents.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/audio
        /// </remarks>

        public static readonly Tag Audio = new() { TagName = "audio", IsVoid = false };

        /// <summary>
        /// The &lt;video&gt; HTML element embeds a media player which supports video playback into the document.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/video
        /// </remarks>

        public static readonly Tag Video = new() { TagName = "video", IsVoid = false };

        /// <summary>
        /// The &lt;track&gt; HTML element is used as a child of the media elements, &lt;audio&gt; and &lt;video&gt;. It lets you specify timed text tracks.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/track
        /// </remarks>

        public static readonly Tag Track = new() { TagName = "track", IsVoid = true };

        /// <summary>
        /// The &lt;map&gt; HTML element is used with &lt;area&gt; elements to define an image map (a clickable link area).
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/map
        /// </remarks>

        public static readonly Tag Map = new() { TagName = "map", IsVoid = false };

        /// <summary>
        /// The &lt;area&gt; HTML element defines an area inside an image map that has predefined clickable areas.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/area
        /// </remarks>

        public static readonly Tag Area = new() { TagName = "area", IsVoid = true };

        #endregion

        #region Embedded Content

        /// <summary>
        /// The &lt;embed&gt; HTML element embeds external content at the specified point in the document.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/embed
        /// </remarks>

        public static readonly Tag Embed = new() { TagName = "embed", IsVoid = true };

        /// <summary>
        /// The &lt;iframe&gt; HTML element represents a nested browsing context, embedding another HTML page into the current one.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/iframe
        /// </remarks>

        public static readonly Tag Iframe = new() { TagName = "iframe", IsVoid = false };

        /// <summary>
        /// The &lt;object&gt; HTML element represents an external resource, which can be treated as an image, a nested browsing context, or a resource to be handled by a plugin.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/object
        /// </remarks>

        public static readonly Tag Object_ = new() { TagName = "object", IsVoid = false };

        /// <summary>
        /// The &lt;param&gt; HTML element defines parameters for an &lt;object&gt; element.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/param
        /// </remarks>

        public static readonly Tag Param = new() { TagName = "param", IsVoid = true };

        /// <summary>
        /// The &lt;portal&gt; HTML element enables the embedding of another HTML page into the current one for the purposes of allowing smoother navigation into new pages.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/portal
        /// </remarks>

        public static readonly Tag Portal = new() { TagName = "portal", IsVoid = false };

        #endregion

        #region Scripting

        /// <summary>
        /// The &lt;canvas&gt; HTML element can be used to draw graphics via scripting (usually JavaScript).
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/canvas
        /// </remarks>

        public static readonly Tag Canvas = new() { TagName = "canvas", IsVoid = false };

        /// <summary>
        /// The &lt;noscript&gt; HTML element defines a section of HTML to be inserted if a script type on the page is unsupported or if scripting is currently turned off in the browser.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/noscript
        /// </remarks>

        public static readonly Tag Noscript = new() { TagName = "noscript", IsVoid = false };

        /// <summary>
        /// The &lt;script&gt; HTML element is used to embed executable code or data; this is typically used to embed or refer to JavaScript code.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/script
        /// </remarks>

        public static readonly Tag Script = new() { TagName = "script", IsVoid = false };

        #endregion

        #region Edits

        /// <summary>
        /// The &lt;del&gt; HTML element represents a range of text that has been deleted from a document.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/del
        /// </remarks>

        public static readonly Tag Del = new() { TagName = "del", IsVoid = false };

        /// <summary>
        /// The &lt;ins&gt; HTML element represents a range of text that has been added to a document.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/ins
        /// </remarks>

        public static readonly Tag Ins = new() { TagName = "ins", IsVoid = false };

        #endregion

        #region Table Content

        /// <summary>
        /// The &lt;table&gt; HTML element represents tabular data — that is, information presented in a two-dimensional table comprised of rows and columns of cells containing data.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/table
        /// </remarks>

        public static readonly Tag Table = new() { TagName = "table", IsVoid = false };

        /// <summary>
        /// The &lt;caption&gt; HTML element specifies the caption (or title) of a table.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/caption
        /// </remarks>

        public static readonly Tag Caption = new() { TagName = "caption", IsVoid = false };

        /// <summary>
        /// The &lt;colgroup&gt; HTML element defines a group of columns within a table.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/colgroup
        /// </remarks>

        public static readonly Tag Colgroup = new() { TagName = "colgroup", IsVoid = false };

        /// <summary>
        /// The &lt;col&gt; HTML element defines a column within a table and is used for defining common semantics on all common cells.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/col
        /// </remarks>

        public static readonly Tag Col = new() { TagName = "col", IsVoid = true };

        /// <summary>
        /// The &lt;tbody&gt; HTML element encapsulates a set of table rows (&lt;tr&gt; elements), indicating that they comprise the body of the table (&lt;table&gt;).
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/tbody
        /// </remarks>

        public static readonly Tag Tbody = new() { TagName = "tbody", IsVoid = false };

        /// <summary>
        /// The &lt;thead&gt; HTML element defines a set of rows defining the head of the columns of the table.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/thead
        /// </remarks>

        public static readonly Tag Thead = new() { TagName = "thead", IsVoid = false };

        /// <summary>
        /// The &lt;tfoot&gt; HTML element defines a set of rows summarizing the columns of the table.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/tfoot
        /// </remarks>

        public static readonly Tag Tfoot = new() { TagName = "tfoot", IsVoid = false };

        /// <summary>
        /// The &lt;tr&gt; HTML element defines a row of cells in a table. The row's cells can then be established using a mix of &lt;td&gt; and &lt;th&gt; elements.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/tr
        /// </remarks>

        public static readonly Tag Tr = new() { TagName = "tr", IsVoid = false };

        /// <summary>
        /// The &lt;td&gt; HTML element defines a cell of a table that contains data.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/td
        /// </remarks>

        public static readonly Tag Td = new() { TagName = "td", IsVoid = false };

        /// <summary>
        /// The &lt;th&gt; HTML element defines a cell as header of a group of table cells.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/th
        /// </remarks>

        public static readonly Tag Th = new() { TagName = "th", IsVoid = false };

        #endregion

        #region Forms

        /// <summary>
        /// The &lt;form&gt; HTML element represents a document section containing interactive controls for submitting information.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/form
        /// </remarks>

        public static readonly Tag Form = new() { TagName = "form", IsVoid = false };

        /// <summary>
        /// The &lt;label&gt; HTML element represents a caption for an item in a user interface.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/label
        /// </remarks>

        public static readonly Tag Label_ = new() { TagName = "label", IsVoid = false };

        /// <summary>
        /// The &lt;input&gt; HTML element is used to create interactive controls for web-based forms in order to accept data from the user.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/input
        /// </remarks>

        public static readonly Tag Input = new() { TagName = "input", IsVoid = true };

        /// <summary>
        /// The &lt;button&gt; HTML element is an interactive element activated by a user with a mouse, keyboard, finger, voice command, or other assistive technology.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/button
        /// </remarks>

        public static readonly Tag Button = new() { TagName = "button", IsVoid = false };

        /// <summary>
        /// The &lt;select&gt; HTML element represents a control that provides a menu of options.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/select
        /// </remarks>

        public static readonly Tag Select = new() { TagName = "select", IsVoid = false };

        /// <summary>
        /// The &lt;datalist&gt; HTML element contains a set of &lt;option&gt; elements that represent the permissible or recommended options available to choose from within other controls.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/datalist
        /// </remarks>

        public static readonly Tag Datalist = new() { TagName = "datalist", IsVoid = false };

        /// <summary>
        /// The &lt;optgroup&gt; HTML element creates a grouping of options within a &lt;select&gt; element.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/optgroup
        /// </remarks>

        public static readonly Tag Optgroup = new() { TagName = "optgroup", IsVoid = false };

        /// <summary>
        /// The &lt;option&gt; HTML element is used to define an item contained in a &lt;select&gt;, an &lt;optgroup&gt;, or a &lt;datalist&gt; element.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/option
        /// </remarks>

        public static readonly Tag Option = new() { TagName = "option", IsVoid = false };

        /// <summary>
        /// The &lt;textarea&gt; HTML element represents a multi-line plain-text editing control.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/textarea
        /// </remarks>

        public static readonly Tag Textarea = new() { TagName = "textarea", IsVoid = false };

        /// <summary>
        /// The &lt;output&gt; HTML element is a container element into which a site or app can inject the results of a calculation or the outcome of a user action.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/output
        /// </remarks>

        public static readonly Tag Output = new() { TagName = "output", IsVoid = false };

        /// <summary>
        /// The &lt;progress&gt; HTML element displays an indicator showing the completion progress of a task.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/progress
        /// </remarks>

        public static readonly Tag Progress = new() { TagName = "progress", IsVoid = false };

        /// <summary>
        /// The &lt;meter&gt; HTML element represents either a scalar value within a known range or a fractional value.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/meter
        /// </remarks>

        public static readonly Tag Meter = new() { TagName = "meter", IsVoid = false };

        /// <summary>
        /// The &lt;fieldset&gt; HTML element is used to group several controls as well as labels (&lt;label&gt;) within a web form.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/fieldset
        /// </remarks>

        public static readonly Tag Fieldset = new() { TagName = "fieldset", IsVoid = false };

        /// <summary>
        /// The &lt;legend&gt; HTML element represents a caption for the content of its parent &lt;fieldset&gt;.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/legend
        /// </remarks>

        public static readonly Tag Legend = new() { TagName = "legend", IsVoid = false };

        #endregion

        #region Interactive Elements

        /// <summary>
        /// The &lt;details&gt; HTML element creates a disclosure widget in which information is visible only when the widget is toggled into an "open" state.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/details
        /// </remarks>

        public static readonly Tag Details = new() { TagName = "details", IsVoid = false };

        /// <summary>
        /// The &lt;summary&gt; HTML element specifies a summary, caption, or legend for a &lt;details&gt; element's disclosure box.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/summary
        /// </remarks>
        public static readonly Tag Summary = new() { TagName = "summary", IsVoid = false };

        /// <summary>
        /// The &lt;dialog&gt; HTML element represents a dialog box or other interactive component, such as a dismissible alert, inspector, or subwindow.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/dialog
        /// </remarks>

        public static readonly Tag Dialog = new() { TagName = "dialog", IsVoid = false };

        #endregion

        #region Web Components

        /// <summary>
        /// The &lt;template&gt; HTML element is a mechanism for holding HTML that is not to be rendered immediately when a page is loaded but may be instantiated subsequently during runtime using JavaScript.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/template
        /// </remarks>

        public static readonly Tag Template = new() { TagName = "template", IsVoid = false };

        /// <summary>
        /// The &lt;slot&gt; HTML element is a placeholder inside a web component that you can fill with your own markup.
        /// </summary>
        /// <remarks>
        /// MDN Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/slot
        /// </remarks>

        public static readonly Tag Slot = new() { TagName = "slot", IsVoid = false };

        #endregion
    }
}
