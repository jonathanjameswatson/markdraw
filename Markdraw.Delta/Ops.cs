using System;
using System.Collections.Generic;

namespace Markdraw.Delta
{
  public class Ops : IEnumerable<IOp>
  {
    private List<IOp> _ops;

    public int Length { get => _ops.Count; }

    public int Characters
    {
      get
      {
        int characters = 0;
        foreach (var op in this)
        {
          characters += op.Length;
        }
        return characters;
      }
    }

    public Ops()
    {
      _ops = new List<IOp>();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }


    public IEnumerator<IOp> GetEnumerator()
    {
      return _ops.GetEnumerator();
    }

    private IOp Pop()
    {
      int n = Length - 1;
      var last = _ops[n];
      _ops.RemoveAt(n);
      return last;
    }

    private IOp Peek()
    {
      if (Length == 0)
      {
        throw new InvalidOperationException("No elements to peek at");
      }

      return _ops[Length - 1];
    }

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

    public Ops Insert(Insert insert)
    {
      _ops.Add(insert);
      MergeBack(Length - 1);
      return this;
    }

    public Ops Insert(string text, TextFormat format)
    {
      return Insert(new TextInsert(text, format));
    }

    public Ops Insert(string text)
    {
      return Insert(new TextInsert(text));
    }

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

    public Ops Delete(int amount)
    {
      _ops.Add(new Delete(amount));
      return this;
    }

    public Ops Retain(int amount)
    {
      _ops.Add(new Retain(amount));
      return this;
    }

    public Ops Retain(int amount, Format format)
    {
      _ops.Add(new Retain(amount, format));
      return this;
    }

    public int? MergeBack(int index)
    {
      if (index >= 1 && index < Length && _ops[index] is TextInsert after && _ops[index - 1] is TextInsert before)
      {
        int beforeLength = before.Length;
        var merged = after.Merge(before);
        if (merged is not null)
        {
          _ops.RemoveAt(index);
          return beforeLength;
        }
      }
      return null;
    }

    public Ops Transform(Ops other)
    {
      int opIndex = 0;
      int opCharacterIndex = 0;

      foreach (var op in other)
      {
        int length = op.Length;

        if (op is Retain retain)
        {
          var format = retain.Format;
          bool shouldFormat = format is not null;

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
              int lengthRemaining = nextInsert.Length - opCharacterIndex;
              int advanced = Math.Min(lengthRemaining, length);

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
                  int? beforeLength = MergeBack(opIndex);
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
            int? beforeLength = MergeBack(opIndex);
            if (beforeLength is not null)
            {
              opCharacterIndex += (int)beforeLength;
              opIndex -= 1;
            }
          }
        }
        else if (op is Delete delete)
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

                int lengthRemaining = nextTextInsert.Length - opCharacterIndex;
                int toDelete = Math.Min(lengthRemaining, length);
                bool deleted = nextTextInsert.DeleteUpTo(toDelete);
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

            int? beforeLength = MergeBack(opIndex);
            if (beforeLength is not null)
            {
              opIndex -= 1;
              opCharacterIndex += (int)beforeLength;
            }
          }
        }
        else if (op is Insert insert)
        {
          if (opCharacterIndex == 0)
          {
            InsertOp(opIndex, op);

            if (opIndex == Length)
            {
              MergeBack(Length - 1);
            }
            else
            {
              int? beforeLength = MergeBack(opIndex);
              if (beforeLength is not null)
              {
                opIndex -= 1;
                opCharacterIndex += (int)beforeLength;
              }
            }

            opIndex += 1;
          }
          else
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
                int beforeAndMiddleLength = before.Length + middle.Length;
                var merged = after.Merge(middle, before);
                if (!(merged is null))
                {
                  _ops.RemoveAt(opIndex);
                  _ops.RemoveAt(opIndex - 1);
                  opIndex -= 2;
                  opCharacterIndex += beforeAndMiddleLength;
                }
              }
            }
          }
        }
      }

      return this;
    }
  }
}