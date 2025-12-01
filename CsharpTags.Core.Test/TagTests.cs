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
        var div = Div.Attr(Class << "container", Id_ << "main");

        // Act
        var result = div.Render();

        // Assert
        Assert.Equal("<div class=\"container\" id=\"main\"></div>", result);
    }

    [Fact]
    public void Tag_Render_WithChildren()
    {
        // Arrange
        var div = Div.Child(
            H1.Child("Hello World"),
            P.Child("This is a paragraph")
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
        var img = Img.Attr(Src << "image.jpg", Alt << "An image");

        // Act
        var result = img.Render();

        // Assert
        Assert.Equal("<img src=\"image.jpg\" alt=\"An image\" />", result);
    }

    [Fact]
    public void Tag_Render_NestedStructure()
    {
        // Arrange
        var html = Html.Child(
            Head.Child(
                Title.Child("Test Page"),
                Meta.Attr(Charset << "UTF-8")
            ),
            Body.Child(
                Div.Attr(Class << "container").Child(
                    H1.Child("Welcome"),
                    P.Child("This is a test page.")
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
        var div = Div.Attr(Class << "container");
        var divWithId = div.AppendAttr(Id_ << "main");

        // Act
        var result = divWithId.Render();

        // Assert
        Assert.Equal("<div class=\"container\" id=\"main\"></div>", result);
    }

    [Fact]
    public void Tag_AppendChildren_AddsToExisting()
    {
        // Arrange
        var div = Div.Child(H1.Child("Title"));
        var divWithParagraph = div.AppendChild(P.Child("Content"));

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
            Li.Child("Item 1"),
            Li.Child("Item 2"),
            Li.Child("Item 3")
        };

        var ul = Ul.Child(items.ToHtml());

        // Act
        var result = ul.Render();

        // Assert
        Assert.Equal("<ul><li>Item 1</li><li>Item 2</li><li>Item 3</li></ul>", result);
    }

    private HtmlElement Counter(int value)
        => Input.Attr(Tpe << InputType.Hidden, Value << value.ToString(), Name << value.ToString());

    [Fact]
    public void Tag_ConditionalRender_FirstConditionTrue()
    {
        var ele = IfH(true, () => Div.Child(Counter(1)))
            .ElseIf(() => true, () => Counter(2))
            .Else(() => Counter(3));

        Assert.Equal(Div.Child(Counter(1)), ele);
    }

    [Fact]
    public void Tag_ConditionalRender_SecondConditionTrue()
    {
        var ele = IfH(false, () => Div.Child(Counter(1)))
            .ElseIf(() => true, () => Counter(2))
            .Else(() => Counter(3));

        Assert.Equal(ele, Counter(2));
    }

    [Fact]
    public void Tag_ConditionalRender_AllConditionsFalse()
    {
        var ele = IfH(false, () => Div.Child(Counter(1)))
            .ElseIf(() => false, () => Counter(2))
            .Else(() => Counter(3));

        Assert.Equal(Counter(3), ele);
    }

    [Fact]
    public void Tag_ConditionalRender_SingleIfWithoutElse()
    {
        var ele = IfH(true, () => Counter(1)).Element;

        Assert.Equal(Counter(1), ele);
    }

    [Fact]
    public void Tag_ConditionalRender_SingleIfFalseWithoutElse()
    {
        var ele = IfH(false, () => Counter(1)).Element;

        Assert.Equal(None_, ele);
    }

    [Fact]
    public void Tag_ConditionalRender_MultipleElseIfConditions()
    {
        var ele = IfH(false, () => Counter(1))
            .ElseIf(() => false, () => Counter(2))
            .ElseIf(() => true, () => Counter(3))
            .ElseIf(() => true, () => Counter(4)) // This should not be evaluated
            .Else(() => Counter(5));

        Assert.Equal(Counter(3), ele);
    }

    [Fact]
    public void Tag_ConditionalRender_ComplexElements()
    {
        var complexElement = Div.Child(
            Span.Child("Hello"),
            Counter(1),
            P.Child("World")
        );

        var ele = IfH(true, () => complexElement)
            .Else(() => Counter(99));

        Assert.Equal(complexElement, ele);
    }

    [Fact]
    public void Tag_ConditionalRender_NestedConditionals()
    {
        var ele = IfH(true,
                () => IfH(false, () => Counter(1))
                .Else(() => Counter(2))
            )
            .Else(() => Counter(3));

        Assert.Equal(Counter(2), ele);
    }

    [Fact]
    public void Tag_ConditionalRender_EmptyElse()
    {
        var ele = IfH(false, () => Counter(1))
            .ElseIf(() => false, () => Counter(2)).Element;

        // Should handle empty else case gracefully
        Assert.Equal(None_, ele);
    }

    [Fact]
    public void Tag_ConditionalRender_DynamicConditions()
    {
        var condition1 = 1 + 1 == 2; // true
        var condition2 = () => 2 + 2 == 5; // false

        var ele = IfH(condition1, () => Counter(1))
            .ElseIf(condition2, () => Counter(2))
            .Else(() => Counter(3));

        Assert.Equal(Counter(1), ele);
    }

    [Fact]
    public void Tag_ConditionalRender_WithAttributes()
    {
        var ele = IfH(true,
                () => Input.Attr(Tpe << InputType.Text, Value << "test", Name << "field1")
            )
            .Else(() => Input.Attr(Tpe << InputType.Hidden, Value << "fallback", Name << "field2"));

        var expected = Input.Attr(Tpe << InputType.Text, Value << "test", Name << "field1");
        Assert.Equal(expected, ele);
    }
}
