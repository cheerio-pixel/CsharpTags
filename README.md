# CsharpTags.Core

A type-safe HTML generation library for C# that provides a fluent, functional approach to building HTML documents with compile-time safety.

## Features

- **Type-safe HTML construction** - Compile-time validation of HTML structure
- **Functional API** - Fluent interface with immutable data structures
- **HTML Encoding** - Automatic encoding of text content and attributes
- **Comprehensive HTML5 Support** - Full coverage of HTML5 elements and attributes
- **ARIA Accessibility** - Built-in support for ARIA attributes
- **CSS Unit Extensions** - Type-safe CSS measurement units

## Installation

```xml
<PackageReference Include="CsharpTags.Core" Version="1.0.0-beta-1" />
```

## Quick Start

```csharp
using CsharpTags.Core.Types;
using static CsharpTags.Core.Types.Prelude;

// Create a simple HTML document
var html = Html.Child(
    Head.Child(
        Title.Child("My Page"),
        Meta.Attr(Charset << "UTF-8")
    ),
    Body.Child(
        Div.Attr(Class << "container").Child(
            H1.Child("Welcome to CsharpTags"),
            P.Child("This is a type-safe HTML generation example."),
            RawStr("<p>This is raw html</p>"),
            Button.Attr(
                Id << "submit-btn",
                Class << "btn btn-primary",
                Disabled_ << false
            ).Child("Click Me")
        )
    )
);

string result = html.Render();
```

## Core Concepts

### HTML Elements

The library provides all standard HTML5 elements as static properties:

```csharp
var div = Div.Child("Hello World");
var link = A.Attr(Href << "/page.html").Child("Click here");
var image = Img.Attr(Src << "photo.jpg", Alt << "A photo");
```

### Attributes

Type-safe attributes with proper encoding:

```csharp
// String attributes (automatically encoded)
var div = Div.Attr(Class << "container<test>"); // becomes class="container&lt;test&gt;"

// Boolean attributes (presence-based)
var input = Input.Attr(Disabled_ << true); // becomes <input disabled />
var input2 = Input.Attr(Disabled_ << false); // becomes <input />

// Integer attributes
var input3 = Input.Attr(TabIndex << 5); // becomes tabindex="5"
```

### Text Content

Automatic HTML encoding for safe text rendering:

```csharp
var safeText = new Str { Value = "<script>alert('xss')</script>" };
// Renders as: &lt;script&gt;alert(&#39;xss&#39;)&lt;/script&gt;
```

### Lists

Combine multiple elements:

```csharp
var items = new HtmlElement[]
{
    Li.Child("Item 1"),
    Li.Child("Item 2"),
    Li.Child("Item 3")
};

var list = Ul.Child(items.ToHtml());
```

## Advanced Usage

### CSS Units

Type-safe CSS measurements using number extensions:

```csharp
var width = 100.Px;        // "100px"
var height = 50.Pct;       // "50%"
var fontSize = 1.2.Rem;    // "1.2rem"
var rotation = 45.Deg;     // "45deg"
```

### Custom Data Attributes

```csharp
var element = Div.Attr(
    DataAttr("user-id") << "12345",
    DataAttr("role") << "admin"
);
// Renders as: <div data-user-id="12345" data-role="admin"></div>
```

### ARIA Attributes

Full accessibility support:

```csharp
var button = Button.Attr(
    Label << "Submit form",
    DescribedBy << "submit-help",
    Disabled << false
).Child("Submit");
```

### Building Complex Structures

```csharp
var page = Html.Child(
    Head.Child(
        Title.Child("Product Page"),
        Meta.Attr(Charset << "UTF-8"),
        Link.Attr(
            Rel << "stylesheet",
            Href << "/css/styles.css"
        )
    ),
    Body.Child(
        Header.Attr(Class << "site-header").Child(
            Nav.Child(
                Ul.Child(
                    Li.Child(A.Attr(Href << "/").Child("Home")),
                    Li.Child(A.Attr(Href << "/about").Child("About")),
                    Li.Child(A.Attr(Href << "/contact").Child("Contact"))
                )
            )
        ),
        Main.Child(
            Article.Child(
                H1.Child("Product Name"),
                Img.Attr(
                    Src << "product.jpg",
                    Alt << "Product image",
                    Width_ << 400,
                    Height_ << 300
                ),
                P.Child("Product description..."),
                Button.Attr(
                    Class << "buy-btn",
                    Id << "purchase-button"
                ).Child("Add to Cart")
            )
        )
    )
);
```

## License

Copyright (c) Frairlyn Camilo Roque Suarez. All rights reserved.

## Contributing

This library is designed with extensibility in mind. Feel free to submit issues and pull requests for additional HTML elements, attributes, or features.

---

**Note**: This is a beta release. API may change before version 1.0.0.
