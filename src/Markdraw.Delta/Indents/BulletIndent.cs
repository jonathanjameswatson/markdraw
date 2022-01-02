using System.Text;

namespace Markdraw.Delta.Indents;

public record BulletIndent(bool Start = false, bool Loose = true) : ListIndent(Loose)
{
  public override string ToString()
  {
    var stringBuilder = new StringBuilder();
    stringBuilder.Append("||-");

    if (Start)
    {
      stringBuilder.Append(" START");
    }

    stringBuilder.Append(Loose ? " LOOSE||" : " TIGHT||");
    return stringBuilder.ToString();
  }

  public override string GetMarkdown()
  {
    return "-";
  }
}
