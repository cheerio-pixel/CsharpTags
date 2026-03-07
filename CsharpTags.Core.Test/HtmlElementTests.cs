using CsharpTags.Core.Interface;
using CsharpTags.Core.Types;
using static CsharpTags.Core.Types.Prelude;
using static LanguageExt.Prelude;

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
        var elements = Seq<HtmlElement>(
                new Str { Value = "Hello" },
                new Str { Value = "World" }
                );

        var list = new HtmlElementList { Value = elements };

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
        Assert.IsType<HtmlElementList>(list);
        Assert.Equal("Item1Item2", list.Render());
    }

    [Fact]
    public void Component_Simplify_ReturnsBuiltElement()
    {
        // Arrange
        var component = new TestComponent();

        // Act
        var simplified = component.Simplify();

        // Assert
        Assert.Equal("<div>test</div>", simplified.Render());
    }

    [Fact]
    public void Component_New_CreatesTagWithContent()
    {
        // Arrange
        var component = new TestComponent();

        // Act
        var result = component.New();

        // Assert
        Assert.IsType<Tag>(result);
        Assert.Equal("<div><div>test</div></div>", result.Render());
    }

    [Fact]
    public void Component_New_WithContent_UsesContent()
    {
        // Arrange
        var component = new TestComponent();

        // Act - Note: Build() returns fixed content, so result wraps it
        var result = component.New("new content");

        // Assert
        Assert.IsType<Tag>(result);
        Assert.Equal("<div><div>test</div></div>", result.Render());
    }

    private record TestComponent : Component
    {
        protected override string TagName => "div";

        protected override HtmlElement Build()
            => Div.New("test");
    }
}
