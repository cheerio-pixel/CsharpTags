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
                Id << "submit-btn",
                Class << "btn btn-primary",
                Disabled_ << false
            ).New("Click Me")
        )
    )
);

string result = html.Render();
```

## Asp Net Core

If you install 

```xml
<PackageReference Include="CsharpTags.AspNetCore" Version="1.0.0-beta-4" />
```
And add the import
```csharp
using CsharpTags.AspNetCore;
```
You will have access to the extension methods .ToActionResult() and .ToResult()

If you use minimal apis you can do something similar to that of Carter.

## Carter

Having the CsharpTags.AspNetCore package you can register the filter Carter like this:
```csharp
var group = app.MapGroup(string.Empty).AddEndpointFilter<HtmlElementEndpointFilter>();
group.MapCarter();
```

For anti forgery tokens run this
```csharp
services.AddAntiforgery();
services.AddHttpContextAccessor();
services.AddHtmlTransformationAntiForgeryToken()
```

This is only going to work for HtmlElement that are processed by HtmlElementEndpointFilter
    

## Core Concepts

### HTML Elements

The library provides all standard HTML5 elements as static properties:

```csharp
var div = Div.New("Hello World");
var link = A.New(Href << "/page.html", "Click here");
var image = Img.New(Src << "photo.jpg", Alt << "A photo");
```

### Attributes

Type-safe attributes with proper encoding:

```csharp
// String attributes (automatically encoded)
var div = Div.New(Class << "container<test>"); // becomes class="container&lt;test&gt;"

// Boolean attributes (presence-based)
var input = Input.New(Disabled_ << true); // becomes <input disabled />
var input2 = Input.New(Disabled_ << false); // becomes <input />

// Integer attributes
var input3 = Input.New(TabIndex << 5); // becomes tabindex="5"
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
    Li.New("Item 1"),
    Li.New("Item 2"),
    Li.New("Item 3")
};

var list = Ul.New(items.ToHtml());
```

## Usage

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
var element = Div.New(
    DataAttr("user-id") << "12345",
    DataAttr("role") << "admin"
);
// Renders as: <div data-user-id="12345" data-role="admin"></div>
```

### ARIA Attributes

Full accessibility support:

```csharp
var button = Button.New(
    Label << "Submit form",
    DescribedBy << "submit-help",
    Disabled << false,
    "Submit");
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
                    Width_ << 400,
                    Height_ << 300
                ),
                P.New("Product description..."),
                Button.New(
                    Class << "buy-btn",
                    Id << "purchase-button",
                    "Add to Cart")
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
