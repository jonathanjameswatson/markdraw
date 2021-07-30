using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

// Modified from https://github.com/xoofx/markdig/tree/master/src/SpecFileGen
namespace SpecFileGen
{
  internal class Program
  {
    private static readonly string ProgramDirectory =
      Path.GetFullPath(
        Path.Combine(
          Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
          "../../../"));

    private static readonly string specName = "CommonMark v. 0.29";
    private static readonly string fileName = "CommonMark.md";

    private static readonly StringBuilder StringBuilder = new(1 << 20);// 1 MB

    private static void Main()
    {
      var inputPath = Path.Combine(ProgramDirectory, "../Markdraw.Parser.Test") + "/" + fileName;
      var source = ParseSpecification(File.ReadAllText(inputPath)).Replace("\r\n", "\n", StringComparison.Ordinal);

      var outputPath = Path.ChangeExtension(inputPath, "generated.cs");
      File.WriteAllText(outputPath, source);
    }
    private static void Write(string text)
    {
      StringBuilder.Append(text);
    }
    private static void Line(string text = null)
    {
      if (text != null) StringBuilder.Append(text);
      StringBuilder.Append('\n');
    }
    private static void Indent(int count = 1)
    {
      StringBuilder.Append(new string(' ', 2 * count));
    }

    private static string ParseSpecification(string specSource)
    {
      Line();
      Write("// ");
      Line(new string('-', 32));
      Write("// ");
      Write(new string(' ', 16 - specName.Length / 2));
      Line(specName);
      Write("// ");
      Line(new string('-', 32));
      Line();
      Line("using System;");
      Line("using Xunit;");
      Line();
      Line("namespace Markdraw.Parser.Test");
      Line("{");

      var lines = specSource.Split(new[] {
        "\r\n", "\n"
      }, StringSplitOptions.None);

      var nameChanged = true;
      var name = "";
      var compressedName = "";
      var number = 0;
      int commentOffset = 0, commentEnd = 0, markdownOffset = 0, markdownEnd = 0, htmlOffset = 0, htmlEnd = 0;
      var first = true;
      LinkedList<(string Heading, string Compressed, int Level)> headings = new();
      var nameBuilder = new StringBuilder(64);

      var i = 0;
      while (i < lines.Length)
      {
        commentOffset = commentEnd = i;
        while (!lines[i].Equals("```````````````````````````````` example", StringComparison.Ordinal))
        {
          var line = lines[i];
          if (line.Length > 2 && line[0] == '#')
          {
            var level = line.IndexOf(' ', StringComparison.Ordinal);
            while (headings.Count != 0)
            {
              if (headings.Last.Value.Level < level) break;
              headings.RemoveLast();
            }
            var heading = line.Substring(level + 1);
            headings.AddLast((heading, CompressedName(heading), level));

            foreach (var (Heading, _, _) in headings)
              nameBuilder.Append(Heading + " / ");
            nameBuilder.Length -= 3;
            name = nameBuilder.ToString();
            nameBuilder.Length = 0;

            foreach (var (_, Compressed, _) in headings)
              nameBuilder.Append(Compressed);
            compressedName = nameBuilder.ToString();
            nameBuilder.Length = 0;

            nameChanged = true;
          }
          i++;

          if (!IsEmpty(line))
            commentEnd = i;

          if (i == lines.Length)
          {
            if (commentOffset != commentEnd)
            {
              while (commentOffset < commentEnd && IsEmpty(lines[commentOffset])) commentOffset++;
              for (i = commentOffset; i < commentEnd; i++)
              {
                var nextLine = lines[i];
                Indent(2);
                Write(nextLine == "" ? "//" : "// ");
                Line(nextLine);
              }
            }
            goto End;
          }
        }
        ;

        markdownOffset = ++i;
        while (!(lines[i].Length == 1 && lines[i][0] == '.')) i++;
        markdownEnd = i++;

        htmlOffset = i;
        while (!lines[i].Equals("````````````````````````````````", StringComparison.Ordinal)) i++;
        htmlEnd = i++;

        if (nameChanged)
        {
          if (!first)
          {
            Indent();
            Line("}");
            Line();
          }
          // Indent(); Line("[TestFixture]");
          Indent();
          Line("public class Test" + compressedName);
          Indent();
          Line("{");
          first = false;
          nameChanged = false;
        }
        else Line();

        WriteTest(name, compressedName, ++number, lines,
          commentOffset, commentEnd,
          markdownOffset, markdownEnd,
          htmlOffset, htmlEnd);
      }

    End:
      if (!first)
      {
        Indent();
        Line("}");
      }
      Line("}");

      var source = StringBuilder.ToString();
      StringBuilder.Length = 0;

      return source;
    }

