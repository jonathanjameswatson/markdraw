using System.Collections.Generic;
using System.Linq;

// ReSharper disable IteratorNeverReturns

namespace Markdraw.Helpers
{
  public static class SequenceHelpers
  {

    private static IEnumerator<T> Repeat<T>(T repeated)
    {
      IEnumerable<T> Enumerable()
      {
        while (true)
        {
          yield return repeated;
        }
      }

      return Enumerable().GetEnumerator();
    }

    public static IEnumerator<T> RepeatAfterFirst<T>(T first, T repeated)
    {
      IEnumerable<T> Enumerable()
      {
        yield return first;
        while (true)
        {
          yield return repeated;
        }
      }

      return Enumerable().GetEnumerator();
    }

    public static IEnumerable<T> YieldFromSequences<T>(IEnumerable<IEnumerator<T>> sequences)
    {
      return sequences.Select(sequence => {
        sequence.MoveNext();
        return sequence.Current;
      });
    }
  }
}
