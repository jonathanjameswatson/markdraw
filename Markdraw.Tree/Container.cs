using System;
using System.Collections.Generic;
using System.Text;
using Markdraw.Delta;

namespace Markdraw.Tree
{
  public class Container : TreeNode, IEnumerable<TreeNode>
  {
    public virtual string OpeningTag { get => @"<div>"; }
    public virtual string InsideOpeningTag { get => @"<p>"; }
    public virtual string InsideClosingTag { get => @"</p>"; }
    public virtual string ClosingTag { get => @"</div>"; }
    public virtual bool WrapAllInside { get => false; }

    private List<TreeNode> _elementsInside;
    public List<TreeNode> ElementsInside { get => _elementsInside; }

    public Container(int depth, Ops ops)
    {
      _elementsInside = new List<TreeNode>();
      var opBuffer = new Ops();
      var lineOpBuffer = new Ops();
      bool indented = false;
      Indent? lastIndent = null;

      foreach (var op in ops)
      {
        if (op is LineInsert lineInsert)
        {
          var indents = lineInsert.Format.Indents;
          int header = lineInsert.Format.Header is null ? 0 : (int)lineInsert.Format.Header;
          int numberOfIndents = indents.Count;
          bool goneForward = numberOfIndents > depth;
          bool goneBack = numberOfIndents <= depth;

          if (indented)
          {
            if (goneBack || indents[depth] != lastIndent)
            {
              AddContainer(lastIndent, opBuffer, depth + 1);

              if (goneBack)
              {
                indented = false;
                AddLeaves(lineOpBuffer, header);
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
              AddLeaves(lineOpBuffer, header);
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
        AddContainer(lastIndent, opBuffer, depth + 1);
      }

      if (lineOpBuffer.Length != 0)
      {
        AddLeaves(lineOpBuffer, 0);
      }
    }

    public Container(List<TreeNode> elementsInside)
    {
      _elementsInside = elementsInside;
    }

    public void AddContainer(Indent? indent, Ops ops, int depth)
    {
      Container newContainer;
      switch (indent)
      {
        case Indent.Quote:
          newContainer = new QuoteContainer(depth, ops);
          break;
        case Indent.Bullet:
          newContainer = new BulletsContainer(depth, ops);
          break;
        case Indent.Number:
          newContainer = new NumbersContainer(depth, ops);
          break;
        case Indent.Code:
          newContainer = new QuoteContainer(depth, ops); // fix this
          break;
        default:
          newContainer = new Container(depth, ops);
          break;
      }

      _elementsInside.Add(newContainer);
    }

    public void AddLeaves(Ops ops, int header)
    {
      var textBuffer = new List<TextInsert>();
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
            _elementsInside.Add(new TextLeaf(textBuffer, header));
            textBuffer = new List<TextInsert>();
          }

          if (op is DividerInsert dividerInsert)
          {
            _elementsInside.Add(new DividerLeaf(dividerInsert));
          }
          else if (op is ImageInsert imageInsert)
          {
            _elementsInside.Add(new ImageLeaf(imageInsert));
          }
          else if (op is CodeInsert codeInsert)
          {
            _elementsInside.Add(new CodeLeaf(codeInsert));
          }
        }
      }

      if (textBuffer.Count != 0)
      {
        _elementsInside.Add(new TextLeaf(textBuffer, header));
      }
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

      stringBuilder.Append(OpeningTag);

      foreach (var child in ElementsInside)
      {
        if (WrapAllInside)
        {
          stringBuilder.Append(InsideOpeningTag);
        }

        stringBuilder.Append(child.ToString());

        if (WrapAllInside)
        {
          stringBuilder.Append(InsideClosingTag);
        }
      }

      stringBuilder.Append(ClosingTag);

      return stringBuilder.ToString();
    }
  }
}
