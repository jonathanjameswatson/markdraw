using Markdraw.Delta.Formats;
using Markdraw.Delta.Operations;

namespace Markdraw.Delta.OperationSequences;

/// <summary>A sequence of inserts representing a transformation.</summary>
/// <remarks>Transformations are not canonical, so two different transformations may have the same effect.</remarks>
public class Transformation : OperationSequence<IOp, Transformation>
{
  /// <summary>Adds a <see cref="Operations.Delete" /> with a given length to the end of this transformation.</summary>
  /// <param name="amount">The length of the <see cref="Operations.Delete" />.</param>
  /// <returns>This sequence of operations.</returns>
  public Transformation Delete(int amount)
  {
    Add(new Delete(amount));
    return this;
  }

  /// <summary>
  ///   Adds a <see cref="Operations.Retain" /> with a given length and no formatting to the end of this
  ///   transformation.
  /// </summary>
  /// <param name="amount">The length of the <see cref="Operations.Retain" />.</param>
  /// <returns>This sequence of operations.</returns>
  public Transformation Retain(int amount)
  {
    Add(new Retain(amount));
    return this;
  }

  /// <summary>
  ///   Adds a <see cref="Operations.Retain" /> with a given length and format to the end of this sequence of
  ///   operations.
  /// </summary>
  /// <param name="amount">The length of the <see cref="Operations.Retain" />.</param>
  /// <param name="formatModifier">The format modifier of the <see cref="Operations.Retain" />.</param>
  /// <returns>This sequence of operations.</returns>
  public Transformation Retain(int amount, IFormatModifier formatModifier)
  {
    Add(new Retain(formatModifier, amount));
    return this;
  }
}
