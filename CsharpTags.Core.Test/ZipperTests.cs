using CsharpTags.Core.Interface;
using CsharpTags.Core.Types;
using static CsharpTags.Core.Types.Prelude;
using static LanguageExt.Prelude;

namespace CsharpTags.Core.Tests;

public class ZipperTests
{
    [Fact]
    public void Zipper_Traverse_SimpleTree_VisitsAllNodes()
    {
        var div = Div.New(
            H1.New("Title"),
            P.New("Paragraph")
        );

        var tagNames = new List<string>();
        var visited = new HashSet<HtmlElement>();
        var zipper = new Zipper<HtmlZipperOps, Tag, HtmlElement>(div);

        while (!zipper.IsEnd)
        {
            if (zipper.Focus is Tag tag)
            {
                if (!visited.Contains(zipper.Focus))
                {
                    visited.Add(zipper.Focus);
                    tagNames.Add(tag.TagName);
                }
                else
                {
                    break;
                }
            }
            zipper = zipper.GoNext();
        }

        Assert.Equal(3, tagNames.Count);
        Assert.Equal(new List<string> { "div", "h1", "p" }, tagNames);
    }
}
