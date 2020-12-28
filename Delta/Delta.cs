
using System.Collections.Generic;

namespace Markdraw.Delta
{
  public class Delta
  {
    private List<Op> _ops;

    private Op pop()
    {
      int n = this._ops.Count - 1;
      Op last = this._ops[n];
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
      Op last = peek();
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
        TextInsert merged = after.merge(before);
        if (!(merged is null))
        {
          pop();
          pop();
          this._ops.Add(merged);
        }
      }

      return this;
    }
  }
}