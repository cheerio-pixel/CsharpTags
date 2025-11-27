using CsharpTags.Core.Types;
using static CsharpTags.Core.Types.Prelude;

namespace CsharpTags.Htmx.Types
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class Prelude
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        #region HTMX Enums

        /// <summary>
        /// Specifies how htmx will synchronize DOM updates between different elements
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-sync/"/> - HTMX hx-sync attribute
        /// The hx-sync attribute allows you to coordinate AJAX requests between different elements,
        /// ensuring that only one request happens at a time or that requests are queued/aborted appropriately.
        /// </remarks>
        public class SyncStrategy
        {
            internal enum SyncStrategy_
            {
                Drop,
                Abort,
                Replace,
                Queue,
                QueueFirst,
                QueueLast,
                QueueAll
            }

            private string Selector { get; set; } = string.Empty;
            private SyncStrategy_ Strategy { get; set; }

            /// <summary>
            /// Represents an intermediate sync strategy that requires a CSS selector to complete.
            /// Use the Modify method or + operator to provide a selector and create a SyncStrategy.
            /// </summary>
            public struct SyncStrategyBuilder
            {
                private SyncStrategy_ Strategy { get; set; }

                internal SyncStrategyBuilder(SyncStrategy_ strategy)
                {
                    Strategy = strategy;
                }

                /// <summary>
                /// Apply a CSS selector to complete the sync strategy.
                /// </summary>
                /// <param name="selector">CSS selector specifying which element to synchronize with</param>
                /// <returns>A complete SyncStrategy</returns>
                public SyncStrategy Modify(string selector) => this + selector;

                /// <summary>
                /// Apply a CSS selector to complete the sync strategy using the + operator.
                /// </summary>
                public static SyncStrategy operator +(SyncStrategyBuilder builder, string selector)
                    => new()
                    {
                        Selector = selector,
                        Strategy = builder.Strategy
                    };
            }

            /// <summary>
            /// Encodes SyncStrategy with CSS selector and sync strategy values
            /// </summary>
            /// <param name="attr">The sync strategy attribute to encode</param>
            /// <returns>hx-sync="selector:strategy" with proper HTMX sync values</returns>
            public static string SyncStrategyEncoder(HtmlAttribute<SyncStrategy> attr)
            {
                var strat = attr.Value.Strategy switch
                {
                    SyncStrategy_.QueueAll => "queue all",
                    SyncStrategy_.QueueFirst => "queue first",
                    SyncStrategy_.QueueLast => "queue last",
                    _ => attr.Value.Strategy.ToString().ToLowerInvariant()
                };
                var value = $"{attr.Value.Selector}:{strat}";
                return $"{attr.Key.Name}=\"{value}\"";
            }

            /// <summary>
            /// Drop the new request if a request is already in progress on the target element.
            /// The new request is simply ignored and discarded.
            /// </summary>
            public static SyncStrategyBuilder Drop { get; } = new(SyncStrategy_.Drop);

            /// <summary>
            /// Abort the current request in progress and replace it with the new request.
            /// The ongoing request is cancelled and the new request takes its place.
            /// </summary>
            public static SyncStrategyBuilder Abort { get; } = new(SyncStrategy_.Abort);

            /// <summary>
            /// Replace the current request in the queue with the new request.
            /// If a request is queued, it will be replaced by this new request.
            /// </summary>
            public static SyncStrategyBuilder Replace { get; } = new(SyncStrategy_.Replace);

            /// <summary>
            /// Queue the new request to run after the current request completes.
            /// Requests are executed in the order they are received.
            /// </summary>
            public static SyncStrategyBuilder Queue { get; } = new(SyncStrategy_.Queue);

            /// <summary>
            /// Queue the new request to run before any other queued requests.
            /// The new request will be executed immediately after the current request completes, before any other queued requests.
            /// </summary>
            /// <remarks>
            /// This strategy adds the new request to the front of the queue, giving it priority over other waiting requests.
            /// </remarks>
            public static SyncStrategyBuilder QueueFirst { get; } = new(SyncStrategy_.QueueFirst);

            /// <summary>
            /// Queue the new request to run after all other queued requests.
            /// The new request will be executed after all currently queued requests have completed.
            /// </summary>
            /// <remarks>
            /// This strategy adds the new request to the end of the queue, following a first-in-first-out pattern.
            /// </remarks>
            public static SyncStrategyBuilder QueueLast { get; } = new(SyncStrategy_.QueueAll);

            /// <summary>
            /// Queue all requests and execute them in sequence.
            /// Every request will be queued and executed one after another, regardless of how many are made.
            /// </summary>
            /// <remarks>
            /// This strategy ensures all requests are processed in the order they were made, without dropping any requests.
            /// Unlike other queue strategies, this will not limit the number of queued requests.
            /// </remarks>
            public static SyncStrategyBuilder QueueAll { get; } = new(SyncStrategy_.QueueAll);
        }

        /// <summary>
        /// Specifies how the response will be swapped in relative to the target
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-swap/"/> - HTMX hx-swap attribute
        /// Visual reference for positioning:
        /// <code>
        /// &lt;!-- BeforeBegin --&gt;
        /// [New Content]
        /// &lt;div target&gt;           &lt;!-- Target Element --&gt;
        ///     &lt;!-- AfterBegin --&gt;
        ///     [New Content]
        ///     Existing content...
        ///     &lt;!-- BeforeEnd --&gt;
        ///     [New Content]
        /// &lt;/div&gt;
        /// &lt;!-- AfterEnd --&gt;
        /// [New Content]
        /// </code>
        /// </remarks>
        public record SwapStrategy
        {
            private enum SwapStrategy_
            {
                InnerHTML,
                OuterHTML,
                TextContent,
                BeforeBegin,
                AfterBegin,
                BeforeEnd,
                AfterEnd,
                Delete,
                None
            }
            private string Mod { get; set; } = string.Empty;
            private SwapStrategy_ Strategy { get; set; }

            /// <summary>
            /// Apply a modifier to this strategy.
            /// </summary>
            public SwapStrategy Modify(string modifier) => this + modifier;

            /// <summary>
            /// Apply a modifier to this strategy.
            /// </summary>
            public static SwapStrategy operator +(SwapStrategy lhs, string rhs)
                => lhs with
                {
                    Mod = rhs
                };

            /// <summary>
            /// Encodes SwapStrategy enum with custom string values
            /// </summary>
            /// <param name="attr">The swap strategy attribute to encode</param>
            /// <returns>hx-swap="value" with proper HTMX swap values</returns>
            public static string SwapStrategyEncoder(HtmlAttribute<SwapStrategy> attr)
            {
                var value = attr.Value.Strategy switch
                {
                    SwapStrategy_.InnerHTML => "innerHTML",
                    SwapStrategy_.OuterHTML => "outerHTML",
                    SwapStrategy_.TextContent => "textContent",
                    _ => attr.Value.Strategy.ToString().ToLowerInvariant()
                };
                return $"{attr.Key.Name}=\"{value}" + (
                        attr.Value.Mod == string.Empty ? string.Empty : " " + attr.Value.Mod
                        ) + "\"";
            }

            /// <summary>
            /// Replace the inner HTML of the target element with the response content.
            /// The target element itself remains unchanged, only its children are replaced.
            /// </summary>
            public readonly static SwapStrategy InnerHTML = new()
            {
                Strategy = SwapStrategy_.InnerHTML
            };

            /// <summary>
            /// Replace the entire target element with the response content.
            /// The target element is completely removed and replaced by the new content.
            /// </summary>
            public readonly static SwapStrategy OuterHTML = new SwapStrategy
            {
                Strategy = SwapStrategy_.OuterHTML
            };

            /// <summary>
            /// Replace the text content of the target element without parsing the response as HTML.
            /// The response content is treated as plain text and any HTML tags will be escaped.
            /// </summary>
            public readonly static SwapStrategy TextContent = new SwapStrategy
            {
                Strategy = SwapStrategy_.TextContent
            };

            /// <summary>
            /// Insert the response content before the target element.
            /// The new content becomes a sibling that appears immediately before the target element.
            /// </summary>
            public readonly static SwapStrategy BeforeBegin  = new SwapStrategy
            {
                Strategy = SwapStrategy_.BeforeBegin
            };

            /// <summary>
            /// Insert the response content before the first child of the target element.
            /// The new content becomes the first child inside the target element.
            /// </summary>
            public readonly static SwapStrategy AfterBegin = new SwapStrategy
            {
                Strategy = SwapStrategy_.AfterBegin
            };

            /// <summary>
            /// Insert the response content after the last child of the target element.
            /// The new content becomes the last child inside the target element.
            /// </summary>
            public readonly static SwapStrategy BeforeEnd = new SwapStrategy
            {
                Strategy = SwapStrategy_.BeforeEnd
            };

            /// <summary>
            /// Insert the response content after the target element
            /// </summary>
            public readonly static SwapStrategy AfterEnd = new SwapStrategy
            {
                Strategy = SwapStrategy_.AfterEnd
            };

            /// <summary>
            /// Delete the target element regardless of the response content.
            /// The target element is removed from the DOM, and the response content is ignored.
            /// </summary>
            public readonly static SwapStrategy Delete = new SwapStrategy
            {
                Strategy = SwapStrategy_.Delete
            };

            /// <summary>
            /// Does not append content from the response to the target element.
            /// The target element remains unchanged, but out-of-band items in the response will still be processed.
            /// </summary>
            public readonly static SwapStrategy None = new SwapStrategy
            {
                Strategy = SwapStrategy_.None
            };
        }

        #endregion

        #region HTMX Core Attributes

        /// <summary>
        /// Issues a GET request to the specified URL
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-get/"/> - HTMX hx-get attribute
        /// </remarks>
        public readonly static HtmlKey<string> HxGet = new()
        {
            Name = "hx-get",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Issues a POST request to the specified URL
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-post/"/> - HTMX hx-post attribute
        /// </remarks>
        public readonly static HtmlKey<string> HxPost = new()
        {
            Name = "hx-post",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Issues a PUT request to the specified URL
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-put/"/> - HTMX hx-put attribute
        /// </remarks>
        public readonly static HtmlKey<string> HxPut = new()
        {
            Name = "hx-put",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Issues a PATCH request to the specified URL
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-patch/"/> - HTMX hx-patch attribute
        /// </remarks>
        public readonly static HtmlKey<string> HxPatch = new()
        {
            Name = "hx-patch",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Issues a DELETE request to the specified URL
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-delete/"/> - HTMX hx-delete attribute
        /// </remarks>
        public readonly static HtmlKey<string> HxDelete = new()
        {
            Name = "hx-delete",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies the event that triggers the request
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-trigger/"/> - HTMX hx-trigger attribute
        /// </remarks>
        public readonly static HtmlKey<string> HxTrigger = new()
        {
            Name = "hx-trigger",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies the target element to swap the response into
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-target/"/> - HTMX hx-target attribute
        /// </remarks>
        public readonly static HtmlKey<string> HxTarget = new()
        {
            Name = "hx-target",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies the target element to swap the response into.
        /// Indicates that this element is the target.
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-target/"/> - HTMX hx-target attribute
        /// </remarks>
        public readonly static HtmlAttribute<string> HxTargetThis = HxTarget << "this";

        /// <summary>
        /// Specifies the target element to swap the response into.
        /// Will find the closest ancestor element or itself,
        /// that matches the given CSS selector (e.g. closest
        /// tr will target the closest table row to the element).
        /// Indicates that this element is the target.
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-target/"/> - HTMX hx-target attribute
        /// </remarks>
        public static HtmlAttribute<string> HxTargetClosest(string cssSelector) => HxTarget << "closest " + cssSelector;

        /// <summary>
        /// Specifies the target element to swap the response into.
        /// Will find the first child descendant element that
        /// matches the given CSS selector
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-target/"/> - HTMX hx-target attribute
        /// </remarks>
        public static HtmlAttribute<string> HxTargetFind(string cssSelector) => HxTarget << "find " + cssSelector;

        /// <summary>
        /// Specifies the target element to swap the response into.
        /// Resolves to element.nextElementSibling
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-target/"/> - HTMX hx-target attribute
        /// </remarks>
        public readonly static HtmlAttribute<string> HxTargetNext_ = HxTarget << "next";

        /// <summary>
        /// Specifies the target element to swap the response into.
        /// Will scan the DOM forward for the first element that
        /// matches the given CSS selector. (e.g. next .error will
        /// target the closest following sibling element with error class)
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-target/"/> - HTMX hx-target attribute
        /// </remarks>
        public static HtmlAttribute<string> HxTargetNext(string cssSelector) => HxTarget << "next " + cssSelector;

        /// <summary>
        /// Specifies the target element to swap the response into.
        /// Resolves to element.previousElementSibling
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-target/"/> - HTMX hx-target attribute
        /// </remarks>
        public readonly static HtmlAttribute<string> HxTargetPrevious_ = HxTarget << "previous";

        /// <summary>
        /// Specifies the target element to swap the response into.
        /// Will scan the DOM backwards for the first element that
        /// matches the given CSS selector. (e.g. previous .error
        /// will target the closest previous sibling with error class)
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-target/"/> - HTMX hx-target attribute
        /// </remarks>
        public static HtmlAttribute<string> HxTargetPrevious(string cssSelector) => HxTarget << "previous " + cssSelector;

        /// <summary>
        /// Specifies how the response will be swapped in relative to the target
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-swap/"/> - HTMX hx-swap attribute
        /// </remarks>
        public readonly static HtmlKey<SwapStrategy> HxSwap = new()
        {
            Name = "hx-swap",
            Encode = SwapStrategy.SwapStrategyEncoder
        };

        /// <summary>
        /// Specifies the time that will elapse between the trigger event and the request being issued
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-delay/"/> - HTMX hx-delay attribute
        /// </remarks>
        public readonly static HtmlKey<int> HxDelay = new()
        {
            Name = "hx-delay",
            Encode = IntAsStringEncoder
        };

        /// <summary>
        /// Specifies the time to wait between the last event and the request being issued
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-settle/"/> - HTMX hx-settle attribute
        /// </remarks>
        public readonly static HtmlKey<int> HxSettle = new()
        {
            Name = "hx-settle",
            Encode = IntAsStringEncoder
        };

        #endregion

        #region HTMX Advanced Attributes

        /// <summary>
        /// Adds values to the parameters to submit with the request
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-vals/"/> - HTMX hx-vals attribute
        /// </remarks>
        public readonly static HtmlKey<string> HxVals = new()
        {
            Name = "hx-vals",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// CSS selector that allows you to choose which part of the response is used to be swapped in
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-select/"/> - HTMX hx-select attribute
        /// </remarks>
        public readonly static HtmlKey<string> HxSelect = new()
        {
            Name = "hx-select",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// CSS selector that allows you to select the content you want to swap from a response
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-select-oob/"/> - HTMX hx-select-oob attribute
        /// </remarks>
        public readonly static HtmlKey<string> HxSelectOob = new()
        {
            Name = "hx-select-oob",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Shows a confirm() dialog before issuing a request
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-confirm/"/> - HTMX hx-confirm attribute
        /// </remarks>
        public readonly static HtmlKey<string> HxConfirm = new()
        {
            Name = "hx-confirm",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// The hx-sync attribute consists of a CSS
        /// selector to indicate the element to
        /// synchronize on, followed optionally by
        /// a colon and then by an optional syncing
        /// strategy.
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-sync/"/> - HTMX hx-sync attribute
        /// </remarks>
        public readonly static HtmlKey<SyncStrategy> HxSync = new()
        {
            Name = "hx-sync",
            Encode = SyncStrategy.SyncStrategyEncoder
        };

        /// <summary>
        /// Disables htmx processing for the given node and any children nodes
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-disabled/"/> - HTMX hx-disabled attribute
        /// </remarks>
        public readonly static HtmlKey<bool> HxDisabled = new()
        {
            Name = "hx-disabled",
            Encode = BooleanPresenceEncoder
        };

        /// <summary>
        /// Adds or removes event listeners from a element
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-on/"/> - HTMX hx-on attribute
        /// </remarks>
        public static HtmlKey<string> HxOn(string @event) => new()
        {
            Name = "hx-on:" + @event,
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Prevents sensitive data being saved to the history cache
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-history-elt/"/> - HTMX hx-history-elt attribute
        /// </remarks>
        public readonly static HtmlKey<bool> HxHistoryElt = new()
        {
            Name = "hx-history-elt",
            Encode = BooleanPresenceEncoder
        };

        /// <summary>
        /// The element to put the htmx-request class on during the request
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-indicator/"/> - HTMX hx-indicator attribute
        /// </remarks>
        public readonly static HtmlKey<string> HxIndicator = new()
        {
            Name = "hx-indicator",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Include additional data in requests
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-include/"/> - HTMX hx-include attribute
        /// </remarks>
        public readonly static HtmlKey<string> HxInclude = new()
        {
            Name = "hx-include",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Handle any event with a inline script
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-on/"/> - HTMX hx-on attribute
        /// </remarks>
        public readonly static HtmlKey<string> HxOnClick = new()
        {
            Name = "hx-on:click",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Handle any event with a inline script
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-on/"/> - HTMX hx-on attribute
        /// </remarks>
        public readonly static HtmlKey<string> HxOnLoad = new()
        {
            Name = "hx-on:load",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Handle any event with a inline script
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-on/"/> - HTMX hx-on attribute
        /// </remarks>
        public readonly static HtmlKey<string> HxOnMouseOver = new()
        {
            Name = "hx-on:mouseover",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Handle any event with a inline script
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-on/"/> - HTMX hx-on attribute
        /// </remarks>
        public readonly static HtmlKey<string> HxOnKeyUp = new()
        {
            Name = "hx-on:keyup",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Handle any event with a inline script
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-on/"/> - HTMX hx-on attribute
        /// </remarks>
        public readonly static HtmlKey<string> HxOnChange = new()
        {
            Name = "hx-on:change",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Handle any event with a inline script
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-on/"/> - HTMX hx-on attribute
        /// </remarks>
        public readonly static HtmlKey<string> HxOnSubmit = new()
        {
            Name = "hx-on:submit",
            Encode = StringAsIsEncoder
        };

        #endregion

        #region HTMX Boost Attributes

        /// <summary>
        /// "Boosts" anchor tags and forms to use AJAX requests instead of full page reloads
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-boost/"/> - HTMX hx-boost attribute
        /// </remarks>
        public readonly static HtmlKey<bool> HxBoost = new()
        {
            Name = "hx-boost",
            Encode = BooleanPresenceEncoder
        };

        /// <summary>
        /// Pushes the URL into the browser location bar and adds an entry to the browser's history
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-push-url/"/> - HTMX hx-push-url attribute
        /// </remarks>
        public readonly static HtmlKey<string> HxPushUrl = new()
        {
            Name = "hx-push-url",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Selects content to swap in from a response, out of band (also known as "OOB")
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-swap-oob/"/> - HTMX hx-swap-oob attribute
        /// </remarks>
        public readonly static HtmlKey<string> HxSwapOob = new()
        {
            Name = "hx-swap-oob",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Shows a confirm() dialog before issuing a request
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-prompt/"/> - HTMX hx-prompt attribute
        /// </remarks>
        public readonly static HtmlKey<string> HxPrompt = new()
        {
            Name = "hx-prompt",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Adds or removes progressively enhanced content
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-disinherit/"/> - HTMX hx-disinherit attribute
        /// </remarks>
        public readonly static HtmlKey<string> HxDisinherit = new()
        {
            Name = "hx-disinherit",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Extensions that are used for this element
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-ext/"/> - HTMX hx-ext attribute
        /// </remarks>
        public readonly static HtmlKey<string> HxExt = new()
        {
            Name = "hx-ext",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Encoding type to use with the request
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-encoding/"/> - HTMX hx-encoding attribute
        /// </remarks>
        public readonly static HtmlKey<string> HxEncoding = new()
        {
            Name = "hx-encoding",
            Encode = StringAsIsEncoder
        };

        #endregion

        #region HTMX Validation & Forms

        /// <summary>
        /// Validates an input before issuing a request
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-validate/"/> - HTMX hx-validate attribute
        /// </remarks>
        public readonly static HtmlKey<bool> HxValidate = new()
        {
            Name = "hx-validate",
            Encode = BooleanPresenceEncoder
        };

        /// <summary>
        /// Adds or overrides request parameters by name
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-params/"/> - HTMX hx-params attribute
        /// </remarks>
        public readonly static HtmlKey<string> HxParams = new()
        {
            Name = "hx-params",
            Encode = StringAsIsEncoder
        };

        #endregion

        #region HTMX Headers & Metadata

        /// <summary>
        /// The header to include in the request
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-headers/"/> - HTMX hx-headers attribute
        /// </remarks>
        public readonly static HtmlKey<string> HxHeaders = new()
        {
            Name = "hx-headers",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// The element to use for the target of the request
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-target/"/> - HTMX hx-target attribute
        /// </remarks>
        public readonly static HtmlKey<string> HxTargetError = new()
        {
            Name = "hx-target-error",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// CSS selector that allows you to choose which part of the response is used to be swapped in
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-select/"/> - HTMX hx-select attribute
        /// </remarks>
        public readonly static HtmlKey<string> HxSelectError = new()
        {
            Name = "hx-select-error",
            Encode = StringAsIsEncoder
        };

        #endregion

        #region HTMX Utility Attributes

        /// <summary>
        /// Adds the disabled attribute and hx-disabled class to the element
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-disabled-elt/"/> - HTMX hx-disabled-elt attribute
        /// </remarks>
        public readonly static HtmlKey<bool> HxDisabledElt = new()
        {
            Name = "hx-disabled-elt",
            Encode = BooleanPresenceEncoder
        };

        #endregion
    }
}
