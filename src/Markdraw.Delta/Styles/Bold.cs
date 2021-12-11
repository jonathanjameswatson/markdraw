namespace Markdraw.Delta.Styles
{
  public record Bold : Style
  {
    public override string Wrap(string contents)
    {
      return $"**{contents}**";
    }

    public override string ToString()
    {
      return "<Bold>";
    }
  }
}
