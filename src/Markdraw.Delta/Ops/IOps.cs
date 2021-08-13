using System.Collections.Generic;
using Markdraw.Delta.Operations;

namespace Markdraw.Delta.Ops
{
  public interface IOps<out T> : IEnumerable<T> where T : IOp {}
}
