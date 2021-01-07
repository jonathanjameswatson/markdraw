using System;
using System.Collections.Generic;
using Markdraw.Delta;

namespace Markdraw.Tree
{
  public class Container : TreeNode
  {
    public List<TreeNode> ElementsInside;

    public Container(int depth, Ops ops) {
      ElementsInside = new List<TreeNode>();
      var opBuffer = new Ops();
      var lineOpBuffer = new Ops();
      bool indented = false;
      Indent lastIndent = null;

      foreach (var op in ops) {
        if (op is LineInsert lineInsert) {
          indents = lineInsert.Format.Indents;
          int numberOfIndents = indents.Count;
          bool goneForwardOrNowhere = numberOfIndents > depth;
          bool goneBack = numberOfIndents <= depth;

          if (indented)
          {
            if (goneBack || indents[depth] != lastIndent) {
              addContainer(lastIndent, opBuffer)

              if (goneBack) {
                indented = false;

              } else {
                lastIndent = indents[depth]
              }
            } else {
              opBuffer.InsertMany(lineOpBuffer)
            }
          } else {
            if (goneForwardOrNowhere)
          }
        }
      }
    }
  }

  public void addContainer(Indent indent, Ops ops) {
    foreach (var op in ops) {
      Container newContainer;
      switch (indent) {
        case Indent.Quote:
          newContainer = new QuoteContainer(depth + 1, opBuffer);
          break;
        case Indent.Bullet:
          newContainer = new BulletsContainer(depth + 1, opBuffer);
          break;
        case Indent.Number:
          newContainer = new NumbersContainer(depth + 1, opBuffer);
          break;
        case Indent.Code:
          newContainer = new QuoteContainer(depth + 1, opBuffer); // fix this
          break;
        default:
          newContainer = new Container(depth + 1, opBuffer);
          break;
      }

      ElementsInside.Add(newContainer);
    }
  }

  public void addLeaves(Ops ops) {
    var textBuffer = new Ops()
    foreach (var op in Ops)
    {
      if (op is TextInsert textInsert) {
        textBuffer.Insert(op);
      } else {
        if (textBuffer.Length != 0) {
          ElementsInside.Add(new TextLeaf(textBuffer));
          textBuffer = new Ops();
        }

        if (op is DividerInsert dividerInsert) {
          ElementsInside.Add(new DividerLeaf(dividerInsert));
        } else if (op is ImageInsert imageInsert) {
          ElementInside.Add(new ImageLeaf(imageInsert));
        } else if (op is CodeInsert codeInsert) {
          ElementInside.Add(new CodeLeaf(codeInsert));
        }
      }
    }

    if (textBuffer.Length != 0) {
      ElementsInside.Add(new TextLeaf(textBuffer));
    }
  }
}
