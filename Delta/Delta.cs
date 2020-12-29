using System;
using System.Collections.Generic;

namespace Markdraw.Delta
{
  public class Delta
  {
    private List<Op> _ops;

    public Delta()
    {
      this._ops = new List<Op>();
    }

    private Op pop()
    {
      int n = this._ops.Count - 1;
      var last = this._ops[n];
      this._ops.RemoveAt(n);
      return last;
    }

    private Op peek()
    {
      return this._ops[this._ops.Count - 1];
    }

    public Delta insert(Insert insert)
    {
      this._ops.Add(insert);
      return this.normalise();
    }

    public Delta insert(string text, TextFormat format)
    {
      return insert(new TextInsert(text, format));
    }

    public Delta insert(string text)
    {
      return insert(new TextInsert(text));
    }

    public Delta delete(int amount)
    {
      this._ops.Add(new Delete(amount));
      return this.normalise();
    }

    public Delta retain(int amount)
    {
      this._ops.Add(new Retain(amount));
      return this;
    }

    public Delta retain(int amount, Format format)
    {
      this._ops.Add(new Retain(amount, format));
      return this;
    }

    private Delta normalise()
    {
      var last = peek();
      int n = this._ops.Count - 1;


      if (last is Delete delete)
      {
        int toDelete = delete.length;

        this._ops.RemoveAt(n);

        while (peek() is Insert insert && toDelete > 0)
        {
          (int subtracted, bool deleted) = insert.subtract(toDelete);

          if (deleted)
          {
            this._ops.RemoveAt(this._ops.Count - 1);
          }

          toDelete -= subtracted;
        }

        if (toDelete > 0)
        {
          this._ops.Add(new Delete(toDelete));
        }
      }
      else if (last is TextInsert after && n > 1 && this._ops[n - 1] is TextInsert before)
      {
        var merged = after.merge(before);
        if (!(merged is null))
        {
          pop();
        }
      }

      return this;
    }

    public Delta transform(Delta other)
    {
      int opIndex = 0;
      int opCharacterIndex = 0;

      foreach (var op in other._ops)
      {
        int length = op.length;

        if (op is Retain retain)
        {
          while (length > 0)
          {
            var next = _ops[opIndex];
            int lengthRemaining = next.length - opCharacterIndex;
            int advanced = Math.Min(lengthRemaining, length);
            opCharacterIndex = (opIndex + advanced) % next.length;
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
                int lengthRemaining = next.length - opCharacterIndex;
                int toDelete = Math.Min(lengthRemaining, length);
                bool deleted = nextTextInsert.deleteAt(opCharacterIndex, toDelete);

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
              int beforeLength = before.length;
              var merged = after.merge(before);
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
              int beforeLength = before.length;
              var merged = after.merge(before);
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
              var after = before.splitAt(opCharacterIndex);
              _ops.Insert(opIndex + 1, op);
              _ops.Insert(opIndex + 2, after);
              opIndex += 2;
              opCharacterIndex = 0;

              if (op is TextInsert middle)
              {
                int beforeAndMiddleLength = before.length + middle.length;
                var merged = after.merge(middle, before);
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