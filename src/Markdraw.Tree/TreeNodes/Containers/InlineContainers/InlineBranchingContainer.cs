using System.Collections.Immutable;
using Markdraw.Delta.Operations.Inserts.Inlines;
using Markdraw.Delta.Styles;
using Markdraw.Tree.TreeNodes.Leaves;

namespace Markdraw.Tree.TreeNodes.Containers.InlineContainers;

public abstract class InlineBranchingContainer : BranchingContainer<Style, InlineInsert, InlineInsert>
{
  protected InlineBranchingContainer(DeltaTree? deltaTree = null, int i = 0) : base(deltaTree, i) {}

  protected InlineBranchingContainer(List<TreeNode> elementsInside, DeltaTree? deltaTree = null, int i = 0) : base(
    elementsInside, deltaTree, i) {}

  protected override string Tag => "p";
  protected sealed override bool AllLeaves => true;

  protected override ImmutableList<Style> GetBranchMarkers(InlineInsert inlineInsert)
  {
    return inlineInsert.Format.Styles;
  }

  protected override Style NextBranchMarker(Style style)
  {
    return style;
  }

  protected override InlineBranchingContainer CreateChildContainer(Style style, IEnumerable<InlineInsert> document, int depth,
    int i)
  {
    return style switch {
      Bold => BoldInlineContainer.CreateInstance(depth, document, ParentTree, i),
      Italic => ItalicInlineContainer.CreateInstance(depth, document, ParentTree, i),
      Link(var url, var title) => LinkInlineContainer.CreateInstance(depth, document, ParentTree, i, url, title),
      _ => OuterInlineContainer.CreateInstance(depth, document, ParentTree, i)
    };
  }

  protected override int AddLeaves(IEnumerable<InlineInsert> document, InlineInsert? lastInlineInsert, int i)
  {
    if (lastInlineInsert?.Format.Code ?? false)
    {
      var codeContainer = CodeInlineContainer.CreateInstance(document, ParentTree, i);
      ElementsInside.Add(codeContainer);
      return i + codeContainer.Length;
    }

    var newI = i;

    foreach (var inlineInsert in document)
    {
      Leaf newElement = new InlineLeaf(inlineInsert, ParentTree, newI);
      ElementsInside.Add(newElement);
      newI += inlineInsert.Length;
    }

    return newI;
  }
}