    private static void WriteTest(string name, string compressedName, int number, string[] lines, int commentOffset, int commentEnd, int markdownOffset, int markdownEnd, int htmlOffset, int htmlEnd)
    {
      if (commentOffset != commentEnd)
      {
        while (commentOffset < commentEnd && IsEmpty(lines[commentOffset])) commentOffset++;
        for (var i = commentOffset; i < commentEnd; i++)
        {
          var nextLine = lines[i];
          Indent(2);
          Write(nextLine == "" ? "//" : "// ");
          Line(nextLine);
        }
      }

      Indent(2);
      Line("[Fact]");
      Indent(2);
      Line("public void " + compressedName + "_Example" + number.ToString().PadLeft(3, '0') + "()");
      Indent(2);
      Line("{");
      Indent(3);
      Line("// Example " + number);
      Indent(3);
      Line("// Section: " + name);

      Indent(3);
      Line("//");
      Indent(3);
      Line("// The following Markdown:");
      for (var i = markdownOffset; i < markdownEnd; i++)
      {
        var nextLine = lines[i];
        Indent(3);
        Write(nextLine == "" ? "//" : "// ");
        Indent();
        Line(nextLine);
      }

      Indent(3);
      Line("//");
      Indent(3);
      Line("// Should be rendered as:");
      for (var i = htmlOffset; i < htmlEnd; i++)
      {
        var nextLine = lines[i];
        Indent(3);
        Write(nextLine == "" ? "//" : "// ");
        Indent();
        Line(nextLine);
      }
      if (htmlOffset >= htmlEnd)
      {
        Indent(3);
        Write("//");
      }

      Line();
      // Indent(3); Line($"Console.WriteLine(\"Example {number}\\nSection {name}\\n\");");

      Indent(3);
      Write("Parser.Parse(\"");
      for (var i = markdownOffset; i < markdownEnd; i++)
      {
        Write(Escape(lines[i]));
        if (i != markdownEnd - 1) Write("\\n");
      }
      Write("\").Is(Parser.Prettify(\"");
      for (var i = htmlOffset; i < htmlEnd; i++)
      {
        Write(Escape(lines[i]));
        if (i != htmlEnd - 1) Write("\\n");
      }
      Line("\"));");

      Indent(2);
      Line("}");
    }
    private static string Escape(string input)
    {
      return input
          .Replace("→", "\t")
          .Replace("\\", "\\\\")
          .Replace("\"", "\\\"")
          .Replace("\0", "\\0")
          .Replace("\a", "\\a")
          .Replace("\b", "\\b")
          .Replace("\f", "\\f")
          .Replace("\n", "\\n")
          .Replace("\r", "\\r")
          .Replace("\t", "\\t")
          .Replace("\v", "\\v")
        ;
    }
    private static string CompressedName(string name)
    {
      var compressedName = "";
      foreach (var part in name.Replace(',', ' ').Split(' ', StringSplitOptions.RemoveEmptyEntries))
      {
        compressedName += char.IsLower(part[0])
          ? char.ToUpper(part[0]) + (part.Length > 1 ? part.Substring(1) : "")
          : part;
      }
      return compressedName;
    }
    private static bool IsEmpty(string str)
    {
      for (var i = 0; i < str.Length; i++)
      {
        if (str[i] != ' ') return false;
      }
      return true;
    }
  }
}
