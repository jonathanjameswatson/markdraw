using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdraw.Delta
{
  public class ModifyingLineFormat : Format
  {
    public Action<List<Indent>> IndentsFunction { get; set; }
    public Func<int, int> HeaderFunction { get; set; }

    public ModifyingLineFormat(Action<List<Indent>> indentsFunction, Func<int, int> headerFunction)
    {
      IndentsFunction = indentsFunction;
      HeaderFunction = headerFunction;
    }

    /*

    public override bool Equals(object obj)
    {
      return (obj is LineFormat lineFormat
              && Indents.SequenceEqual(lineFormat.Indents)
              && Header == lineFormat.Header
             );
    }

    public override int GetHashCode()
    {
      return (Indents, Header).GetHashCode();
    }

    */
  }
}