
using System.Diagnostics.CodeAnalysis;
using CsharpTags.Core.Interface;

namespace CsharpTags.Core.Types
{
    /// <summary>
    /// Interface defining operations required for a zipper to navigate and manipulate 
    /// a tree-like structure with branch and element nodes.
    /// </summary>
    /// <typeparam name="TBranch">The type representing branch nodes (nodes with children).</typeparam>
    /// <typeparam name="TElement">The type representing all nodes in the tree (both branches and leaves).</typeparam>
    public interface ZipOps<TBranch, TElement>
    {
        /// <summary>
        /// Attempts to cast a node to a branch node. Returns Some(branch) if successful, 
        /// None if the node is not a branch (i.e., it's a leaf).
        /// </summary>
        /// <param name="node">The node to check and cast.</param>
        /// <returns>Option containing the branch if node is a branch, None otherwise.</returns>
        /// <remarks>
        /// This method also serves as a type test to determine if a node is a branch.
        /// </remarks>
        public abstract static Option<TBranch> AsBranch(TElement node);

        /// <summary>
        /// Creates a new branch node with the specified children.
        /// </summary>
        /// <param name="branch">The branch node to use as a template or base.</param>
        /// <param name="children">The new sequence of child nodes for the branch.</param>
        /// <returns>A new branch element with the given children.</returns>
        public abstract static TElement MakeNode(TBranch branch, Seq<TElement> children);

        /// <summary>
        /// Retrieves the children of a branch node.
        /// </summary>
        /// <param name="branch">The branch node whose children to retrieve.</param>
        /// <returns>A sequence of child elements.</returns>
        public abstract static Seq<TElement> ChildrenOf(TBranch branch);
    }

