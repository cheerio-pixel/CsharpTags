using CsharpTags.AspNetCore.Transformers;
using CsharpTags.Core.Interface;
using CsharpTags.Core.Types;
using LanguageExt;
using Microsoft.Extensions.DependencyInjection;
using static LanguageExt.Prelude;
using static CsharpTags.Core.Types.Prelude;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using CsharpTags.AspNetCore.Filters;

namespace CsharpTags.AspNetCore
{
    /// <summary>
    /// Extensions for asp net core
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Register a transformation to the output html proccessed by <see cref="HtmlElementEndpointFilter"/>
        /// </summary>
        /// <param name="services">Service collection that we are extending</param>
        /// <param name="transformer">Transformer function</param>
        /// <returns></returns>
        public static IServiceCollection AddHtmlTranformation(this IServiceCollection services, Func<IServiceProvider, Func<HtmlElement, Option<HtmlElement>>> transformer)
            => services.AddTransient(ser => new HtmlTransformer(transformer(ser)));

        /// <summary>
        /// Add Html transformation that auto inserts the antiforgery
        /// token if is not already there.
        /// </summary>
        /// <remarks>
        /// Should also call services.AddHttpContextAccessor() for
        /// this transformer to take affect.
        /// </remarks>
        /// <param name="services">Service collection that we are extending</param>
        /// <returns>The same service collection for chaining</returns>
        public static IServiceCollection AddHtmlTransformationAntiForgeryToken(this IServiceCollection services)
        => services.AddHtmlTranformation(services => element =>
        {
            if (element is Tag form && form.TagName == Form.TagName && Enumerable.Contains(form.Attributes, Method << "post"))

            {
                var httpContextAccessor = services.GetService<IHttpContextAccessor>();
                var httpContext = httpContextAccessor?.HttpContext;
                if (httpContext is null)
                {
                    return None;
                }
                var antiforgery = services.GetRequiredService<IAntiforgery>();
                var token = antiforgery.GetAndStoreTokens(httpContext);
                var tokenName = Name << token.FormFieldName;
                var zipper = new Zipper<HtmlZipperOps, Tag, HtmlElement>(form);
                if (!zipper.Any(x => x is Tag tag
                            && tag.TagName == Input.TagName
                            && Enumerable.Contains(tag.Attributes, tokenName)
                            ))
                {
                    return form.AppendChild(Input.Attr(
                                tokenName,
                                Value << token.RequestToken!,
                                Tpe << InputType.Hidden
                                ));

                }

            }
            return None;
        });
    }
}
