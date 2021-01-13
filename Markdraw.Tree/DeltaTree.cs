using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Markdraw.Delta;
using Markdraw.MarkdownToDelta;

namespace Markdraw.Tree
{
  public class DeltaTree
  {
    public Ops Delta { get; set; }
    public Container Root { get; set; }

    public DeltaTree(string markdown = "")
    {
      Delta = MarkdownToDeltaConverter.Parse(markdown);
      Root = new Container(0, Delta, this);
    }

    public DeltaTree(Ops ops)
    {
      Delta = ops;
      Root = new Container(0, Delta, this);
    }

    public static Container Parse(string markdown)
    {
      Ops ops = MarkdownToDeltaConverter.Parse(markdown);
      return new Container(0, ops);
    }

    public static Container Parse(Ops ops)
    {
      return new Container(0, ops);
    }
  }
}
