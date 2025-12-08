using CsharpTags.Core.Interface;
using LanguageExt;

namespace CsharpTags.AspNetCore.Transformers
{
    /// <summary>
    /// Alias for Transfomation function
    /// </summary>
    public record HtmlTransformer(Func<HtmlElement, Option<HtmlElement>> F);
}
