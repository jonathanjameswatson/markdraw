﻿using System.Collections.Immutable;
using Markdraw.Delta.Indents;
using Markdraw.Delta.Operations.Inserts;
using Markdraw.Delta.Operations.Inserts.Inlines;
using Markdraw.Tree.TreeNodes.Containers.InlineContainers;
using Markdraw.Tree.TreeNodes.Leaves;

namespace Markdraw.Tree.TreeNodes.Containers.BlockContainers;

public class BlockContainer : BranchingContainer<Indent, LineInsert, IInsert>
{
  protected BlockContainer(DeltaTree? deltaTree = null, int i = 0) : base(deltaTree, i) {}

  public BlockContainer(List<TreeNode> elementsInside, DeltaTree? deltaTree = null, int i = 0) : base(elementsInside,
    deltaTree, i) {}

  protected virtual bool LooseInlines => true;

  protected sealed override bool AllLeaves => false;

  public static BlockContainer CreateInstance(int depth, IEnumerable<IInsert> document, DeltaTree? deltaTree = null,
    int i = 0)
  {
    var container = new BlockContainer(deltaTree, i);
    container.Initialise(depth, document, i);
    return container;
  }

  protected override ImmutableList<Indent> GetBranchMarkers(LineInsert lineInsert)
  {
    return lineInsert.Format.Indents;
  }

  protected override Indent NextBranchMarker(Indent indent)
  {
    return indent switch {
      BulletIndent { Start: true } bulletIndent => bulletIndent with {
        Start = false
      },
      NumberIndent { Start: > -1 } numberIndent => numberIndent with {
        Start = -1
      },
      QuoteIndent { Start: true } quoteIndent => quoteIndent with {
        Start = false
      },
      _ => indent
    };
  }

  protected override BlockContainer CreateChildContainer(Indent indent, IEnumerable<IInsert> document, int depth, int i)
  {
    return indent switch {
      QuoteIndent => QuoteContainer.CreateInstance(depth, document, ParentTree, i),
      BulletIndent { Loose: var loose } => BulletsContainer.CreateInstance(depth - 1, document, ParentTree, i, loose),
      NumberIndent { Loose: var loose, Start: var start } => NumbersContainer.CreateInstance(depth - 1, document,
        ParentTree, i, start, loose),
      _ => CreateInstance(depth, document, ParentTree, i)
    };
  }

  protected override int AddLeaves(IEnumerable<IInsert> document, LineInsert? lastLineInsert, int i)
  {
    var header = lastLineInsert?.Format.Header ?? 0;
    var inlineBuffer = new List<InlineInsert>();
    var newI = i;

    var enumerable = document as IInsert[] ?? document.ToArray();
    if (!enumerable.Any())
    {
      ElementsInside.Add(OuterInlineContainer.CreateInstance(0, inlineBuffer, ParentTree, newI, header, LooseInlines));
      return i;
    }

    foreach (var op in enumerable)
    {
      if (op is InlineInsert inlineInsert)
      {
        inlineBuffer.Add(inlineInsert);
      }
      else
      {
        if (inlineBuffer.Count != 0)
        {
          var textLeaf = OuterInlineContainer.CreateInstance(0, inlineBuffer, ParentTree, newI, header, LooseInlines);
          ElementsInside.Add(textLeaf);
          inlineBuffer = new List<InlineInsert>();
          newI += textLeaf.Length;
        }

        Leaf newElement = op switch {
          DividerInsert dividerInsert => new DividerLeaf(dividerInsert, ParentTree, newI),
          CodeInsert codeInsert => new CodeLeaf(codeInsert, ParentTree, newI),
          BlockHtmlInsert blockHtmlInsert => new BlockHtmlLeaf(blockHtmlInsert, ParentTree, newI),
          _ => throw new ArgumentOutOfRangeException(nameof(document))
        };

        ElementsInside.Add(newElement);

        newI += 1;
      }
    }

    if (inlineBuffer.Count == 0) return newI;

    var finalTextLeaf = OuterInlineContainer.CreateInstance(0, inlineBuffer, ParentTree, newI, header, LooseInlines);
    ElementsInside.Add(finalTextLeaf);
    newI += finalTextLeaf.Length;

    return newI;
  }
}
