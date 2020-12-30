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

    private Ops Normalise()
    {
      var last = Peek();
      int n = Length - 1;


      if (last is Delete delete)
      {
        int toDelete = delete.Length;

        _ops.RemoveAt(n);

        while (Peek() is Insert insert && toDelete > 0)
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
      else if (last is TextInsert after && n > 0 && _ops[n - 1] is TextInsert before)
      {
        var merged = after.Merge(before);
        if (!(merged is null))
        {
          Pop();
        }
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
          while (length > 0)
          {
            var next = _ops[opIndex];
            int lengthRemaining = next.Length - opCharacterIndex;
            int advanced = Math.Min(lengthRemaining, length);
            opCharacterIndex = (opIndex + advanced) % next.Length;
            opIndex += advanced;
            length -= advanced;
            if (opCharacterIndex == 0)
            {
              opIndex += 1;
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
              throw new Exception();
            }

            if (_ops[opIndex] is TextInsert after && opIndex >= 1 && _ops[opIndex - 1] is TextInsert before)
            {
              int beforeLength = before.Length;
              var merged = after.Merge(before);
              if (!(merged is null))
              {
                _ops.RemoveAt(opIndex);
                opIndex -= 1;
                opCharacterIndex += beforeLength;
              }
            }
          }
        }
        else if (op is Insert insert)
        {
          if (opCharacterIndex == 0)
          {
            _ops.Insert(opIndex, op);
            opIndex += 1;

            if (_ops[opIndex] is TextInsert after && opIndex >= 1 && _ops[opIndex - 1] is TextInsert before)
            {
              int beforeLength = before.Length;
              var merged = after.Merge(before);
              if (!(merged is null))
              {
                _ops.RemoveAt(opIndex);
                opIndex -= 1;
                opCharacterIndex += beforeLength;
              }
            }
          }
          else
          {
            if (_ops[opIndex] is TextInsert before)
            {
              var after = before.SplitAt(opCharacterIndex);
              _ops.Insert(opIndex + 1, op);
              _ops.Insert(opIndex + 2, after);
              opIndex += 2;
              opCharacterIndex = 0;

              if (op is TextInsert middle)
              {
                int beforeAndMiddleLength = before.Length + middle.Length;
                var merged = after.Merge(middle, before);
                if (!(merged is null))
                {
                  _ops.RemoveAt(opIndex);
                  _ops.RemoveAt(opIndex);
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