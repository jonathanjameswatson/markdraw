using System;
using System.Collections.Generic;

namespace Markdraw.Delta
{
  public class Ops : IEnumerable<IOp>
  {
    private List<IOp> _ops;

    public int Length { get => _ops.Count; }

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
      return Normalise();
    }

    public Ops Insert(string text, TextFormat format)
    {
      return Insert(new TextInsert(text, format));
    }

    public Ops Insert(string text)
    {
      return Insert(new TextInsert(text));
    }

    public Ops Delete(int amount)
    {
      _ops.Add(new Delete(amount));
      return Normalise();
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
      if (_ops[index] is TextInsert after && index >= 1 && _ops[index - 1] is TextInsert before)
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

    private Ops Normalise()
    {
      var last = Peek();
      int n = Length - 1;


      if (last is Delete delete)
      {
        int toDelete = delete.Length;

        _ops.RemoveAt(n);

        while (toDelete > 0 && Peek() is Insert insert)
        {
          (int subtracted, bool deleted) = insert.Subtract(toDelete);

          if (deleted)
          {
            _ops.RemoveAt(Length - 1);
          }

          toDelete -= subtracted;
        }

        if (toDelete > 0)
        {
          _ops.Add(new Delete(toDelete));
        }
      }
      else
      {
        MergeBack(n);
      }

      return this;
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

              var toFormat = nextInsert;
              bool extraMerge = false;

              if (opCharacterIndex != 0)
              {
                var textInsert = toFormat as TextInsert;
                var after = textInsert.SplitAt(opCharacterIndex);
                opCharacterIndex = 0;
                InsertOp(opIndex + 1, after);
                toFormat = textInsert;
                extraMerge = true;
              }

              if (shouldFormat)
              {
                toFormat.SetFormat(format);
                MergeBack(opIndex);
              }

              opIndex += 1;

              if (extraMerge)
              {
                MergeBack(opIndex);
              }
            }
            else
            {
              throw new InvalidOperationException("Only a list of inserts should be transformed.");
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
                int lengthRemaining = next.Length - opCharacterIndex;
                int toDelete = Math.Min(lengthRemaining, length);
                bool deleted = nextTextInsert.DeleteAt(opCharacterIndex, toDelete);

                if (deleted)
                {
                  _ops.RemoveAt(opIndex);
                }
                else
                {
                  opIndex += 1;
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
            opIndex += 1;
            if (opIndex == Length)
            {
              Normalise();
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