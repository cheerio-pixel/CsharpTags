using System.Net;

namespace CsharpTags.Core.Types
{
    /// <summary>
    /// Represents a strongly-typed HTML attribute key with encoding logic.
    /// Provides type-safe HTML attribute definition and fluent creation syntax.
    /// </summary>
    /// <typeparam name="T">The type of the attribute value (string, bool, int, double)</typeparam>
    /// <remarks>
    /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Attributes"/> - MDN HTML Attribute Reference
    /// </remarks>
    public record HtmlKey<T>
    {
        /// <summary>
        /// The name of the HTML attribute (e.g., "class", "href", "disabled")
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// The encoding function that converts an attribute value to its HTML string representation
        /// </summary>
        public required Func<HtmlAttribute<T>, string> Encode { get; init; }

        /// <summary>
        /// Binds a value to this attribute key, creating a concrete HtmlAttribute instance
        /// </summary>
        /// <param name="right">The value to bind to this attribute</param>
        /// <returns>A new HtmlAttribute instance with this key and the provided value</returns>
        /// <example>
        /// <code>
        /// var classAttr = Prelude.Class.Bind("container");
        /// // Results in: class="container"
        /// </code>
        /// </example>
        public HtmlAttribute<T> Bind(T right)
            => this << right;

        /// <summary>
        /// Operator for fluent attribute creation: HtmlKey << value
        /// </summary>
        /// <param name="left">The HtmlKey instance</param>
        /// <param name="right">The value to associate with the attribute</param>
        /// <returns>A new HtmlAttribute instance</returns>
        /// <example>
        /// <code>
        /// var hrefAttr = Prelude.Href << "/page.html";
        /// var disabledAttr = Prelude.Disabled_ << true;
        /// </code>
        /// </example>
        public static HtmlAttribute<T> operator <<(HtmlKey<T> left, T right)
            => new()
            {
                Key = left,
                Value = right
            };
    }

    /// <summary>
    /// Interface for renderable HTML attributes
    /// </summary>
    public interface IHtmlAttribute
    {
        /// <summary>
        /// Renders the attribute to its HTML string representation
        /// </summary>
        /// <returns>The HTML-encoded attribute string</returns>
        public string Render();
    }

    /// <summary>
    /// Represents a concrete HTML attribute instance with a key and value.
    /// Implements rendering logic through the associated HtmlKey's encoder.
    /// </summary>
    /// <typeparam name="T">The type of the attribute value</typeparam>
    /// <remarks>
    /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Attributes"/> - MDN HTML Attribute Reference
    /// </remarks>
    public record HtmlAttribute<T> : IHtmlAttribute
    {
        /// <summary>
        /// The attribute key defining the attribute's name and encoding behavior
        /// </summary>
        public required HtmlKey<T> Key { get; init; }

        /// <summary>
        /// The attribute value
        /// </summary>
        public required T Value { get; init; }

        /// <summary>
        /// Renders the attribute to its HTML string representation using the key's encoder
        /// </summary>
        /// <returns>The HTML-encoded attribute string</returns>
        /// <example>
        /// <code>
        /// var attr = Prelude.Class << "container";
        /// Console.WriteLine(attr.Render()); // class="container"
        /// </code>
        /// </example>
        public string Render()
            => Key.Encode(this);
    }

    /// <summary>
    /// Provides predefined HTML attributes and encoding functions for type-safe HTML generation.
    /// Includes global attributes, form attributes, ARIA attributes, and media attributes.
    /// </summary>
    /// <remarks>
    /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Attributes"/> - MDN HTML Attribute Reference
    /// </remarks>
    public static partial class Prelude
    {
        #region Encoding Functions

        /// <summary>
        /// Encodes string values with HTML encoding and wraps in quotes
        /// </summary>
        /// <param name="attr">The string attribute to encode</param>
        /// <returns>HTML-encoded attribute string: name="value"</returns>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes#attr-class"/> - MDN Global Attributes
        /// </remarks>
        public static string StringAsIsEncoder(HtmlAttribute<string> attr)
            => $"{attr.Key.Name}=\"{WebUtility.HtmlEncode(attr.Value)}\"";

        /// <summary>
        /// Encodes boolean values as presence attributes (renders only name when true)
        /// </summary>
        /// <param name="attr">The boolean attribute to encode</param>
        /// <returns>Attribute name if true, empty string if false</returns>
        /// <remarks>
        /// Used for attributes like disabled, readonly, required.
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Attributes#boolean_attributes"/> - MDN Boolean Attributes
        /// </remarks>
        public static string BooleanPresenceEncoder(HtmlAttribute<bool> attr)
            => attr.Value ? attr.Key.Name : "";

        /// <summary>
        /// Encodes boolean values as "true"/"false" strings
        /// </summary>
        /// <param name="attr">The boolean attribute to encode</param>
        /// <returns>name="true" or name="false"</returns>
        public static string BooleanAsIsEncoder(HtmlAttribute<bool> attr)
            => $"{attr.Key.Name}=\"{attr.Value.ToString().ToLowerInvariant()}\"";

