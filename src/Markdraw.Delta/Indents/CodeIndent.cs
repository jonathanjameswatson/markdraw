namespace Markdraw.Delta.Indents;

public record CodeIndent : Indent
{
  public override string ToString()
  {
    return "    ";
  }
}