    /// <summary>
    /// A zipper data structure for navigating and manipulating tree-like structures.
    /// The zipper maintains a "focus" on a specific node and provides operations
    /// to move around and modify the tree efficiently.
    /// </summary>
    /// <typeparam name="Z">The ZipOps implementation type that provides tree operations.</typeparam>
    /// <typeparam name="TBranch">The type representing branch nodes.</typeparam>
    /// <typeparam name="TElement">The type representing all nodes in the tree.</typeparam>
    /// <remarks>
    /// This implementation is inspired by the zipper pattern from functional programming,
    /// which allows efficient navigation and modification of immutable data structures.
    /// </remarks>
    public record Zipper<Z, TBranch, TElement>
        where Z : ZipOps<TBranch, TElement>
        where TBranch : TElement
    {
        /// <summary>
        /// Left siblings of the currently focused node, in reverse order 
        /// (closest sibling is at the head).
        /// </summary>
        public required Seq<TElement> Left { get; init; }

        /// <summary>
        /// The node currently in focus (the "cursor" position in the tree).
        /// </summary>
        public required TElement Focus { get; init; }

        /// <summary>
        /// Right siblings of the currently focused node, in normal order.
        /// </summary>
        public required Seq<TElement> Right { get; init; }

        /// <summary>
        /// Ancestor branch nodes from root to parent of current focus, 
        /// in reverse order (parent is at head).
        /// </summary>
        public required Seq<TBranch> ParentNodes { get; init; }

        /// <summary>
        /// The zipper location that leads to this location (the parent zipper).
        /// None if this is the root location.
        /// </summary>
        public required Option<Zipper<Z, TBranch, TElement>> Path { get; init; }

        /// <summary>
        /// Indicates whether the tree has been modified at or below this location.
        /// Used to propagate changes up the tree.
        /// </summary>
        public required bool IsChanged { get; init; }

        /// <summary>
        /// Indicates whether this location is the end of a traversal 
        /// (no more nodes to visit).
        /// </summary>
        public required bool IsEnd { get; init; }


        private Zipper()
        {
        }

        /// <summary>
        /// Creates a new zipper focused on the root node of a tree.
        /// </summary>
        /// <param name="node">The root node of the tree.</param>
        [SetsRequiredMembers]
        public Zipper(TElement node)
        {
            Left = Seq<TElement>();
            Focus = node;
            Right = Seq<TElement>();
            ParentNodes = Seq<TBranch>();
            Path = None;
            IsChanged = false;
            IsEnd = false;
        }

        /// <summary>
        /// Gets the children of the currently focused node, if it is a branch.
        /// </summary>
        /// <returns>Some(children) if focus is a branch, None otherwise.</returns>
        public Option<Seq<TElement>> Children()
            => Z.AsBranch(Focus).Map(Z.ChildrenOf);

        /// <summary>
        /// Moves focus down to the first child of the current node.
        /// </summary>
        /// <returns>
        /// Some(zipper) with focus on first child if current node is a branch with children,
        /// None otherwise.
        /// </returns>
        public Option<Zipper<Z, TBranch, TElement>> GoDown()
        {
            var thiz = this;
            return
            from asBranch in Z.AsBranch(Focus)
            let children = Z.ChildrenOf(asBranch)
            from head in children.Head
            select new Zipper<Z, TBranch, TElement>()
            {
                Left = Seq<TElement>(),
                Focus = head,
                Right = children.Tail,
                ParentNodes = asBranch.Cons(thiz.ParentNodes),
                Path = thiz,
                IsChanged = false,
                IsEnd = false
            };
        }

        /// <summary>
        /// Moves focus up to the parent node.
        /// </summary>
        /// <returns>
        /// Some(zipper) with focus on parent if not at root, None otherwise.
        /// </returns>
        /// <remarks>
        /// If the current location has been changed (IsChanged = true), 
        /// the parent node will be reconstructed with the modified children.
        /// </remarks>
        public Option<Zipper<Z, TBranch, TElement>> GoUp()
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
        /// Navigates all the way to the root of the tree, applying any pending changes.
        /// </summary>
        /// <returns>The root node of the tree with all modifications applied.</returns>
        /// <remarks>
        /// This is typically called after a series of modifications to get the final tree.
        /// </remarks>
        public TElement Root()
        {
            var current = this;

            while (true)
            {
                if (current.Path.IsNone)
                {
                    return current.Focus;
                }

                var parent = current.GoUp();

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

        /// <summary>
        /// Moves focus to the right sibling.
        /// </summary>
        /// <returns>
        /// Some(zipper) with focus on right sibling if one exists, None otherwise.
        /// </returns>
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

        /// <summary>
        /// Moves focus to the left sibling.
        /// </summary>
        /// <returns>
        /// Some(zipper) with focus on left sibling if one exists, None otherwise.
        /// </returns>
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

        /// <summary>
        /// Moves focus to the rightmost sibling in the current sibling group.
        /// </summary>
        /// <returns>A new zipper focused on the rightmost sibling.</returns>
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

        /// <summary>
        /// Moves focus to the leftmost sibling in the current sibling group.
        /// </summary>
        /// <returns>A new zipper focused on the leftmost sibling.</returns>
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

        /// <summary>
        /// Inserts a new sibling to the left of the current focus.
        /// </summary>
        /// <param name="element">The node to insert.</param>
        /// <returns>
        /// Some(zipper) with new sibling inserted if not at root, None otherwise.
        /// </returns>
        public Option<Zipper<Z, TBranch, TElement>> InsertLeft(TElement element)
        {
            if (Path.IsNone)
            {
                return None;
            }
            return this with
            {
                Left = element.Cons(Left),
                IsChanged = true
            };
        }

        /// <summary>
        /// Inserts a new sibling to the right of the current focus.
        /// </summary>
        /// <param name="element">The node to insert.</param>
        /// <returns>
        /// Some(zipper) with new sibling inserted if not at root, None otherwise.
        /// </returns>
        public Option<Zipper<Z, TBranch, TElement>> InsertRight(TElement element)
        {
            if (Path.IsNone)
            {
                return None;
            }
            return this with
            {
                Right = element.Cons(Right),
                IsChanged = true
            };
        }

        /// <summary>
        /// Replaces the currently focused node with a new node.
        /// </summary>
        /// <param name="element">The new node to replace the focus.</param>
        /// <returns>A new zipper with the focus replaced.</returns>
        public Zipper<Z, TBranch, TElement> Replace(TElement element)
            => this with
            {
                Focus = element,
                IsChanged = true
            };

        /// <summary>
        /// Applies an edit function to the focused node.
        /// </summary>
        /// <param name="edit">
        /// Function that takes the current focus and returns Some(newNode) to replace it,
        /// or None to leave it unchanged.
        /// </param>
        /// <returns>
        /// A new zipper with the edited focus if edit returned Some, otherwise the same zipper.
        /// </returns>
        public Zipper<Z, TBranch, TElement> Edit(Func<TElement, Option<TElement>> edit)
            => edit(Focus).Match(
                    Some: Replace,
                    None: () => this
                    );

        /// <summary>
        /// Inserts a new child at the beginning of the current node's children.
        /// </summary>
        /// <param name="element">The node to insert as a child.</param>
        /// <returns>
        /// Some(zipper) with new child inserted if focus is a branch, None otherwise.
        /// </returns>
        public Option<Zipper<Z, TBranch, TElement>> InsertChild(TElement element)
            =>
            from focusAsBranch in Z.AsBranch(Focus)
            let children = Z.ChildrenOf(focusAsBranch)
            select Replace(Z.MakeNode(focusAsBranch, element.Cons(children)));

        /// <summary>
        /// Appends a new child at the end of the current node's children.
        /// </summary>
        /// <param name="element">The node to append as a child.</param>
        /// <returns>
        /// Some(zipper) with new child appended if focus is a branch, None otherwise.
        /// </returns>
        public Option<Zipper<Z, TBranch, TElement>> AppendChild(TElement element)
            =>
            from focusAsBranch in Z.AsBranch(Focus)
            let children = Z.ChildrenOf(focusAsBranch)
            select Replace(Z.MakeNode(focusAsBranch, children.Add(element)));

        /// <summary>
        /// Moves to the next node in depth-first pre-order traversal.
        /// </summary>
        /// <returns>
        /// A new zipper focused on the next node, or a zipper with IsEnd = true
        /// if there are no more nodes.
        /// </returns>
        /// <remarks>
        /// Traversal order: down (first child), right (next sibling), up then right (uncle).
        /// </remarks>
        public Zipper<Z, TBranch, TElement> GoNext()
        {
            if (IsEnd)
            {
                return this;
            }
            var down = GoDown();
            if (down.IsSome)
            {
                return (Zipper<Z, TBranch, TElement>)down;
            }
            var right = GoRight();
            if (right.IsSome)
            {
                return (Zipper<Z, TBranch, TElement>)right;
            }
            var current = this;
            while (true)
            {
                var up = GoUp();
                if (up.IsSome)
                {
                    var upRight
                        = up.Bind(x => x.GoRight());
                    if (upRight.IsSome)
                    {
                        return (Zipper<Z, TBranch, TElement>)upRight;
                    }
                    else
                    {
                        current = (Zipper<Z, TBranch, TElement>)up;
                    }
                }
                else
                {
                    return new Zipper<Z, TBranch, TElement>(current)
                    {
                        IsEnd = true
                    };
                }
            }
        }

        /// <summary>
        /// Moves to the next node that satisfies a predicate.
        /// </summary>
        /// <param name="predicate">Function that tests each node.</param>
        /// <returns>
        /// A zipper focused on the next matching node, or a zipper with IsEnd = true
        /// if no matching node is found.
        /// </returns>
        public Zipper<Z, TBranch, TElement> GoNextUntil(Func<TElement, bool> predicate)
        {
            var next = GoNext();
            while (true)
            {
                if (next.IsEnd)
                {
                    return next;
                }
                if (predicate(next.Focus))
                {
                    return next;
                }
                next = GoNext();
            }
        }

        /// <summary>
        /// Moves to the previous node in depth-first pre-order traversal.
        /// </summary>
        /// <returns>
        /// Some(zipper) focused on previous node if one exists, None otherwise.
        /// </returns>
        /// <remarks>
        /// Traversal order: left (previous sibling), then down to its rightmost descendant.
        /// </remarks>
        public Option<Zipper<Z, TBranch, TElement>> GoPrev()
        {
            var left = GoLeft();
            if (left.IsSome)
            {
                var newLoc = (Zipper<Z, TBranch, TElement>)left;
                while (true)
                {
                    var down = newLoc.GoDown();
                    if (down.IsSome)
                    {
                        newLoc = ((Zipper<Z, TBranch, TElement>)down).GoRightmost();
                    }
                    else
                    {
                        return newLoc;
                    }
                }
            }
            else
            {
                return GoUp();
            }
        }

        /// <summary>
        /// Removes the currently focused node from the tree.
        /// </summary>
        /// <returns>
        /// Some(zipper) with new focus after removal if successful, None if at root.
        /// </returns>
        /// <remarks>
        /// If there are left siblings, focuses on the last left sibling and goes to its
        /// rightmost descendant. Otherwise, removes the node from its parent's children.
        /// </remarks>
        public Option<Zipper<Z, TBranch, TElement>> Remove()
        {
            if (Path.IsNone)
            {
                return None;
            }

            if (!Left.IsEmpty)
            {
                var lastLeft = Left.Last;
                var newLeft = Left.Init;

                var newLoc = new Zipper<Z, TBranch, TElement>()
                {
                    Left = newLeft,
                    Focus = (TElement)lastLeft,
                    Right = Right,
                    ParentNodes = ParentNodes,
                    Path = Path,
                    IsChanged = true,
                    IsEnd = IsEnd
                };

                while (true)
                {
                    var down = newLoc.GoDown();
                    if (down.IsSome)
                    {

                        var downLoc = (Zipper<Z, TBranch, TElement>)down;
                        newLoc = downLoc.GoRightmost();
                    }
                    else
                    {
                        return newLoc;
                    }
                }
            }
            else
            {
                var parentNode = ParentNodes.Head;
                if (parentNode.IsNone)
                {
                    return None;
                }

                var newFocus = Z.MakeNode((TBranch)parentNode, Right);

                return Path.Match(
                    Some: parentPath => parentPath with
                    {
                        Focus = newFocus,
                        IsChanged = true
                    },
                    None: () => new Zipper<Z, TBranch, TElement>(newFocus)
                );
            }
        }

        /// <summary>
        /// Transforms a tree by applying a function to each node.
        /// </summary>
        /// <param name="start">The root node of the tree to transform.</param>
        /// <param name="map">
        /// Function that takes a node and returns Some(newNode) to replace it,
        /// or None to leave it unchanged.
        /// </param>
        /// <returns>The transformed tree with all modifications applied.</returns>
        /// <remarks>
        /// Performs a depth-first traversal, applying the map function to each node.
        /// </remarks>
        public static TElement Transform(TElement start, Func<TElement, Option<TElement>> map)
        {
            var current = new Zipper<Z, TBranch, TElement>(start);
            while (true)
            {
                if (current.IsEnd)
                {
                    return current.Focus;
                }
                current = current.Edit(map).GoNext();
            }
        }
    }

    public class HtmlZipperOps : ZipOps<Tag, HtmlElement>
    {
        /// <inheritdoc/>
        public static Option<Tag> AsBranch(HtmlElement node)
        {
            if (node is not Tag result)
            {
                return None;
            }
            return result;
        }

        /// <inheritdoc/>
        public static Seq<HtmlElement> ChildrenOf(Tag branch)
        {
            return branch.Children;
        }

        /// <inheritdoc/>
        public static HtmlElement MakeNode(Tag branch, Seq<HtmlElement> children)
        {
            return branch.Child(children);
        }
    }
}
