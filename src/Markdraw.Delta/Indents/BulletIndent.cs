namespace Markdraw.Delta.Indents
{
  public record BulletIndent(bool Start = false, bool Loose = false) : ListIndent(Loose)
  {
    public override BulletIndent NextIndent => this with {
      Start = false
    };

    public override string ToString()
    {
      return "-";
    }
  }
}
