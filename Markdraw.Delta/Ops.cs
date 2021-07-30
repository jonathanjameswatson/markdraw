using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdraw.Delta
{
  /// <summary>
  ///   A sequence of operations representing a Markdown document or transformation.
  /// </summary>
  /// <remarks>
  ///   Each unique document can only be represented by one canonical sequence of operations. However, transformations can
  ///   currently have different representations as deletes and retains are not merged when they are added.
  /// </remarks>
  public class Ops : IEnumerable<IOp>
  {
    private readonly List<IOp> _ops = new();

    /// <summary>
    ///   The number of operations stored.
    /// </summary>
    public int Length => _ops.Count;

    /// <summary>
    ///   The sum of lengths of operations intended to be used as the number of characters in a document.
    /// </summary>
    public int Characters => this.Sum(op => op.Length);

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    /// <inheritdoc />
    public IEnumerator<IOp> GetEnumerator()
    {
      return _ops.GetEnumerator();
    }

    /// <summary>
    ///   Inserts <paramref name="op" /> as the <paramref name="index" />th operation in this sequence.
    ///   If <paramref name="index" /> is the number of operations or greater, the operation is added to the end.
    /// </summary>
    /// <param name="index">The index at which the operation is inserted.</param>
    /// <param name="op">The operation that is inserted.</param>
    private void InsertOp(int index, IOp op)
    {
      if (index >= Length)
      {
        _ops.Add(op);
      }
      else
      {
        _ops.Insert(index, op);
      }
    }

    /// <summary>
    ///   Adds a <see cref="Delta.Insert" /> to the end of this sequence of operations, which is then normalised.
    /// </summary>
    /// <param name="insert">The insert to be added.</param>
    /// <returns></returns>
    public Ops Insert(Insert insert)
    {
      _ops.Add(insert);
      MergeBack(Length - 1);
      return this;
    }

    /// <summary>
    ///   Creates a <see cref="TextInsert" /> given its contents and format and adds it to the end of this sequence of
    ///   operations, which is then normalised.
    /// </summary>
    /// <param name="text">A string of the contents of the text added to this sequence.</param>
    /// <param name="format">The format of the text added to this sequence.</param>
    /// <returns></returns>
    public Ops Insert(string text, TextFormat format)
    {
      return Insert(new TextInsert(text, format));
    }

    /// <summary>
    ///   Creates a <see cref="TextInsert" /> given its contents and adds it to the end of this sequence of operations,
    ///   which is then normalised.
    /// </summary>
    /// <param name="text">A string of the contents of the text added to this sequence.</param>
    /// <returns></returns>
    public Ops Insert(string text)
    {
      return Insert(new TextInsert(text));
    }

    /// <summary>
    ///   Inserts a sequence of <see cref="Delta.Insert" />s to this sequence of operations, which is returned.
    /// </summary>
    /// <param name="inserts">Another sequence of operations that must only contain <see cref="Delta.Insert" />s.</param>
    /// <returns>This sequence of operations.</returns>
    /// <exception cref="ArgumentException">
    ///   Occurs if any non-<see cref="Delta.Insert" />s are found in <paramref name="inserts" />
    /// </exception>
    public Ops InsertMany(Ops inserts)
    {
      foreach (var op in inserts)
      {
        if (op is Insert insert)
        {
          Insert(insert);
        }
        else
        {
          throw new ArgumentException("Only an Ops of inserts can be inserted.");
        }
      }

      return this;
    }

    /// <summary>
    ///   Adds a <see cref="Delta.Delete" /> with a given length to the end of this sequence of operations.
    /// </summary>
    /// <param name="amount">The length of the <see cref="Delta.Delete" />.</param>
    /// <returns>This sequence of operations.</returns>
    public Ops Delete(int amount)
    {
      _ops.Add(new Delete(amount));
      return this;
    }

    /// <summary>
    ///   Adds a <see cref="Delta.Retain" /> with a given length and no formatting to the end of this sequence of operations.
    /// </summary>
    /// <param name="amount">The length of the <see cref="Delta.Retain" />.</param>
    /// <returns>This sequence of operations.</returns>
    public Ops Retain(int amount)
    {
      _ops.Add(new Retain(amount));
      return this;
    }

    /// <summary>
    ///   Adds a <see cref="Delta.Retain" /> with a given length and format to the end of this sequence of operations.
    /// </summary>
    /// <param name="amount">The length of the <see cref="Delta.Retain" />.</param>
    /// <param name="format">The format of the <see cref="Delta.Retain" />.</param>
    /// <returns>This sequence of operations.</returns>
    public Ops Retain(int amount, Format format)
    {
      _ops.Add(new Retain(amount, format));
      return this;
    }

    /// <summary>
    ///   If <paramref name="index" /> is the index of any <see cref="TextInsert" /> that is not the first operation in
    ///   this sequence of operations, then this method will merge it with a <see cref="TextInsert" /> behind it with the
    ///   same format if one exists.
    /// </summary>
    /// <param name="index">The index at which a <see cref="TextInsert" /> should be attempted to be merged back.</param>
    /// <returns></returns>
    private int? MergeBack(int index)
    {
      if (index < 1 || index >= Length || _ops[index] is not TextInsert after ||
        _ops[index - 1] is not TextInsert before) return null;
      var beforeLength = before.Length;
      var merged = after.Merge(before);
      if (merged is null) return null;
      _ops.RemoveAt(index);
      return beforeLength;
    }

    /// <summary>
    ///   Transforms this sequence of operations (assuming it represents a document) with another sequence of operations
    ///   <paramref name="other" /> representing a transformation.
    /// </summary>
    /// <param name="other">A sequence of operations representing a transformation.</param>
    /// <returns>This sequence of operations.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public Ops Transform(Ops other)
    {
      var opIndex = 0;
      var opCharacterIndex = 0;

      foreach (var op in other)
      {
        var length = op.Length;

        switch (op)
        {
          case Retain retain:
          {
            var format = retain.Format;
            var shouldFormat = format is not null;

            if (shouldFormat && opCharacterIndex != 0 && _ops[opIndex] is TextInsert before)
            {
              var after = before.SplitAt(opCharacterIndex);
              InsertOp(opIndex + 1, after);
              opIndex += 1;
              opCharacterIndex = 0;
            }

            while (length > 0)
            {
              var next = _ops[opIndex];
              if (next is Insert nextInsert)
              {
                var lengthRemaining = nextInsert.Length - opCharacterIndex;
                var advanced = Math.Min(lengthRemaining, length);

                opCharacterIndex = (opCharacterIndex + advanced) % nextInsert.Length;
                length -= advanced;

                if (opCharacterIndex != 0)
                {
                  if (shouldFormat)
                  {
                    var textInsert = nextInsert as TextInsert;
                    var after = textInsert.SplitAt(opCharacterIndex);
                    opCharacterIndex = 0;
                    InsertOp(opIndex + 1, after);
                  }
                }

                if (shouldFormat)
                {
                  nextInsert.SetFormat(format);

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
              else
              {
                throw new InvalidOperationException("Only a list of inserts should be transformed.");
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
          case Delete _:
          {
            while (length > 0)
            {
              var next = _ops[opIndex];
              if (next is Insert nextInsert)
              {
                if (nextInsert is TextInsert nextTextInsert)
                {
                  if (opCharacterIndex != 0)
                  {
                    var after = nextTextInsert.SplitAt(opCharacterIndex);
                    opCharacterIndex = 0;
                    opIndex += 1;
                    InsertOp(opIndex, after);
                    nextTextInsert = after;
                  }

                  var lengthRemaining = nextTextInsert.Length - opCharacterIndex;
                  var toDelete = Math.Min(lengthRemaining, length);
                  var deleted = nextTextInsert.DeleteUpTo(toDelete);
                  length -= toDelete;

                  if (deleted)
                  {
                    _ops.RemoveAt(opIndex);
                  }

                  opCharacterIndex = deleted ? 0 : opCharacterIndex;
                }
                else
                {
                  _ops.RemoveAt(opIndex);
                  length -= 1;
                }
              }
              else
              {
                throw new InvalidOperationException("Only a list of inserts should be transformed.");
              }

              var beforeLength = MergeBack(opIndex);
              if (beforeLength is null) continue;
              opIndex -= 1;
              opCharacterIndex += (int)beforeLength;
            }

            break;
          }
          case Insert _ when opCharacterIndex == 0:
          {
            InsertOp(opIndex, op);

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
          case Insert _:
          {
            if (_ops[opIndex] is TextInsert before)
            {
              var after = before.SplitAt(opCharacterIndex);
              InsertOp(opIndex + 1, op);
              InsertOp(opIndex + 2, after);
              opIndex += 2;
              opCharacterIndex = 0;

              if (op is TextInsert middle)
              {
                var beforeAndMiddleLength = before.Length + middle.Length;
                var merged = after.Merge(middle, before);
                if (merged is not null)
                {
                  _ops.RemoveAt(opIndex);
                  _ops.RemoveAt(opIndex - 1);
                  opIndex -= 2;
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
    ///   Finds the first <see cref="Format" /> of type <typeparamref name="T" /> in a sequence of operations representing
    ///   a document starting from position <paramref name="start" />, returning <see langword="null" /> if nothing can
    ///   be found past this position.
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
        pos += _ops[opIndex].Length;
        opIndex += 1;
      }

      opIndex -= 1;

      while (opIndex < _ops.Count)
      {
        var op = _ops[opIndex];
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

    /// <inheritdoc />
    public override string ToString()
    {
      var stringBuilder = new StringBuilder();
      var buffer = new StringBuilder();

      foreach (var op in _ops)
      {
        if (op is LineInsert lineInsert)
        {
          stringBuilder.Append(lineInsert.LineInsertString());
          stringBuilder.Append(buffer);
          stringBuilder.Append('\n');
          buffer.Clear();
        }
        else
        {
          buffer.Append(op);
        }
      }

      if (buffer.Length != 0)
      {
        stringBuilder.Append(buffer);
      }

      return stringBuilder.ToString();
    }
  }
}
