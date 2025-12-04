using CsharpTags.Core.Types;
using static CsharpTags.Core.Types.Prelude;
using CsharpTags.Core.Interface;

namespace CsharpTags.Core.Test
{
    public class ConditionalRenderingTests
    {
        private HtmlElement Counter(int value)
            => Input.Attr(Tpe << InputType.Hidden, Value << value.ToString(), Name << value.ToString());

        [Fact]
        public void Tag_ConditionalRender_FirstConditionTrue()
        {
            var ele = IfH(true, () => Div.Child(Counter(1)))
                .ElseIf(() => true, () => Counter(2))
                .Else(() => Counter(3));

            Assert.Equal(Div.Child(Counter(1)), ele);
        }

        [Fact]
        public void Tag_ConditionalRender_SecondConditionTrue()
        {
            var ele = IfH(false, () => Div.Child(Counter(1)))
                .ElseIf(() => true, () => Counter(2))
                .Else(() => Counter(3));

            Assert.Equal(ele, Counter(2));
        }

        [Fact]
        public void Tag_ConditionalRender_AllConditionsFalse()
        {
            var ele = IfH(false, () => Div.Child(Counter(1)))
                .ElseIf(() => false, () => Counter(2))
                .Else(() => Counter(3));

            Assert.Equal(Counter(3), ele);
        }

        [Fact]
        public void Tag_ConditionalRender_SingleIfWithoutElse()
        {
            var ele = IfH(true, () => Counter(1)).Element;

            Assert.Equal(Counter(1), ele);
        }

        [Fact]
        public void Tag_ConditionalRender_SingleIfFalseWithoutElse()
        {
            var ele = IfH(false, () => Counter(1)).Element;

            Assert.Equal(None_, ele);
        }

        [Fact]
        public void Tag_ConditionalRender_MultipleElseIfConditions()
        {
            var ele = IfH(false, () => Counter(1))
                .ElseIf(() => false, () => Counter(2))
                .ElseIf(() => true, () => Counter(3))
                .ElseIf(() => true, () => Counter(4)) // This should not be evaluated
                .Else(() => Counter(5));

            Assert.Equal(Counter(3), ele);
        }

        [Fact]
        public void Tag_ConditionalRender_ComplexElements()
        {
            var complexElement = Div.Child(
                Span.Child("Hello"),
                Counter(1),
                P.Child("World")
            );

            var ele = IfH(true, () => complexElement)
                .Else(() => Counter(99));

            Assert.Equal(complexElement, ele);
        }

        [Fact]
        public void Tag_ConditionalRender_NestedConditionals()
        {
            var ele = IfH(true,
                    () => IfH(false, () => Counter(1))
                    .Else(() => Counter(2))
                )
                .Else(() => Counter(3));

            Assert.Equal(Counter(2), ele);
        }

        [Fact]
        public void Tag_ConditionalRender_EmptyElse()
        {
            var ele = IfH(false, () => Counter(1))
                .ElseIf(() => false, () => Counter(2)).Element;

            // Should handle empty else case gracefully
            Assert.Equal(None_, ele);
        }

        [Fact]
        public void Tag_ConditionalRender_DynamicConditions()
        {
            var condition1 = 1 + 1 == 2; // true
            var condition2 = () => 2 + 2 == 5; // false

            var ele = IfH(condition1, () => Counter(1))
                .ElseIf(condition2, () => Counter(2))
                .Else(() => Counter(3));

            Assert.Equal(Counter(1), ele);
        }

        [Fact]
        public void Tag_ConditionalRender_WithAttributes()
        {
            var ele = IfH(true,
                    () => Input.Attr(Tpe << InputType.Text, Value << "test", Name << "field1")
                )
                .Else(() => Input.Attr(Tpe << InputType.Hidden, Value << "fallback", Name << "field2"));

            var expected = Input.Attr(Tpe << InputType.Text, Value << "test", Name << "field1");
            Assert.Equal(expected, ele);
        }
    }
}
