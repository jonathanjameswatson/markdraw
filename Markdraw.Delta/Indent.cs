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

  public record Indent
  {

    private Indent(IndentType indentType, int length = 1)
    {
      Type = indentType;
      Length = length;
    }
    public IndentType Type { get; }
    public int Length { get; }

    public static Indent Quote => new(IndentType.Quote);
    public static Indent Bullet => new(IndentType.Bullet);
    public static Indent Code => new(IndentType.Code);

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
  }
}