        /// <summary>
        /// Encodes boolean values as "yes"/"no" strings
        /// </summary>
        /// <param name="attr">The boolean attribute to encode</param>
        /// <returns>name="yes" or name="no"</returns>
        public static string BooleanAsYesNoEncoder(HtmlAttribute<bool> attr)
            => $"{attr.Key.Name}=\"{(attr.Value ? "yes" : "no")}\"";

        /// <summary>
        /// Encodes integer values as strings
        /// </summary>
        /// <param name="attr">The integer attribute to encode</param>
        /// <returns>name="value"</returns>
        public static string IntAsIsEncoder(HtmlAttribute<int> attr)
            => $"{attr.Key.Name}=\"{attr.Value}\"";

        /// <summary>
        /// Encodes double values as strings
        /// </summary>
        /// <param name="attr">The double attribute to encode</param>
        /// <returns>name="value"</returns>
        public static string DoubleAsIsEncoder(HtmlAttribute<double> attr)
            => $"{attr.Key.Name}=\"{attr.Value}\"";

        /// <summary>
        /// Encodes boolean values as "on"/"off" strings
        /// </summary>
        /// <param name="attr">The boolean attribute to encode</param>
        /// <returns>name="on" or name="off"</returns>
        public static string BooleanAsOnOffAsIsEncoder(HtmlAttribute<bool> attr)
            => $"{attr.Key.Name}=\"{(attr.Value ? "on" : "off")}\"";

        /// <summary>
        /// Encodes boolean values as "true"/"false" strings (alias for BooleanAsIsEncoder)
        /// </summary>
        /// <param name="attr">The boolean attribute to encode</param>
        /// <returns>name="true" or name="false"</returns>
        public static string BooleanAsTrueFalseStringEncoder(HtmlAttribute<bool> attr)
            => $"{attr.Key.Name}=\"{attr.Value.ToString().ToLowerInvariant()}\"";

        /// <summary>
        /// Encodes integer values as strings (alias for IntAsIsEncoder)
        /// </summary>
        /// <param name="attr">The integer attribute to encode</param>
        /// <returns>name="value"</returns>
        public static string IntAsStringEncoder(HtmlAttribute<int> attr)
            => $"{attr.Key.Name}=\"{attr.Value}\"";

        /// <summary>
        /// Encodes boolean values as "on"/"off" strings (alias for BooleanAsOnOffAsIsEncoder)
        /// </summary>
        /// <param name="attr">The boolean attribute to encode</param>
        /// <returns>name="on" or name="off"</returns>
        public static string BooleanAsOnOffStringEncoder(HtmlAttribute<bool> attr)
            => $"{attr.Key.Name}=\"{(attr.Value ? "on" : "off")}\"";

        #endregion

        #region Global Attributes
        // Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes

