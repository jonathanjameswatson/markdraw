using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Markdraw.Delta;
using Markdraw.MarkdownToDelta;

namespace Markdraw.Tree
{
  public class DeltaTree
  {
    private Ops _ops;
    public Container Contents { get; }

    public DeltaTree(string markdown = "")
    {
      _ops = MarkdownToDeltaConverter.Parse(markdown);
      Contents = new Container(0, _ops);
    }
  }
}
