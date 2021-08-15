namespace Markdraw.Delta.Indents
{
  public abstract record Indent
  {
    public static QuoteIndent Quote => new QuoteIndent();
    public static CodeIndent Code => new CodeIndent();
    public static ContinueIndent Continue => new ContinueIndent();
    public static BulletIndent LooseBullet => Bullet();
    public static NumberIndent LooseNumber => Number();

    public static BulletIndent Bullet(bool start = true, bool loose = false)
    {
      return new BulletIndent {
        Start = start,
        Loose = loose
      };
    }

    public static NumberIndent Number(int start = 1, bool loose = false)
    {
      return new NumberIndent {
        Start = start,
        Loose = loose
      };
    }
  }

}
