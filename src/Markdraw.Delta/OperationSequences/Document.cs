using Markdraw.Delta.Formats;
using Markdraw.Delta.Operations;
using Markdraw.Delta.Operations.Inserts;
using Markdraw.Delta.Operations.Inserts.Inlines;

namespace Markdraw.Delta.OperationSequences;

internal abstract record TransformState(int InsertIndex);

internal abstract record InsertTransformState(int InsertIndex, IInsert Insert) : TransformState(InsertIndex);

internal record AtomTransformState(int InsertIndex, IInsert Insert) : InsertTransformState(InsertIndex, Insert);

internal record SplittableTransformState(int InsertIndex, int CharacterIndex, ISplittableInsert SplittableInsert) : InsertTransformState(InsertIndex,
  SplittableInsert);

internal record FinalTransformState(int InsertIndex) : TransformState(InsertIndex);

internal record SplitResult(SplittableTransformState CurrentState, ISplittableInsert Before);

/// <summary>A sequence of <see cref="IInsert" />s representing a Markdown document.</summary>
/// <remarks>Each unique document is represented by one canonical sequence of inserts.</remarks>
public class Document : OperationSequence<IInsert, Document>
{
  /// <summary>The number of characters in the document.</summary>
  /// <remarks>Certain inserts result in one character, such as dividers and code blocks.</remarks>
  public int Characters => this.Sum(insert => insert.Length);

