# CsharpTags.Htmx

A type-safe HTMX attribute library for C# that provides strongly-typed HTML attributes for HTMX functionality.

## Overview

This library offers a type-safe way to work with HTMX attributes in C#, providing compile-time safety and IntelliSense support for all HTMX attributes and swap strategies. It's built on top of `CsharpTags.Core` and follows functional programming principles.

## Installation

```xml
<PackageReference Include="CsharpTags.Htmx" Version="1.0.0-beta-3" />
```

## Quick Start

```csharp
using static CsharpTags.Core.Types.Prelude;
using static CsharpTags.Htmx.Types.Prelude;

// Create an element with HTMX attributes
var button = Button.Attr(
    HxPost << "/api/update",
    HxTarget << "#result",
    HxSwap << SwapStrategy.InnerHTML,
    ).Child(
    "Update Content"
);

// Use with target helpers
var form = Form.Attr(
    HxPost << "/submit",
    HxTargetClosest("tr"),
    HxSwap << SwapStrategy.OuterHTML,
    ).Child(
    // ... form content
).;
```

## Core Attributes

### HTTP Methods
- `HxGet` - GET requests
- `HxPost` - POST requests  
- `HxPut` - PUT requests
- `HxPatch` - PATCH requests
- `HxDelete` - DELETE requests

### Target Selection
```csharp
HxTargetThis                    // Target the current element
HxTargetClosest("tr")          // Target closest table row
HxTargetFind(".error")         // Find first child matching selector
HxTargetNext(".item")          // Target next sibling matching selector
HxTargetPrevious(".item")      // Target previous sibling matching selector
```

### Swap Modifiers

```csharp
// Add modifiers to swap strategies
HxSwap << SwapStrategy.InnerHTML + "swap:1s"
HxSwap << SwapStrategy.OuterHTML.Modify("transition:true")
```

## Advanced Features

### Synchronization Strategies
```csharp
HxSyncDrop("#form")        // Drop if request in flight
HxSyncAbort("#form")       // Abort on new request  
HxSyncReplace("#form")     // Replace current request
HxSyncQueue("#form")       // Add to queue
HxSyncQueueFirst("#form")  // Add to front of queue
HxSyncQueueLast("#form")   // Add to end of queue
HxSyncQueueAll("#form")    // Queue all requests
```

### Event Handling
```csharp
HxOnClick << "alert('Clicked!')"
HxOnSubmit << "validateForm()"
HxOnChange << "updatePreview()"
HxOnKeyUp << "search()"

// Or create custom event handlers
HxOn("custom-event") << "handleCustom()"
```

### Form Enhancements
```csharp
HxBoost << true                    // Boost links and forms
HxValidate << true                 // Validate before submit
HxConfirm << "Are you sure?"      // Confirmation dialog
HxPrompt << "Enter value:"         // Prompt for input
```

## Complete Example

```csharp
using static CsharpTags.Core.Types.Prelude;
using static CsharpTags.Htmx.Types.Prelude;

var userInterface = Div.Child(
    // Search with debouncing
    Input.Attr(
        Type_ << "text",
        HxGet << "/api/search",
        HxTrigger << "keyup changed delay:500ms",
        HxTarget << "#results",
        Placeholder << "Search users..."
    ),
    
    // Results area
    Div.Attr(id_ << "results"),
    
    // Update user form
    Form.Attr(
        HxPut << "/api/users/1",
        HxTargetClosest("tr"),
        HxSwap << SwapStrategy.OuterHTML + "transition:true",
        ).Child(
        Input.Attr(Type_ << "text", Name << "username", Value << "john_doe"),
        Button.Attr(HxOnClick << "this.closest('form').requestSubmit()")
        .Child("Save")
    )
);
```

## Requirements

- .NET 10.0 or later
- CsharpTags.Core package
- HTMX library in your frontend

## Links

- [HTMX Documentation](https://htmx.org/docs/)

## License

MIT License - see LICENSE file for details.
