using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Markdraw.Delta.Formats;
using Markdraw.Delta.Operations;
using Markdraw.Delta.Operations.Inserts;

namespace Markdraw.Delta.Ops
{

  public abstract class Ops<T, TSelf> : IOps<T> where T : IOp where TSelf : Ops<T, TSelf>
  {
    private readonly List<T> _ops = new();

    /// <summary>
    ///   The number of operations stored.
    /// </summary>
    public int Length => _ops.Count;

    /// <inheritdoc />
    public IEnumerator<T> GetEnumerator()
    {
      return _ops.GetEnumerator();
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    /// <summary>
    ///   Inserts <paramref name="op" /> as the <paramref name="index" />th operation in this sequence.
    ///   If <paramref name="index" /> is the number of operations or greater, the operation is added to the end.
    /// </summary>
    /// <param name="index">The index at which the operation is inserted.</param>
    /// <param name="op">The operation that is inserted.</param>
    protected void InsertOp(int index, T op)
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
    ///   Appends <paramref name="op" /> to this sequence of operations without attempting to merge it.
    /// </summary>
    /// <param name="op">The operation that is added.</param>
    protected void Add(T op)
    {
      _ops.Add(op);
    }

    /// <summary>
    ///   Removes the <paramref name="index" />th operation in this sequence.
    /// </summary>
    /// <param name="index">The index at which the operation is removed.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///   Thrown if <paramref name="index"/> is less than 0 or the number of operations or greater.
    /// </exception>
    protected void RemoveAt(int index)
    {
      _ops.RemoveAt(index);
    }

    /// <summary>
    ///   Gets the <paramref name="index" />th operation in this sequence.
    /// </summary>
    /// <param name="index">The index at which the returned operation is found.</param>
    /// <returns>The <paramref name="index" />th operation in this sequence.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///   Thrown if <paramref name="index"/> is less than 0 or the number of operations or greater.
    /// </exception>
    protected T Get(int index)
    {
      return _ops[index];
    }

    /// <summary>
    ///   If <paramref name="index" /> is the index of any <see cref="TextInsert" /> that is not the first operation in
    ///   this sequence of operations, then this method will merge it with a <see cref="TextInsert" /> behind it with the
    ///   same format if one exists.
    /// </summary>
    /// <param name="index">The index at which a <see cref="TextInsert" /> should be attempted to be merged back.</param>
    /// <returns></returns>
    protected int? MergeBack(int index)
    {
      if (index < 1 || index >= Length || _ops[index] is not TextInsert after ||
        _ops[index - 1] is not TextInsert before) return null;
      var beforeLength = before.Length;
      var merged = after.Merge(before);
      if (merged is null) return null;
      RemoveAt(index);
      return beforeLength;
    }

    /// <summary>
    ///   Adds a <see cref="Operations.Inserts.Insert" /> to the end of this sequence of operations, which is then normalised.
    /// </summary>
    /// <param name="insert">The insert to be added.</param>
    /// <returns></returns>
    public abstract TSelf Insert(Insert insert);
    /*
    {
      _ops.Add(insert);
      MergeBack(Length - 1);
      return this;
    }
    */

    /// <summary>
    ///   Creates a <see cref="TextInsert" /> given its contents and format and adds it to the end of this sequence of
    ///   operations, which is then normalised.
    /// </summary>
    /// <param name="text">A string of the contents of the text added to this sequence.</param>
    /// <param name="format">The format of the text added to this sequence.</param>
    /// <returns></returns>
    public TSelf Insert(string text, TextFormat format=null)
    {
      return Insert(new TextInsert(text, format ?? new TextFormat()));
    }

    /// <summary>
    ///   Appends a document to this sequence of operations, which is returned.
    /// </summary>
    /// <param name="inserts">A document.</param>
    /// <returns>This sequence of operations.</returns>
    public TSelf InsertMany(Document inserts)
    {
      foreach (var insert in inserts)
      {
        Insert(insert);
      }

      return this as TSelf;
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
