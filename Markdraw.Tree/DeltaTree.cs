using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Markdraw.Delta;
using Markdraw.MarkdownToDeltaConverter;

namespace Markdraw.Tree
{
  public class DeltaTree
  {
    private Ops _ops;
    public Container Contents;

    public DeltaTree(string markdown="")
    {
      _ops = MarkdownToDeltaConverter.Parse(markdown);
      Contents = new Container()
    }
  }
}