  /// <summary>Transforms this document with a transformation.</summary>
  /// <param name="transformation">A transformation.</param>
  /// <returns>This document.</returns>
  public Document Transform(IEnumerable<IOp> transformation)
  {
    TransformState GetTransformState(int insertIndex)
    {
      if (insertIndex == Length)
      {
        return new FinalTransformState(insertIndex);
      }

      return Get(insertIndex) switch {
        ISplittableInsert splittableInsert => new SplittableTransformState(insertIndex, 0, splittableInsert),
        var insert => new AtomTransformState(insertIndex, insert)
      };
    }

    var transformState = GetTransformState(0);

    SplitResult SplitAtCharacterIndex(SplittableTransformState splittableTransformState, int characterIndex)
    {
      var (opIndex, _, splittableInsert) = splittableTransformState;
      switch (splittableInsert.SplitAt(characterIndex))
      {
        case var (newBefore, after):
          Set(opIndex, newBefore);
          InsertOp(opIndex + 1, after);
          var newSplittableTransformState = new SplittableTransformState(opIndex + 1, 0, after);
          transformState = newSplittableTransformState;
          return new SplitResult(newSplittableTransformState, newBefore);
        default:
          throw new InvalidOperationException("Text insert couldn't be split.");
      }
    }

    SplittableTransformState? TryMergeBack(SplittableTransformState splittableTransformState)
    {
      var (opIndex, _, _) = splittableTransformState;
      switch (MergeBack(opIndex))
      {
        case null:
          return null;
        case var beforeLength:
          var previous = Get(opIndex - 1) switch {
            ISplittableInsert splittableInsert => splittableInsert,
            _ => throw new InvalidOperationException("The previous insert must be splittable if a merge back succeeds.")
          };
          var newSplittableTransformState = new SplittableTransformState(opIndex - 1, (int)beforeLength,
            previous);
          transformState = newSplittableTransformState;
          return newSplittableTransformState;
      }
    }

    foreach (var op in transformation)
    {
      switch (op)
      {
        case Retain (var formatModifier, var length):
        {
          if (formatModifier is not null
              && transformState is SplittableTransformState(_, var initialCharacterIndex and > 0, _)
                initialSplittableTransformState)
          {
            SplitAtCharacterIndex(initialSplittableTransformState, initialCharacterIndex);
          }

          var remainingRetainLength = length;
          while (remainingRetainLength > 0)
          {
            switch (transformState)
            {
              case FinalTransformState:
                throw new InvalidOperationException("Cannot retain past the end of a document.");
              case InsertTransformState insertTransformState:
                var (currentInsertIndex, currentInsert) = insertTransformState;

                var maximumProgress = currentInsert.Length;
                if (transformState is SplittableTransformState(_, var currentCharacterIndex, _)
                    splittableTransformState)
                {
                  maximumProgress -= currentCharacterIndex;
                  if (formatModifier is not null
                      && currentCharacterIndex + remainingRetainLength < currentInsert.Length)
                  {
                    (_, currentInsert) = SplitAtCharacterIndex(splittableTransformState,
                      currentCharacterIndex + remainingRetainLength);
                    // may need to update transform state here later
                  }
                }

                var progress = Math.Min(maximumProgress, remainingRetainLength);
                remainingRetainLength -= progress;

                if (formatModifier is not null)
                {
                  var newInsert = currentInsert.SetFormat(formatModifier);
                  if (newInsert is not null)
                  {
                    currentInsert = newInsert;
                    Set(currentInsertIndex, currentInsert);
                  }

                  if (currentInsertIndex >= 1 && MergeBack(currentInsertIndex) is not null)
                  {
                    currentInsertIndex -= 1;
                  }
                }

                transformState = transformState switch {
                  SplittableTransformState(_, var characterIndex, var splittableInsert) currentSplittableTransformState when
                    characterIndex + progress < splittableInsert.Length => currentSplittableTransformState with {
                      CharacterIndex = characterIndex + progress
                    },
                  _ => GetTransformState(currentInsertIndex + 1)
                };

                break;
            }
          }

          if (formatModifier is not null
              && transformState is SplittableTransformState(> 0, _, _) finalSplittableTransformState)
          {
            TryMergeBack(finalSplittableTransformState);
          }

          break;
        }
        case Delete (var length):
        {
          var remainingDeleteLength = length;
          while (remainingDeleteLength > 0)
          {
            switch (transformState)
            {
              case FinalTransformState:
                throw new InvalidOperationException("Cannot delete past the end of a document.");
              case SplittableTransformState(var currentInsertIndex, var currentCharacterIndex, var currentSplittableInsert)
                splittableTransformState:
                if (currentCharacterIndex > 0)
                {
                  (splittableTransformState, _) = SplitAtCharacterIndex(splittableTransformState,
                    currentCharacterIndex);
                  (currentInsertIndex, _, currentSplittableInsert) = splittableTransformState;
                }

                var amountDeleted = Math.Min(currentSplittableInsert.Length, remainingDeleteLength);
                remainingDeleteLength -= amountDeleted;

                switch (currentSplittableInsert.DeleteUpTo(amountDeleted))
                {
                  case null:
                    RemoveAt(currentInsertIndex);
                    transformState = GetTransformState(currentInsertIndex);
                    break;
                  case var newSplittableInsert:
                    Set(currentInsertIndex, newSplittableInsert);
                    currentSplittableInsert = newSplittableInsert;
                    transformState = splittableTransformState with {
                      Insert = currentSplittableInsert
                    };
                    break;
                }
                break;
              case AtomTransformState(var currentInsertIndex, _):
                remainingDeleteLength -= 1;
                RemoveAt(currentInsertIndex);
                transformState = GetTransformState(currentInsertIndex);
                break;
            }
          }

          if (transformState is SplittableTransformState(> 0, _, _) finalSplittableTransformState)
          {
            TryMergeBack(finalSplittableTransformState);
          }

          break;
        }
        case IInsert insert
          when transformState is SplittableTransformState(_, > 0 and var currentCharacterIndex, _)
            splittableTransformState:
        {
          var ((currentInsertIndex, _, after), newBefore) = SplitAtCharacterIndex(splittableTransformState,
            currentCharacterIndex);
          InsertOp(currentInsertIndex, insert);
          currentInsertIndex += 1;
          transformState = GetTransformState(currentInsertIndex);

          if (insert is ISplittableInsert middle)
          {
            var merged = after.Merge(middle, newBefore);
            if (merged is not null)
            {
              RemoveAt(currentInsertIndex);
              RemoveAt(currentInsertIndex - 1);
              currentInsertIndex -= 2;
              Set(currentInsertIndex, merged);
              transformState = new SplittableTransformState(currentInsertIndex, newBefore.Length + middle.Length,
                merged);
            }
          }

          break;
        }
        case IInsert insert:
        {
          var currentInsertIndex = transformState.InsertIndex;

          InsertOp(currentInsertIndex, insert);
          transformState = GetTransformState(currentInsertIndex);

          if (transformState is SplittableTransformState splittableTransformState)
          {
            switch (TryMergeBack(splittableTransformState))
            {
              case null:
                break;
              case var (newInsertIndex, _, _):
                currentInsertIndex = newInsertIndex;
                break;
            }
          }

          currentInsertIndex += 1;
          transformState = GetTransformState(currentInsertIndex);

          if (transformState is SplittableTransformState finalSplittableTransformState)
          {
            TryMergeBack(finalSplittableTransformState);
          }

          break;
        }
      }
    }

    return this;
  }

  /// <summary>
  ///   Finds the first <see cref="Format" /> of type <typeparamref name="T" /> in this document starting from
  ///   position <paramref name="start" />, returning <see langword="null" /> if nothing can be found past this position.
  /// </summary>
  /// <param name="start">The position to start searching, which must be positive.</param>
  /// <typeparam name="T">The type of <see cref="Format" /> that must be found.</typeparam>
  /// <returns>
  ///   The first <see cref="Format" /> of type <typeparamref name="T" /> found or <see langword="null" /> if this
  ///   does not exist.
  /// </returns>
  /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="start" /> is negative.</exception>
  public T? GetFirstFormat<T>(int start) where T : Format
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
        case InlineInsert { Format: T t }:
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
  /// <summary>Converts this document to a Markdown string.</summary>
  /// <returns>A Markdown string representing this document.</returns>
  public override string ToString()
  {
    return base.ToString();
  }
}
