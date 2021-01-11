using System;

namespace Markdraw.Delta
{
  public enum IndentType
  {
    Quote,
    Bullet,
    Number,
    Code,
    Empty
  }

  public class Indent
  {
    public IndentType Type { get; set; }
    public int Length { get; set; }

    public static Indent Quote { get => new Indent(IndentType.Quote); }
    public static Indent Bullet { get => new Indent(IndentType.Bullet); }
    public static Indent Code { get => new Indent(IndentType.Code); }

    private Indent(IndentType indentType, int length)
    {
      Type = indentType;
      Length = length;
    }

    private Indent(IndentType indentType) : this(indentType, 1) { }

    public static Indent Number(int length)
    {
      return new Indent(IndentType.Number, length);
    }

    public static Indent Empty(int length)
    {
      return new Indent(IndentType.Empty, length);
    }

    public bool IsNumber()
    {
      return Type == IndentType.Number;
    }

    public bool IsEmpty()
    {
      return Type == IndentType.Empty;
    }

    public override bool Equals(object obj)
    {
      return obj is Indent indent && indent.Type == Type && indent.Length == Length;
    }

    public override int GetHashCode()
    {
      return (Type, Length).GetHashCode();
    }

    public static bool operator ==(Indent lhs, Indent rhs)
    {
      if (Object.ReferenceEquals(lhs, null))
      {
        if (Object.ReferenceEquals(rhs, null))
        {
          // null == null = true.
          return true;
        }

        // Only the left side is null.
        return false;
      }

      // Equals handles case of null on right side.
      return lhs.Equals(rhs);
    }

    public static bool operator !=(Indent lhs, Indent rhs)
    {
      return !(lhs == rhs);
    }
  }
}