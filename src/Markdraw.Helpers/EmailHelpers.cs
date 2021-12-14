using System.Text.RegularExpressions;

namespace Markdraw.Helpers;

public static class EmailHelpers
{
  private static readonly Regex EmailRegex =
    new(
      @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$",
      RegexOptions.Compiled);

  public static bool IsEmail(string url)
  {
    return EmailRegex.IsMatch(url);
  }
}
