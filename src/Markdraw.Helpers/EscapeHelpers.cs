﻿using System.Linq;
using System.Text;
using Markdig.Helpers;

namespace Markdraw.Helpers
{
  public static class EscapeHelpers
  {
    public static string EscapeUrl(string url)
    {
      return string.Join("", url.Select(c => HtmlHelper.EscapeUrlCharacter(c) ?? c.ToString()));
    }

    public static string Escape(string content, bool softEscape = false)
    {
      var stringBuilder = new StringBuilder();
      var length = content.Length;

      foreach (var c in content)
      {
        switch (c)
        {
          case '<':
            stringBuilder.Append("&lt;");
            break;
          case '>':
            if (!softEscape)
            {
              stringBuilder.Append("&gt;");
            }
            else
            {
              stringBuilder.Append(c);
            }
            break;
          case '&':
            stringBuilder.Append("&amp;");
            break;
          case '"':
            if (!softEscape)
            {
              stringBuilder.Append("&quot;");
            }
            else
            {
              stringBuilder.Append(c);
            }
            break;
          default:
            stringBuilder.Append(c);
            break;
        }
      }

      return stringBuilder.ToString();
    }

  }
}