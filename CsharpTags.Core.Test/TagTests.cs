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
}
