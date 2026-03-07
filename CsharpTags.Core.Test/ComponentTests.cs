using CsharpTags.Core.Interface;
using CsharpTags.Core.Types;
using static CsharpTags.Core.Types.Prelude;
using static LanguageExt.Prelude;

namespace CsharpTags.Core.Tests;

public class ComponentTests
{
    // Test component implementations
    private record SimpleComponent : Component
    {
        protected override string TagName => "div";
        protected override IHtml Build() => Span.New("Simple Component Content");
    }

    private record ComponentWithAttributes : Component
    {
        protected override string TagName => "section";
        protected override IHtml Build() => H1.New("Title");
    }

    private record ComponentWithChildren(params IHtml[] ChildrenContent) : Component(ChildrenContent)
    {
        protected override string TagName => "article";
        protected override IHtml Build() => Div.New(ChildrenContent);
    }

    private record ComponentWithMixedContent(string ContainerClass, string ContainerId) : Component
    {
        protected override string TagName => "main";
        protected override IHtml Build() => Div.New(Class << ContainerClass, Id_ << ContainerId, "Content");
    }

    private record NestedComponent : Component
    {
        protected override string TagName => "section";
        protected override IHtml Build() => Span.New("Nested content");
    }

    [Fact]
    public void Component_Simplify_ReturnsCorrectTag()
    {
        // Arrange
        var component = new SimpleComponent();

        // Act
        var result = component.Simplify();

        // Assert
        Assert.Equal("div", ((Tag)result).TagName);
        Assert.False(((Tag)result).IsVoid);
    }

    [Fact]
    public void Component_Build_ReturnsCorrectHtml()
    {
        // Arrange
        var component = new SimpleComponent();
        var buildMethod = component.GetType().GetMethod("Build", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var result = buildMethod?.Invoke(component, null) as HtmlElement;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("<span>Simple Component Content</span>", result.Render());
    }

    [Fact]
    public void Component_TagName_ReturnsCorrectValue()
    {
        // Arrange
        var component = new ComponentWithAttributes();

        // Act & Assert
        var tagNameProperty = component.GetType().GetProperty("TagName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var tagName = tagNameProperty?.GetValue(component) as string;
        Assert.Equal("section", tagName);
    }

    [Fact]
    public void Component_WithNoAttributes_HasEmptyAttributes()
    {
        // Arrange
        var component = new SimpleComponent();

        // Act
        var attributes = component.Attributes;

        // Assert
        Assert.Empty(attributes);
    }

    [Fact]
    public void Component_WithNoChildren_HasEmptyChildren()
    {
        // Arrange
        var component = new SimpleComponent();

        // Act
        var children = component.Children;

        // Assert
        Assert.Empty(children);
    }

    [Fact]
    public void Component_WithConstructorContent_HasCorrectAttributes()
    {
        // Arrange
        var component = new ComponentWithMixedContent("container", "main-content");

        // Act
        var attributes = component.Attributes.ToList();

        // Assert - Component has no constructor content, so no attributes
        Assert.Empty(attributes);
    }

    [Fact]
    public void Component_WithChildren_HasCorrectChildren()
    {
        // Arrange
        var component = new ComponentWithChildren(P.New("Paragraph 1"), P.New("Paragraph 2"));

        // Act
        var children = component.Children.ToList();

        // Assert
        Assert.Equal(2, children.Count);
    }

    [Fact]
    public void Component_Content_CanBeModified()
    {
        // Arrange
        var component = new SimpleComponent();

        // Act
        var modifiedComponent = component with { Content = Seq<IHtml>(Class << "modified") };

        // Assert
        Assert.Single(modifiedComponent.Attributes);
    }

    [Fact]
    public void Component_Simplify_RendersBuildResultWrappedInTagName()
    {
        // Arrange
        var component = new NestedComponent();

        // Act
        var simplified = component.Simplify();
        var rendered = simplified.Render();

        // Assert - Simplify wraps Build() result in a Tag with TagName
        Assert.Equal("<section><span>Nested content</span></section>", rendered);
    }

    [Fact]
    public void Component_Render_ThroughSimplify_WorksCorrectly()
    {
        // Arrange
        var component = new ComponentWithMixedContent("container", "main-content");

        // Act
        var simplified = component.Simplify();
        var rendered = simplified.Render();

        // Assert - Simplify creates outer tag with TagName wrapping Build() result
        Assert.Equal("<main><div class=\"container\" id=\"main-content\">Content</div></main>", rendered);
    }

    [Fact]
    public void Component_Attributes_AreCalculatedCorrectly()
    {
        // Arrange
        var component = new ComponentWithMixedContent("test", "test-id");

        // Act - Access attributes multiple times
        var attrs1 = component.Attributes.ToList();
        var attrs2 = component.Attributes.ToList();

        // Assert - Both should be empty since component has no constructor content
        Assert.Empty(attrs1);
        Assert.Empty(attrs2);
    }

    [Fact]
    public void Component_Children_AreCalculatedCorrectly()
    {
        // Arrange
        var component = new ComponentWithChildren(H1.New("Title"), P.New("Content"));

        // Act - Access children multiple times
        var children1 = component.Children.ToList();
        var children2 = component.Children.ToList();

        // Assert - Both should contain same data
        Assert.Equal(2, children1.Count);
        Assert.Equal(2, children2.Count);
        Assert.Equal(children1.Select(c => c.Render()), children2.Select(c => c.Render()));
    }

    [Fact]
    public void Component_ModifyingContent_InvalidatesCache()
    {
        // Arrange
        var component = new SimpleComponent();
        var originalAttrs = component.Attributes;

        // Act
        var modified = component with { Content = Seq<IHtml>(Class << "new-class") };
        var newAttrs = modified.Attributes;

        // Assert
        Assert.NotEqual(originalAttrs, newAttrs);
        Assert.Single(newAttrs);
        var attrNames = newAttrs.Select(a => a.Render().Split('=')[0]).ToList();
        Assert.Contains("class", attrNames);
    }

    [Fact]
    public void Component_ImplementsIHtml()
    {
        // Arrange & Act
        var component = new SimpleComponent();

        // Assert
        Assert.IsAssignableFrom<IHtml>(component);
    }

    [Fact]
    public void Component_ImplementsIWrapHtmlElement()
    {
        // Arrange & Act
        var component = new SimpleComponent();

        // Assert
        Assert.IsAssignableFrom<IWrapHtmlElement>(component);
    }

    [Fact]
    public void Component_Constructor_AcceptsNoContent()
    {
        // Arrange & Act
        var component = new SimpleComponent();

        // Assert
        Assert.Empty(component.Content);
    }

    [Fact]
    public void Component_Constructor_AcceptsContent()
    {
        // Arrange & Act
        var component = new ComponentWithChildren(H1.New("Title"));

        // Assert
        Assert.Single(component.Content);
    }
}
