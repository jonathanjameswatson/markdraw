﻿using Markdraw.Delta.Formats;
using Markdraw.Delta.Operations;
using Markdraw.Delta.Operations.Inserts;

namespace Markdraw.Delta.Ops
{
  public class Transformation : Ops<IOp, Transformation>, ITransformation
  {
    /// <inheritdoc />
    public override Transformation Insert(Insert insert)
    {
      Add(insert);
      MergeBack(Length - 1);
      return this;
    }

    /// <summary>
    ///   Adds a <see cref="Operations.Delete" /> with a given length to the end of this transformation.
    /// </summary>
    /// <param name="amount">The length of the <see cref="Operations.Delete" />.</param>
    /// <returns>This sequence of operations.</returns>
    public Transformation Delete(int amount)
    {
      Add(new Delete(amount));
      return this;
    }

    /// <summary>
    ///   Adds a <see cref="Operations.Retain" /> with a given length and no formatting to the end of this transformation.
    /// </summary>
    /// <param name="amount">The length of the <see cref="Operations.Retain" />.</param>
    /// <returns>This sequence of operations.</returns>
    public Transformation Retain(int amount)
    {
      Add(new Retain(amount));
      return this;
    }

    /// <summary>
    ///   Adds a <see cref="Operations.Retain" /> with a given length and format to the end of this sequence of operations.
    /// </summary>
    /// <param name="amount">The length of the <see cref="Operations.Retain" />.</param>
    /// <param name="format">The format of the <see cref="Operations.Retain" />.</param>
    /// <returns>This sequence of operations.</returns>
    public Transformation Retain(int amount, Format format)
    {
      Add(new Retain(amount, format));
      return this;
    }
  }
}
