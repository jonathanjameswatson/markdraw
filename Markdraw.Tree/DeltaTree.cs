using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Markdraw.Delta;
using Markdraw.MarkdownToDelta;

namespace Markdraw.Tree
{
  public class DeltaTree
  {
    private Ops _delta;
    private Container _root;
    public Ops Delta
    {
      get => _delta;
      set
      {
        _delta = value;
        _root = new Container(0, Delta, this, 0);
      }
    }
    public Container Root { get => _root; }
    public bool HasI { get; set; }

    public DeltaTree(string markdown = "")
    {
      Delta = MarkdownToDeltaConverter.Parse(markdown);
      HasI = true;
    }

    public DeltaTree(Ops ops)
    {
      Delta = ops;
      HasI = true;
    }

    public void SetWithMarkdown(string markdown)
    {
      Delta = MarkdownToDeltaConverter.Parse(markdown);
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
