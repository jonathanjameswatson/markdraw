using System;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Text;

// Modified from https://github.com/xoofx/markdig/tree/master/src/SpecFileGen
namespace SpecFileGen
{
  class Program
  {
    static readonly string ProgramDirectory =
        Path.GetFullPath(
            Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "../../../"));

    static readonly string specName = "CommonMark v. 0.29";
    static readonly string fileName = "CommonMark.md";

    static void Main()
    {
      string inputPath = Path.Combine(ProgramDirectory, "../Markdraw.Parser.Test") + "/" + fileName;
      string source = ParseSpecification(File.ReadAllText(inputPath)).Replace("\r\n", "\n", StringComparison.Ordinal);

      string outputPath = System.IO.Path.ChangeExtension(inputPath, "generated.cs");
      File.WriteAllText(outputPath, source);
    }

    static readonly StringBuilder StringBuilder = new StringBuilder(1 << 20); // 1 MB
    static void Write(string text)
    {
      StringBuilder.Append(text);
    }
    static void Line(string text = null)
    {
      if (text != null) StringBuilder.Append(text);
      StringBuilder.Append('\n');
    }
    static void Indent(int count = 1)
    {
      StringBuilder.Append(new string(' ', 2 * count));
    }

    static string ParseSpecification(string specSource)
    {
      Line();
      Write("// "); Line(new string('-', 32));
      Write("// "); Write(new string(' ', 16 - specName.Length / 2)); Line(specName);
      Write("// "); Line(new string('-', 32));
      Line();
      Line("using System;");
      Line("using Xunit;");
      Line();
      Line("namespace Markdraw.Parser.Test");
      Line("{");

      var lines = specSource.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

      bool nameChanged = true;
      string name = "";
      string compressedName = "";
      int number = 0;
      int commentOffset = 0, commentEnd = 0, markdownOffset = 0, markdownEnd = 0, htmlOffset = 0, htmlEnd = 0;
      bool first = true;
      LinkedList<(string Heading, string Compressed, int Level)> headings = new LinkedList<(string, string, int)>();
      StringBuilder nameBuilder = new StringBuilder(64);

      int i = 0;
      while (i < lines.Length)
      {
        commentOffset = commentEnd = i;
        while (!lines[i].Equals("```````````````````````````````` example", StringComparison.Ordinal))
        {
          string line = lines[i];
          if (line.Length > 2 && line[0] == '#')
          {
            int level = line.IndexOf(' ', StringComparison.Ordinal);
            while (headings.Count != 0)
            {
              if (headings.Last.Value.Level < level) break;
              headings.RemoveLast();
            }
            string heading = line.Substring(level + 1);
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
                string nextLine = lines[i];
                Indent(2); Write(nextLine == "" ? "//" : "// "); Line(nextLine);
              }
            }
            goto End;
          }
        };

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
            Indent(); Line("}");
            Line();
          }
          // Indent(); Line("[TestFixture]");
          Indent(); Line("public class Test" + compressedName);
          Indent(); Line("{");
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
        Indent(); Line("}");
      }
      Line("}");

      string source = StringBuilder.ToString();
      StringBuilder.Length = 0;

      return source;
    }

    static void WriteTest(string name, string compressedName, int number, string[] lines, int commentOffset, int commentEnd, int markdownOffset, int markdownEnd, int htmlOffset, int htmlEnd)
    {
      if (commentOffset != commentEnd)
      {
        while (commentOffset < commentEnd && IsEmpty(lines[commentOffset])) commentOffset++;
        for (int i = commentOffset; i < commentEnd; i++)
        {
          string nextLine = lines[i];
          Indent(2); Write(nextLine == "" ? "//" : "// "); Line(nextLine);
        }
      }

      Indent(2); Line("[Fact]");
      Indent(2); Line("public void " + compressedName + "_Example" + number.ToString().PadLeft(3, '0') + "()");
      Indent(2); Line("{");
      Indent(3); Line("// Example " + number);
      Indent(3); Line("// Section: " + name);

      Indent(3); Line("//");
      Indent(3); Line("// The following Markdown:");
      for (int i = markdownOffset; i < markdownEnd; i++)
      {
        string nextLine = lines[i];
        Indent(3); Write(nextLine == "" ? "//" : "// "); Indent(); Line(nextLine);
      }

      Indent(3); Line("//");
      Indent(3); Line("// Should be rendered as:");
      for (int i = htmlOffset; i < htmlEnd; i++)
      {
        string nextLine = lines[i];
        Indent(3); Write(nextLine == "" ? "//" : "// "); Indent(); Line(nextLine);
      }
      if (htmlOffset >= htmlEnd)
      {
        Indent(3); Write("//");
      }

      Line();
      // Indent(3); Line($"Console.WriteLine(\"Example {number}\\nSection {name}\\n\");");

      Indent(3);
      Write("Parser.Parse(\"");
      for (int i = markdownOffset; i < markdownEnd; i++)
      {
        Write(Escape(lines[i]));
        if (i != markdownEnd - 1) Write("\\n");
      }
      Write("\").Is(Parser.Prettify(\"");
      for (int i = htmlOffset; i < htmlEnd; i++)
      {
        Write(Escape(lines[i]));
        if (i != htmlEnd - 1) Write("\\n");
      }
      Line("\"));");

      Indent(2); Line("}");
    }
    static string Escape(string input)
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
    static string CompressedName(string name)
    {
      string compressedName = "";
      foreach (var part in name.Replace(',', ' ').Split(' ', StringSplitOptions.RemoveEmptyEntries))
      {
        compressedName += char.IsLower(part[0])
            ? char.ToUpper(part[0]) + (part.Length > 1 ? part.Substring(1) : "")
            : part;
      }
      return compressedName;
    }
    static bool IsEmpty(string str)
    {
      for (int i = 0; i < str.Length; i++)
      {
        if (str[i] != ' ') return false;
      }
      return true;
    }
  }
}
