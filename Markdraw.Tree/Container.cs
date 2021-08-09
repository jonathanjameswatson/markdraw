using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Markdraw.Delta;
using Markdraw.Delta.Indents;
using Markdraw.Delta.Operations.Inserts;

namespace Markdraw.Tree
{
  public class Container : TreeNode, IEnumerable<TreeNode>
  {

    protected virtual string Tag => "div";
    protected virtual string StartingTag => null;
    protected virtual string InsideTag => "li";
    protected bool Loose = true;
    protected virtual bool WrapAllInside => false;
    public List<TreeNode> ElementsInside { get; } = new();

    protected Container(DeltaTree deltaTree = null, int i = 0) : base(deltaTree, i) {}

    public static Container CreateInstance(int depth, Ops ops, DeltaTree deltaTree = null, int i = 0)
    {
      var container = new Container(deltaTree, i);

      return Initialise(depth, ops, i, container);
    }

    protected static T Initialise<T>(int depth, Ops ops, int i, T container) where T : Container
    {
      var opBuffer = new Ops();
      var lineOpBuffer = new Ops();
      var indented = false;
      Indent lastIndent = null;
      var currentI = i;

      foreach (var op in ops)
      {
        switch (op)
        {
          case LineInsert lineInsert:
          {
            var indents = lineInsert.Format.Indents;
            var header = lineInsert.Format.Header ?? 0;
            var numberOfIndents = indents.Count;
            var goneForward = numberOfIndents > depth;
            var goneBack = numberOfIndents <= depth;

            if (indented)
            {
              if (goneBack || indents[depth] != NextIndent(lastIndent))
              {
                currentI = container.AddContainer(lastIndent, opBuffer, depth + 1, currentI);
                currentI += 1;

                if (goneBack)
                {
                  indented = false;
                  currentI = container.AddLeaves(lineOpBuffer, header, currentI);
                  currentI += 1;
                }
                else
                {
                  lastIndent = indents[depth];
                  opBuffer = lineOpBuffer;
                }
              }
              else
              {
                opBuffer.InsertMany(lineOpBuffer);
              }

              opBuffer.Insert(lineInsert);
            }
            else
            {
              if (goneForward)
              {
                lastIndent = indents[depth];
                indented = true;

                opBuffer = lineOpBuffer;
                opBuffer.Insert(lineInsert);
              }
              else
              {
                currentI = container.AddLeaves(lineOpBuffer, header, currentI);
                currentI += 1;
              }
            }

            lineOpBuffer = new Ops();
            break;
          }
          case Insert insert:
            lineOpBuffer.Insert(insert);
            break;
          default:
            throw new ArgumentException("ops must only contain inserts.");
        }
      }

      if (opBuffer.Length != 0 && indented)
      {
        currentI = container.AddContainer(lastIndent, opBuffer, depth + 1, currentI);
        currentI += 1;
      }

      if (lineOpBuffer.Length != 0)
      {
        container.AddLeaves(lineOpBuffer, 0, currentI);
      }

      if (container.ElementsInside.Count > 0)
      {
        var lastElement = container.ElementsInside[^1];

        container.Length = lastElement.I + lastElement.Length - i;
      }
      else
      {
        container.Length = 0;
      }

      return container;
    }

    public Container(List<TreeNode> elementsInside, DeltaTree deltaTree = null, int i = 0) : base(deltaTree, i)
    {
      ElementsInside = elementsInside;
    }

    private static Indent NextIndent(Indent indent)
    {
      return indent switch {
        BulletIndent { Start: true } bulletIndent => bulletIndent with {
          Start = false
        },
        NumberIndent { Start: > 0 } numberIndent => numberIndent with {
          Start = 0
        },
        _ => indent
      };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }


    public IEnumerator<TreeNode> GetEnumerator()
    {
      return ElementsInside.GetEnumerator();
    }

    private int AddContainer(Indent indent, Ops ops, int depth, int i)
    {
      var newContainer = indent switch {
        QuoteIndent => QuoteContainer.CreateInstance(depth, ops, ParentTree, i),
        BulletIndent { Loose: var loose } => BulletsContainer.CreateInstance(depth, ops, ParentTree, i, loose),
        NumberIndent { Loose: var loose, Start: var start } => NumbersContainer.CreateInstance(depth, ops, ParentTree, i, start, loose),
        CodeIndent => QuoteContainer.CreateInstance(depth, ops, ParentTree, i),
        _ => CreateInstance(depth, ops, ParentTree, i)
      };

      ElementsInside.Add(newContainer);

      return i + newContainer.Length;
    }

    private int AddLeaves(Ops ops, int header, int i)
    {
      var inlineBuffer = new List<InlineInsert>();
      var newI = i;

      foreach (var op in ops)
      {
        if (op is InlineInsert inlineInsert)
        {
          inlineBuffer.Add(inlineInsert);
        }
        else
        {
          if (inlineBuffer.Count != 0)
          {
            var textLeaf = new InlineLeaf(inlineBuffer, header == 0 ? "p" : $"h{header}", ParentTree, newI, Loose);
            ElementsInside.Add(textLeaf);
            inlineBuffer = new List<InlineInsert>();
            newI += textLeaf.Length;
          }

          switch (op)
          {
            case DividerInsert dividerInsert:
              ElementsInside.Add(new DividerLeaf(dividerInsert, ParentTree, newI));
              break;
            case CodeInsert codeInsert:
              ElementsInside.Add(new CodeLeaf(codeInsert, ParentTree, newI));
              break;
          }

          newI += 1;
        }
      }

      if (inlineBuffer.Count == 0) return newI;

      var finalTextLeaf = new InlineLeaf(inlineBuffer, header == 0 ? "p" : $"h{header}", ParentTree, newI, Loose);
      ElementsInside.Add(finalTextLeaf);
      newI += finalTextLeaf.Length;

      return newI;
    }

    public override string ToString()
    {
      var stringBuilder = new StringBuilder();

      stringBuilder.Append(StartingTag ?? $@"<{Tag}>");

      foreach (var child in ElementsInside)
      {
        if (WrapAllInside)
        {
          stringBuilder.Append($@"<{InsideTag}>");
        }

        stringBuilder.Append(child);

        if (WrapAllInside)
        {
          stringBuilder.Append($@"</{InsideTag}>");
        }
      }

      stringBuilder.Append($@"</{Tag}>");

      return stringBuilder.ToString();
    }
  }

}
