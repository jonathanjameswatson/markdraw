using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Markdraw.Delta.Indents;
using Markdraw.Delta.Operations.Inserts;

namespace Markdraw.Tree
{
  public abstract class BranchingContainer<TBranchMarker, TBranchInsert, TInsert> : Container
    where TBranchMarker : class
    where TBranchInsert : Insert
    where TInsert : Insert
  {

    protected BranchingContainer(DeltaTree deltaTree = null, int i = 0) : base(deltaTree, i) {}

    protected BranchingContainer(List<TreeNode> elementsInside, DeltaTree deltaTree = null, int i = 0) : base(elementsInside, deltaTree, i) {}

    protected virtual bool AllLeaves => false;

    protected abstract TBranchMarker NextBranchMarker(TBranchMarker branchMarker);

    [return: NotNull]
    protected abstract ImmutableList<TBranchMarker> GetBranchMarkers([NotNull] TBranchInsert branchInsert);

    protected abstract BranchingContainer<TBranchMarker, TBranchInsert, TInsert> CreateChildContainer(TBranchMarker branchMarker, IEnumerable<TInsert> document, int depth, int i);

    protected abstract int AddLeaves(IEnumerable<TInsert> document, TBranchInsert lastInsert, int i);

    protected void Initialise(int depth, IEnumerable<TInsert> document, int i)
    {
      var opBuffer = new List<TInsert>();
      var leafOpBuffer = new List<TInsert>();
      var foundBranchMarker = false;
      TBranchMarker lastBranchMarker = null;
      var currentI = i;

      foreach (var insert in document)
      {
        if (insert is TBranchInsert branchInsert)
        {
          var branchMarkers = GetBranchMarkers(branchInsert);

          if (AllLeaves)
          {
            leafOpBuffer.Add(insert);
          }

          if (foundBranchMarker)
          {
            var goneBack = branchMarkers.Count <= depth;
            if (goneBack || branchMarkers[depth] is not ContinueIndent && !branchMarkers[depth].Equals(NextBranchMarker(lastBranchMarker)))
            {
              currentI = AddContainer(lastBranchMarker, opBuffer, depth + 1, currentI);
              currentI += 1;

              if (goneBack)
              {
                foundBranchMarker = false;
                currentI = AddLeaves(leafOpBuffer, branchInsert, currentI);
                currentI += 1;
              }
              else
              {
                lastBranchMarker = branchMarkers[depth];
                opBuffer = leafOpBuffer;
              }
            }
            else
            {
              opBuffer.AddRange(leafOpBuffer);
            }

            if (!AllLeaves)
            {
              opBuffer.Add(insert);
            }
          }
          else
          {
            if (branchMarkers.Count > depth)
            {
              lastBranchMarker = branchMarkers[depth];
              foundBranchMarker = true;

              opBuffer = leafOpBuffer;

              if (!AllLeaves)
              {
                opBuffer.Add(insert);
              }
            }
            else
            {
              if (!AllLeaves)
              {
                opBuffer.Add(insert);
              }

              currentI = AddLeaves(leafOpBuffer, branchInsert, currentI);
              currentI += 1;
            }
          }

          leafOpBuffer = new List<TInsert>();
        }
        else
        {
          leafOpBuffer.Add(insert);
        }
      }

      if (foundBranchMarker)
      {
        currentI = AddContainer(lastBranchMarker, opBuffer, depth + 1, currentI);
        currentI += 1;
      }

      if (leafOpBuffer.Count != 0)
      {
        AddLeaves(leafOpBuffer, null, currentI);
      }

      if (ElementsInside.Count > 0)
      {
        var lastElement = ElementsInside[^1];

        Length = lastElement.I + lastElement.Length - i;
      }
      else
      {
        Length = 0;
      }
    }

    private int AddContainer(TBranchMarker branchMarker, IEnumerable<TInsert> document, int depth, int i)
    {
      var newContainer = CreateChildContainer(branchMarker, document, depth, i);

      ElementsInside.Add(newContainer);

      return i + newContainer.Length;
    }
  }

}
