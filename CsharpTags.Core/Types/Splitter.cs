using CsharpTags.Core.Interface;

namespace CsharpTags.Core.Types
{
    internal static class Splitter
    {
        internal static (Seq<HtmlElement>, Seq<HtmlAttribute>) CalculateSplit(Seq<IHtml> content)
        {
            var elements = Seq<HtmlElement>();
            var attributes = Seq<HtmlAttribute>();
            var queue = new Queue<IHtml>(content);

            while (queue.Count > 0)
            {
                var element = queue.Dequeue();

                if (element is IWrapListHtmlElement htmlElementList)
                {
                    foreach (var item in htmlElementList.Unwrap())
                    {
                        queue.Enqueue(item);
                    }
                }
                else if (element is IWrapHtmlElement wrappedHtmlElement)
                {
                    var unwrapped = wrappedHtmlElement.Simplify();
                    while (unwrapped is IWrapHtmlElement nested)
                    {
                        unwrapped = nested.Simplify();
                    }
                    queue.Enqueue(unwrapped);
                }
                else if (element is HtmlElement htmlElement2)
                {
                    elements = elements.Add(htmlElement2);
                }
                else if (element is IWrapListHtmlAttribute listAttr)
                {
                    foreach (var attr in listAttr.Unwrap())
                    {
                        queue.Enqueue(attr);
                    }
                }
                else if (element is HtmlAttribute attr)
                {
                    attributes = attributes.Add(attr);
                }
                else if (element is HtmlList list)
                {
                    foreach (var item in list.Value)
                    {
                        queue.Enqueue(item);
                    }
                }
                else
                {
                    throw new NotImplementedException(element.GetType().FullName);
                }
            }
            return (elements, attributes);
        }
    }
}
