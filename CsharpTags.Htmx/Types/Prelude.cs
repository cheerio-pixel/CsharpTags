using CsharpTags.Core.Types;
using static CsharpTags.Core.Types.Prelude;

namespace CsharpTags.Htmx.Types
{
    public static class Prelude
    {
        #region HTMX Enums

        /// <summary>
        /// Specifies how the response will be swapped in relative to the target
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-swap/"/> - HTMX hx-swap attribute
        /// </remarks>
        public enum SwapStrategy
        {
            InnerHTML,
            OuterHTML,
            BeforeBegin,
            AfterBegin,
            BeforeEnd,
            AfterEnd,
            Delete,
            None
        }

        /// <summary>
        /// Specifies the swap modifier for hx-swap attribute
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-swap/"/> - HTMX hx-swap attribute modifiers
        /// </remarks>
        public enum SwapModifier
        {
            Transition,
            Swap,
            Settle,
            IgnoreTitle,
            Scroll,
            Show,
            FocusScroll
        }

        /// <summary>
        /// Specifies the trigger modifier for hx-trigger attribute
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-trigger/"/> - HTMX hx-trigger attribute modifiers
        /// </remarks>
        public enum TriggerModifier
        {
            Once,
            Changed,
            Delay,
            Throttle,
            From,
            Target,
            Consume,
            Queue
        }

        /// <summary>
        /// Specifies the sync strategy for hx-sync attribute
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-sync/"/> - HTMX hx-sync attribute
        /// </remarks>
        public enum SyncStrategy
        {
            Drop,
            Abort,
            Replace,
            Queue,
            Last
        }

        /// <summary>
        /// Specifies the target selection strategy
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-target/"/> - HTMX hx-target attribute
        /// </remarks>
        public enum TargetStrategy
        {
            This,
            Closest,
            Find,
            Next,
            Previous
        }

        /// <summary>
        /// Specifies the indicator selection strategy
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-indicator/"/> - HTMX hx-indicator attribute
        /// </remarks>
        public enum IndicatorStrategy
        {
            This,
            Closest,
            Find,
            Next,
            Previous
        }

        #endregion

        #region Encoding Functions for Enums

        /// <summary>
        /// Encodes SwapStrategy enum with custom string values
        /// </summary>
        /// <param name="attr">The swap strategy attribute to encode</param>
        /// <returns>hx-swap="value" with proper HTMX swap values</returns>
        public static string SwapStrategyEncoder(HtmlAttribute<SwapStrategy> attr)
        {
            var value = attr.Value switch
            {
                SwapStrategy.InnerHTML => "innerHTML",
                SwapStrategy.OuterHTML => "outerHTML",
                _ => attr.Value.ToString().ToLowerInvariant()
            };
            return $"{attr.Key.Name}=\"{value}\"";
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
        /// Specifies the target element using a strategy
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-target/"/> - HTMX hx-target attribute
        /// </remarks>
        public readonly static HtmlKey<TargetStrategy> HxTargetStrategy = new()
        {
            Name = "hx-target",
            Encode = EnumAsKebabCaseEncoder
        };

        /// <summary>
        /// Specifies how the response will be swapped in relative to the target
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-swap/"/> - HTMX hx-swap attribute
        /// </remarks>
        public readonly static HtmlKey<string> HxSwap = new()
        {
            Name = "hx-swap",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Specifies how the response will be swapped in using a SwapStrategy enum
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-swap/"/> - HTMX hx-swap attribute
        /// </remarks>
        public readonly static HtmlKey<SwapStrategy> HxSwapStrategy = new()
        {
            Name = "hx-swap",
            Encode = SwapStrategyEncoder
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
        /// Controls how requests are queued when they are issued
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-sync/"/> - HTMX hx-sync attribute
        /// </remarks>
        public readonly static HtmlKey<string> HxSync = new()
        {
            Name = "hx-sync",
            Encode = StringAsIsEncoder
        };

        /// <summary>
        /// Controls how requests are queued using a SyncStrategy enum
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-sync/"/> - HTMX hx-sync attribute
        /// </remarks>
        public readonly static HtmlKey<SyncStrategy> HxSyncStrategy = new()
        {
            Name = "hx-sync",
            Encode = EnumAsLowerCaseEncoder
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
        /// The element to put the htmx-request class on using a strategy
        /// </summary>
        /// <remarks>
        /// Reference: <see href="https://htmx.org/attributes/hx-indicator/"/> - HTMX hx-indicator attribute
        /// </remarks>
        public readonly static HtmlKey<IndicatorStrategy> HxIndicatorStrategy = new()
        {
            Name = "hx-indicator",
            Encode = EnumAsKebabCaseEncoder
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
