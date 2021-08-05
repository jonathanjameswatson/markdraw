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

    public Container(int depth, Ops ops, DeltaTree deltaTree = null, int i = 0) : base(deltaTree, i)
    {
      ElementsInside = new List<TreeNode>();
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
              if (goneBack || indents[depth] != lastIndent)
              {
                currentI = AddContainer(lastIndent, opBuffer, depth + 1, currentI);
                currentI += 1;

                if (goneBack)
                {
                  indented = false;
                  currentI = AddLeaves(lineOpBuffer, header, currentI);
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
                currentI = AddLeaves(lineOpBuffer, header, currentI);
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
        currentI = AddContainer(lastIndent, opBuffer, depth + 1, currentI);
        currentI += 1;
      }

      if (lineOpBuffer.Length != 0)
      {
        AddLeaves(lineOpBuffer, 0, currentI);
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

    public Container(List<TreeNode> elementsInside, DeltaTree deltaTree = null, int i = 0) : base(deltaTree, i)
    {
      ElementsInside = elementsInside;
    }
    protected virtual string Tag => "div";
    protected virtual string InsideTag => "li";
    protected virtual bool WrapAllInside => false;
    public List<TreeNode> ElementsInside { get; }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }


    public IEnumerator<TreeNode> GetEnumerator()
    {
      return ElementsInside.GetEnumerator();
    }

    private int AddContainer(Indent indent, Ops ops, int depth, int start)
    {
      Container newContainer = indent switch {
        QuoteIndent => new QuoteContainer(depth, ops, ParentTree, start),
        BulletIndent => new BulletsContainer(depth, ops, ParentTree, start),
        NumberIndent => new NumbersContainer(depth, ops, ParentTree, start),
        CodeIndent => new QuoteContainer(depth, ops, ParentTree, start),
        _ => new Container(depth, ops, ParentTree, start)
      };

      ElementsInside.Add(newContainer);

      return start + newContainer.Length;
    }

    private int AddLeaves(Ops ops, int header, int start)
    {
      var textBuffer = new List<TextInsert>();
      var i = start;

      foreach (var op in ops)
      {
        if (op is TextInsert textInsert)
        {
          textBuffer.Add(textInsert);
        }
        else
        {
          if (textBuffer.Count != 0)
          {
            var textLeaf = new TextLeaf(textBuffer, header == 0 ? "p" : $"h{header}", ParentTree, i);
            ElementsInside.Add(textLeaf);
            textBuffer = new List<TextInsert>();
            i += textLeaf.Length;
          }

          switch (op)
          {
            case DividerInsert dividerInsert:
              ElementsInside.Add(new DividerLeaf(dividerInsert, ParentTree, i));
              break;
            case ImageInsert imageInsert:
              ElementsInside.Add(new ImageLeaf(imageInsert, ParentTree, i));
              break;
            case CodeInsert codeInsert:
              ElementsInside.Add(new CodeLeaf(codeInsert, ParentTree, i));
              break;
          }

          i += 1;
        }
      }

      if (textBuffer.Count == 0) return i;

      var finalTextLeaf = new TextLeaf(textBuffer, header == 0 ? "p" : $"h{header}", ParentTree, i);
      ElementsInside.Add(finalTextLeaf);
      i += finalTextLeaf.Length;

      return i;
    }

    public override string ToString()
    {
      var stringBuilder = new StringBuilder();

      stringBuilder.Append($@"<{Tag}>");

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
