using CsharpTags.Core.Types;
using static CsharpTags.Core.Types.Prelude;
using CsharpTags.Core.Interface;

namespace CsharpTags.Core.Tests;

public class TagTests
{
    [Fact]
    public void Tag_Render_SingleTag_NoAttributes_NoChildren()
    {
        // Arrange
        var div = Div;

        // Act
        var result = div.Render();

        // Assert
        Assert.Equal("<div></div>", result);
    }

    [Fact]
    public void Tag_Render_WithAttributes()
    {
        // Arrange
        var div = Div.New(Class << "container", Id_ << "main");

        // Act
        var result = div.Render();

        // Assert
        Assert.Equal("<div class=\"container\" id=\"main\"></div>", result);
    }

    [Fact]
    public void Tag_Render_WithChildren()
    {
        // Arrange
        var div = Div.New(
            H1.New("Hello World"),
            P.New("This is a paragraph")
        );

        // Act
        var result = div.Render();

        // Assert
        Assert.Equal("<div><h1>Hello World</h1><p>This is a paragraph</p></div>", result);
    }

    [Fact]
    public void Tag_Render_VoidTag()
    {
        // Arrange
        var img = Img.New(Src << "image.jpg", Alt << "An image");

        // Act
        var result = img.Render();

        // Assert
        Assert.Equal("<img src=\"image.jpg\" alt=\"An image\" />", result);
    }

    [Fact]
    public void Tag_Render_NestedStructure()
    {
        // Arrange
        var html = Html.New(
            Head.New(
                Title.New("Test Page"),
                Meta.New(Charset << "UTF-8")
            ),
            Body.New(
                Div.New(Class << "container",
                    H1.New("Welcome"),
                    P.New("This is a test page.")
                )
            )
        );

        // Act
        var result = html.Render();

        // Assert
        Assert.Contains("<html>", result);
        Assert.Contains("<head>", result);
        Assert.Contains("<title>Test Page</title>", result);
        Assert.Contains("<body>", result);
        Assert.Contains("<div class=\"container\">", result);
        Assert.Contains("<h1>Welcome</h1>", result);
    }

    [Fact]
    public void Tag_AppendAttributes_AddsToExisting()
    {
        // Arrange
        var div = Div.New(Class << "container");
        var divWithId = div.Append(Id_ << "main");

        // Act
        var result = divWithId.Render();

        // Assert
        Assert.Equal("<div class=\"container\" id=\"main\"></div>", result);
    }

    [Fact]
    public void Tag_AppendChildren_AddsToExisting()
    {
        // Arrange
        var div = Div.New(H1.New("Title"));
        var divWithParagraph = div.Append(P.New("Content"));

        // Act
        var result = divWithParagraph.Render();

        // Assert
        Assert.Equal("<div><h1>Title</h1><p>Content</p></div>", result);
    }

    [Fact]
    public void Tag_Render_WithListChildren()
    {
        // Arrange
        var items = new HtmlElement[]
        {
            Li.New("Item 1"),
            Li.New("Item 2"),
            Li.New("Item 3")
        };

        var ul = Ul.New(items.ToHtml());

        // Act
        var result = ul.Render();

        // Assert
        Assert.Equal("<ul><li>Item 1</li><li>Item 2</li><li>Item 3</li></ul>", result);
    }

    [Fact]
    public void Tag_New_WithMultipleParams_ShouldWork()
    {
        // Arrange & Act
        var div = Div.New(
            Class << "container",
            P.New("Paragraph 1"),
            P.New("Paragraph 2"),
            Id_ << "main"
        );

        var result = div.Render();

        // Assert
        Assert.Contains("class=\"container\"", result);
        Assert.Contains("id=\"main\"", result);
        Assert.Contains("<p>Paragraph 1</p>", result);
        Assert.Contains("<p>Paragraph 2</p>", result);
    }

    [Fact]
    public void Tag_Append_WithMultipleParams_ShouldWork()
    {
        // Arrange
        var div = Div.New(P.New("Initial"));

        // Act
        var updated = div.Append(
            P.New("Appended 1"),
            P.New("Appended 2"),
            Class << "appended-class"
        );

        var result = updated.Render();

        // Assert
        Assert.Contains("<p>Initial</p>", result);
        Assert.Contains("<p>Appended 1</p>", result);
        Assert.Contains("<p>Appended 2</p>", result);
        Assert.Contains("class=\"appended-class\"", result);
    }

