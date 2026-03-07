# CsharpTags.Htmx

A type-safe HTMX attribute library for C# that provides strongly-typed HTML attributes for HTMX functionality.

## Overview

This library offers a type-safe way to work with HTMX attributes in C#, providing compile-time safety and IntelliSense support for all HTMX attributes and swap strategies. It's built on top of `CsharpTags.Core` and follows functional programming principles.

## Installation

```xml
<PackageReference Include="CsharpTags.Htmx" Version="1.0.0-beta-4" />
```

## Quick Start

```csharp
using static CsharpTags.Core.Types.Prelude;
using static CsharpTags.Htmx.Types.Prelude;

// Create an element with HTMX attributes
var button = Button.New(
    HxPost << "/api/update",
    HxTarget << "#result",
    HxSwap << SwapStrategy.InnerHTML,
    "Update Content"
);

// Use with target helpers
var form = Form.New(
    HxPost << "/submit",
    HxTargetClosest("tr"),
    HxSwap << SwapStrategy.OuterHTML,
    // ... form content
);
```

## Core Attributes

### HTTP Methods

All standard HTTP methods are supported:

- `HxGet` - GET requests
- `HxPost` - POST requests  
- `HxPut` - PUT requests
- `HxPatch` - PATCH requests
- `HxDelete` - DELETE requests

```csharp
// GET request
var search = Input.New(
    TypeAttr << InputType.Text,
    HxGet << "/api/search",
    HxTarget << "#results"
);

// POST request
var form = Form.New(
    HxPost << "/api/submit",
    HxTarget << "#response",
    // ... form fields
);
```

### Target Selection

HTMX provides flexible target selection:

```csharp
// Target specific element by ID
HxTarget << "#results"

// Target the current element
HxTargetThis

// Target closest ancestor matching selector
HxTargetClosest("tr")
HxTargetClosest(".container")

// Find first child matching selector
HxTargetFind(".error")
HxTargetFind("input[type='text']")

// Target next sibling
HxTargetNext_
HxTargetNext(".item")

// Target previous sibling
HxTargetPrevious_
HxTargetPrevious(".item")
```

### Swap Strategies

Control how response content is swapped into the DOM:

```csharp
// Basic strategies
HxSwap << SwapStrategy.InnerHTML;     // Replace inner HTML (default)
HxSwap << SwapStrategy.OuterHTML;       // Replace entire element
HxSwap << SwapStrategy.BeforeBegin;     // Insert before target
HxSwap << SwapStrategy.AfterBegin;      // Insert as first child
HxSwap << SwapStrategy.BeforeEnd;       // Insert as last child
HxSwap << SwapStrategy.AfterEnd;        // Insert after target
HxSwap << SwapStrategy.Delete;          // Delete target element
HxSwap << SwapStrategy.None;            // Don't swap
HxSwap << SwapStrategy.TextContent;     // Replace text only
```

### Swap Modifiers

Add modifiers to swap strategies for advanced behavior:

```csharp
// Combine swap strategy with modifiers using + operator
HxSwap << SwapStrategy.InnerHTML + "swap:1s"
HxSwap << SwapStrategy.OuterHTML + "transition:true"
HxSwap << SwapStrategy.InnerHTML + "scroll:top"

// Or use Modify method
HxSwap << SwapStrategy.OuterHTML.Modify("focus-scroll:true settle:100ms")

// Multiple modifiers
HxSwap << SwapStrategy.InnerHTML + "swap:500ms transition:true scroll:bottom"
```

### Synchronization Strategies

Control how HTMX synchronizes concurrent requests:

```csharp
// Build sync strategy with + operator
HxSync << (SyncStrategy.Drop + "#form")        // Drop if request in flight
HxSync << (SyncStrategy.Abort + "#form")       // Abort on new request  
HxSync << (SyncStrategy.Replace + "#form")    // Replace current request
HxSync << (SyncStrategy.Queue + "#form")      // Add to queue
HxSync << (SyncStrategy.QueueFirst + "#form") // Add to front of queue
HxSync << (SyncStrategy.QueueLast + "#form")  // Add to end of queue
HxSync << (SyncStrategy.QueueAll + "#form")   // Queue all requests
```

### Triggers

Define when requests are triggered:

```csharp
// Basic triggers
HxTrigger << "click"
HxTrigger << "change"
HxTrigger << "submit"
HxTrigger << "keyup"

// Advanced triggers with modifiers
HxTrigger << "keyup changed delay:500ms"
HxTrigger << "click once"
HxTrigger << "revealed"
HxTrigger << "intersect once"

// Multiple triggers
HxTrigger << "click, keyup[key=='Enter']"
```

### Event Handling

Handle HTMX events inline:

```csharp
// Built-in common event handlers
HxOnClick << "alert('Clicked!')"
HxOnSubmit << "validateForm()"
HxOnChange << "updatePreview()"
HxOnKeyUp << "search()"
HxOnLoad << "initialize()"
HxOnMouseOver << "highlight()"

// Custom event handlers
HxOn("custom-event") << "handleCustom()"

// HTMX-specific events
HxOn("htmx:beforeRequest") << "showLoading()"
HxOn("htmx:afterRequest") << "hideLoading()"
HxOn("htmx:responseError") << "showError()"
```

### Form Enhancements

Enhance forms with HTMX attributes:

```csharp
// Boost links and forms to use AJAX
HxBoost << true

// Validate before submitting
HxValidate << true

// Confirmation dialog
HxConfirm << "Are you sure?"

// Prompt for input
HxPrompt << "Enter value:"

// Include additional values
HxVals << "{\"key\": \"value\"}"

// Select subset of response
HxSelect << "#content"

// Out-of-band swap
HxSwapOob << "true"
```

