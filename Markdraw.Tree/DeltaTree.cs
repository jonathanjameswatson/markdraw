using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Markdraw.Delta;
using Markdraw.MarkdownToDelta;

namespace Markdraw.Tree
{
  public static class DeltaTree
  {
    public static Container Parse(string markdown = "")
    {
      var ops = MarkdownToDeltaConverter.Parse(markdown);
      return Parse(ops);
    }

    public static Container Parse(Ops ops)
    {
      return new Container(0, ops);
    }
  }
}
