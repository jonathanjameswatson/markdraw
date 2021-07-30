namespace Markdraw.Delta
{
  public class Delete : LengthOp
  {
    public Delete(int length) : base(length) {}

    public override bool Equals(object obj)
    {
      return obj is Delete x && x.Length == Length;
    }

    public override int GetHashCode()
    {
      return Length.GetHashCode();
    }

    public override string ToString()
    {
      return $"[DELETE {Length}]";
    }
  }
}
