using CsharpTags.Core.Interface;
using LanguageExt;

namespace CsharpTags.AspNetCore.Transformers
{
    internal record HtmlTransformer(Func<HtmlElement, Option<HtmlElement>> F);
}
