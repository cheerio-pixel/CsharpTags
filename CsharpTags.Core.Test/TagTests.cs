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

    [Fact]
    public void Tag_Children_ShouldMaintainExactOrder()
    {
        // Arrange
        var div = Div.New(
            Div.New(Id_ << "first"),
            Div.New(Id_ << "second"),
            Div.New(Id_ << "third"),
            Div.New(Id_ << "fourth"),
            Div.New(Id_ << "fifth")
        );

        // Act
        var result = div.Render();

        // Assert - verify exact order using substring positions
        var firstPos = result.IndexOf("id=\"first\"");
        var secondPos = result.IndexOf("id=\"second\"");
        var thirdPos = result.IndexOf("id=\"third\"");
        var fourthPos = result.IndexOf("id=\"fourth\"");
        var fifthPos = result.IndexOf("id=\"fifth\"");

        Assert.True(firstPos < secondPos, "First should come before second");
        Assert.True(secondPos < thirdPos, "Second should come before third");
        Assert.True(thirdPos < fourthPos, "Third should come before fourth");
        Assert.True(fourthPos < fifthPos, "Fourth should come before fifth");
    }

    [Fact]
    public void Tag_NestedDeeply_ShouldMaintainCorrectHierarchy()
    {
        // Arrange - 5 levels deep
        var html = Html.New(
            Body.New(
                Div.New(Class << "level-1",
                    Div.New(Class << "level-2",
                        Div.New(Class << "level-3",
                            Div.New(Class << "level-4",
                                Div.New(Class << "level-5",
                                    Span.New("Deep content")
                                )
                            )
                        )
                    )
                )
            )
        );

        // Act
        var result = html.Render();

        // Assert - verify nesting order
        Assert.True(result.IndexOf("class=\"level-1\"") < result.IndexOf("class=\"level-2\""));
        Assert.True(result.IndexOf("class=\"level-2\"") < result.IndexOf("class=\"level-3\""));
        Assert.True(result.IndexOf("class=\"level-3\"") < result.IndexOf("class=\"level-4\""));
        Assert.True(result.IndexOf("class=\"level-4\"") < result.IndexOf("class=\"level-5\""));
        Assert.Contains("Deep content", result);
    }

    [Fact]
    public void Tag_MultipleSiblingGroups_ShouldRenderInOrder()
    {
        // Arrange - multiple sibling groups at different nesting levels
        var div = Div.New(
            Section.New(
                H1.New("Section Header"),
                P.New("First paragraph"),
                P.New("Second paragraph")
            ),
            Section.New(
                H2.New("Another Section"),
                Ul.New(
                    Li.New("Item 1"),
                    Li.New("Item 2"),
                    Li.New("Item 3")
                )
            ),
            Section.New(
                P.New("Final section")
            )
        );

        // Act
        var result = div.Render();

        // Assert - verify sections are in order
        var section1Pos = result.IndexOf("<section>");
        var section2Pos = result.IndexOf("<section>", section1Pos + 1);
        var section3Pos = result.IndexOf("<section>", section2Pos + 1);

        Assert.True(section1Pos >= 0, "First section should exist");
        Assert.True(section2Pos > section1Pos, "Second section should come after first");
        Assert.True(section3Pos > section2Pos, "Third section should come after second");

        // Verify content within sections
        Assert.Contains("Section Header", result);
        Assert.Contains("First paragraph", result);
        Assert.Contains("Another Section", result);
        Assert.Contains("Item 1", result);
        Assert.Contains("Item 2", result);
        Assert.Contains("Item 3", result);
        Assert.Contains("Final section", result);
    }

    [Fact]
    public void Tag_MixedTextAndTags_ShouldRenderInCorrectOrder()
    {
        // Arrange - interleaved text and tags
        var div = Div.New(
            P.New("Start text"),
            "Middle text",
            Span.New("In span"),
            "End text"
        );

        // Act
        var result = div.Render();

        // Assert - verify exact order
        var pPos = result.IndexOf("<p>");
        var spanPos = result.IndexOf("<span>");
        
        Assert.True(pPos < spanPos, "P should come before span");
        Assert.Contains("Start text", result);
        Assert.Contains("Middle text", result);
        Assert.Contains("In span", result);
        Assert.Contains("End text", result);
    }

    [Fact]
    public void Tag_WideTree_ShouldRenderAllSiblings()
    {
        // Arrange - tree with many siblings at same level
        var items = Enumerable.Range(1, 20).Select(i => Li.New($"Item {i}"));
        var ul = Ul.New(items.ToHtml());

        // Act
        var result = ul.Render();

        // Assert - all items present in order
        for (int i = 1; i <= 20; i++)
        {
            Assert.Contains($"Item {i}", result);
        }
        
        // Verify order
        for (int i = 1; i < 20; i++)
        {
            var currentPos = result.IndexOf($"Item {i}");
            var nextPos = result.IndexOf($"Item {i + 1}");
            Assert.True(currentPos < nextPos, $"Item {i} should come before Item {i + 1}");
        }
    }
}
