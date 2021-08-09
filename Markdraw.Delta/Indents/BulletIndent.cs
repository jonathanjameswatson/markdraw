namespace Markdraw.Delta.Indents
{
  public record BulletIndent : ListIndent
  {
    public bool Start { get; init; } = false;
  };
}