        /// <summary>
        /// Specifies the character encoding for the HTML document
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/meta#attr-charset"/> - MDN charset attribute
        /// </remarks>
        public readonly static HtmlKey<string> Charset = new()
        {
            Name = "charset",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Indicates whether the element's content is editable
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/contenteditable"/> - MDN contenteditable attribute
        /// </remarks>
        public readonly static HtmlKey<bool> ContentEditable = new()
        {
            Name = "contenteditable",
            Encode = BooleanAsTrueFalseStringEncoder
        };

        /// <summary>
        /// Specifies the ID of a context menu to use for the element
        /// </summary>
        public readonly static HtmlKey<string> ContextMenuId = new()
        {
            Name = "contextmenu",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies what types of content can be dropped on an element
        /// </summary>
        public readonly static HtmlKey<string> DropZone = new()
        {
            Name = "dropzone",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies the URL that will process the form control when the form is submitted
        /// </summary>
        public readonly static HtmlKey<string> FormAction = new()
        {
            Name = "formaction",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies the form element that the input element belongs to
        /// </summary>
        public readonly static HtmlKey<string> FormId = new()
        {
            Name = "form",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies the height of the element in pixels
        /// </summary>
        public readonly static HtmlKey<int> Height_ = new()
        {
            Name = "height",
            Encode = IntAsStringEncoder
        };

        /// <summary>
        /// Specifies the URL of the linked resource
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/a#attr-href"/> - MDN href attribute
        /// </remarks>
        public readonly static HtmlKey<string> Href = new()
        {
            Name = "href",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies the ID of a datalist element that provides pre-defined options for an input element
        /// </summary>
        public readonly static HtmlKey<string> ListId = new()
        {
            Name = "list",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies the maximum value for an input element
        /// </summary>
        public readonly static HtmlKey<string> Max_ = new()
        {
            Name = "max",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies the minimum value for an input element
        /// </summary>
        public readonly static HtmlKey<string> Min_ = new()
        {
            Name = "min",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies the URL of the media resource
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/img#attr-src"/> - MDN src attribute
        /// </remarks>
        public readonly static HtmlKey<string> Src = new()
        {
            Name = "src",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies the legal number intervals for an input element
        /// </summary>
        public readonly static HtmlKey<string> Step_ = new()
        {
            Name = "step",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies the type of element (alias for Type_)
        /// </summary>
        public readonly static HtmlKey<string> Typ = new()
        {
            Name = "type",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies the type of element (alias for Type_)
        /// </summary>
        public readonly static HtmlKey<string> Tpe = new()
        {
            Name = "type",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies the type of element
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/input#attr-type"/> - MDN type attribute
        /// </remarks>
        public readonly static HtmlKey<string> Type_ = new()
        {
            Name = "type",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies whether the text of an element can be selected
        /// </summary>
        public readonly static HtmlKey<bool> Unselectable = new()
        {
            Name = "unselectable",
            Encode = BooleanAsOnOffStringEncoder
        };

        /// <summary>
        /// Specifies the width of the element in pixels
        /// </summary>
        public readonly static HtmlKey<int> Width_ = new()
        {
            Name = "width",
            Encode = IntAsStringEncoder
        };

        /// <summary>
        /// Specifies a unique ID for an element
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/id"/> - MDN id attribute
        /// </remarks>
        public readonly static HtmlKey<string> Id_ = new()
        {
            Name = "id",
            Encode = StringAsIsEncoder
        };

        // Global Attributes

        /// <summary>
        /// Specifies one or more class names for an element
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/class"/> - MDN class attribute
        /// </remarks>
        public readonly static HtmlKey<string> Class = new()
        {
            Name = "class",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies inline CSS styles for an element
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/style"/> - MDN style attribute
        /// </remarks>
        public readonly static HtmlKey<string> Style = new()
        {
            Name = "style",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies extra information about an element (displayed as a tooltip)
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/title"/> - MDN title attribute
        /// </remarks>
        public readonly static HtmlKey<string> Title_ = new()
        {
            Name = "title",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies the language of the element's content
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/lang"/> - MDN lang attribute
        /// </remarks>
        public readonly static HtmlKey<string> Lang = new()
        {
            Name = "lang",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies the text direction for the content in an element
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/dir"/> - MDN dir attribute
        /// </remarks>
        public readonly static HtmlKey<string> Dir = new()
        {
            Name = "dir",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies the tab order of an element
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/tabindex"/> - MDN tabindex attribute
        /// </remarks>
        public readonly static HtmlKey<int> TabIndex = new()
        {
            Name = "tabindex",
            Encode = IntAsStringEncoder
        };

        /// <summary>
        /// Specifies a keyboard shortcut to activate/focus an element
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/accesskey"/> - MDN accesskey attribute
        /// </remarks>
        public readonly static HtmlKey<string> AccessKey = new()
        {
            Name = "accesskey",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies whether an element is draggable
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/draggable"/> - MDN draggable attribute
        /// </remarks>
        public readonly static HtmlKey<bool> Draggable = new()
        {
            Name = "draggable",
            Encode = BooleanAsTrueFalseStringEncoder
        };

        /// <summary>
        /// Specifies whether the element is to have its spelling and grammar checked
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/spellcheck"/> - MDN spellcheck attribute
        /// </remarks>
        public readonly static HtmlKey<bool> Spellcheck = new()
        {
            Name = "spellcheck",
            Encode = BooleanAsTrueFalseStringEncoder
        };

        /// <summary>
        /// Specifies whether the element's content should be translated
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/translate"/> - MDN translate attribute
        /// </remarks>
        public readonly static HtmlKey<bool> Translate = new()
        {
            Name = "translate",
            Encode = BooleanAsYesNoEncoder
        };

        /// <summary>
        /// Specifies the ARIA role of the element for accessibility
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Roles"/> - MDN ARIA Roles
        /// </remarks>
        public readonly static HtmlKey<string> Role = new()
        {
            Name = "role",
            Encode = StringAsIsEncoder
        };

        #endregion

        #region Form Attributes
        // Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/form#attributes

        /// <summary>
        /// Specifies the types of files that the server accepts
        /// </summary>
        public readonly static HtmlKey<string> Accept = new()
        {
            Name = "accept",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies the character encodings that are to be used for the form submission
        /// </summary>
        public readonly static HtmlKey<string> AcceptCharset = new()
        {
            Name = "accept-charset",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies where to send the form-data when a form is submitted
        /// </summary>
        public readonly static HtmlKey<string> Action = new()
        {
            Name = "action",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies whether input elements should have autocomplete enabled
        /// </summary>
        public readonly static HtmlKey<string> Autocomplete = new()
        {
            Name = "autocomplete",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies that an input element should automatically get focus when the page loads
        /// </summary>
        public readonly static HtmlKey<bool> Autofocus = new()
        {
            Name = "autofocus",
            Encode = BooleanPresenceEncoder
        };

        /// <summary>
        /// Specifies that an input element should be pre-selected when the page loads (for checkboxes or radio buttons)
        /// </summary>
        public readonly static HtmlKey<bool> Checked_ = new()
        {
            Name = "checked",
            Encode = BooleanPresenceEncoder
        };

        /// <summary>
        /// Specifies that an input element should be disabled
        /// </summary>
        public readonly static HtmlKey<bool> Disabled_ = new()
        {
            Name = "disabled",
            Encode = BooleanPresenceEncoder
        };

        /// <summary>
        /// Specifies how the form-data should be encoded when submitting to the server
        /// </summary>
        public readonly static HtmlKey<string> Enctype = new()
        {
            Name = "enctype",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies which form element a label is bound to
        /// </summary>
        public readonly static HtmlKey<string> For = new()
        {
            Name = "for",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies how form-data should be encoded before sending to a server
        /// </summary>
        public readonly static HtmlKey<string> FormEnctype = new()
        {
            Name = "formenctype",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies the HTTP method for sending form-data
        /// </summary>
        public readonly static HtmlKey<string> FormMethod = new()
        {
            Name = "formmethod",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies that the form should not be validated when submitted
        /// </summary>
        public readonly static HtmlKey<bool> FormNoValidate = new()
        {
            Name = "formnovalidate",
            Encode = BooleanPresenceEncoder
        };

        /// <summary>
        /// Specifies where to display the response after submitting the form
        /// </summary>
        public readonly static HtmlKey<string> FormTarget = new()
        {
            Name = "formtarget",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies the maximum number of characters allowed in an input element
        /// </summary>
        public readonly static HtmlKey<int> MaxLength = new()
        {
            Name = "maxlength",
            Encode = IntAsStringEncoder
        };

        /// <summary>
        /// Specifies the minimum number of characters required in an input element
        /// </summary>
        public readonly static HtmlKey<int> MinLength = new()
        {
            Name = "minlength",
            Encode = IntAsStringEncoder
        };

        /// <summary>
        /// Specifies the HTTP method to use when sending form-data
        /// </summary>
        public readonly static HtmlKey<string> Method = new()
        {
            Name = "method",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies that a user can enter more than one value in an input element
        /// </summary>
        public readonly static HtmlKey<bool> Multiple = new()
        {
            Name = "multiple",
            Encode = BooleanPresenceEncoder
        };

        /// <summary>
        /// Specifies the name of an input element
        /// </summary>
        public readonly static HtmlKey<string> Name = new()
        {
            Name = "name",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies that the form should not be validated when submitted
        /// </summary>
        public readonly static HtmlKey<bool> NoValidate = new()
        {
            Name = "novalidate",
            Encode = BooleanPresenceEncoder
        };

        /// <summary>
        /// Specifies a regular expression that an input element's value is checked against
        /// </summary>
        public readonly static HtmlKey<string> Pattern = new()
        {
            Name = "pattern",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies a short hint that describes the expected value of an input element
        /// </summary>
        public readonly static HtmlKey<string> Placeholder = new()
        {
            Name = "placeholder",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies that an input field is read-only
        /// </summary>
        public readonly static HtmlKey<bool> ReadOnly_ = new()
        {
            Name = "readonly",
            Encode = BooleanPresenceEncoder
        };

        /// <summary>
        /// Specifies that an input field must be filled out before submitting the form
        /// </summary>
        public readonly static HtmlKey<bool> Required_ = new()
        {
            Name = "required",
            Encode = BooleanPresenceEncoder
        };

        /// <summary>
        /// Specifies the width, in characters, of an input element
        /// </summary>
        public readonly static HtmlKey<int> Size = new()
        {
            Name = "size",
            Encode = IntAsStringEncoder
        };

        /// <summary>
        /// Specifies where to display the response after submitting the form
        /// </summary>
        public readonly static HtmlKey<string> Target = new()
        {
            Name = "target",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies the value of an input element
        /// </summary>
        public readonly static HtmlKey<string> Value = new()
        {
            Name = "value",
            Encode = StringAsIsEncoder
        };

        #endregion

        #region Media Attributes
        // Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/video#attributes

        /// <summary>
        /// Specifies that the audio/video will start playing as soon as it is ready
        /// </summary>
        public readonly static HtmlKey<bool> Autoplay = new()
        {
            Name = "autoplay",
            Encode = BooleanPresenceEncoder
        };

        /// <summary>
        /// Specifies that audio/video controls should be displayed
        /// </summary>
        public readonly static HtmlKey<bool> Controls_ = new()
        {
            Name = "controls",
            Encode = BooleanPresenceEncoder
        };

        /// <summary>
        /// Specifies how the element handles cross-origin requests
        /// </summary>
        public readonly static HtmlKey<string> Crossorigin = new()
        {
            Name = "crossorigin",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies that the audio/video will start over again, every time it is finished
        /// </summary>
        public readonly static HtmlKey<bool> Loop = new()
        {
            Name = "loop",
            Encode = BooleanPresenceEncoder
        };

        /// <summary>
        /// Specifies that the audio output of the video should be muted
        /// </summary>
        public readonly static HtmlKey<bool> Muted = new()
        {
            Name = "muted",
            Encode = BooleanPresenceEncoder
        };

        /// <summary>
        /// Specifies if and how the author thinks the audio/video should be loaded when the page loads
        /// </summary>
        public readonly static HtmlKey<string> Preload = new()
        {
            Name = "preload",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies an image to be shown while the video is downloading, or until the user hits the play button
        /// </summary>
        public readonly static HtmlKey<string> Poster = new()
        {
            Name = "poster",
            Encode = StringAsIsEncoder
        };

        #endregion

        #region Link/Image Attributes
        // Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/a#attributes
        // Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/img#attributes

        /// <summary>
        /// Specifies an alternate text for an image
        /// </summary>
        public readonly static HtmlKey<string> Alt = new()
        {
            Name = "alt",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies the URL of the image to use in different situations
        /// </summary>
        public readonly static HtmlKey<string> Srcset = new()
        {
            Name = "srcset",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies the sizes of the image for different page layouts
        /// </summary>
        public readonly static HtmlKey<string> Sizes = new()
        {
            Name = "sizes",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies the relationship between the current document and the linked document
        /// </summary>
        public readonly static HtmlKey<string> Rel = new()
        {
            Name = "rel",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies the language of the linked document
        /// </summary>
        public readonly static HtmlKey<string> Hreflang = new()
        {
            Name = "hreflang",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies what media/device the linked document is optimized for
        /// </summary>
        public readonly static HtmlKey<string> Media = new()
        {
            Name = "media",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies that the target will be downloaded when a user clicks on the hyperlink
        /// </summary>
        public readonly static HtmlKey<string> Download = new()
        {
            Name = "download",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies a space-separated list of URLs to be notified if the user follows the hyperlink
        /// </summary>
        public readonly static HtmlKey<string> Ping = new()
        {
            Name = "ping",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies which referrer information to send with the link
        /// </summary>
        public readonly static HtmlKey<string> ReferrerPolicy = new()
        {
            Name = "referrerpolicy",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies how the browser should load the image (eager, lazy)
        /// </summary>
        public readonly static HtmlKey<string> Loading = new()
        {
            Name = "loading",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies how the browser should decode the image (sync, async, auto)
        /// </summary>
        public readonly static HtmlKey<string> Decoding = new()
        {
            Name = "decoding",
            Encode = StringAsIsEncoder
        };

        #endregion

        #region Table Attributes
        // Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/td#attributes

        /// <summary>
        /// Specifies the number of columns a table cell should span
        /// </summary>
        public readonly static HtmlKey<int> Colspan = new()
        {
            Name = "colspan",
            Encode = IntAsStringEncoder
        };

        /// <summary>
        /// Specifies the number of rows a table cell should span
        /// </summary>
        public readonly static HtmlKey<int> Rowspan = new()
        {
            Name = "rowspan",
            Encode = IntAsStringEncoder
        };

        /// <summary>
        /// Specifies one or more header cells a cell is related to
        /// </summary>
        public readonly static HtmlKey<string> Headers = new()
        {
            Name = "headers",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies whether a header cell is a header for a column, row, or group of columns or rows
        /// </summary>
        public readonly static HtmlKey<string> Scope = new()
        {
            Name = "scope",
            Encode = StringAsIsEncoder
        };

        #endregion

        #region Text Content Attributes

        /// <summary>
        /// Specifies the URL of the source code or reference for a quotation
        /// </summary>
        public readonly static HtmlKey<string> Cite = new()
        {
            Name = "cite",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies the date and time associated with the element
        /// </summary>
        public readonly static HtmlKey<string> Datetime = new()
        {
            Name = "datetime",
            Encode = StringAsIsEncoder
        };

        #endregion

        #region Interactive Attributes

        /// <summary>
        /// Specifies the URL of the resource to be used by the object
        /// </summary>
        public readonly static HtmlKey<string> Data = new()
        {
            Name = "data",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies that the details should be visible (open) to the user
        /// </summary>
        public readonly static HtmlKey<bool> Open = new()
        {
            Name = "open",
            Encode = BooleanPresenceEncoder
        };

        #endregion

        #region Iframe Attributes
        // Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/iframe#attributes

        /// <summary>
        /// Enables an extra set of restrictions for the content in an iframe
        /// </summary>
        public readonly static HtmlKey<string> Sandbox = new()
        {
            Name = "sandbox",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies a feature policy for the iframe
        /// </summary>
        public readonly static HtmlKey<string> Allow = new()
        {
            Name = "allow",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies the HTML content of the page to show in the iframe
        /// </summary>
        public readonly static HtmlKey<string> Srcdoc = new()
        {
            Name = "srcdoc",
            Encode = StringAsIsEncoder
        };

        #endregion

        #region Meta Attributes
        // Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/meta

        /// <summary>
        /// Specifies the value associated with the http-equiv or name attribute
        /// </summary>
        public readonly static HtmlKey<string> Content = new()
        {
            Name = "content",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Provides an HTTP header for the information/value of the content attribute
        /// </summary>
        public readonly static HtmlKey<string> HttpEquiv = new()
        {
            Name = "http-equiv",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies the property type for Open Graph or other meta tags
        /// </summary>
        public readonly static HtmlKey<string> Property = new()
        {
            Name = "property",
            Encode = StringAsIsEncoder
        };

        #endregion

        #region Script/Style Attributes
        // Reference: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/script#attributes

        /// <summary>
        /// Specifies that the script is executed asynchronously
        /// </summary>
        public readonly static HtmlKey<bool> Async = new()
        {
            Name = "async",
            Encode = BooleanPresenceEncoder
        };
        /// <summary>
        /// Specifies that the script is executed when the page has finished parsing
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/script#attr-defer"/> - MDN defer attribute
        /// </remarks>
        public readonly static HtmlKey<bool> Defer = new()
        {
            Name = "defer",
            Encode = BooleanPresenceEncoder
        };

        /// <summary>
        /// Specifies a Subresource Integrity (SRI) value for the script
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/Security/Subresource_Integrity"/> - MDN Subresource Integrity
        /// </remarks>
        public readonly static HtmlKey<string> Integrity = new()
        {
            Name = "integrity",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies that the script should not be executed in browsers supporting ES modules
        /// </summary>
        public readonly static HtmlKey<bool> Nomodule = new()
        {
            Name = "nomodule",
            Encode = BooleanPresenceEncoder
        };

        /// <summary>
        /// Specifies a cryptographic nonce (number used once) for Content Security Policy
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/nonce"/> - MDN nonce attribute
        /// </remarks>
        public readonly static HtmlKey<string> Nonce = new()
        {
            Name = "nonce",
            Encode = StringAsIsEncoder
        };

        #endregion

        #region Misc Attributes

        /// <summary>
        /// Specifies the visible width of a text control in characters
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/textarea#attr-cols"/> - MDN cols attribute
        /// </remarks>
        public readonly static HtmlKey<int> Cols = new()
        {
            Name = "cols",
            Encode = IntAsStringEncoder
        };

        /// <summary>
        /// Specifies the visible number of lines in a text control
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/textarea#attr-rows"/> - MDN rows attribute
        /// </remarks>
        public readonly static HtmlKey<int> Rows = new()
        {
            Name = "rows",
            Encode = IntAsStringEncoder
        };

        /// <summary>
        /// Specifies how the text in a text area is to be wrapped when submitted in a form
        /// </summary>
        public readonly static HtmlKey<string> Wrap = new()
        {
            Name = "wrap",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies the start value of an ordered list
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/ol#attr-start"/> - MDN start attribute
        /// </remarks>
        public readonly static HtmlKey<int> Start = new()
        {
            Name = "start",
            Encode = IntAsStringEncoder
        };

        /// <summary>
        /// Specifies that the list order should be descending (9,8,7...)
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/ol#attr-reversed"/> - MDN reversed attribute
        /// </remarks>
        public readonly static HtmlKey<bool> Reversed = new()
        {
            Name = "reversed",
            Encode = BooleanPresenceEncoder
        };

        /// <summary>
        /// Specifies the kind of text track for the track element
        /// </summary>
        public readonly static HtmlKey<string> Kind = new()
        {
            Name = "kind",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies the language of the track text data
        /// </summary>
        public readonly static HtmlKey<string> Srclang = new()
        {
            Name = "srclang",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies that the track is to be enabled if the user's preferences do not indicate that another track would be more appropriate
        /// </summary>
        public readonly static HtmlKey<bool> Default = new()
        {
            Name = "default",
            Encode = BooleanPresenceEncoder
        };

        /// <summary>
        /// Specifies that an option should be pre-selected when the page loads
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/option#attr-selected"/> - MDN selected attribute
        /// </remarks>
        public readonly static HtmlKey<bool> Selected_ = new()
        {
            Name = "selected",
            Encode = BooleanPresenceEncoder
        };

        /// <summary>
        /// Specifies an image as a client-side image map
        /// </summary>
        public readonly static HtmlKey<string> Usemap = new()
        {
            Name = "usemap",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies an image as a server-side image map
        /// </summary>
        public readonly static HtmlKey<bool> Ismap = new()
        {
            Name = "ismap",
            Encode = BooleanPresenceEncoder
        };

        /// <summary>
        /// Specifies the shape of the area in an image map
        /// </summary>
        public readonly static HtmlKey<string> Shape = new()
        {
            Name = "shape",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies the coordinates of the area in an image map
        /// </summary>
        public readonly static HtmlKey<string> Coords = new()
        {
            Name = "coords",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies a hint to the browser about the type of virtual keyboard to display
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/inputmode"/> - MDN inputmode attribute
        /// </remarks>
        public readonly static HtmlKey<string> Inputmode = new()
        {
            Name = "inputmode",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies what action label (or icon) to present for the enter key on virtual keyboards
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/enterkeyhint"/> - MDN enterkeyhint attribute
        /// </remarks>
        public readonly static HtmlKey<string> Enterkeyhint = new()
        {
            Name = "enterkeyhint",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies whether and how text input should be automatically capitalized
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/autocapitalize"/> - MDN autocapitalize attribute
        /// </remarks>
        public readonly static HtmlKey<string> Autocapitalize = new()
        {
            Name = "autocapitalize",
            Encode = StringAsIsEncoder
        };

        #endregion

        #region Data Attributes

        /// <summary>
        /// Creates a custom data-* attribute with the specified name
        /// </summary>
        /// <param name="name">The name for the data attribute (without the 'data-' prefix)</param>
        /// <returns>A new HtmlKey for the custom data attribute</returns>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/data-*"/> - MDN data-* attributes
        /// </remarks>
        /// <example>
        /// <code>
        /// var dataUserId = Prelude.DataAttr("user-id") << "12345";
        /// // Renders as: data-user-id="12345"
        /// </code>
        /// </example>
        public static HtmlKey<string> DataAttr(string name) => new()
        {
            Name = $"data-{name}",
            Encode = StringAsIsEncoder
        };

        #endregion

        #region ARIA Attributes
        // Reference: https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes

        /// <summary>
        /// Identifies the currently active element when focus is on a composite widget, combobox, textbox, group, or application
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes/aria-activedescendant"/> - MDN aria-activedescendant
        /// </remarks>
        public readonly static HtmlKey<string> ActiveDescendant = new()
        {
            Name = "aria-activedescendant",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Indicates whether assistive technologies will present all, or only parts of, the changed region based on the change notifications defined by the aria-relevant attribute
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes/aria-atomic"/> - MDN aria-atomic
        /// </remarks>
        public readonly static HtmlKey<bool> Atomic = new()
        {
            Name = "aria-atomic",
            Encode = BooleanAsTrueFalseStringEncoder
        };

        /// <summary>
        /// Indicates whether inputting text could trigger display of one or more predictions of the user's intended value for a combobox, searchbox, or textbox
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes/aria-autocomplete"/> - MDN aria-autocomplete
        /// </remarks>
        public readonly static HtmlKey<string> AutoComplete = new()
        {
            Name = "aria-autocomplete",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Indicates an element is being modified and that assistive technologies may want to wait until the changes are complete before exposing them to the user
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes/aria-busy"/> - MDN aria-busy
        /// </remarks>
        public readonly static HtmlKey<bool> Busy = new()
        {
            Name = "aria-busy",
            Encode = BooleanAsTrueFalseStringEncoder
        };

        /// <summary>
        /// Indicates the current "checked" state of checkboxes, radio buttons, and other widgets
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes/aria-checked"/> - MDN aria-checked
        /// </remarks>
        public readonly static HtmlKey<string> Checked = new()
        {
            Name = "aria-checked",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Identifies the element (or elements) whose contents or presence are controlled by the current element
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes/aria-controls"/> - MDN aria-controls
        /// </remarks>
        public readonly static HtmlKey<string> Controls = new()
        {
            Name = "aria-controls",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Indicates the element that represents the current item within a container or set of related elements
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes/aria-current"/> - MDN aria-current
        /// </remarks>
        public readonly static HtmlKey<string> Current = new()
        {
            Name = "aria-current",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Identifies the element (or elements) that describes the object
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes/aria-describedby"/> - MDN aria-describedby
        /// </remarks>
        public readonly static HtmlKey<string> DescribedBy = new()
        {
            Name = "aria-describedby",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Indicates that the element is perceivable but disabled, so it is not editable or otherwise operable
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes/aria-disabled"/> - MDN aria-disabled
        /// </remarks>
        public readonly static HtmlKey<bool> Disabled = new()
        {
            Name = "aria-disabled",
            Encode = BooleanAsTrueFalseStringEncoder
        };

        /// <summary>
        /// Indicates what functions can be performed when a dragged object is released on the drop target
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes/aria-dropeffect"/> - MDN aria-dropeffect
        /// </remarks>
        public readonly static HtmlKey<string> DropEffect = new()
        {
            Name = "aria-dropeffect",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Indicates whether the element, or another grouping element it controls, is currently expanded or collapsed
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes/aria-expanded"/> - MDN aria-expanded
        /// </remarks>
        public readonly static HtmlKey<bool> Expanded = new()
        {
            Name = "aria-expanded",
            Encode = BooleanAsTrueFalseStringEncoder
        };

        /// <summary>
        /// Identifies the next element (or elements) in an alternate reading order of content which, at the user's discretion, allows assistive technology to override the general default of reading in document source order
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes/aria-flowto"/> - MDN aria-flowto
        /// </remarks>
        public readonly static HtmlKey<string> FlowTo = new()
        {
            Name = "aria-flowto",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Indicates an element's "grabbed" state in a drag-and-drop operation
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes/aria-grabbed"/> - MDN aria-grabbed
        /// </remarks>
        public readonly static HtmlKey<bool> Grabbed = new()
        {
            Name = "aria-grabbed",
            Encode = BooleanAsTrueFalseStringEncoder
        };

        /// <summary>
        /// Indicates the availability and type of interactive popup element, such as menu or dialog, that can be triggered by an element
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes/aria-haspopup"/> - MDN aria-haspopup
        /// </remarks>
        public readonly static HtmlKey<bool> HasPopup = new()
        {
            Name = "aria-haspopup",
            Encode = BooleanAsTrueFalseStringEncoder
        };

        /// <summary>
        /// Indicates whether the element is exposed to an accessibility API
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes/aria-hidden"/> - MDN aria-hidden
        /// </remarks>
        public readonly static HtmlKey<bool> Hidden = new()
        {
            Name = "aria-hidden",
            Encode = BooleanAsTrueFalseStringEncoder
        };

        /// <summary>
        /// Indicates the entered value does not conform to the format expected by the application
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes/aria-invalid"/> - MDN aria-invalid
        /// </remarks>
        public readonly static HtmlKey<string> Invalid = new()
        {
            Name = "aria-invalid",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Defines a string value that labels the current element
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes/aria-label"/> - MDN aria-label
        /// </remarks>
        public readonly static HtmlKey<string> Label = new()
        {
            Name = "aria-label",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Identifies the element (or elements) that labels the current element
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes/aria-labelledby"/> - MDN aria-labelledby
        /// </remarks>
        public readonly static HtmlKey<string> LabelledBy = new()
        {
            Name = "aria-labelledby",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Defines the hierarchical level of an element within a structure
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes/aria-level"/> - MDN aria-level
        /// </remarks>
        public readonly static HtmlKey<int> Level = new()
        {
            Name = "aria-level",
            Encode = IntAsStringEncoder
        };

        /// <summary>
        /// Indicates that an element will be updated, and describes the types of updates the user agents, assistive technologies, and user can expect from the live region
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes/aria-live"/> - MDN aria-live
        /// </remarks>
        public readonly static HtmlKey<string> Live = new()
        {
            Name = "aria-live",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Indicates whether a text box accepts multiple lines of input or only a single line
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes/aria-multiline"/> - MDN aria-multiline
        /// </remarks>
        public readonly static HtmlKey<bool> MultiLine = new()
        {
            Name = "aria-multiline",
            Encode = BooleanAsTrueFalseStringEncoder
        };

        /// <summary>
        /// Indicates that the user may select more than one item from the current selectable descendants
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes/aria-multiselectable"/> - MDN aria-multiselectable
        /// </remarks>
        public readonly static HtmlKey<bool> MultiSelectable = new()
        {
            Name = "aria-multiselectable",
            Encode = BooleanAsTrueFalseStringEncoder
        };

        /// <summary>
        /// Indicates whether the element's orientation is horizontal, vertical, or unknown/ambiguous
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes/aria-orientation"/> - MDN aria-orientation
        /// </remarks>
        public readonly static HtmlKey<string> Orientation = new()
        {
            Name = "aria-orientation",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Identifies an element (or elements) in order to define a visual, functional, or contextual parent/child relationship between DOM elements where the DOM hierarchy cannot be used to represent the relationship
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes/aria-owns"/> - MDN aria-owns
        /// </remarks>
        public readonly static HtmlKey<string> Owns = new()
        {
            Name = "aria-owns",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Defines an element's number or position in the current set of listitems or treeitems
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes/aria-posinset"/> - MDN aria-posinset
        /// </remarks>
        public readonly static HtmlKey<int> PosInSet = new()
        {
            Name = "aria-posinset",
            Encode = IntAsStringEncoder
        };

        /// <summary>
        /// Indicates the current "pressed" state of toggle buttons
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes/aria-pressed"/> - MDN aria-pressed
        /// </remarks>
        public readonly static HtmlKey<string> Pressed = new()
        {
            Name = "aria-pressed",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Indicates that the element is not editable, but is otherwise operable
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes/aria-readonly"/> - MDN aria-readonly
        /// </remarks>
        public readonly static HtmlKey<bool> ReadOnly = new()
        {
            Name = "aria-readonly",
            Encode = BooleanAsTrueFalseStringEncoder
        };

        /// <summary>
        /// Indicates what notifications the user agent will trigger when the accessibility tree within a live region is modified
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes/aria-relevant"/> - MDN aria-relevant
        /// </remarks>
        public readonly static HtmlKey<string> Relevant = new()
        {
            Name = "aria-relevant",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Indicates that user input is required on the element before a form may be submitted
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes/aria-required"/> - MDN aria-required
        /// </remarks>
        public readonly static HtmlKey<bool> Required = new()
        {
            Name = "aria-required",
            Encode = BooleanAsTrueFalseStringEncoder
        };

        /// <summary>
        /// Indicates the current "selected" state of various widgets
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes/aria-selected"/> - MDN aria-selected
        /// </remarks>
        public readonly static HtmlKey<bool> Selected = new()
        {
            Name = "aria-selected",
            Encode = BooleanAsTrueFalseStringEncoder
        };

        /// <summary>
        /// Defines the number of items in the current set of listitems or treeitems
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes/aria-setsize"/> - MDN aria-setsize
        /// </remarks>
        public readonly static HtmlKey<int> SetSize = new()
        {
            Name = "aria-setsize",
            Encode = IntAsStringEncoder
        };

        /// <summary>
        /// Indicates if items in a table or grid are sorted in ascending or descending order
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes/aria-sort"/> - MDN aria-sort
        /// </remarks>
        public readonly static HtmlKey<string> Sort = new()
        {
            Name = "aria-sort",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Defines the maximum allowed value for a range widget
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes/aria-valuemax"/> - MDN aria-valuemax
        /// </remarks>
        public readonly static HtmlKey<double> ValueMax = new()
        {
            Name = "aria-valuemax",
            Encode = DoubleAsIsEncoder
        };

        /// <summary>
        /// Defines the minimum allowed value for a range widget
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes/aria-valuemin"/> - MDN aria-valuemin
        /// </remarks>
        public readonly static HtmlKey<double> ValueMin = new()
        {
            Name = "aria-valuemin",
            Encode = DoubleAsIsEncoder
        };

        /// <summary>
        /// Defines the current value for a range widget
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes/aria-valuenow"/> - MDN aria-valuenow
        /// </remarks>
        public readonly static HtmlKey<double> ValueNow = new()
        {
            Name = "aria-valuenow",
            Encode = DoubleAsIsEncoder
        };

        /// <summary>
        /// Defines the human-readable text alternative of aria-valuenow for a range widget
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes/aria-valuetext"/> - MDN aria-valuetext
        /// </remarks>
        public readonly static HtmlKey<string> ValueText = new()
        {
            Name = "aria-valuetext",
            Encode = StringAsIsEncoder
        };

        #endregion
    }
}
