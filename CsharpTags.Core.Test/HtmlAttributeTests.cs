using CsharpTags.Core.Types;
using static CsharpTags.Core.Types.Prelude;

namespace CsharpTags.Core.Tests;

public class HtmlAttributeTests
{
    [Fact]
    public void StringAttribute_Render_EncodesValue()
    {
        // Arrange
        var attr = Class << "container<test>";

        // Act
        var result = attr.Render();

        // Assert
        Assert.Equal("class=\"container&lt;test&gt;\"", result);
    }

    [Fact]
    public void BooleanPresenceAttribute_Render_True_RendersName()
    {
        // Arrange
        var attr = Disabled_ << true;

        // Act
        var result = attr.Render();

        // Assert
        Assert.Equal("disabled", result);
    }

    [Fact]
    public void BooleanPresenceAttribute_Render_False_RendersEmpty()
    {
        // Arrange
        var attr = Disabled_ << false;

        // Act
        var result = attr.Render();

        // Assert
        Assert.Equal("", result);
    }

    [Fact]
    public void BooleanAsIsAttribute_Render_True_RendersTrue()
    {
        // Arrange
        var attr = Draggable << true;

        // Act
        var result = attr.Render();

        // Assert
        Assert.Equal("draggable=\"true\"", result);
    }

    [Fact]
    public void BooleanAsIsAttribute_Render_False_RendersFalse()
    {
        // Arrange
        var attr = Draggable << false;

        // Act
        var result = attr.Render();

        // Assert
        Assert.Equal("draggable=\"false\"", result);
    }

    [Fact]
    public void IntegerAttribute_Render_RendersNumber()
    {
        // Arrange
        var attr = TabIndex << 5;

        // Act
        var result = attr.Render();

        // Assert
        Assert.Equal("tabindex=\"5\"", result);
    }

    [Fact]
    public void DataAttribute_CreatesCustomDataAttribute()
    {
        // Arrange
        var dataAttr = DataAttr("user-id") << "12345";

        // Act
        var result = dataAttr.Render();

        // Assert
        Assert.Equal("data-user-id=\"12345\"", result);
    }

    [Fact]
    public void BindOperator_CreatesAttribute()
    {
        // Arrange
        var attr = Href.Bind("/page.html");

        // Act
        var result = attr.Render();

        // Assert
        Assert.Equal("href=\"/page.html\"", result);
    }
}
