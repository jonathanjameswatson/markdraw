using System;
using System.Collections.Generic;
using System.Linq;
using Markdraw.Delta.Formats;
using Markdraw.Delta.Operations;
using Markdraw.Delta.Operations.Inserts;
using Markdraw.Delta.Operations.Inserts.Inlines;

namespace Markdraw.Delta.OperationSequences
{
  /// <summary>
  ///   A sequence of <see cref="Operations.Inserts.Insert" />s representing a Markdown document.
  /// </summary>
  /// <remarks>
  ///   Each unique document is represented by one canonical sequence of inserts.
  /// </remarks>
  public class Document : OperationSequence<Insert, Document>
  {
    /// <summary>
    ///   The number of characters in the document.
    /// </summary>
    /// <remarks>
    ///   Certain inserts result in one character, such as dividers and code blocks.
    /// </remarks>
    public int Characters => this.Sum(op => op.Length);

    /// <summary>
    ///   Transforms this document with a transformation.
    /// </summary>
    /// <param name="transformation">A transformation.</param>
    /// <returns>This document.</returns>
    public Document Transform(IEnumerable<Op> transformation)
    {
      var opIndex = 0;
      var opCharacterIndex = 0;

      foreach (var op in transformation)
      {
        switch (op)
        {
          case Retain (var formatModifier, var length):
          {
            var shouldFormat = formatModifier is not null;

            if (shouldFormat && opCharacterIndex != 0 && Get(opIndex) is TextInsert before)
            {
              var (newBefore, after) = before.SplitAt(opCharacterIndex);
              Set(opIndex, newBefore);
              InsertOp(opIndex + 1, after);
              opIndex += 1;
              opCharacterIndex = 0;
            }

            while (length > 0)
            {
              var next = Get(opIndex);
              var lengthRemaining = next.Length - opCharacterIndex;
              var advanced = Math.Min(lengthRemaining, length);

              opCharacterIndex = (opCharacterIndex + advanced) % next.Length;
              length -= advanced;

              if (opCharacterIndex != 0)
              {
                if (shouldFormat)
                {
                  var textInsert = next as TextInsert;
                  var (newBefore, after) = textInsert.SplitAt(opCharacterIndex);
                  next = newBefore;
                  opCharacterIndex = 0;
                  Set(opIndex, newBefore);
                  InsertOp(opIndex + 1, after);
                }
              }

              if (shouldFormat)
              {
                var newNext = next.SetFormat(formatModifier);
                if (newNext is not null)
                {
                  next = newNext;
                  Set(opIndex, next);
                }

                if (opIndex >= 1)
                {
                  var beforeLength = MergeBack(opIndex);
                  if (beforeLength is not null)
                  {
                    opIndex -= 1;
                  }
                }
              }

              if (opCharacterIndex == 0)
              {
                opIndex += 1;
              }
            }

            if (shouldFormat && opIndex < Length && opIndex >= 1)
            {
              var beforeLength = MergeBack(opIndex);
              if (beforeLength is not null)
              {
                opCharacterIndex += (int)beforeLength;
                opIndex -= 1;
              }
            }

            break;
          }
          case Delete (var length):
          {
            while (length > 0)
            {
              var next = Get(opIndex);
              if (next is TextInsert nextTextInsert)
              {
                if (opCharacterIndex != 0)
                {
                  var (newBefore, after) = nextTextInsert.SplitAt(opCharacterIndex);
                  opCharacterIndex = 0;
                  Set(opIndex, newBefore);
                  opIndex += 1;
                  InsertOp(opIndex, after);
                  nextTextInsert = after;
                }

                var lengthRemaining = nextTextInsert.Length - opCharacterIndex;
                var toDelete = Math.Min(lengthRemaining, length);
                var deleted = nextTextInsert.DeleteUpTo(toDelete);
                length -= toDelete;

                if (deleted is null)
                {
                  RemoveAt(opIndex);
                }
                else
                {
                  Set(opIndex, deleted);
                }

                opCharacterIndex = deleted is null ? 0 : opCharacterIndex;
              }
              else
              {
                RemoveAt(opIndex);
                length -= 1;
              }

              var beforeLength = MergeBack(opIndex);
              if (beforeLength is null) continue;
              opIndex -= 1;
              opCharacterIndex += (int)beforeLength;
            }

            break;
          }
          case Insert insert when opCharacterIndex == 0:
          {
            InsertOp(opIndex, insert);

            if (opIndex == Length - 1)
            {
              MergeBack(Length - 1);
            }
            else
            {
              var beforeLength = MergeBack(opIndex);
              if (beforeLength is not null)
              {
                opIndex -= 1;
                opCharacterIndex += (int)beforeLength;
              }
            }

            opIndex += 1;

            if (opIndex < Length - 1)
            {
              var beforeLength = MergeBack(opIndex);
              if (beforeLength is not null)
              {
                opIndex -= 1;
                opCharacterIndex += (int)beforeLength;
              }
            }

            break;
          }
          case Insert insert:
          {
            if (Get(opIndex) is TextInsert before)
            {
              var (newBefore, after) = before.SplitAt(opCharacterIndex);
              Set(opIndex, newBefore);
              InsertOp(opIndex + 1, insert);
              InsertOp(opIndex + 2, after);
              opIndex += 2;
              opCharacterIndex = 0;

              if (op is TextInsert middle)
              {
                var beforeAndMiddleLength = newBefore.Length + middle.Length;
                var merged = after.Merge(middle, newBefore);
                if (merged is not null)
                {
                  RemoveAt(opIndex);
                  RemoveAt(opIndex - 1);
                  opIndex -= 2;
                  Set(opIndex, merged);
                  opCharacterIndex += beforeAndMiddleLength;
                }
              }
            }

            break;
          }
        }
      }

      return this;
    }

    /// <summary>
    ///   Finds the first <see cref="Format" /> of type <typeparamref name="T" /> in this document starting from position
    ///   <paramref name="start" />,
    ///   returning <see langword="null" /> if nothing can be found past this position.
    /// </summary>
    /// <param name="start">The position to start searching, which must be positive.</param>
    /// <typeparam name="T">The type of <see cref="Format" /> that must be found.</typeparam>
    /// <returns>
    ///   The first <see cref="Format" /> of type <typeparamref name="T" /> found or <see langword="null" /> if this does
    ///   not exist.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="start" /> is negative.</exception>
    public T GetFirstFormat<T>(int start) where T : Format
    {
      if (start < 0)
      {
        throw new ArgumentOutOfRangeException(nameof(start), "Start cannot be negative");
      }

      var opIndex = 0;
      var pos = 0;

      while (pos <= start)
      {
        pos += Get(opIndex).Length;
        opIndex += 1;
      }

      opIndex -= 1;

      while (opIndex < Length)
      {
        var op = Get(opIndex);
        switch (op)
        {
          case TextInsert { Format: T t }:
            return t;
          case LineInsert { Format: T t }:
            return t;
          default:
            opIndex += 1;
            break;
        }
      }

      return null;
    }

    // ReSharper disable once RedundantOverriddenMember
    /// <summary>
    ///   Converts this document to a Markdown string.
    /// </summary>
    /// <returns>A Markdown string representing this document.</returns>
    public override string ToString()
    {
      return base.ToString();
    }
  }
}
