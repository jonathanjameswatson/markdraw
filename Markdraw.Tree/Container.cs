using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Markdraw.Delta;

namespace Markdraw.Tree
{
  public class Container : TreeNode, IEnumerable<TreeNode>
  {

    public Container(int depth, Ops ops) : this(depth, ops, null, 0) {}
    public Container(int depth, Ops ops, DeltaTree deltaTree, int i) : base(deltaTree, i)
    {
      ElementsInside = new List<TreeNode>();
      var opBuffer = new Ops();
      var lineOpBuffer = new Ops();
      var indented = false;
      Indent lastIndent = null;
      var currentI = i;

      foreach (var op in ops)
      {
        if (op is LineInsert lineInsert)
        {
          var indents = lineInsert.Format.NonEmptyIndents;
          var header = lineInsert.Format.Header is null ? 0 : (int)lineInsert.Format.Header;
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
        }
        else if (op is Insert insert)
        {
          lineOpBuffer.Insert(insert);
        }
        else
        {
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
        currentI = AddLeaves(lineOpBuffer, 0, currentI);
        currentI += 1;
      }

      if (ElementsInside.Count > 0)
      {
        var lastElement = ElementsInside[ElementsInside.Count - 1];

        _length = lastElement.I + lastElement.Length - i;
      }
      else
      {
        _length = 0;
      }
    }

    public Container(List<TreeNode> elementsInside) : this(elementsInside, null, 0) {}

    public Container(List<TreeNode> elementsInside, DeltaTree deltaTree, int i) : base(deltaTree, i)
    {
      ElementsInside = elementsInside;
    }
    public virtual string Tag => "div";
    public virtual string InsideTag => "li";
    public virtual bool WrapAllInside => false;
    public List<TreeNode> ElementsInside { get; }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }


    public IEnumerator<TreeNode> GetEnumerator()
    {
      return ElementsInside.GetEnumerator();
    }

    public int AddContainer(Indent indent, Ops ops, int depth, int start)
    {
      Container newContainer;
      if (indent == Indent.Quote)
      {
        newContainer = new QuoteContainer(depth, ops, ParentTree, start);
      }
      else if (indent == Indent.Bullet)
      {
        newContainer = new BulletsContainer(depth, ops, ParentTree, start);
      }
      else if (indent.IsNumber())
      {
        newContainer = new NumbersContainer(depth, ops, ParentTree, start);
      }
      else if (indent == Indent.Code)
      {
        newContainer = new QuoteContainer(depth, ops, ParentTree, start);// fix this
      }
      else
      {
        newContainer = new Container(depth, ops, ParentTree, start);
      }

      ElementsInside.Add(newContainer);

      return start + newContainer.Length;
    }

    public int AddLeaves(Ops ops, int header, int start)
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

          if (op is DividerInsert dividerInsert)
          {
            ElementsInside.Add(new DividerLeaf(dividerInsert, ParentTree, i));
          }
          else if (op is ImageInsert imageInsert)
          {
            ElementsInside.Add(new ImageLeaf(imageInsert, ParentTree, i));
          }
          else if (op is CodeInsert codeInsert)
          {
            ElementsInside.Add(new CodeLeaf(codeInsert, ParentTree, i));
          }

          i += 1;
        }
      }

      if (textBuffer.Count != 0)
      {
        var textLeaf = new TextLeaf(textBuffer, header == 0 ? "p" : $"h{header}", ParentTree, i);
        ElementsInside.Add(textLeaf);
        i += textLeaf.Length;
      }

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
