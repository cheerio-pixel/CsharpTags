using CsharpTags.Core.Interface;
using CsharpTags.Core.Types;
using static CsharpTags.Core.Types.Prelude;

namespace CsharpTags.Core.Tests;

public class HtmlElementTests
{
    [Fact]
    public void Str_Render_EncodesHtml()
    {
        // Arrange
        var str = new Str { Value = "<script>alert('xss')</script>" };

        // Act
        var result = str.Render();

        // Assert
        Assert.Equal("&lt;script&gt;alert(&#39;xss&#39;)&lt;/script&gt;", result);
    }

    [Fact]
    public void Str_Render_HandlesNormalText()
    {
        // Arrange
        var str = new Str { Value = "Hello World" };

        // Act
        var result = str.Render();

        // Assert
        Assert.Equal("Hello World", result);
    }

    [Fact]
    public void List_Render_ConcatenatesElements()
    {
        // Arrange
        var elements = new HtmlElement[]
        {
            new Str { Value = "Hello" },
            new Str { Value = "World" }
        };

        var list = new HtmlList { Value = elements };

        // Act
        var result = list.Render();

        // Assert
        Assert.Equal("HelloWorld", result);
    }

    [Fact]
    public void List_ToHtmlExtension_CreatesList()
    {
        // Arrange
        var elements = new HtmlElement[]
        {
            new Str { Value = "Item1" },
            new Str { Value = "Item2" }
        };

        // Act
        var list = elements.ToHtml();

        // Assert
        Assert.IsType<HtmlList>(list);
        Assert.Equal("Item1Item2", list.Render());
    }
}
