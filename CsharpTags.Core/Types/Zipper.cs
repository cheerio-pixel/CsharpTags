
using System.Diagnostics.CodeAnalysis;

namespace CsharpTags.Core.Types
{
    public interface ZipOps<TBranch, TElement>
    {
        /// <summary>
        /// If <paramref name="node"/> is a branch, then return it otherwise fail
        /// </summary>
        public abstract static Option<TBranch> AsBranch(TElement node);
        /// <summary>
        /// Given a <paramref name="branch"/> node, make a new branch node with supplied <paramref name="children"/>
        /// </summary>
        public abstract static TElement MakeNode(TBranch branch, Seq<TElement> children);
        /// <summary>
        /// Return a Seq of children of a branch
        /// </summary>
        public abstract static Seq<TElement> ChildrenOf(TBranch branch);
    }

    /// <summary>
    /// Zipper data structure over a <typeparamref name="TBranch"/> and a <typeparamref name="TElement"/>
    /// </summary>
    public record Zipper<Z, TBranch, TElement>
        where Z : ZipOps<TBranch, TElement>
        where TBranch : TElement
    {
        /// <summary>
        /// The left siblings of the current focus
        /// </summary>
        public required Seq<TElement> Left { get; init; }
        /// <summary>
        /// The current node that is being focused.
        /// </summary>
        public required TElement Focus { get; init; }
        /// <summary>
        /// The right siblings of the current focus.
        /// </summary>
        public required Seq<TElement> Right { get; init; }

        /// <summary>
        /// A sequence of nodes that lead up to this node.
        /// </summary>
        public required Seq<TBranch> ParentNodes { get; init; }
        /// <summary>
        /// The previous zipper leading to this location, will be None
        /// if root
        /// </summary>
        public required Option<Zipper<Z, TBranch, TElement>> Path { get; init; }
        /// <summary>
        /// Flag signifying if primitive operations should update the zipper
        /// </summary>
        public required bool IsChanged { get; init; }

        private Zipper()
        {
        }

        [SetsRequiredMembers]
        public Zipper(TElement node)
        {
            Left = Seq<TElement>();
            Focus = node;
            Right = Seq<TElement>();
            ParentNodes = Seq<TBranch>();
            Path = None;
            IsChanged = false;
        }

        public Option<Seq<TElement>> Children()
            => Z.AsBranch(Focus).Map(Z.ChildrenOf);

        public Option<Zipper<Z, TBranch, TElement>> Down()
        {
            var thiz = this;
            return
            from asBranch in Z.AsBranch(Focus)
            from children in thiz.Children()
            from head in children.Head
            select new Zipper<Z, TBranch, TElement>()
            {
                Left = Seq<TElement>(),
                Focus = head,
                Right = children.Tail,
                ParentNodes = asBranch.Cons(thiz.ParentNodes),
                Path = thiz,
                IsChanged = false
            };
        }

        public Option<Zipper<Z, TBranch, TElement>> Up()
        {
            return ParentNodes.Head.Map(parentNode =>
            {
                if (IsChanged)
                {
                    var newFocus = Z.MakeNode(parentNode, Left.Concat(Focus.Cons(Right)));
                    return
                    Path.Match(
                            x => x with
                            {
                                Focus = newFocus,
                                IsChanged = true
                            },
                    () => new(newFocus)
                    );
                }

                return
                Path.Match(
                 x => x with
                 {
                     Focus = parentNode
                 },
                () => new Zipper<Z, TBranch, TElement>(parentNode)
                        );
            });
        }

        /// <summary>
        /// Zip all the way to the top, applying changes in the way.
        /// </summary>
        public TElement Root()
        {
            var current = this;

            while (true)
            {
                if (current.Path.IsNone)
                {
                    return current.Focus;
                }

                var parent = current.Up();

                if (parent.IsSome)
                {
                    current = (Zipper<Z, TBranch, TElement>)parent;
                    continue;
                }
                else
                {
                    return current.Focus;
                }
            }
        }

        public Option<Zipper<Z, TBranch, TElement>> GoRight()
        {
            var thiz = this;
            return
                from path in thiz.Path
                from r in thiz.Right.Head
                select thiz with
                {
                    Right = thiz.Right.Tail,
                    Focus = r,
                    Left = thiz.Focus.Cons(thiz.Left)
                };
        }

        public Option<Zipper<Z, TBranch, TElement>> GoLeft()
        {
            var thiz = this;
            return
                from l in thiz.Left.Head
                select thiz with
                {
                    Right = thiz.Focus.Cons(thiz.Right),
                    Focus = l,
                    Left = thiz.Left.Tail
                };
        }

        public Zipper<Z, TBranch, TElement> GoRightmost()
            =>
            Right.Last.Match(
                    Some: newFocus => this with
                    {
                        Right = Seq<TElement>(),
                        Focus = newFocus,
                        Left = Focus.Cons(Left).Concat(Right.Init)
                    },
                    None: () => this
                    );

        public Zipper<Z, TBranch, TElement> GoLeftmost()
            =>
            Left.Last.Match(
                    Some: newFocus => this with
                    {
                        Right = Focus.Cons(Right).Concat(Left.Init),
                        Focus = newFocus,
                        Left = Seq<TElement>()
                    },
                    None: () => this
                    );

        public Option<Zipper<Z, TBranch,>
    }
}
