namespace Markdraw.Delta.Indents;

public abstract record Indent
{
  public static readonly QuoteIndent Quote = new(true);
  public static readonly ContinueIndent Continue = new();
  public static readonly BulletIndent LooseBullet = new(true);
  public static readonly NumberIndent LooseNumber = new(1);

  public static NumberIndent Number(int start = 1, bool loose = true)
  {
    return new NumberIndent(start, loose);
  }

  public static BulletIndent Bullet(bool start = true, bool loose = true)
  {
    return new BulletIndent(start, loose);
  }

  public abstract string GetMarkdown();
}
