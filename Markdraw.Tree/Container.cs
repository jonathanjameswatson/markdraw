using System;
using System.Collections.Generic;
using System.Text;
using Markdraw.Delta;

namespace Markdraw.Tree
{
  public class Container : TreeNode, IEnumerable<TreeNode>
  {
    public virtual string Tag { get => "div"; }
    public virtual string InsideTag { get => "li"; }
    public virtual bool WrapAllInside { get => false; }

    private List<TreeNode> _elementsInside;
    public List<TreeNode> ElementsInside { get => _elementsInside; }

    public Container(int depth, Ops ops) : this(depth, ops, null, 0) { }
    public Container(int depth, Ops ops, DeltaTree deltaTree, int i) : base(deltaTree, i)
    {
      _elementsInside = new List<TreeNode>();
      var opBuffer = new Ops();
      var lineOpBuffer = new Ops();
      bool indented = false;
      Indent lastIndent = null;
      int currentI = i;

      foreach (var op in ops)
      {
        if (op is LineInsert lineInsert)
        {
          var indents = lineInsert.Format.NonEmptyIndents;
          int header = lineInsert.Format.Header is null ? 0 : (int)lineInsert.Format.Header;
          int numberOfIndents = indents.Count;
          bool goneForward = numberOfIndents > depth;
          bool goneBack = numberOfIndents <= depth;

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

    public Container(List<TreeNode> elementsInside) : this(elementsInside, null, 0) { }

    public Container(List<TreeNode> elementsInside, DeltaTree deltaTree, int i) : base(deltaTree, i)
    {
      _elementsInside = elementsInside;
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
        newContainer = new QuoteContainer(depth, ops, ParentTree, start); // fix this
      }
      else
      {
        newContainer = new Container(depth, ops, ParentTree, start);
      }

      _elementsInside.Add(newContainer);

      return start + newContainer.Length;
    }

    public int AddLeaves(Ops ops, int header, int start)
    {
      var textBuffer = new List<TextInsert>();
      int i = start;

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
            var textLeaf = new TextLeaf(textBuffer, header, ParentTree, i);
            _elementsInside.Add(textLeaf);
            textBuffer = new List<TextInsert>();
            i += textLeaf.Length;
          }

          if (op is DividerInsert dividerInsert)
          {
            _elementsInside.Add(new DividerLeaf(dividerInsert, ParentTree, i));
          }
          else if (op is ImageInsert imageInsert)
          {
            _elementsInside.Add(new ImageLeaf(imageInsert, ParentTree, i));
          }
          else if (op is CodeInsert codeInsert)
          {
            _elementsInside.Add(new CodeLeaf(codeInsert, ParentTree, i));
          }

          i += 1;
        }
      }

      if (textBuffer.Count != 0)
      {
        var textLeaf = new TextLeaf(textBuffer, header, ParentTree, i);
        _elementsInside.Add(textLeaf);
        i += textLeaf.Length;
      }

      return i;
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }


    public IEnumerator<TreeNode> GetEnumerator()
    {
      return ElementsInside.GetEnumerator();
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

        stringBuilder.Append(child.ToString());

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
