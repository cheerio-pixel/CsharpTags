# CsharpTags.Core

[![CI](https://github.com/cheerio-pixel/CsharpTags/actions/workflows/ci.yml/badge.svg)](https://github.com/cheerio-pixel/CsharpTags/actions/workflows/ci.yml)
[![Publish](https://github.com/cheerio-pixel/CsharpTags/actions/workflows/publish.yml/badge.svg)](https://github.com/cheerio-pixel/CsharpTags/actions/workflows/publish.yml)
[![NuGet](https://img.shields.io/nuget/v/CsharpTags.Core.svg)](https://www.nuget.org/packages/CsharpTags.Core/)

A type-safe HTML generation library for C# that provides a fluent, functional approach to building HTML documents with compile-time safety.

**Targets:** .NET 8.0 and .NET 10.0

## Features

- **Type-safe HTML construction** - Compile-time validation of HTML structure
- **Functional API** - Fluent interface with immutable data structures
- **HTML Encoding** - Automatic encoding of text content and attributes
- **Comprehensive HTML5 Support** - Full coverage of HTML5 elements and attributes
- **ARIA Accessibility** - Built-in support for ARIA attributes
- **CSS Unit Extensions** - Type-safe CSS measurement units (15 units)
- **Components** - Reusable, composable HTML elements with custom build logic
- **Conditional Rendering** - Type-safe if-elseif-else rendering helpers
- **Tree Transformation** - Transform HTML element trees using zipper data structures
- **HTMX Integration** - Full HTMX attribute support via CsharpTags.Htmx package

## Installation

```xml
<PackageReference Include="CsharpTags.Core" Version="1.0.0-beta-4" />
```

## Quick Start

```csharp
using CsharpTags.Core.Types;
using static CsharpTags.Core.Types.Prelude;

// Create a simple HTML document
var html = Html.New(
    Head.New(
        Title.New("My Page"),
        Meta.New(Charset << "UTF-8")
    ),
    Body.New(
        Div.New(
            Class << "container",
            H1.New("Welcome to CsharpTags"),
            P.New("This is a type-safe HTML generation example."),
            RawStr("<p>This is raw html</p>"),
            Button.New(
                Id_ << "submit-btn",
                Class << "btn btn-primary",
                DisabledAttr << false
            ).New("Click Me")
        )
    )
);

string result = html.Render();
```

## Core Concepts

### HTML Elements

The library provides all standard HTML5 elements as static properties in the `Prelude` class:

```csharp
using static CsharpTags.Core.Types.Prelude;

var div = Div.New("Hello World");
var link = A.New(Href << "/page.html", "Click here");
var image = Img.New(Src << "photo.jpg", Alt << "A photo");
```

### Attributes

Type-safe attributes with proper encoding using the `<<` operator:

```csharp
// String attributes (automatically encoded)
var div = Div.New(Class << "container<test>"); // becomes class="container&lt;test&gt;"

// Boolean attributes (presence-based)
var input = Input.New(DisabledAttr << true);  // becomes <input disabled />
var input2 = Input.New(DisabledAttr << false); // becomes <input />

// Integer attributes
var input3 = Input.New(TabIndex << 5); // becomes tabindex="5"
```

The library provides **200+ built-in attributes** organized by category:

- **Global**: `Id_`, `Class`, `StyleAttr`, `TitleAttr`, `Lang`, `Dir`, `TabIndex`, `Draggable`, `AccessKey`, `ContentEditable`, `Spellcheck`, `Translate`, etc.
- **Form**: `Name`, `Value`, `Placeholder`, `RequiredAttr`, `DisabledAttr`, `ReadOnlyAttr`, `Pattern`, `Autocomplete`, `Autofocus`, `CheckedAttr`, `MaxLength`, `MinLength`, `Multiple`, `Size`, etc.
- **Input Types**: `TypeAttr` (text, password, email, checkbox, radio, date, number, etc.)
- **Link/Image**: `Href`, `Src`, `Alt`, `Srcset`, `Sizes`, `Loading`, `Decoding`, `Rel`, `Hreflang`, `Media`, `Download`, `Ping`, `ReferrerPolicy`, etc.
- **Media**: `Autoplay`, `ControlsAttr`, `Loop`, `Muted`, `Poster`, `Preload`, `Crossorigin`, etc.
- **Table**: `Colspan`, `Rowspan`, `Headers`, `Scope`
- **Event Handlers**: `On("click")`, `On("change")`, `On("focus")`, etc.
- **ARIA**: `Role`, `Label` (via Htmx), `DescribedBy` (via Htmx), etc.
- **Custom Data**: `DataAttr("user-id")` for `data-*` attributes

```csharp
// Custom data attributes
var element = Div.New(
    DataAttr("user-id") << "12345",
    DataAttr("role") << "admin"
);
// Renders as: <div data-user-id="12345" data-role="admin"></div>

// Event handlers (unescaped)
var button = Button.New(On("click") << "alert('clicked')", "Click me");

// Input types
var password = Input.New(TypeAttr << InputType.Password);
var date = Input.New(TypeAttr << InputType.Date);
```

### Text Content

Automatic HTML encoding for safe text rendering:

```csharp
var safeText = new Str { Value = "<script>alert('xss')</script>" };
// Renders as: &lt;script&gt;alert(&#39;xss&#39;)&lt;/script&gt;

// Or use implicit conversion
var paragraph = P.New("Hello World");  // string automatically becomes Str
```

### Lists and Collections

Combine multiple elements:

```csharp
// Using HList for inline element lists
var items = HList(
    Li.New("Item 1"),
    Li.New("Item 2"),
    Li.New("Item 3")
);

var list = Ul.New(items);

// Convert collections using ToHtml extension
var elements = new List<HtmlElement> { Li.New("A"), Li.New("B") };
var list2 = Ul.New(elements.ToHtml());
```

### Components

Reusable, composable HTML elements. Components are abstract records that extend `Component` and define a `TagName`. The content passed to the constructor becomes the content of the rendered Tag:

```csharp
// Define a custom component
public record MyCard : Component
{
    protected override string TagName => "div";
    
    // Required but unused - Component uses constructor content, not Build()
    protected override IHtml Build() => Prelude.None_;
}

// Use the component - pass content to constructor
var card = new MyCard(Class << "card", "Card content");
var html = Div.New(card);
// Renders: <div><div class="card">Card content</div></div>
```

Components can define factory methods for cleaner APIs:

```csharp
public record Alert : Component
{
    protected override string TagName => "div";
    protected override IHtml Build() => Prelude.None_;

    // Factory method for creating styled alerts
    public static Tag Info(string message) 
        => new Alert(Class << "alert alert-info", message).Simplify();
    
    public static Tag Error(string message) 
        => new Alert(Class << "alert alert-danger", message).Simplify();
}

// Usage
var infoAlert = Alert.Info("Operation successful!");
var errorAlert = Alert.Error("Something went wrong!");
```

Components implement `IWrapHtmlElement` and are automatically simplified when rendered:

```csharp
// Component is simplified to a Tag when used as content
var page = Body.New(
    new HeaderComponent(),           // Simplified to <header>...</header>
    new SidebarComponent(),          // Simplified to <aside>...</aside>
    new MainContentComponent(articles)  // Simplified to <main>...</main>
);
```

### Conditional Rendering

Type-safe if-elseif-else rendering helpers:

```csharp
// Simple conditional rendering
var content = WhenH(isLoggedIn, Span.New("Welcome!"));

// If-elseif-else pattern
var page = IfH(isAdmin, () => AdminPanel())
    .ElseIf(isUser, () => UserDashboard())
    .Else(() => LoginForm());

// Unless (render when false)
var hidden = UnlessH(isHidden, Div.New("This is visible"));
```

### Conditional Attributes

Render attributes only when conditions are met:

```csharp
// Using If() method on HtmlKey
var input = Input.New(
    TypeAttr << InputType.Text,
    RequiredAttr.If(isRequired) << true,
    Placeholder.If(hasPlaceholder) << "Enter value..."
);

// Or using RenderWhen helper
var attr = RenderWhen(isRequired, RequiredAttr << true);
```

### Raw HTML

Render unescaped HTML content:

```csharp
// Using RawStr factory method
var raw = RawStr("<strong>Bold text</strong>");
// Renders as: <strong>Bold text</strong> (not encoded)

// Or using RawStr record directly
var raw2 = new RawStr { Value = "<em>Italic</em>" };
```

## Advanced Usage

### CSS Units

Type-safe CSS measurements using number extensions (requires .NET 10):

```csharp
// Length units (absolute)
var width = 100.Px;        // "100px"
var height = 72.Pt;        // "72pt" (points)
var margin = 25.4.Mm;      // "25.4mm" (millimeters)
var size = 10.Cm;          // "10cm" (centimeters)
var printSize = 1.In;      // "1in" (inches)
var columnWidth = 6.Pc;    // "6pc" (picas)

// Length units (relative)
var fontSize = 1.2.Rem;    // "1.2rem" (root em)
var lineHeight = 1.5.Em;   // "1.5em" (relative to font-size)
var charWidth = 10.Ch;     // "10ch" (width of "0")
var xHeight = 1.Ex;        // "1ex" (x-height)

// Angle units
var rotation = 45.Deg;     // "45deg" (degrees)
var gradient = 100.Grad;   // "100grad" (gradians)
var radians = 1.57.Rad;    // "1.57rad" (radians)
var spin = 0.25.Turn;      // "0.25turn" (turns)

// Percentage
var percentWidth = 50.Pct; // "50%"
```

### Custom Data Attributes

```csharp
var element = Div.New(
    DataAttr("user-id") << "12345",
    DataAttr("role") << "admin"
);
// Renders as: <div data-user-id="12345" data-role="admin"></div>
```

### ARIA Attributes

Full accessibility support. Import from CsharpTags.Htmx for extended ARIA support:

```csharp
using static CsharpTags.Htmx.Types.Prelude;

var button = Button.New(
    Label << "Submit form",
    DescribedBy << "submit-help",
    Disabled << false,
    "Submit");

// More ARIA attributes
var modal = Div.New(
    Role << "dialog",
    AriaLabelledBy << "modal-title",
    AriaDescribedBy << "modal-desc",
    AriaHidden << false,
    "Modal content");

var checkbox = Input.New(
    TypeAttr << InputType.Checkbox,
    Role << "checkbox",
    AriaChecked << "true",
    AriaLabel << "Accept terms");
```

### Tree Transformation with Zippers

Transform HTML element trees using zipper data structures for efficient navigation and modification:

```csharp
// Wrap all divs in a container
var transformed = html.Transform(element =>
{
    if (element is Tag { TagName: "div" } div)
        return Some(Main.New(div));
    return Some(element);
});

// Remove all empty elements
var cleaned = html.Transform(element =>
{
    if (element is Tag tag && tag.Children.IsEmpty && tag.Attributes.IsEmpty)
        return None;
    return Some(element);
});

// Manual zipper navigation
var zipper = new Zipper<HtmlZipperOps, Tag, HtmlElement>(html);

// Navigate the tree
var down = zipper.GoDown();        // Move to first child
var right = zipper.GoRight();      // Move to right sibling
var up = zipper.GoUp();            // Move to parent
var left = zipper.GoLeft();        // Move to left sibling

// Modify elements
var modified = zipper.Replace(Div.New("New content"));
var edited = zipper.Edit(elem => elem is Tag ? Some(Span.New("Replaced")) : None);

// Insert elements
var withSibling = zipper.InsertRight(P.New("New paragraph"));
var withChild = zipper.InsertChild(Span.New("Child"));

// Remove elements
var removed = zipper.Remove();

// Get back to root after modifications
var result = zipper.Root();
```

### Building Complex Structures

```csharp
var page = Html.New(
    Head.New(
        Title.New("Product Page"),
        Meta.New(Charset << "UTF-8"),
        Link.New(
            Rel << "stylesheet",
            Href << "/css/styles.css"
        )
    ),
    Body.New(
        Header.New(Class << "site-header").New(
            Nav.New(
                Ul.New(
                    Li.New(A.New(Href << "/", "Home")),
                    Li.New(A.New(Href << "/about", "About")),
                    Li.New(A.New(Href << "/contact", "Contact"))
                )
            )
        ),
        Main.New(
            Article.New(
                H1.New("Product Name"),
                Img.New(
                    Src << "product.jpg",
                    Alt << "Product image",
                    WidthAttr << 400,
                    HeightAttr << 300
                ),
                P.New("Product description..."),
                Button.New(
                    Class << "buy-btn",
                    Id_ << "purchase-button",
                    "Add to Cart")
            )
        )
    )
);
```

### Custom Encoders

Create custom attribute encoders for special use cases:

```csharp
// String encoder (HTML encodes automatically)
var customAttr = new HtmlKey<string>
{
    Name = "data-custom",
    Encode = val => WebUtility.HtmlEncode(val)
};

// Boolean as "yes"/"no"
var yesNoAttr = new HtmlKey<bool>
{
    Name = "data-enabled",
    Encode = val => val ? "yes" : "no"
};

// Enum as kebab-case
var enumAttr = new HtmlKey<MyEnum>
{
    Name = "data-mode",
    Encode = val => System.Text.RegularExpressions.Regex.Replace(
        val.ToString(), "([a-z])([A-Z])", "$1-$2").ToLowerInvariant()
};

// Either key (union type for flexible input)
var flexibleType = Prelude.EitherKey<string, int>(
    "data-value",
    s => s,  // string encoder
    i => i.ToString()  // int encoder
);
// Works with both string and int
var elem1 = Div.New(flexibleType << "hello");
var elem2 = Div.New(flexibleType << 42);
```

## ASP.NET Core Integration

Install the ASP.NET Core package:

```xml
<PackageReference Include="CsharpTags.AspNetCore" Version="1.0.0-beta-4" />
```

### MVC Controllers

Convert `HtmlElement` to `IActionResult`:

```csharp
using CsharpTags.AspNetCore;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        var html = Div.New(
            Class << "container",
            H1.New("Welcome"),
            P.New("Hello from CsharpTags!")
        );
        
        return html.ToActionResult();
    }
}
```

### Minimal APIs

Convert `HtmlElement` to `IResult`:

```csharp
using CsharpTags.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () =>
{
    var html = Html.New(
        Head.New(Title.New("Home")),
        Body.New(H1.New("Hello World"))
    );
    return html.ToResult();
});

app.Run();
```

### Endpoint Filters

Use the `HtmlElementEndpointFilter` for automatic HTML rendering:

```csharp
using CsharpTags.AspNetCore.Filters;

// Apply to specific endpoints
app.MapGet("/users", () => GetUserList())
   .AddEndpointFilter<HtmlElementEndpointFilter>();

// Or apply to a group
var group = app.MapGroup("/api")
   .AddEndpointFilter<HtmlElementEndpointFilter>();

group.MapGet("/users", () => GetUserList());
group.MapGet("/posts", () => GetPostList());
```

### HTML Transformations

Register custom transformations that apply to all HTML elements processed by the filter:

```csharp
using CsharpTags.AspNetCore;
using static LanguageExt.Prelude;

// Add transformation to services
builder.Services.AddHtmlTranformation(services => element =>
{
    // Add a class to all divs
    if (element is Tag { TagName: "div" } div)
    {
        return Some(div.New(div.Attributes.Append(Class << "processed"), div.Children));
    }
    return None;
});

// Anti-forgery token auto-insertion
builder.Services.AddAntiforgery();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHtmlTransformationAntiForgeryToken();
```

### Route Handler Extensions

```csharp
using Microsoft.AspNetCore.Builder;

// Add ProducesHtml extension for OpenAPI/documentation support
app.MapGet("/page", () => GetPage())
   .ProducesHtml();  // Adds text/html content type to API documentation
```

## HTMX Integration

For HTMX support, install the HTMX package:

```xml
<PackageReference Include="CsharpTags.Htmx" Version="1.0.0-beta-4" />
```

### Basic HTMX Usage

```csharp
using static CsharpTags.Core.Types.Prelude;
using static CsharpTags.Htmx.Types.Prelude;

var button = Button.New(
    HxPost << "/api/update",
    HxTarget << "#result",
    HxSwap << SwapStrategy.InnerHTML,
    "Update Content"
);
```

### HTMX Target Helpers

```csharp
// Various target selection strategies
HxTargetThis                              // hx-target="this"
HxTargetClosest("tr")                     // hx-target="closest tr"
HxTargetFind(".error")                    // hx-target="find .error"
HxTargetNext(".item")                     // hx-target="next .item"
HxTargetPrevious(".item")                 // hx-target="previous .item"
HxTargetNext_                             // hx-target="next"
HxTargetPrevious_                         // hx-target="previous"
```

### Swap Strategies

```csharp
// Basic strategies
HxSwap << SwapStrategy.InnerHTML;          // Replace inner HTML
HxSwap << SwapStrategy.OuterHTML;          // Replace entire element
HxSwap << SwapStrategy.BeforeBegin;        // Insert before element
HxSwap << SwapStrategy.AfterBegin;         // Insert as first child
HxSwap << SwapStrategy.BeforeEnd;          // Insert as last child
HxSwap << SwapStrategy.AfterEnd;           // Insert after element
HxSwap << SwapStrategy.Delete;             // Delete target element
HxSwap << SwapStrategy.None;               // Don't swap
HxSwap << SwapStrategy.TextContent;        // Replace text only

// With modifiers
HxSwap << SwapStrategy.InnerHTML + "swap:1s transition:true";
HxSwap << SwapStrategy.OuterHTML.Modify("focus-scroll:true");
```

### Sync Strategies

```csharp
// Control request synchronization
HxSync << (SyncStrategy.Drop + "#form");      // Drop if request in flight
HxSync << (SyncStrategy.Abort + "#form");     // Abort on new request
HxSync << (SyncStrategy.Replace + "#form");   // Replace current request
HxSync << (SyncStrategy.Queue + "#form");     // Add to queue
HxSync << (SyncStrategy.QueueFirst + "#form"); // Add to front of queue
HxSync << (SyncStrategy.QueueLast + "#form");  // Add to end of queue
HxSync << (SyncStrategy.QueueAll + "#form");    // Queue all requests
```

### HTMX Events

```csharp
// Built-in event handlers
HxOnClick << "alert('Clicked!')"
HxOnSubmit << "validateForm()"
HxOnChange << "updatePreview()"
HxOnKeyUp << "search()"
HxOnLoad << "initialize()"
HxOnMouseOver << "highlight()"

// Custom events
HxOn("custom-event") << "handleCustom()"
```

### Complete HTMX Example

```csharp
using static CsharpTags.Core.Types.Prelude;
using static CsharpTags.Htmx.Types.Prelude;

var userInterface = Div.New(
    // Search with debouncing
    Input.New(
        TypeAttr << InputType.Text,
        HxGet << "/api/search",
        HxTrigger << "keyup changed delay:500ms",
        HxTarget << "#results",
        Placeholder << "Search users..."
    ),
    
    // Results area
    Div.New(Id_ << "results"),
    
    // Update user form
    Form.New(
        HxPut << "/api/users/1",
        HxTargetClosest("tr"),
        HxSwap << SwapStrategy.OuterHTML + "transition:true",
        Input.New(TypeAttr << InputType.Text, Name << "username", Value << "john_doe"),
        Button.New(HxOnClick << "this.closest('form').requestSubmit()")
            .New("Save")
    )
);
```

## License

Copyright (c) Frairlyn Camilo Roque Suarez. All rights reserved.

## Contributing

This library is designed with extensibility in mind. Feel free to submit issues and pull requests for additional HTML elements, attributes, or features.

---

**Note**: This is a beta release. API may change before version 1.0.0.
