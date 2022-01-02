﻿using System.Collections;
using System.Text;
using Markdraw.Delta.Formats;
using Markdraw.Delta.Operations;
using Markdraw.Delta.Operations.Inserts;
using Markdraw.Delta.Operations.Inserts.Inlines;

namespace Markdraw.Delta.OperationSequences;

/// <summary>
///   A sequence of operations that can be extended to represent a Markdown document or transformation. This
///   sequence may only contain elements of type <typeparamref name="T" />.
/// </summary>
/// <remarks><typeparamref name="TSelf" /> should be the derived class. It is used in chaining.</remarks>
/// <typeparam name="T">A class that all elements in this sequence must be or extend.</typeparam>
/// <typeparam name="TSelf">The class of instances returned by methods that use chaining.</typeparam>
public abstract class OperationSequence<T, TSelf> : IEnumerable<T>
  where T : class, IOp where TSelf : OperationSequence<T, TSelf>
{
  private readonly List<T> _ops = new();

  /// <summary>The number of operations stored.</summary>
  public int Length => _ops.Count;

  /// <inheritdoc cref="IEnumerable{T}.GetEnumerator()" />
  public IEnumerator<T> GetEnumerator()
  {
    return _ops.GetEnumerator();
  }

  /// <inheritdoc cref="IEnumerable.GetEnumerator()" />
  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }

  /// <summary>
  ///   Inserts <paramref name="op" /> as the <paramref name="index" />th operation in this sequence. If
  ///   <paramref name="index" /> is the number of operations or greater, the operation is added to the end.
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

  /// <summary>Appends <paramref name="op" /> to this sequence of operations without attempting to merge it.</summary>
  /// <param name="op">The operation that is added.</param>
  protected void Add(T op)
  {
    _ops.Add(op);
  }

  /// <summary>Removes the <paramref name="index" />th operation in this sequence.</summary>
  /// <param name="index">The index at which the operation is removed.</param>
  /// <exception cref="ArgumentOutOfRangeException">
  ///   Thrown if <paramref name="index" /> is less than 0 or the number of
  ///   operations or greater.
  /// </exception>
  protected void RemoveAt(int index)
  {
    _ops.RemoveAt(index);
  }

  /// <summary>Gets the <paramref name="index" />th operation in this sequence.</summary>
  /// <param name="index">The index at which the returned operation is found.</param>
  /// <returns>The <paramref name="index" />th operation in this sequence.</returns>
  /// <exception cref="ArgumentOutOfRangeException">
  ///   Thrown if <paramref name="index" /> is less than 0 or the number of
  ///   operations or greater.
  /// </exception>
  protected T Get(int index)
  {
    return _ops[index];
  }

  /// <summary>Sets the <paramref name="index" />th operation in this sequence to operation <paramref name="op" />.</summary>
  /// <param name="index">The index at which the returned operation is found.</param>
  /// <param name="op">The operation that is put in the sequence.</param>
  /// <returns>The <paramref name="index" />th operation in this sequence.</returns>
  /// <exception cref="ArgumentOutOfRangeException">
  ///   Thrown if <paramref name="index" /> is less than 0 or the number of
  ///   operations or greater.
  /// </exception>
  protected void Set(int index, T op)
  {
    _ops[index] = op;
  }

  /// <summary>
  ///   If <paramref name="index" /> is the index of any <see cref="TextInsert" /> that is not the first operation in
  ///   this sequence of operations, then this method will merge it with a <see cref="TextInsert" /> behind it with the same
  ///   format if one exists.
  /// </summary>
  /// <param name="index">The index at which a <see cref="TextInsert" /> should be attempted to be merged back.</param>
  /// <returns>
  ///   Returns <see langword="null" /> if a merge doesn't occur or the previous length of the
  ///   <see cref="TextInsert" /> behind what was the <paramref name="index" />th operation in this sequence of operations
  ///   otherwise.
  /// </returns>
  protected int? MergeBack(int index)
  {
    if (index < 1 || index >= Length || Get(index) is not ISplittableInsert after
        || Get(index - 1) is not ISplittableInsert before) return null;
    var beforeLength = before.Length;
    var merged = after.Merge(before);
    if (merged is null) return null;
    Set(index - 1, (merged as T)!);
    RemoveAt(index);
    return beforeLength;
  }

  /// <summary>Adds a <see cref="IInsert" /> to the end of this sequence of operations, which is then normalised.</summary>
  /// <param name="insert">The insert to be added.</param>
  /// <returns>This sequence of operations.</returns>
  /// <exception cref="InvalidOperationException">
  ///   This method must not be ran on classes where <see cref="IInsert" /> does
  ///   not extend <typeparamref name="T" />.
  /// </exception>
  public TSelf Insert(IInsert insert)
  {
    if (insert is not T castedInsert)
    {
      throw new InvalidOperationException("Insert must be castable to T.");
    }
    Add(castedInsert);
    MergeBack(Length - 1);
    return (this as TSelf)!;
  }

  /// <summary>
  ///   Creates a <see cref="TextInsert" /> given its contents and format and adds it to the end of this sequence of
  ///   operations, which is then normalised.
  /// </summary>
  /// <param name="text">A string of the contents of the text added to this sequence.</param>
  /// <param name="format">The format of the text added to this sequence.</param>
  /// <returns>This sequence of operations.</returns>
  public TSelf Insert(string text, InlineFormat? format = null)
  {
    return Insert(new TextInsert(text, format ?? new InlineFormat()));
  }

  /// <summary>Appends a document to this sequence of operations, which is returned.</summary>
  /// <param name="inserts">A document.</param>
  /// <returns>This sequence of operations.</returns>
  public TSelf InsertMany(Document inserts)
  {
    return inserts.Aggregate((this as TSelf)!, (_, insert) => Insert(insert));
  }

  /// <summary>
  ///   Returns a representation of this operation sequence with inserts being represented by Markdown strings and
  ///   other operations being represented by their string forms.
  /// </summary>
  /// <returns>This instance's string representation.</returns>
  public override string ToString()
  {
    var stringBuilder = new StringBuilder();
    var lineBuffer = new StringBuilder();
    var indentStates = new List<IndentState>();
    var lastIndentCount = 0;

    foreach (var op in _ops)
    {
      if (op is LineInsert lineInsert)
      {
        stringBuilder.Append(lineInsert.LineInsertString(indentStates, lastIndentCount));
        stringBuilder.Append(lineBuffer);
        stringBuilder.Append('\n');
        stringBuilder.Append('\n');
        lineBuffer.Clear();
        lastIndentCount = lineInsert.Format.Indents.Count;
      }
      else
      {
        lineBuffer.Append(op);
      }
    }

    if (lineBuffer.Length != 0)
    {
      stringBuilder.Append(lineBuffer);
    }

    return stringBuilder.ToString();
  }
}