    [Fact]
    public void Tag_Attr_WithMultipleParams_ShouldWork()
    {
        // Arrange
        var div = Div;

        // Act
#pragma warning disable CS0618
        var withAttrs = div.Attr(
            Class << "container",
            Id_ << "main",
            StyleAttr << "color: red;"
        );
#pragma warning restore CS0618

        var result = withAttrs.Render();

        // Assert
        Assert.Contains("class=\"container\"", result);
        Assert.Contains("id=\"main\"", result);
        Assert.Contains("style=\"color: red;\"", result);
    }

    [Fact]
    public void Tag_Child_WithMultipleParams_ShouldWork()
    {
        // Arrange
        var div = Div;

        // Act
#pragma warning disable CS0618
        var withChildren = div.Child(
            H1.New("Title"),
            P.New("Paragraph 1"),
            P.New("Paragraph 2")
        );
#pragma warning restore CS0618

        var result = withChildren.Render();

        // Assert
        Assert.Contains("<h1>Title</h1>", result);
        Assert.Contains("<p>Paragraph 1</p>", result);
        Assert.Contains("<p>Paragraph 2</p>", result);
    }

    [Fact]
    public void Tag_AppendAttr_WithMultipleParams_ShouldWork()
    {
        // Arrange
        var div = Div.New(Class << "initial");

        // Act
#pragma warning disable CS0618
        var withMoreAttrs = div.AppendAttr(
            Id_ << "main",
            StyleAttr << "color: blue;"
        );
#pragma warning restore CS0618

        var result = withMoreAttrs.Render();

        // Assert
        Assert.Contains("class=\"initial\"", result);
        Assert.Contains("id=\"main\"", result);
        Assert.Contains("style=\"color: blue;\"", result);
    }

    [Fact]
    public void Tag_AppendChild_WithMultipleParams_ShouldWork()
    {
        // Arrange
        var div = Div.New(H1.New("Title"));

        // Act
#pragma warning disable CS0618
        var withMoreChildren = div.AppendChild(
            P.New("First"),
            P.New("Second"),
            P.New("Third")
        );
#pragma warning restore CS0618

        var result = withMoreChildren.Render();

        // Assert
        Assert.Contains("<h1>Title</h1>", result);
        Assert.Contains("<p>First</p>", result);
        Assert.Contains("<p>Second</p>", result);
        Assert.Contains("<p>Third</p>", result);
    }

    [Fact]
    public void Tag_ComplexNestedStructure_ShouldRenderCorrectly()
    {
        // Arrange
        var html = Html.New(
            Head.New(
                Title.New("Test Page"),
                Meta.New(Charset << "UTF-8"),
                Link.New(Rel << "stylesheet", Href << "style.css")
            ),
            Body.New(
                Header.New(
                    Nav.New(
                        A.New(Href << "/", "Home"),
                        A.New(Href << "/about", "About"),
                        A.New(Href << "/contact", "Contact")
                    )
                ),
                Main.New(
                    Section.New(
                        H1.New("Welcome"),
                        Article.New(
                            H2.New("Article Title"),
                            P.New("Article content here...")
                        )
                    )
                ),
                Footer.New(
                    P.New("Copyright 2025")
                )
            )
        );

        // Act
        var result = html.Render();

        // Assert
        Assert.Contains("<html>", result);
        Assert.Contains("<head>", result);
        Assert.Contains("<title>Test Page</title>", result);
        Assert.Contains("<body>", result);
        Assert.Contains("<header>", result);
        Assert.Contains("<nav>", result);
        Assert.Contains("<a href=\"/\">Home</a>", result);
        Assert.Contains("<main>", result);
        Assert.Contains("<section>", result);
        Assert.Contains("<article>", result);
        Assert.Contains("<footer>", result);
    }

    [Fact]
    public void Tag_MixedAttributesAndChildren_ShouldSeparateCorrectly()
    {
        // Arrange
        var div = Div.New(
            // Attributes
            Class << "mixed-test",
            Id_ << "mixed-id",
            // Children
            H1.New("Header"),
            P.New("Paragraph"),
            // More attributes
            StyleAttr << "margin: 10px;",
            // More children
            Span.New("Span content")
        );

        // Act
        var result = div.Render();

        // Assert
        // Check all attributes are in opening tag
        Assert.Contains("<div class=\"mixed-test\" id=\"mixed-id\" style=\"margin: 10px;\">", result);
        // Check children are in correct order
        Assert.Contains("<h1>Header</h1>", result);
        Assert.Contains("<p>Paragraph</p>", result);
        Assert.Contains("<span>Span content</span>", result);
        Assert.Contains("</div>", result);
    }
}
