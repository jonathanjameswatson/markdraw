using System.Collections.Generic;
using Markdraw.Delta;

namespace Markdraw.Tree
{
  public class Container : TreeNode
  {
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
          int numberOfIndents = indents.Count;
          bool goneForwardOrNowhere = numberOfIndents > depth;
          bool goneBack = numberOfIndents <= depth;

          if (indented)
          {
            if (goneBack || indents[depth] != lastIndent)
            {
              addContainer(lastIndent, opBuffer, depth + 1);

              if (goneBack)
              {
                indented = false;
              }
              else
              {
                lastIndent = indents[depth];
              }
            }
            else
            {
              opBuffer.InsertMany(lineOpBuffer);
            }
          }
          else
          {
            if (goneForwardOrNowhere)
            {

            }
          }
        }
      }
    }

    public void addContainer(Indent? indent, Ops ops, int depth)
    {
      foreach (var op in ops)
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
    }

    public void addLeaves(Ops ops)
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
            _elementsInside.Add(new TextLeaf(textBuffer));
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
        _elementsInside.Add(new TextLeaf(textBuffer));
      }
    }
  }
}