### Advanced Attributes

```csharp
// Push URL to browser history
HxPushUrl << "/new-url"
HxPushUrl << "true"

// Specify indicator element
HxIndicator << "#loading"

// Include additional elements in request
HxInclude << "[name='filter']"

// Set request headers
HxHeaders << "{\"X-Custom\": \"value\"}"

// Encoding type
HxEncoding << "multipart/form-data"

// Disable HTMX processing
HxDisabled << true

// History management
HxHistoryElt << true

// Disinherit from parent
HxDisinherit << "hx-trigger"

// Use extensions
HxExt << "json-enc"
```

### Timing and Delays

```csharp
// Delay before request
HxDelay << 500           // 500ms delay

// Settle time after swap
HxSettle << 100          // 100ms settle
```

## Complete Examples

### Search with Debouncing

```csharp
using static CsharpTags.Core.Types.Prelude;
using static CsharpTags.Htmx.Types.Prelude;

var searchComponent = Div.New(
    // Search input with debouncing
    Input.New(
        TypeAttr << InputType.Text,
        Name << "query",
        HxGet << "/api/search",
        HxTrigger << "keyup changed delay:300ms",
        HxTarget << "#search-results",
        HxIndicator << "#loading",
        Placeholder << "Search users..."
    ),
    
    // Loading indicator
    Div.New(
        Id_ << "loading",
        Class << "htmx-indicator",
        "Searching..."
    ),
    
    // Results container
    Div.New(Id_ << "search-results")
);
```

### Inline Edit Form

```csharp
var editForm = Form.New(
    HxPut << "/api/users/{userId}",
    HxTargetClosest("tr"),              // Replace the table row
    HxSwap << SwapStrategy.OuterHTML + "transition:true",
    HxConfirm << "Save changes?",
    
    // Form fields
    Input.New(
        TypeAttr << InputType.Text,
        Name << "username",
        Value << currentUsername
    ),
    
    // Save button
    Button.New(
        TypeAttr << InputType.Submit,
        "Save"
    ),
    
    // Cancel button (triggers cancel action)
    Button.New(
        HxGet << $"/api/users/{userId}/display",
        HxTargetClosest("tr"),
        "Cancel"
    )
);
```

### Infinite Scroll

```csharp
var feed = Div.New(
    Id_ << "feed",
    
    // Current items
    items.Select(item => Div.New(
        Class << "feed-item",
        H3.New(item.Title),
        P.New(item.Content)
    )).ToHtml(),
    
    // Load more trigger
    Div.New(
        HxGet << $"/api/feed?page={nextPage}",
        HxTrigger << "revealed",
        HxTarget << "#feed",
        HxSwap << SwapStrategy.BeforeEnd,
        Class << "loading-indicator",
        "Loading more..."
    )
);
```

### Tabbed Interface

```csharp
var tabs = Div.New(
    Class << "tabs",
    
    // Tab buttons
    Div.New(
        Class << "tab-buttons",
        Button.New(
            HxGet << "/content/tab1",
            HxTarget << "#tab-content",
            Class << "active",
            "Tab 1"
        ),
        Button.New(
            HxGet << "/content/tab2",
            HxTarget << "#tab-content",
            "Tab 2"
        ),
        Button.New(
            HxGet << "/content/tab3",
            HxTarget << "#tab-content",
            "Tab 3"
        )
    ),
    
    // Tab content area
    Div.New(
        Id_ << "tab-content",
        Class << "tab-content",
        initialContent
    )
);
```

### Modal Dialog

```csharp
var modal = Div.New(
    Id_ << "modal",
    Role << "dialog",
    AriaModal << "true",
    
    // Close on click outside
    HxOn("click") << "if(event.target === this) htmx.remove(this)",
    
    Div.New(
        Class << "modal-content",
        
        // Header
        Div.New(
            Class << "modal-header",
            H2.New("Confirm Action"),
            Button.New(
                HxOnClick << "htmx.remove(#modal)",
                Class << "close",
                "&times;"
            )
        ),
        
        // Body
        Div.New(
            Class << "modal-body",
            P.New("Are you sure you want to proceed?")
        ),
        
        // Footer with actions
        Div.New(
            Class << "modal-footer",
            Button.New(
                HxPost << "/api/action",
                HxTarget << "#result",
                HxSwap << SwapStrategy.OuterHTML,
                HxConfirm << "This cannot be undone. Continue?",
                Class << "btn-primary",
                "Confirm"
            ),
            Button.New(
                HxOnClick << "htmx.remove(#modal)",
                Class << "btn-secondary",
                "Cancel"
            )
        )
    )
);
```

## Error Handling

Handle HTMX errors gracefully:

```csharp
var formWithErrorHandling = Form.New(
    HxPost << "/api/submit",
    HxTarget << "#response",
    
    // Success handling
    HxOn("htmx:afterRequest") << """
        if(event.detail.successful) {
            showNotification('Saved successfully!');
        }
        """,
    
    // Error handling
    HxOn("htmx:responseError") << """
        showError('Failed to save. Please try again.');
        """,
    
    // Form fields
    Input.New(TypeAttr << InputType.Text, Name << "data"),
    Button.New(TypeAttr << InputType.Submit, "Submit")
);
```

## Requirements

- .NET 8.0 or .NET 10.0
- CsharpTags.Core package
- HTMX library in your frontend (typically via CDN or npm)

## Links

- [HTMX Documentation](https://htmx.org/docs/)
- [HTMX Reference](https://htmx.org/reference/)
- [CsharpTags.Core Documentation](../README.md)

## License

MIT License - see LICENSE file for details.
