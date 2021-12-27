namespace Markdraw.Delta.Indents;

public record BulletIndent(bool Start = false, bool Loose = true) : ListIndent(Loose)
{
  public override string ToString()
  {
    return "-";
  }
}
