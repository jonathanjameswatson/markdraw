
// --------------------------------
//        CommonMark v. 0.29
// --------------------------------

using System;
using Xunit;

namespace Markdraw.Parser.Test
{
  public class TestPreliminariesTabs
  {
    // ---
    // title: CommonMark Spec
    // author: John MacFarlane
    // version: 0.29
    // date: '2019-04-06'
    // license: '[CC-BY-SA 4.0](http://creativecommons.org/licenses/by-sa/4.0/)'
    // ...
    //
    // # Introduction
    //
    // ## What is Markdown?
    //
    // Markdown is a plain text format for writing structured documents,
    // based on conventions for indicating formatting in email
    // and usenet posts.  It was developed by John Gruber (with
    // help from Aaron Swartz) and released in 2004 in the form of a
    // [syntax description](http://daringfireball.net/projects/markdown/syntax)
    // and a Perl script (`Markdown.pl`) for converting Markdown to
    // HTML.  In the next decade, dozens of implementations were
    // developed in many languages.  Some extended the original
    // Markdown syntax with conventions for footnotes, tables, and
    // other document elements.  Some allowed Markdown documents to be
    // rendered in formats other than HTML.  Websites like Reddit,
    // StackOverflow, and GitHub had millions of people using Markdown.
    // And Markdown started to be used beyond the web, to author books,
    // articles, slide shows, letters, and lecture notes.
    //
    // What distinguishes Markdown from many other lightweight markup
    // syntaxes, which are often easier to write, is its readability.
    // As Gruber writes:
    //
    // > The overriding design goal for Markdown's formatting syntax is
    // > to make it as readable as possible. The idea is that a
    // > Markdown-formatted document should be publishable as-is, as
    // > plain text, without looking like it's been marked up with tags
    // > or formatting instructions.
    // > (<http://daringfireball.net/projects/markdown/>)
    //
    // The point can be illustrated by comparing a sample of
    // [AsciiDoc](http://www.methods.co.nz/asciidoc/) with
    // an equivalent sample of Markdown.  Here is a sample of
    // AsciiDoc from the AsciiDoc manual:
    //
    // ```
    // 1. List item one.
    // +
    // List item one continued with a second paragraph followed by an
    // Indented block.
    // +
    // .................
    // $ ls *.sh
    // $ mv *.sh ~/tmp
    // .................
    // +
    // List item continued with a third paragraph.
    //
    // 2. List item two continued with an open block.
    // +
    // --
    // This paragraph is part of the preceding list item.
    //
    // a. This list is nested and does not require explicit item
    // continuation.
    // +
    // This paragraph is part of the preceding list item.
    //
    // b. List item b.
    //
    // This paragraph belongs to item two of the outer list.
    // --
    // ```
    //
    // And here is the equivalent in Markdown:
    // ```
    // 1.  List item one.
    //
    //     List item one continued with a second paragraph followed by an
    //     Indented block.
    //
    //         $ ls *.sh
    //         $ mv *.sh ~/tmp
    //
    //     List item continued with a third paragraph.
    //
    // 2.  List item two continued with an open block.
    //
    //     This paragraph is part of the preceding list item.
    //
    //     1. This list is nested and does not require explicit item continuation.
    //
    //        This paragraph is part of the preceding list item.
    //
    //     2. List item b.
    //
    //     This paragraph belongs to item two of the outer list.
    // ```
    //
    // The AsciiDoc version is, arguably, easier to write. You don't need
    // to worry about indentation.  But the Markdown version is much easier
    // to read.  The nesting of list items is apparent to the eye in the
    // source, not just in the processed document.
    //
    // ## Why is a spec needed?
    //
    // John Gruber's [canonical description of Markdown's
    // syntax](http://daringfireball.net/projects/markdown/syntax)
    // does not specify the syntax unambiguously.  Here are some examples of
    // questions it does not answer:
    //
    // 1.  How much indentation is needed for a sublist?  The spec says that
    //     continuation paragraphs need to be indented four spaces, but is
    //     not fully explicit about sublists.  It is natural to think that
    //     they, too, must be indented four spaces, but `Markdown.pl` does
    //     not require that.  This is hardly a "corner case," and divergences
    //     between implementations on this issue often lead to surprises for
    //     users in real documents. (See [this comment by John
    //     Gruber](http://article.gmane.org/gmane.text.markdown.general/1997).)
    //
    // 2.  Is a blank line needed before a block quote or heading?
    //     Most implementations do not require the blank line.  However,
    //     this can lead to unexpected results in hard-wrapped text, and
    //     also to ambiguities in parsing (note that some implementations
    //     put the heading inside the blockquote, while others do not).
    //     (John Gruber has also spoken [in favor of requiring the blank
    //     lines](http://article.gmane.org/gmane.text.markdown.general/2146).)
    //
    // 3.  Is a blank line needed before an indented code block?
    //     (`Markdown.pl` requires it, but this is not mentioned in the
    //     documentation, and some implementations do not require it.)
    //
    //     ``` markdown
    //     paragraph
    //         code?
    //     ```
    //
    // 4.  What is the exact rule for determining when list items get
    //     wrapped in `<p>` tags?  Can a list be partially "loose" and partially
    //     "tight"?  What should we do with a list like this?
    //
    //     ``` markdown
    //     1. one
    //
    //     2. two
    //     3. three
    //     ```
    //
    //     Or this?
    //
    //     ``` markdown
    //     1.  one
    //         - a
    //
    //         - b
    //     2.  two
    //     ```
    //
    //     (There are some relevant comments by John Gruber
    //     [here](http://article.gmane.org/gmane.text.markdown.general/2554).)
    //
    // 5.  Can list markers be indented?  Can ordered list markers be right-aligned?
    //
    //     ``` markdown
    //      8. item 1
    //      9. item 2
    //     10. item 2a
    //     ```
    //
    // 6.  Is this one list with a thematic break in its second item,
    //     or two lists separated by a thematic break?
    //
    //     ``` markdown
    //     * a
    //     * * * * *
    //     * b
    //     ```
    //
    // 7.  When list markers change from numbers to bullets, do we have
    //     two lists or one?  (The Markdown syntax description suggests two,
    //     but the perl scripts and many other implementations produce one.)
    //
    //     ``` markdown
    //     1. fee
    //     2. fie
    //     -  foe
    //     -  fum
    //     ```
    //
    // 8.  What are the precedence rules for the markers of inline structure?
    //     For example, is the following a valid link, or does the code span
    //     take precedence ?
    //
    //     ``` markdown
    //     [a backtick (`)](/url) and [another backtick (`)](/url).
    //     ```
    //
    // 9.  What are the precedence rules for markers of emphasis and strong
    //     emphasis?  For example, how should the following be parsed?
    //
    //     ``` markdown
    //     *foo *bar* baz*
    //     ```
    //
    // 10. What are the precedence rules between block-level and inline-level
    //     structure?  For example, how should the following be parsed?
    //
    //     ``` markdown
    //     - `a long code span can contain a hyphen like this
    //       - and it can screw things up`
    //     ```
    //
    // 11. Can list items include section headings?  (`Markdown.pl` does not
    //     allow this, but does allow blockquotes to include headings.)
    //
    //     ``` markdown
    //     - # Heading
    //     ```
    //
    // 12. Can list items be empty?
    //
    //     ``` markdown
    //     * a
    //     *
    //     * b
    //     ```
    //
    // 13. Can link references be defined inside block quotes or list items?
    //
    //     ``` markdown
    //     > Blockquote [foo].
    //     >
    //     > [foo]: /url
    //     ```
    //
    // 14. If there are multiple definitions for the same reference, which takes
    //     precedence?
    //
    //     ``` markdown
    //     [foo]: /url1
    //     [foo]: /url2
    //
    //     [foo][]
    //     ```
    //
    // In the absence of a spec, early implementers consulted `Markdown.pl`
    // to resolve these ambiguities.  But `Markdown.pl` was quite buggy, and
    // gave manifestly bad results in many cases, so it was not a
    // satisfactory replacement for a spec.
    //
    // Because there is no unambiguous spec, implementations have diverged
    // considerably.  As a result, users are often surprised to find that
    // a document that renders one way on one system (say, a GitHub wiki)
    // renders differently on another (say, converting to docbook using
    // pandoc).  To make matters worse, because nothing in Markdown counts
    // as a "syntax error," the divergence often isn't discovered right away.
    //
    // ## About this document
    //
    // This document attempts to specify Markdown syntax unambiguously.
    // It contains many examples with side-by-side Markdown and
    // HTML.  These are intended to double as conformance tests.  An
    // accompanying script `spec_tests.py` can be used to run the tests
    // against any Markdown program:
    //
    //     python test/spec_tests.py --spec spec.txt --program PROGRAM
    //
    // Since this document describes how Markdown is to be parsed into
    // an abstract syntax tree, it would have made sense to use an abstract
    // representation of the syntax tree instead of HTML.  But HTML is capable
    // of representing the structural distinctions we need to make, and the
    // choice of HTML for the tests makes it possible to run the tests against
    // an implementation without writing an abstract syntax tree renderer.
    //
    // This document is generated from a text file, `spec.txt`, written
    // in Markdown with a small extension for the side-by-side tests.
    // The script `tools/makespec.py` can be used to convert `spec.txt` into
    // HTML or CommonMark (which can then be converted into other formats).
    //
    // In the examples, the `→` character is used to represent tabs.
    //
    // # Preliminaries
    //
    // ## Characters and lines
    //
    // Any sequence of [characters] is a valid CommonMark
    // document.
    //
    // A [character](@) is a Unicode code point.  Although some
    // code points (for example, combining accents) do not correspond to
    // characters in an intuitive sense, all code points count as characters
    // for purposes of this spec.
    //
    // This spec does not specify an encoding; it thinks of lines as composed
    // of [characters] rather than bytes.  A conforming parser may be limited
    // to a certain encoding.
    //
    // A [line](@) is a sequence of zero or more [characters]
    // other than newline (`U+000A`) or carriage return (`U+000D`),
    // followed by a [line ending] or by the end of file.
    //
    // A [line ending](@) is a newline (`U+000A`), a carriage return
    // (`U+000D`) not followed by a newline, or a carriage return and a
    // following newline.
    //
    // A line containing no characters, or a line containing only spaces
    // (`U+0020`) or tabs (`U+0009`), is called a [blank line](@).
    //
    // The following definitions of character classes will be used in this spec:
    //
    // A [whitespace character](@) is a space
    // (`U+0020`), tab (`U+0009`), newline (`U+000A`), line tabulation (`U+000B`),
    // form feed (`U+000C`), or carriage return (`U+000D`).
    //
    // [Whitespace](@) is a sequence of one or more [whitespace
    // characters].
    //
    // A [Unicode whitespace character](@) is
    // any code point in the Unicode `Zs` general category, or a tab (`U+0009`),
    // carriage return (`U+000D`), newline (`U+000A`), or form feed
    // (`U+000C`).
    //
    // [Unicode whitespace](@) is a sequence of one
    // or more [Unicode whitespace characters].
    //
    // A [space](@) is `U+0020`.
    //
    // A [non-whitespace character](@) is any character
    // that is not a [whitespace character].
    //
    // An [ASCII punctuation character](@)
    // is `!`, `"`, `#`, `$`, `%`, `&`, `'`, `(`, `)`,
    // `*`, `+`, `,`, `-`, `.`, `/` (U+0021–2F), 
    // `:`, `;`, `<`, `=`, `>`, `?`, `@` (U+003A–0040),
    // `[`, `\`, `]`, `^`, `_`, `` ` `` (U+005B–0060), 
    // `{`, `|`, `}`, or `~` (U+007B–007E).
    //
    // A [punctuation character](@) is an [ASCII
    // punctuation character] or anything in
    // the general Unicode categories  `Pc`, `Pd`, `Pe`, `Pf`, `Pi`, `Po`, or `Ps`.
    //
    // ## Tabs
    //
    // Tabs in lines are not expanded to [spaces].  However,
    // in contexts where whitespace helps to define block structure,
    // tabs behave as if they were replaced by spaces with a tab stop
    // of 4 characters.
    //
    // Thus, for example, a tab can be used instead of four spaces
    // in an indented code block.  (Note, however, that internal
    // tabs are passed through as literal tabs, not expanded to
    // spaces.)
    [Fact]
    public void PreliminariesTabs_Example001()
    {
      // Example 1
      // Section: Preliminaries / Tabs
      //
      // The following Markdown:
      //   →foo→baz→→bim
      //
      // Should be rendered as:
      //   <pre><code>foo→baz→→bim
      //   </code></pre>

      Parser.Parse("\tfoo\tbaz\t\tbim").Is(Parser.Prettify("<pre><code>foo\tbaz\t\tbim\n</code></pre>"));

      Parser.DoubleParse("\tfoo\tbaz\t\tbim").Is(Parser.Prettify("<pre><code>foo\tbaz\t\tbim\n</code></pre>"));
    }

    [Fact]
    public void PreliminariesTabs_Example002()
    {
      // Example 2
      // Section: Preliminaries / Tabs
      //
      // The following Markdown:
      //     →foo→baz→→bim
      //
      // Should be rendered as:
      //   <pre><code>foo→baz→→bim
      //   </code></pre>

      Parser.Parse("  \tfoo\tbaz\t\tbim").Is(Parser.Prettify("<pre><code>foo\tbaz\t\tbim\n</code></pre>"));

      Parser.DoubleParse("  \tfoo\tbaz\t\tbim").Is(Parser.Prettify("<pre><code>foo\tbaz\t\tbim\n</code></pre>"));
    }

    [Fact]
    public void PreliminariesTabs_Example003()
    {
      // Example 3
      // Section: Preliminaries / Tabs
      //
      // The following Markdown:
      //       a→a
      //       ὐ→a
      //
      // Should be rendered as:
      //   <pre><code>a→a
      //   ὐ→a
      //   </code></pre>

      Parser.Parse("    a\ta\n    ὐ\ta").Is(Parser.Prettify("<pre><code>a\ta\nὐ\ta\n</code></pre>"));

      Parser.DoubleParse("    a\ta\n    ὐ\ta").Is(Parser.Prettify("<pre><code>a\ta\nὐ\ta\n</code></pre>"));
    }

    // In the following example, a continuation paragraph of a list
    // item is indented with a tab; this has exactly the same effect
    // as indentation with four spaces would:
    [Fact]
    public void PreliminariesTabs_Example004()
    {
      // Example 4
      // Section: Preliminaries / Tabs
      //
      // The following Markdown:
      //     - foo
      //  
      //   →bar
      //
      // Should be rendered as:
      //   <ul>
      //   <li>
      //   <p>foo</p>
      //   <p>bar</p>
      //   </li>
      //   </ul>

      Parser.Parse("  - foo\n\n\tbar").Is(Parser.Prettify("<ul>\n<li>\n<p>foo</p>\n<p>bar</p>\n</li>\n</ul>"));

      Parser.DoubleParse("  - foo\n\n\tbar").Is(Parser.Prettify("<ul>\n<li>\n<p>foo</p>\n<p>bar</p>\n</li>\n</ul>"));
    }

    [Fact]
    public void PreliminariesTabs_Example005()
    {
      // Example 5
      // Section: Preliminaries / Tabs
      //
      // The following Markdown:
      //   - foo
      //  
      //   →→bar
      //
      // Should be rendered as:
      //   <ul>
      //   <li>
      //   <p>foo</p>
      //   <pre><code>  bar
      //   </code></pre>
      //   </li>
      //   </ul>

      Parser.Parse("- foo\n\n\t\tbar").Is(Parser.Prettify("<ul>\n<li>\n<p>foo</p>\n<pre><code>  bar\n</code></pre>\n</li>\n</ul>"));

      Parser.DoubleParse("- foo\n\n\t\tbar").Is(Parser.Prettify("<ul>\n<li>\n<p>foo</p>\n<pre><code>  bar\n</code></pre>\n</li>\n</ul>"));
    }

    // Normally the `>` that begins a block quote may be followed
    // optionally by a space, which is not considered part of the
    // content.  In the following case `>` is followed by a tab,
    // which is treated as if it were expanded into three spaces.
    // Since one of these spaces is considered part of the
    // delimiter, `foo` is considered to be indented six spaces
    // inside the block quote context, so we get an indented
    // code block starting with two spaces.
    [Fact]
    public void PreliminariesTabs_Example006()
    {
      // Example 6
      // Section: Preliminaries / Tabs
      //
      // The following Markdown:
      //   >→→foo
      //
      // Should be rendered as:
      //   <blockquote>
      //   <pre><code>  foo
      //   </code></pre>
      //   </blockquote>

      Parser.Parse(">\t\tfoo").Is(Parser.Prettify("<blockquote>\n<pre><code>  foo\n</code></pre>\n</blockquote>"));

      Parser.DoubleParse(">\t\tfoo").Is(Parser.Prettify("<blockquote>\n<pre><code>  foo\n</code></pre>\n</blockquote>"));
    }

    [Fact]
    public void PreliminariesTabs_Example007()
    {
      // Example 7
      // Section: Preliminaries / Tabs
      //
      // The following Markdown:
      //   -→→foo
      //
      // Should be rendered as:
      //   <ul>
      //   <li>
      //   <pre><code>  foo
      //   </code></pre>
      //   </li>
      //   </ul>

      Parser.Parse("-\t\tfoo").Is(Parser.Prettify("<ul>\n<li>\n<pre><code>  foo\n</code></pre>\n</li>\n</ul>"));

      Parser.DoubleParse("-\t\tfoo").Is(Parser.Prettify("<ul>\n<li>\n<pre><code>  foo\n</code></pre>\n</li>\n</ul>"));
    }

    [Fact]
    public void PreliminariesTabs_Example008()
    {
      // Example 8
      // Section: Preliminaries / Tabs
      //
      // The following Markdown:
      //       foo
      //   →bar
      //
      // Should be rendered as:
      //   <pre><code>foo
      //   bar
      //   </code></pre>

      Parser.Parse("    foo\n\tbar").Is(Parser.Prettify("<pre><code>foo\nbar\n</code></pre>"));

      Parser.DoubleParse("    foo\n\tbar").Is(Parser.Prettify("<pre><code>foo\nbar\n</code></pre>"));
    }

    [Fact]
    public void PreliminariesTabs_Example009()
    {
      // Example 9
      // Section: Preliminaries / Tabs
      //
      // The following Markdown:
      //    - foo
      //      - bar
      //   → - baz
      //
      // Should be rendered as:
      //   <ul>
      //   <li>foo
      //   <ul>
      //   <li>bar
      //   <ul>
      //   <li>baz</li>
      //   </ul>
      //   </li>
      //   </ul>
      //   </li>
      //   </ul>

      Parser.Parse(" - foo\n   - bar\n\t - baz").Is(Parser.Prettify("<ul>\n<li>foo\n<ul>\n<li>bar\n<ul>\n<li>baz</li>\n</ul>\n</li>\n</ul>\n</li>\n</ul>"));

      Parser.DoubleParse(" - foo\n   - bar\n\t - baz").Is(Parser.Prettify("<ul>\n<li>foo\n<ul>\n<li>bar\n<ul>\n<li>baz</li>\n</ul>\n</li>\n</ul>\n</li>\n</ul>"));
    }

    [Fact]
    public void PreliminariesTabs_Example010()
    {
      // Example 10
      // Section: Preliminaries / Tabs
      //
      // The following Markdown:
      //   #→Foo
      //
      // Should be rendered as:
      //   <h1>Foo</h1>

      Parser.Parse("#\tFoo").Is(Parser.Prettify("<h1>Foo</h1>"));

      Parser.DoubleParse("#\tFoo").Is(Parser.Prettify("<h1>Foo</h1>"));
    }

    [Fact]
    public void PreliminariesTabs_Example011()
    {
      // Example 11
      // Section: Preliminaries / Tabs
      //
      // The following Markdown:
      //   *→*→*→
      //
      // Should be rendered as:
      //   <hr />

      Parser.Parse("*\t*\t*\t").Is(Parser.Prettify("<hr />"));

      Parser.DoubleParse("*\t*\t*\t").Is(Parser.Prettify("<hr />"));
    }
  }

  public class TestBlocksAndInlinesPrecedence
  {
    // ## Insecure characters
    //
    // For security reasons, the Unicode character `U+0000` must be replaced
    // with the REPLACEMENT CHARACTER (`U+FFFD`).
    //
    // # Blocks and inlines
    //
    // We can think of a document as a sequence of
    // [blocks](@)---structural elements like paragraphs, block
    // quotations, lists, headings, rules, and code blocks.  Some blocks (like
    // block quotes and list items) contain other blocks; others (like
    // headings and paragraphs) contain [inline](@) content---text,
    // links, emphasized text, images, code spans, and so on.
    //
    // ## Precedence
    //
    // Indicators of block structure always take precedence over indicators
    // of inline structure.  So, for example, the following is a list with
    // two items, not a list with one item containing a code span:
    [Fact]
    public void BlocksAndInlinesPrecedence_Example012()
    {
      // Example 12
      // Section: Blocks and inlines / Precedence
      //
      // The following Markdown:
      //   - `one
      //   - two`
      //
      // Should be rendered as:
      //   <ul>
      //   <li>`one</li>
      //   <li>two`</li>
      //   </ul>

      Parser.Parse("- `one\n- two`").Is(Parser.Prettify("<ul>\n<li>`one</li>\n<li>two`</li>\n</ul>"));

      Parser.DoubleParse("- `one\n- two`").Is(Parser.Prettify("<ul>\n<li>`one</li>\n<li>two`</li>\n</ul>"));
    }
  }

  public class TestLeafBlocksThematicBreaks
  {
    // This means that parsing can proceed in two steps:  first, the block
    // structure of the document can be discerned; second, text lines inside
    // paragraphs, headings, and other block constructs can be parsed for inline
    // structure.  The second step requires information about link reference
    // definitions that will be available only at the end of the first
    // step.  Note that the first step requires processing lines in sequence,
    // but the second can be parallelized, since the inline parsing of
    // one block element does not affect the inline parsing of any other.
    //
    // ## Container blocks and leaf blocks
    //
    // We can divide blocks into two types:
    // [container blocks](@),
    // which can contain other blocks, and [leaf blocks](@),
    // which cannot.
    //
    // # Leaf blocks
    //
    // This section describes the different kinds of leaf block that make up a
    // Markdown document.
    //
    // ## Thematic breaks
    //
    // A line consisting of 0-3 spaces of indentation, followed by a sequence
    // of three or more matching `-`, `_`, or `*` characters, each followed
    // optionally by any number of spaces or tabs, forms a
    // [thematic break](@).
    [Fact]
    public void LeafBlocksThematicBreaks_Example013()
    {
      // Example 13
      // Section: Leaf blocks / Thematic breaks
      //
      // The following Markdown:
      //   ***
      //   ---
      //   ___
      //
      // Should be rendered as:
      //   <hr />
      //   <hr />
      //   <hr />

      Parser.Parse("***\n---\n___").Is(Parser.Prettify("<hr />\n<hr />\n<hr />"));

      Parser.DoubleParse("***\n---\n___").Is(Parser.Prettify("<hr />\n<hr />\n<hr />"));
    }

    // Wrong characters:
    [Fact]
    public void LeafBlocksThematicBreaks_Example014()
    {
      // Example 14
      // Section: Leaf blocks / Thematic breaks
      //
      // The following Markdown:
      //   +++
      //
      // Should be rendered as:
      //   <p>+++</p>

      Parser.Parse("+++").Is(Parser.Prettify("<p>+++</p>"));

      Parser.DoubleParse("+++").Is(Parser.Prettify("<p>+++</p>"));
    }

    [Fact]
    public void LeafBlocksThematicBreaks_Example015()
    {
      // Example 15
      // Section: Leaf blocks / Thematic breaks
      //
      // The following Markdown:
      //   ===
      //
      // Should be rendered as:
      //   <p>===</p>

      Parser.Parse("===").Is(Parser.Prettify("<p>===</p>"));

      Parser.DoubleParse("===").Is(Parser.Prettify("<p>===</p>"));
    }

    // Not enough characters:
    [Fact]
    public void LeafBlocksThematicBreaks_Example016()
    {
      // Example 16
      // Section: Leaf blocks / Thematic breaks
      //
      // The following Markdown:
      //   --
      //   **
      //   __
      //
      // Should be rendered as:
      //   <p>--
      //   **
      //   __</p>

      Parser.Parse("--\n**\n__").Is(Parser.Prettify("<p>--\n**\n__</p>"));

      Parser.DoubleParse("--\n**\n__").Is(Parser.Prettify("<p>--\n**\n__</p>"));
    }

    // One to three spaces indent are allowed:
    [Fact]
    public void LeafBlocksThematicBreaks_Example017()
    {
      // Example 17
      // Section: Leaf blocks / Thematic breaks
      //
      // The following Markdown:
      //    ***
      //     ***
      //      ***
      //
      // Should be rendered as:
      //   <hr />
      //   <hr />
      //   <hr />

      Parser.Parse(" ***\n  ***\n   ***").Is(Parser.Prettify("<hr />\n<hr />\n<hr />"));

      Parser.DoubleParse(" ***\n  ***\n   ***").Is(Parser.Prettify("<hr />\n<hr />\n<hr />"));
    }

    // Four spaces is too many:
    [Fact]
    public void LeafBlocksThematicBreaks_Example018()
    {
      // Example 18
      // Section: Leaf blocks / Thematic breaks
      //
      // The following Markdown:
      //       ***
      //
      // Should be rendered as:
      //   <pre><code>***
      //   </code></pre>

      Parser.Parse("    ***").Is(Parser.Prettify("<pre><code>***\n</code></pre>"));

      Parser.DoubleParse("    ***").Is(Parser.Prettify("<pre><code>***\n</code></pre>"));
    }

    [Fact]
    public void LeafBlocksThematicBreaks_Example019()
    {
      // Example 19
      // Section: Leaf blocks / Thematic breaks
      //
      // The following Markdown:
      //   Foo
      //       ***
      //
      // Should be rendered as:
      //   <p>Foo
      //   ***</p>

      Parser.Parse("Foo\n    ***").Is(Parser.Prettify("<p>Foo\n***</p>"));

      Parser.DoubleParse("Foo\n    ***").Is(Parser.Prettify("<p>Foo\n***</p>"));
    }

    // More than three characters may be used:
    [Fact]
    public void LeafBlocksThematicBreaks_Example020()
    {
      // Example 20
      // Section: Leaf blocks / Thematic breaks
      //
      // The following Markdown:
      //   _____________________________________
      //
      // Should be rendered as:
      //   <hr />

      Parser.Parse("_____________________________________").Is(Parser.Prettify("<hr />"));

      Parser.DoubleParse("_____________________________________").Is(Parser.Prettify("<hr />"));
    }

    // Spaces are allowed between the characters:
    [Fact]
    public void LeafBlocksThematicBreaks_Example021()
    {
      // Example 21
      // Section: Leaf blocks / Thematic breaks
      //
      // The following Markdown:
      //    - - -
      //
      // Should be rendered as:
      //   <hr />

      Parser.Parse(" - - -").Is(Parser.Prettify("<hr />"));

      Parser.DoubleParse(" - - -").Is(Parser.Prettify("<hr />"));
    }

    [Fact]
    public void LeafBlocksThematicBreaks_Example022()
    {
      // Example 22
      // Section: Leaf blocks / Thematic breaks
      //
      // The following Markdown:
      //    **  * ** * ** * **
      //
      // Should be rendered as:
      //   <hr />

      Parser.Parse(" **  * ** * ** * **").Is(Parser.Prettify("<hr />"));

      Parser.DoubleParse(" **  * ** * ** * **").Is(Parser.Prettify("<hr />"));
    }

    [Fact]
    public void LeafBlocksThematicBreaks_Example023()
    {
      // Example 23
      // Section: Leaf blocks / Thematic breaks
      //
      // The following Markdown:
      //   -     -      -      -
      //
      // Should be rendered as:
      //   <hr />

      Parser.Parse("-     -      -      -").Is(Parser.Prettify("<hr />"));

      Parser.DoubleParse("-     -      -      -").Is(Parser.Prettify("<hr />"));
    }

    // Spaces are allowed at the end:
    [Fact]
    public void LeafBlocksThematicBreaks_Example024()
    {
      // Example 24
      // Section: Leaf blocks / Thematic breaks
      //
      // The following Markdown:
      //   - - - -    
      //
      // Should be rendered as:
      //   <hr />

      Parser.Parse("- - - -    ").Is(Parser.Prettify("<hr />"));

      Parser.DoubleParse("- - - -    ").Is(Parser.Prettify("<hr />"));
    }

    // However, no other characters may occur in the line:
    [Fact]
    public void LeafBlocksThematicBreaks_Example025()
    {
      // Example 25
      // Section: Leaf blocks / Thematic breaks
      //
      // The following Markdown:
      //   _ _ _ _ a
      //  
      //   a------
      //  
      //   ---a---
      //
      // Should be rendered as:
      //   <p>_ _ _ _ a</p>
      //   <p>a------</p>
      //   <p>---a---</p>

      Parser.Parse("_ _ _ _ a\n\na------\n\n---a---").Is(Parser.Prettify("<p>_ _ _ _ a</p>\n<p>a------</p>\n<p>---a---</p>"));

      Parser.DoubleParse("_ _ _ _ a\n\na------\n\n---a---").Is(Parser.Prettify("<p>_ _ _ _ a</p>\n<p>a------</p>\n<p>---a---</p>"));
    }

    // It is required that all of the [non-whitespace characters] be the same.
    // So, this is not a thematic break:
    [Fact]
    public void LeafBlocksThematicBreaks_Example026()
    {
      // Example 26
      // Section: Leaf blocks / Thematic breaks
      //
      // The following Markdown:
      //    *-*
      //
      // Should be rendered as:
      //   <p><em>-</em></p>

      Parser.Parse(" *-*").Is(Parser.Prettify("<p><em>-</em></p>"));

      Parser.DoubleParse(" *-*").Is(Parser.Prettify("<p><em>-</em></p>"));
    }

    // Thematic breaks do not need blank lines before or after:
    [Fact]
    public void LeafBlocksThematicBreaks_Example027()
    {
      // Example 27
      // Section: Leaf blocks / Thematic breaks
      //
      // The following Markdown:
      //   - foo
      //   ***
      //   - bar
      //
      // Should be rendered as:
      //   <ul>
      //   <li>foo</li>
      //   </ul>
      //   <hr />
      //   <ul>
      //   <li>bar</li>
      //   </ul>

      Parser.Parse("- foo\n***\n- bar").Is(Parser.Prettify("<ul>\n<li>foo</li>\n</ul>\n<hr />\n<ul>\n<li>bar</li>\n</ul>"));

      Parser.DoubleParse("- foo\n***\n- bar").Is(Parser.Prettify("<ul>\n<li>foo</li>\n</ul>\n<hr />\n<ul>\n<li>bar</li>\n</ul>"));
    }

    // Thematic breaks can interrupt a paragraph:
    [Fact]
    public void LeafBlocksThematicBreaks_Example028()
    {
      // Example 28
      // Section: Leaf blocks / Thematic breaks
      //
      // The following Markdown:
      //   Foo
      //   ***
      //   bar
      //
      // Should be rendered as:
      //   <p>Foo</p>
      //   <hr />
      //   <p>bar</p>

      Parser.Parse("Foo\n***\nbar").Is(Parser.Prettify("<p>Foo</p>\n<hr />\n<p>bar</p>"));

      Parser.DoubleParse("Foo\n***\nbar").Is(Parser.Prettify("<p>Foo</p>\n<hr />\n<p>bar</p>"));
    }

    // If a line of dashes that meets the above conditions for being a
    // thematic break could also be interpreted as the underline of a [setext
    // heading], the interpretation as a
    // [setext heading] takes precedence. Thus, for example,
    // this is a setext heading, not a paragraph followed by a thematic break:
    [Fact]
    public void LeafBlocksThematicBreaks_Example029()
    {
      // Example 29
      // Section: Leaf blocks / Thematic breaks
      //
      // The following Markdown:
      //   Foo
      //   ---
      //   bar
      //
      // Should be rendered as:
      //   <h2>Foo</h2>
      //   <p>bar</p>

      Parser.Parse("Foo\n---\nbar").Is(Parser.Prettify("<h2>Foo</h2>\n<p>bar</p>"));

      Parser.DoubleParse("Foo\n---\nbar").Is(Parser.Prettify("<h2>Foo</h2>\n<p>bar</p>"));
    }

    // When both a thematic break and a list item are possible
    // interpretations of a line, the thematic break takes precedence:
    [Fact]
    public void LeafBlocksThematicBreaks_Example030()
    {
      // Example 30
      // Section: Leaf blocks / Thematic breaks
      //
      // The following Markdown:
      //   * Foo
      //   * * *
      //   * Bar
      //
      // Should be rendered as:
      //   <ul>
      //   <li>Foo</li>
      //   </ul>
      //   <hr />
      //   <ul>
      //   <li>Bar</li>
      //   </ul>

      Parser.Parse("* Foo\n* * *\n* Bar").Is(Parser.Prettify("<ul>\n<li>Foo</li>\n</ul>\n<hr />\n<ul>\n<li>Bar</li>\n</ul>"));

      Parser.DoubleParse("* Foo\n* * *\n* Bar").Is(Parser.Prettify("<ul>\n<li>Foo</li>\n</ul>\n<hr />\n<ul>\n<li>Bar</li>\n</ul>"));
    }

    // If you want a thematic break in a list item, use a different bullet:
    [Fact]
    public void LeafBlocksThematicBreaks_Example031()
    {
      // Example 31
      // Section: Leaf blocks / Thematic breaks
      //
      // The following Markdown:
      //   - Foo
      //   - * * *
      //
      // Should be rendered as:
      //   <ul>
      //   <li>Foo</li>
      //   <li>
      //   <hr />
      //   </li>
      //   </ul>

      Parser.Parse("- Foo\n- * * *").Is(Parser.Prettify("<ul>\n<li>Foo</li>\n<li>\n<hr />\n</li>\n</ul>"));

      Parser.DoubleParse("- Foo\n- * * *").Is(Parser.Prettify("<ul>\n<li>Foo</li>\n<li>\n<hr />\n</li>\n</ul>"));
    }
  }

  public class TestLeafBlocksATXHeadings
  {
    // ## ATX headings
    //
    // An [ATX heading](@)
    // consists of a string of characters, parsed as inline content, between an
    // opening sequence of 1--6 unescaped `#` characters and an optional
    // closing sequence of any number of unescaped `#` characters.
    // The opening sequence of `#` characters must be followed by a
    // [space] or by the end of line. The optional closing sequence of `#`s must be
    // preceded by a [space] and may be followed by spaces only.  The opening
    // `#` character may be indented 0-3 spaces.  The raw contents of the
    // heading are stripped of leading and trailing spaces before being parsed
    // as inline content.  The heading level is equal to the number of `#`
    // characters in the opening sequence.
    //
    // Simple headings:
    [Fact]
    public void LeafBlocksATXHeadings_Example032()
    {
      // Example 32
      // Section: Leaf blocks / ATX headings
      //
      // The following Markdown:
      //   # foo
      //   ## foo
      //   ### foo
      //   #### foo
      //   ##### foo
      //   ###### foo
      //
      // Should be rendered as:
      //   <h1>foo</h1>
      //   <h2>foo</h2>
      //   <h3>foo</h3>
      //   <h4>foo</h4>
      //   <h5>foo</h5>
      //   <h6>foo</h6>

      Parser.Parse("# foo\n## foo\n### foo\n#### foo\n##### foo\n###### foo").Is(Parser.Prettify("<h1>foo</h1>\n<h2>foo</h2>\n<h3>foo</h3>\n<h4>foo</h4>\n<h5>foo</h5>\n<h6>foo</h6>"));

      Parser.DoubleParse("# foo\n## foo\n### foo\n#### foo\n##### foo\n###### foo").Is(Parser.Prettify("<h1>foo</h1>\n<h2>foo</h2>\n<h3>foo</h3>\n<h4>foo</h4>\n<h5>foo</h5>\n<h6>foo</h6>"));
    }

    // More than six `#` characters is not a heading:
    [Fact]
    public void LeafBlocksATXHeadings_Example033()
    {
      // Example 33
      // Section: Leaf blocks / ATX headings
      //
      // The following Markdown:
      //   ####### foo
      //
      // Should be rendered as:
      //   <p>####### foo</p>

      Parser.Parse("####### foo").Is(Parser.Prettify("<p>####### foo</p>"));

      Parser.DoubleParse("####### foo").Is(Parser.Prettify("<p>####### foo</p>"));
    }

    // At least one space is required between the `#` characters and the
    // heading's contents, unless the heading is empty.  Note that many
    // implementations currently do not require the space.  However, the
    // space was required by the
    // [original ATX implementation](http://www.aaronsw.com/2002/atx/atx.py),
    // and it helps prevent things like the following from being parsed as
    // headings:
    [Fact]
    public void LeafBlocksATXHeadings_Example034()
    {
      // Example 34
      // Section: Leaf blocks / ATX headings
      //
      // The following Markdown:
      //   #5 bolt
      //  
      //   #hashtag
      //
      // Should be rendered as:
      //   <p>#5 bolt</p>
      //   <p>#hashtag</p>

      Parser.Parse("#5 bolt\n\n#hashtag").Is(Parser.Prettify("<p>#5 bolt</p>\n<p>#hashtag</p>"));

      Parser.DoubleParse("#5 bolt\n\n#hashtag").Is(Parser.Prettify("<p>#5 bolt</p>\n<p>#hashtag</p>"));
    }

    // This is not a heading, because the first `#` is escaped:
    [Fact]
    public void LeafBlocksATXHeadings_Example035()
    {
      // Example 35
      // Section: Leaf blocks / ATX headings
      //
      // The following Markdown:
      //   \## foo
      //
      // Should be rendered as:
      //   <p>## foo</p>

      Parser.Parse("\\## foo").Is(Parser.Prettify("<p>## foo</p>"));

      Parser.DoubleParse("\\## foo").Is(Parser.Prettify("<p>## foo</p>"));
    }

    // Contents are parsed as inlines:
    [Fact]
    public void LeafBlocksATXHeadings_Example036()
    {
      // Example 36
      // Section: Leaf blocks / ATX headings
      //
      // The following Markdown:
      //   # foo *bar* \*baz\*
      //
      // Should be rendered as:
      //   <h1>foo <em>bar</em> *baz*</h1>

      Parser.Parse("# foo *bar* \\*baz\\*").Is(Parser.Prettify("<h1>foo <em>bar</em> *baz*</h1>"));

      Parser.DoubleParse("# foo *bar* \\*baz\\*").Is(Parser.Prettify("<h1>foo <em>bar</em> *baz*</h1>"));
    }

    // Leading and trailing [whitespace] is ignored in parsing inline content:
    [Fact]
    public void LeafBlocksATXHeadings_Example037()
    {
      // Example 37
      // Section: Leaf blocks / ATX headings
      //
      // The following Markdown:
      //   #                  foo                     
      //
      // Should be rendered as:
      //   <h1>foo</h1>

      Parser.Parse("#                  foo                     ").Is(Parser.Prettify("<h1>foo</h1>"));

      Parser.DoubleParse("#                  foo                     ").Is(Parser.Prettify("<h1>foo</h1>"));
    }

    // One to three spaces indentation are allowed:
    [Fact]
    public void LeafBlocksATXHeadings_Example038()
    {
      // Example 38
      // Section: Leaf blocks / ATX headings
      //
      // The following Markdown:
      //    ### foo
      //     ## foo
      //      # foo
      //
      // Should be rendered as:
      //   <h3>foo</h3>
      //   <h2>foo</h2>
      //   <h1>foo</h1>

      Parser.Parse(" ### foo\n  ## foo\n   # foo").Is(Parser.Prettify("<h3>foo</h3>\n<h2>foo</h2>\n<h1>foo</h1>"));

      Parser.DoubleParse(" ### foo\n  ## foo\n   # foo").Is(Parser.Prettify("<h3>foo</h3>\n<h2>foo</h2>\n<h1>foo</h1>"));
    }

    // Four spaces are too much:
    [Fact]
    public void LeafBlocksATXHeadings_Example039()
    {
      // Example 39
      // Section: Leaf blocks / ATX headings
      //
      // The following Markdown:
      //       # foo
      //
      // Should be rendered as:
      //   <pre><code># foo
      //   </code></pre>

      Parser.Parse("    # foo").Is(Parser.Prettify("<pre><code># foo\n</code></pre>"));

      Parser.DoubleParse("    # foo").Is(Parser.Prettify("<pre><code># foo\n</code></pre>"));
    }

    [Fact]
    public void LeafBlocksATXHeadings_Example040()
    {
      // Example 40
      // Section: Leaf blocks / ATX headings
      //
      // The following Markdown:
      //   foo
      //       # bar
      //
      // Should be rendered as:
      //   <p>foo
      //   # bar</p>

      Parser.Parse("foo\n    # bar").Is(Parser.Prettify("<p>foo\n# bar</p>"));

      Parser.DoubleParse("foo\n    # bar").Is(Parser.Prettify("<p>foo\n# bar</p>"));
    }

    // A closing sequence of `#` characters is optional:
    [Fact]
    public void LeafBlocksATXHeadings_Example041()
    {
      // Example 41
      // Section: Leaf blocks / ATX headings
      //
      // The following Markdown:
      //   ## foo ##
      //     ###   bar    ###
      //
      // Should be rendered as:
      //   <h2>foo</h2>
      //   <h3>bar</h3>

      Parser.Parse("## foo ##\n  ###   bar    ###").Is(Parser.Prettify("<h2>foo</h2>\n<h3>bar</h3>"));

      Parser.DoubleParse("## foo ##\n  ###   bar    ###").Is(Parser.Prettify("<h2>foo</h2>\n<h3>bar</h3>"));
    }

    // It need not be the same length as the opening sequence:
    [Fact]
    public void LeafBlocksATXHeadings_Example042()
    {
      // Example 42
      // Section: Leaf blocks / ATX headings
      //
      // The following Markdown:
      //   # foo ##################################
      //   ##### foo ##
      //
      // Should be rendered as:
      //   <h1>foo</h1>
      //   <h5>foo</h5>

      Parser.Parse("# foo ##################################\n##### foo ##").Is(Parser.Prettify("<h1>foo</h1>\n<h5>foo</h5>"));

      Parser.DoubleParse("# foo ##################################\n##### foo ##").Is(Parser.Prettify("<h1>foo</h1>\n<h5>foo</h5>"));
    }

    // Spaces are allowed after the closing sequence:
    [Fact]
    public void LeafBlocksATXHeadings_Example043()
    {
      // Example 43
      // Section: Leaf blocks / ATX headings
      //
      // The following Markdown:
      //   ### foo ###     
      //
      // Should be rendered as:
      //   <h3>foo</h3>

      Parser.Parse("### foo ###     ").Is(Parser.Prettify("<h3>foo</h3>"));

      Parser.DoubleParse("### foo ###     ").Is(Parser.Prettify("<h3>foo</h3>"));
    }

    // A sequence of `#` characters with anything but [spaces] following it
    // is not a closing sequence, but counts as part of the contents of the
    // heading:
    [Fact]
    public void LeafBlocksATXHeadings_Example044()
    {
      // Example 44
      // Section: Leaf blocks / ATX headings
      //
      // The following Markdown:
      //   ### foo ### b
      //
      // Should be rendered as:
      //   <h3>foo ### b</h3>

      Parser.Parse("### foo ### b").Is(Parser.Prettify("<h3>foo ### b</h3>"));

      Parser.DoubleParse("### foo ### b").Is(Parser.Prettify("<h3>foo ### b</h3>"));
    }

    // The closing sequence must be preceded by a space:
    [Fact]
    public void LeafBlocksATXHeadings_Example045()
    {
      // Example 45
      // Section: Leaf blocks / ATX headings
      //
      // The following Markdown:
      //   # foo#
      //
      // Should be rendered as:
      //   <h1>foo#</h1>

      Parser.Parse("# foo#").Is(Parser.Prettify("<h1>foo#</h1>"));

      Parser.DoubleParse("# foo#").Is(Parser.Prettify("<h1>foo#</h1>"));
    }

    // Backslash-escaped `#` characters do not count as part
    // of the closing sequence:
    [Fact]
    public void LeafBlocksATXHeadings_Example046()
    {
      // Example 46
      // Section: Leaf blocks / ATX headings
      //
      // The following Markdown:
      //   ### foo \###
      //   ## foo #\##
      //   # foo \#
      //
      // Should be rendered as:
      //   <h3>foo ###</h3>
      //   <h2>foo ###</h2>
      //   <h1>foo #</h1>

      Parser.Parse("### foo \\###\n## foo #\\##\n# foo \\#").Is(Parser.Prettify("<h3>foo ###</h3>\n<h2>foo ###</h2>\n<h1>foo #</h1>"));

      Parser.DoubleParse("### foo \\###\n## foo #\\##\n# foo \\#").Is(Parser.Prettify("<h3>foo ###</h3>\n<h2>foo ###</h2>\n<h1>foo #</h1>"));
    }

    // ATX headings need not be separated from surrounding content by blank
    // lines, and they can interrupt paragraphs:
    [Fact]
    public void LeafBlocksATXHeadings_Example047()
    {
      // Example 47
      // Section: Leaf blocks / ATX headings
      //
      // The following Markdown:
      //   ****
      //   ## foo
      //   ****
      //
      // Should be rendered as:
      //   <hr />
      //   <h2>foo</h2>
      //   <hr />

      Parser.Parse("****\n## foo\n****").Is(Parser.Prettify("<hr />\n<h2>foo</h2>\n<hr />"));

      Parser.DoubleParse("****\n## foo\n****").Is(Parser.Prettify("<hr />\n<h2>foo</h2>\n<hr />"));
    }

    [Fact]
    public void LeafBlocksATXHeadings_Example048()
    {
      // Example 48
      // Section: Leaf blocks / ATX headings
      //
      // The following Markdown:
      //   Foo bar
      //   # baz
      //   Bar foo
      //
      // Should be rendered as:
      //   <p>Foo bar</p>
      //   <h1>baz</h1>
      //   <p>Bar foo</p>

      Parser.Parse("Foo bar\n# baz\nBar foo").Is(Parser.Prettify("<p>Foo bar</p>\n<h1>baz</h1>\n<p>Bar foo</p>"));

      Parser.DoubleParse("Foo bar\n# baz\nBar foo").Is(Parser.Prettify("<p>Foo bar</p>\n<h1>baz</h1>\n<p>Bar foo</p>"));
    }

    // ATX headings can be empty:
    [Fact]
    public void LeafBlocksATXHeadings_Example049()
    {
      // Example 49
      // Section: Leaf blocks / ATX headings
      //
      // The following Markdown:
      //   ## 
      //   #
      //   ### ###
      //
      // Should be rendered as:
      //   <h2></h2>
      //   <h1></h1>
      //   <h3></h3>

      Parser.Parse("## \n#\n### ###").Is(Parser.Prettify("<h2></h2>\n<h1></h1>\n<h3></h3>"));

      Parser.DoubleParse("## \n#\n### ###").Is(Parser.Prettify("<h2></h2>\n<h1></h1>\n<h3></h3>"));
    }
  }

  public class TestLeafBlocksSetextHeadings
  {
    // ## Setext headings
    //
    // A [setext heading](@) consists of one or more
    // lines of text, each containing at least one [non-whitespace
    // character], with no more than 3 spaces indentation, followed by
    // a [setext heading underline].  The lines of text must be such
    // that, were they not followed by the setext heading underline,
    // they would be interpreted as a paragraph:  they cannot be
    // interpretable as a [code fence], [ATX heading][ATX headings],
    // [block quote][block quotes], [thematic break][thematic breaks],
    // [list item][list items], or [HTML block][HTML blocks].
    //
    // A [setext heading underline](@) is a sequence of
    // `=` characters or a sequence of `-` characters, with no more than 3
    // spaces indentation and any number of trailing spaces.  If a line
    // containing a single `-` can be interpreted as an
    // empty [list items], it should be interpreted this way
    // and not as a [setext heading underline].
    //
    // The heading is a level 1 heading if `=` characters are used in
    // the [setext heading underline], and a level 2 heading if `-`
    // characters are used.  The contents of the heading are the result
    // of parsing the preceding lines of text as CommonMark inline
    // content.
    //
    // In general, a setext heading need not be preceded or followed by a
    // blank line.  However, it cannot interrupt a paragraph, so when a
    // setext heading comes after a paragraph, a blank line is needed between
    // them.
    //
    // Simple examples:
    [Fact]
    public void LeafBlocksSetextHeadings_Example050()
    {
      // Example 50
      // Section: Leaf blocks / Setext headings
      //
      // The following Markdown:
      //   Foo *bar*
      //   =========
      //  
      //   Foo *bar*
      //   ---------
      //
      // Should be rendered as:
      //   <h1>Foo <em>bar</em></h1>
      //   <h2>Foo <em>bar</em></h2>

      Parser.Parse("Foo *bar*\n=========\n\nFoo *bar*\n---------").Is(Parser.Prettify("<h1>Foo <em>bar</em></h1>\n<h2>Foo <em>bar</em></h2>"));

      Parser.DoubleParse("Foo *bar*\n=========\n\nFoo *bar*\n---------").Is(Parser.Prettify("<h1>Foo <em>bar</em></h1>\n<h2>Foo <em>bar</em></h2>"));
    }

    // The content of the header may span more than one line:
    [Fact]
    public void LeafBlocksSetextHeadings_Example051()
    {
      // Example 51
      // Section: Leaf blocks / Setext headings
      //
      // The following Markdown:
      //   Foo *bar
      //   baz*
      //   ====
      //
      // Should be rendered as:
      //   <h1>Foo <em>bar
      //   baz</em></h1>

      Parser.Parse("Foo *bar\nbaz*\n====").Is(Parser.Prettify("<h1>Foo <em>bar\nbaz</em></h1>"));

      Parser.DoubleParse("Foo *bar\nbaz*\n====").Is(Parser.Prettify("<h1>Foo <em>bar\nbaz</em></h1>"));
    }

    // The contents are the result of parsing the headings's raw
    // content as inlines.  The heading's raw content is formed by
    // concatenating the lines and removing initial and final
    // [whitespace].
    [Fact]
    public void LeafBlocksSetextHeadings_Example052()
    {
      // Example 52
      // Section: Leaf blocks / Setext headings
      //
      // The following Markdown:
      //     Foo *bar
      //   baz*→
      //   ====
      //
      // Should be rendered as:
      //   <h1>Foo <em>bar
      //   baz</em></h1>

      Parser.Parse("  Foo *bar\nbaz*\t\n====").Is(Parser.Prettify("<h1>Foo <em>bar\nbaz</em></h1>"));

      Parser.DoubleParse("  Foo *bar\nbaz*\t\n====").Is(Parser.Prettify("<h1>Foo <em>bar\nbaz</em></h1>"));
    }

    // The underlining can be any length:
    [Fact]
    public void LeafBlocksSetextHeadings_Example053()
    {
      // Example 53
      // Section: Leaf blocks / Setext headings
      //
      // The following Markdown:
      //   Foo
      //   -------------------------
      //  
      //   Foo
      //   =
      //
      // Should be rendered as:
      //   <h2>Foo</h2>
      //   <h1>Foo</h1>

      Parser.Parse("Foo\n-------------------------\n\nFoo\n=").Is(Parser.Prettify("<h2>Foo</h2>\n<h1>Foo</h1>"));

      Parser.DoubleParse("Foo\n-------------------------\n\nFoo\n=").Is(Parser.Prettify("<h2>Foo</h2>\n<h1>Foo</h1>"));
    }

    // The heading content can be indented up to three spaces, and need
    // not line up with the underlining:
    [Fact]
    public void LeafBlocksSetextHeadings_Example054()
    {
      // Example 54
      // Section: Leaf blocks / Setext headings
      //
      // The following Markdown:
      //      Foo
      //   ---
      //  
      //     Foo
      //   -----
      //  
      //     Foo
      //     ===
      //
      // Should be rendered as:
      //   <h2>Foo</h2>
      //   <h2>Foo</h2>
      //   <h1>Foo</h1>

      Parser.Parse("   Foo\n---\n\n  Foo\n-----\n\n  Foo\n  ===").Is(Parser.Prettify("<h2>Foo</h2>\n<h2>Foo</h2>\n<h1>Foo</h1>"));

      Parser.DoubleParse("   Foo\n---\n\n  Foo\n-----\n\n  Foo\n  ===").Is(Parser.Prettify("<h2>Foo</h2>\n<h2>Foo</h2>\n<h1>Foo</h1>"));
    }

    // Four spaces indent is too much:
    [Fact]
    public void LeafBlocksSetextHeadings_Example055()
    {
      // Example 55
      // Section: Leaf blocks / Setext headings
      //
      // The following Markdown:
      //       Foo
      //       ---
      //  
      //       Foo
      //   ---
      //
      // Should be rendered as:
      //   <pre><code>Foo
      //   ---
      //  
      //   Foo
      //   </code></pre>
      //   <hr />

      Parser.Parse("    Foo\n    ---\n\n    Foo\n---").Is(Parser.Prettify("<pre><code>Foo\n---\n\nFoo\n</code></pre>\n<hr />"));

      Parser.DoubleParse("    Foo\n    ---\n\n    Foo\n---").Is(Parser.Prettify("<pre><code>Foo\n---\n\nFoo\n</code></pre>\n<hr />"));
    }

    // The setext heading underline can be indented up to three spaces, and
    // may have trailing spaces:
    [Fact]
    public void LeafBlocksSetextHeadings_Example056()
    {
      // Example 56
      // Section: Leaf blocks / Setext headings
      //
      // The following Markdown:
      //   Foo
      //      ----      
      //
      // Should be rendered as:
      //   <h2>Foo</h2>

      Parser.Parse("Foo\n   ----      ").Is(Parser.Prettify("<h2>Foo</h2>"));

      Parser.DoubleParse("Foo\n   ----      ").Is(Parser.Prettify("<h2>Foo</h2>"));
    }

    // Four spaces is too much:
    [Fact]
    public void LeafBlocksSetextHeadings_Example057()
    {
      // Example 57
      // Section: Leaf blocks / Setext headings
      //
      // The following Markdown:
      //   Foo
      //       ---
      //
      // Should be rendered as:
      //   <p>Foo
      //   ---</p>

      Parser.Parse("Foo\n    ---").Is(Parser.Prettify("<p>Foo\n---</p>"));

      Parser.DoubleParse("Foo\n    ---").Is(Parser.Prettify("<p>Foo\n---</p>"));
    }

    // The setext heading underline cannot contain internal spaces:
    [Fact]
    public void LeafBlocksSetextHeadings_Example058()
    {
      // Example 58
      // Section: Leaf blocks / Setext headings
      //
      // The following Markdown:
      //   Foo
      //   = =
      //  
      //   Foo
      //   --- -
      //
      // Should be rendered as:
      //   <p>Foo
      //   = =</p>
      //   <p>Foo</p>
      //   <hr />

      Parser.Parse("Foo\n= =\n\nFoo\n--- -").Is(Parser.Prettify("<p>Foo\n= =</p>\n<p>Foo</p>\n<hr />"));

      Parser.DoubleParse("Foo\n= =\n\nFoo\n--- -").Is(Parser.Prettify("<p>Foo\n= =</p>\n<p>Foo</p>\n<hr />"));
    }

    // Trailing spaces in the content line do not cause a line break:
    [Fact]
    public void LeafBlocksSetextHeadings_Example059()
    {
      // Example 59
      // Section: Leaf blocks / Setext headings
      //
      // The following Markdown:
      //   Foo  
      //   -----
      //
      // Should be rendered as:
      //   <h2>Foo</h2>

      Parser.Parse("Foo  \n-----").Is(Parser.Prettify("<h2>Foo</h2>"));

      Parser.DoubleParse("Foo  \n-----").Is(Parser.Prettify("<h2>Foo</h2>"));
    }

    // Nor does a backslash at the end:
    [Fact]
    public void LeafBlocksSetextHeadings_Example060()
    {
      // Example 60
      // Section: Leaf blocks / Setext headings
      //
      // The following Markdown:
      //   Foo\
      //   ----
      //
      // Should be rendered as:
      //   <h2>Foo\</h2>

      Parser.Parse("Foo\\\n----").Is(Parser.Prettify("<h2>Foo\\</h2>"));

      Parser.DoubleParse("Foo\\\n----").Is(Parser.Prettify("<h2>Foo\\</h2>"));
    }

    // Since indicators of block structure take precedence over
    // indicators of inline structure, the following are setext headings:
    [Fact]
    public void LeafBlocksSetextHeadings_Example061()
    {
      // Example 61
      // Section: Leaf blocks / Setext headings
      //
      // The following Markdown:
      //   `Foo
      //   ----
      //   `
      //  
      //   <a title="a lot
      //   ---
      //   of dashes"/>
      //
      // Should be rendered as:
      //   <h2>`Foo</h2>
      //   <p>`</p>
      //   <h2>&lt;a title=&quot;a lot</h2>
      //   <p>of dashes&quot;/&gt;</p>

      Parser.Parse("`Foo\n----\n`\n\n<a title=\"a lot\n---\nof dashes\"/>").Is(Parser.Prettify("<h2>`Foo</h2>\n<p>`</p>\n<h2>&lt;a title=&quot;a lot</h2>\n<p>of dashes&quot;/&gt;</p>"));

      Parser.DoubleParse("`Foo\n----\n`\n\n<a title=\"a lot\n---\nof dashes\"/>").Is(Parser.Prettify("<h2>`Foo</h2>\n<p>`</p>\n<h2>&lt;a title=&quot;a lot</h2>\n<p>of dashes&quot;/&gt;</p>"));
    }

    // The setext heading underline cannot be a [lazy continuation
    // line] in a list item or block quote:
    [Fact]
    public void LeafBlocksSetextHeadings_Example062()
    {
      // Example 62
      // Section: Leaf blocks / Setext headings
      //
      // The following Markdown:
      //   > Foo
      //   ---
      //
      // Should be rendered as:
      //   <blockquote>
      //   <p>Foo</p>
      //   </blockquote>
      //   <hr />

      Parser.Parse("> Foo\n---").Is(Parser.Prettify("<blockquote>\n<p>Foo</p>\n</blockquote>\n<hr />"));

      Parser.DoubleParse("> Foo\n---").Is(Parser.Prettify("<blockquote>\n<p>Foo</p>\n</blockquote>\n<hr />"));
    }

    [Fact]
    public void LeafBlocksSetextHeadings_Example063()
    {
      // Example 63
      // Section: Leaf blocks / Setext headings
      //
      // The following Markdown:
      //   > foo
      //   bar
      //   ===
      //
      // Should be rendered as:
      //   <blockquote>
      //   <p>foo
      //   bar
      //   ===</p>
      //   </blockquote>

      Parser.Parse("> foo\nbar\n===").Is(Parser.Prettify("<blockquote>\n<p>foo\nbar\n===</p>\n</blockquote>"));

      Parser.DoubleParse("> foo\nbar\n===").Is(Parser.Prettify("<blockquote>\n<p>foo\nbar\n===</p>\n</blockquote>"));
    }

    [Fact]
    public void LeafBlocksSetextHeadings_Example064()
    {
      // Example 64
      // Section: Leaf blocks / Setext headings
      //
      // The following Markdown:
      //   - Foo
      //   ---
      //
      // Should be rendered as:
      //   <ul>
      //   <li>Foo</li>
      //   </ul>
      //   <hr />

      Parser.Parse("- Foo\n---").Is(Parser.Prettify("<ul>\n<li>Foo</li>\n</ul>\n<hr />"));

      Parser.DoubleParse("- Foo\n---").Is(Parser.Prettify("<ul>\n<li>Foo</li>\n</ul>\n<hr />"));
    }

    // A blank line is needed between a paragraph and a following
    // setext heading, since otherwise the paragraph becomes part
    // of the heading's content:
    [Fact]
    public void LeafBlocksSetextHeadings_Example065()
    {
      // Example 65
      // Section: Leaf blocks / Setext headings
      //
      // The following Markdown:
      //   Foo
      //   Bar
      //   ---
      //
      // Should be rendered as:
      //   <h2>Foo
      //   Bar</h2>

      Parser.Parse("Foo\nBar\n---").Is(Parser.Prettify("<h2>Foo\nBar</h2>"));

      Parser.DoubleParse("Foo\nBar\n---").Is(Parser.Prettify("<h2>Foo\nBar</h2>"));
    }

    // But in general a blank line is not required before or after
    // setext headings:
    [Fact]
    public void LeafBlocksSetextHeadings_Example066()
    {
      // Example 66
      // Section: Leaf blocks / Setext headings
      //
      // The following Markdown:
      //   ---
      //   Foo
      //   ---
      //   Bar
      //   ---
      //   Baz
      //
      // Should be rendered as:
      //   <hr />
      //   <h2>Foo</h2>
      //   <h2>Bar</h2>
      //   <p>Baz</p>

      Parser.Parse("---\nFoo\n---\nBar\n---\nBaz").Is(Parser.Prettify("<hr />\n<h2>Foo</h2>\n<h2>Bar</h2>\n<p>Baz</p>"));

      Parser.DoubleParse("---\nFoo\n---\nBar\n---\nBaz").Is(Parser.Prettify("<hr />\n<h2>Foo</h2>\n<h2>Bar</h2>\n<p>Baz</p>"));
    }

    // Setext headings cannot be empty:
    [Fact]
    public void LeafBlocksSetextHeadings_Example067()
    {
      // Example 67
      // Section: Leaf blocks / Setext headings
      //
      // The following Markdown:
      //  
      //   ====
      //
      // Should be rendered as:
      //   <p>====</p>

      Parser.Parse("\n====").Is(Parser.Prettify("<p>====</p>"));

      Parser.DoubleParse("\n====").Is(Parser.Prettify("<p>====</p>"));
    }

    // Setext heading text lines must not be interpretable as block
    // constructs other than paragraphs.  So, the line of dashes
    // in these examples gets interpreted as a thematic break:
    [Fact]
    public void LeafBlocksSetextHeadings_Example068()
    {
      // Example 68
      // Section: Leaf blocks / Setext headings
      //
      // The following Markdown:
      //   ---
      //   ---
      //
      // Should be rendered as:
      //   <hr />
      //   <hr />

      Parser.Parse("---\n---").Is(Parser.Prettify("<hr />\n<hr />"));

      Parser.DoubleParse("---\n---").Is(Parser.Prettify("<hr />\n<hr />"));
    }

    [Fact]
    public void LeafBlocksSetextHeadings_Example069()
    {
      // Example 69
      // Section: Leaf blocks / Setext headings
      //
      // The following Markdown:
      //   - foo
      //   -----
      //
      // Should be rendered as:
      //   <ul>
      //   <li>foo</li>
      //   </ul>
      //   <hr />

      Parser.Parse("- foo\n-----").Is(Parser.Prettify("<ul>\n<li>foo</li>\n</ul>\n<hr />"));

      Parser.DoubleParse("- foo\n-----").Is(Parser.Prettify("<ul>\n<li>foo</li>\n</ul>\n<hr />"));
    }

    [Fact]
    public void LeafBlocksSetextHeadings_Example070()
    {
      // Example 70
      // Section: Leaf blocks / Setext headings
      //
      // The following Markdown:
      //       foo
      //   ---
      //
      // Should be rendered as:
      //   <pre><code>foo
      //   </code></pre>
      //   <hr />

      Parser.Parse("    foo\n---").Is(Parser.Prettify("<pre><code>foo\n</code></pre>\n<hr />"));

      Parser.DoubleParse("    foo\n---").Is(Parser.Prettify("<pre><code>foo\n</code></pre>\n<hr />"));
    }

    [Fact]
    public void LeafBlocksSetextHeadings_Example071()
    {
      // Example 71
      // Section: Leaf blocks / Setext headings
      //
      // The following Markdown:
      //   > foo
      //   -----
      //
      // Should be rendered as:
      //   <blockquote>
      //   <p>foo</p>
      //   </blockquote>
      //   <hr />

      Parser.Parse("> foo\n-----").Is(Parser.Prettify("<blockquote>\n<p>foo</p>\n</blockquote>\n<hr />"));

      Parser.DoubleParse("> foo\n-----").Is(Parser.Prettify("<blockquote>\n<p>foo</p>\n</blockquote>\n<hr />"));
    }

    // If you want a heading with `> foo` as its literal text, you can
    // use backslash escapes:
    [Fact]
    public void LeafBlocksSetextHeadings_Example072()
    {
      // Example 72
      // Section: Leaf blocks / Setext headings
      //
      // The following Markdown:
      //   \> foo
      //   ------
      //
      // Should be rendered as:
      //   <h2>&gt; foo</h2>

      Parser.Parse("\\> foo\n------").Is(Parser.Prettify("<h2>&gt; foo</h2>"));

      Parser.DoubleParse("\\> foo\n------").Is(Parser.Prettify("<h2>&gt; foo</h2>"));
    }

    // **Compatibility note:**  Most existing Markdown implementations
    // do not allow the text of setext headings to span multiple lines.
    // But there is no consensus about how to interpret
    //
    // ``` markdown
    // Foo
    // bar
    // ---
    // baz
    // ```
    //
    // One can find four different interpretations:
    //
    // 1. paragraph "Foo", heading "bar", paragraph "baz"
    // 2. paragraph "Foo bar", thematic break, paragraph "baz"
    // 3. paragraph "Foo bar --- baz"
    // 4. heading "Foo bar", paragraph "baz"
    //
    // We find interpretation 4 most natural, and interpretation 4
    // increases the expressive power of CommonMark, by allowing
    // multiline headings.  Authors who want interpretation 1 can
    // put a blank line after the first paragraph:
    [Fact]
    public void LeafBlocksSetextHeadings_Example073()
    {
      // Example 73
      // Section: Leaf blocks / Setext headings
      //
      // The following Markdown:
      //   Foo
      //  
      //   bar
      //   ---
      //   baz
      //
      // Should be rendered as:
      //   <p>Foo</p>
      //   <h2>bar</h2>
      //   <p>baz</p>

      Parser.Parse("Foo\n\nbar\n---\nbaz").Is(Parser.Prettify("<p>Foo</p>\n<h2>bar</h2>\n<p>baz</p>"));

      Parser.DoubleParse("Foo\n\nbar\n---\nbaz").Is(Parser.Prettify("<p>Foo</p>\n<h2>bar</h2>\n<p>baz</p>"));
    }

    // Authors who want interpretation 2 can put blank lines around
    // the thematic break,
    [Fact]
    public void LeafBlocksSetextHeadings_Example074()
    {
      // Example 74
      // Section: Leaf blocks / Setext headings
      //
      // The following Markdown:
      //   Foo
      //   bar
      //  
      //   ---
      //  
      //   baz
      //
      // Should be rendered as:
      //   <p>Foo
      //   bar</p>
      //   <hr />
      //   <p>baz</p>

      Parser.Parse("Foo\nbar\n\n---\n\nbaz").Is(Parser.Prettify("<p>Foo\nbar</p>\n<hr />\n<p>baz</p>"));

      Parser.DoubleParse("Foo\nbar\n\n---\n\nbaz").Is(Parser.Prettify("<p>Foo\nbar</p>\n<hr />\n<p>baz</p>"));
    }

    // or use a thematic break that cannot count as a [setext heading
    // underline], such as
    [Fact]
    public void LeafBlocksSetextHeadings_Example075()
    {
      // Example 75
      // Section: Leaf blocks / Setext headings
      //
      // The following Markdown:
      //   Foo
      //   bar
      //   * * *
      //   baz
      //
      // Should be rendered as:
      //   <p>Foo
      //   bar</p>
      //   <hr />
      //   <p>baz</p>

      Parser.Parse("Foo\nbar\n* * *\nbaz").Is(Parser.Prettify("<p>Foo\nbar</p>\n<hr />\n<p>baz</p>"));

      Parser.DoubleParse("Foo\nbar\n* * *\nbaz").Is(Parser.Prettify("<p>Foo\nbar</p>\n<hr />\n<p>baz</p>"));
    }

    // Authors who want interpretation 3 can use backslash escapes:
    [Fact]
    public void LeafBlocksSetextHeadings_Example076()
    {
      // Example 76
      // Section: Leaf blocks / Setext headings
      //
      // The following Markdown:
      //   Foo
      //   bar
      //   \---
      //   baz
      //
      // Should be rendered as:
      //   <p>Foo
      //   bar
      //   ---
      //   baz</p>

      Parser.Parse("Foo\nbar\n\\---\nbaz").Is(Parser.Prettify("<p>Foo\nbar\n---\nbaz</p>"));

      Parser.DoubleParse("Foo\nbar\n\\---\nbaz").Is(Parser.Prettify("<p>Foo\nbar\n---\nbaz</p>"));
    }
  }

  public class TestLeafBlocksIndentedCodeBlocks
  {
    // ## Indented code blocks
    //
    // An [indented code block](@) is composed of one or more
    // [indented chunks] separated by blank lines.
    // An [indented chunk](@) is a sequence of non-blank lines,
    // each indented four or more spaces. The contents of the code block are
    // the literal contents of the lines, including trailing
    // [line endings], minus four spaces of indentation.
    // An indented code block has no [info string].
    //
    // An indented code block cannot interrupt a paragraph, so there must be
    // a blank line between a paragraph and a following indented code block.
    // (A blank line is not needed, however, between a code block and a following
    // paragraph.)
    [Fact]
    public void LeafBlocksIndentedCodeBlocks_Example077()
    {
      // Example 77
      // Section: Leaf blocks / Indented code blocks
      //
      // The following Markdown:
      //       a simple
      //         indented code block
      //
      // Should be rendered as:
      //   <pre><code>a simple
      //     indented code block
      //   </code></pre>

      Parser.Parse("    a simple\n      indented code block").Is(Parser.Prettify("<pre><code>a simple\n  indented code block\n</code></pre>"));

      Parser.DoubleParse("    a simple\n      indented code block").Is(Parser.Prettify("<pre><code>a simple\n  indented code block\n</code></pre>"));
    }

    // If there is any ambiguity between an interpretation of indentation
    // as a code block and as indicating that material belongs to a [list
    // item][list items], the list item interpretation takes precedence:
    [Fact]
    public void LeafBlocksIndentedCodeBlocks_Example078()
    {
      // Example 78
      // Section: Leaf blocks / Indented code blocks
      //
      // The following Markdown:
      //     - foo
      //  
      //       bar
      //
      // Should be rendered as:
      //   <ul>
      //   <li>
      //   <p>foo</p>
      //   <p>bar</p>
      //   </li>
      //   </ul>

      Parser.Parse("  - foo\n\n    bar").Is(Parser.Prettify("<ul>\n<li>\n<p>foo</p>\n<p>bar</p>\n</li>\n</ul>"));

      Parser.DoubleParse("  - foo\n\n    bar").Is(Parser.Prettify("<ul>\n<li>\n<p>foo</p>\n<p>bar</p>\n</li>\n</ul>"));
    }

    [Fact]
    public void LeafBlocksIndentedCodeBlocks_Example079()
    {
      // Example 79
      // Section: Leaf blocks / Indented code blocks
      //
      // The following Markdown:
      //   1.  foo
      //  
      //       - bar
      //
      // Should be rendered as:
      //   <ol>
      //   <li>
      //   <p>foo</p>
      //   <ul>
      //   <li>bar</li>
      //   </ul>
      //   </li>
      //   </ol>

      Parser.Parse("1.  foo\n\n    - bar").Is(Parser.Prettify("<ol>\n<li>\n<p>foo</p>\n<ul>\n<li>bar</li>\n</ul>\n</li>\n</ol>"));

      Parser.DoubleParse("1.  foo\n\n    - bar").Is(Parser.Prettify("<ol>\n<li>\n<p>foo</p>\n<ul>\n<li>bar</li>\n</ul>\n</li>\n</ol>"));
    }

    // The contents of a code block are literal text, and do not get parsed
    // as Markdown:
    [Fact]
    public void LeafBlocksIndentedCodeBlocks_Example080()
    {
      // Example 80
      // Section: Leaf blocks / Indented code blocks
      //
      // The following Markdown:
      //       <a/>
      //       *hi*
      //  
      //       - one
      //
      // Should be rendered as:
      //   <pre><code>&lt;a/&gt;
      //   *hi*
      //  
      //   - one
      //   </code></pre>

      Parser.Parse("    <a/>\n    *hi*\n\n    - one").Is(Parser.Prettify("<pre><code>&lt;a/&gt;\n*hi*\n\n- one\n</code></pre>"));

      Parser.DoubleParse("    <a/>\n    *hi*\n\n    - one").Is(Parser.Prettify("<pre><code>&lt;a/&gt;\n*hi*\n\n- one\n</code></pre>"));
    }

    // Here we have three chunks separated by blank lines:
    [Fact]
    public void LeafBlocksIndentedCodeBlocks_Example081()
    {
      // Example 81
      // Section: Leaf blocks / Indented code blocks
      //
      // The following Markdown:
      //       chunk1
      //  
      //       chunk2
      //     
      //    
      //    
      //       chunk3
      //
      // Should be rendered as:
      //   <pre><code>chunk1
      //  
      //   chunk2
      //  
      //  
      //  
      //   chunk3
      //   </code></pre>

      Parser.Parse("    chunk1\n\n    chunk2\n  \n \n \n    chunk3").Is(Parser.Prettify("<pre><code>chunk1\n\nchunk2\n\n\n\nchunk3\n</code></pre>"));

      Parser.DoubleParse("    chunk1\n\n    chunk2\n  \n \n \n    chunk3").Is(Parser.Prettify("<pre><code>chunk1\n\nchunk2\n\n\n\nchunk3\n</code></pre>"));
    }

    // Any initial spaces beyond four will be included in the content, even
    // in interior blank lines:
    [Fact]
    public void LeafBlocksIndentedCodeBlocks_Example082()
    {
      // Example 82
      // Section: Leaf blocks / Indented code blocks
      //
      // The following Markdown:
      //       chunk1
      //         
      //         chunk2
      //
      // Should be rendered as:
      //   <pre><code>chunk1
      //     
      //     chunk2
      //   </code></pre>

      Parser.Parse("    chunk1\n      \n      chunk2").Is(Parser.Prettify("<pre><code>chunk1\n  \n  chunk2\n</code></pre>"));

      Parser.DoubleParse("    chunk1\n      \n      chunk2").Is(Parser.Prettify("<pre><code>chunk1\n  \n  chunk2\n</code></pre>"));
    }

    // An indented code block cannot interrupt a paragraph.  (This
    // allows hanging indents and the like.)
    [Fact]
    public void LeafBlocksIndentedCodeBlocks_Example083()
    {
      // Example 83
      // Section: Leaf blocks / Indented code blocks
      //
      // The following Markdown:
      //   Foo
      //       bar
      //  
      //
      // Should be rendered as:
      //   <p>Foo
      //   bar</p>

      Parser.Parse("Foo\n    bar\n").Is(Parser.Prettify("<p>Foo\nbar</p>"));

      Parser.DoubleParse("Foo\n    bar\n").Is(Parser.Prettify("<p>Foo\nbar</p>"));
    }

    // However, any non-blank line with fewer than four leading spaces ends
    // the code block immediately.  So a paragraph may occur immediately
    // after indented code:
    [Fact]
    public void LeafBlocksIndentedCodeBlocks_Example084()
    {
      // Example 84
      // Section: Leaf blocks / Indented code blocks
      //
      // The following Markdown:
      //       foo
      //   bar
      //
      // Should be rendered as:
      //   <pre><code>foo
      //   </code></pre>
      //   <p>bar</p>

      Parser.Parse("    foo\nbar").Is(Parser.Prettify("<pre><code>foo\n</code></pre>\n<p>bar</p>"));

      Parser.DoubleParse("    foo\nbar").Is(Parser.Prettify("<pre><code>foo\n</code></pre>\n<p>bar</p>"));
    }

    // And indented code can occur immediately before and after other kinds of
    // blocks:
    [Fact]
    public void LeafBlocksIndentedCodeBlocks_Example085()
    {
      // Example 85
      // Section: Leaf blocks / Indented code blocks
      //
      // The following Markdown:
      //   # Heading
      //       foo
      //   Heading
      //   ------
      //       foo
      //   ----
      //
      // Should be rendered as:
      //   <h1>Heading</h1>
      //   <pre><code>foo
      //   </code></pre>
      //   <h2>Heading</h2>
      //   <pre><code>foo
      //   </code></pre>
      //   <hr />

      Parser.Parse("# Heading\n    foo\nHeading\n------\n    foo\n----").Is(Parser.Prettify("<h1>Heading</h1>\n<pre><code>foo\n</code></pre>\n<h2>Heading</h2>\n<pre><code>foo\n</code></pre>\n<hr />"));

      Parser.DoubleParse("# Heading\n    foo\nHeading\n------\n    foo\n----").Is(Parser.Prettify("<h1>Heading</h1>\n<pre><code>foo\n</code></pre>\n<h2>Heading</h2>\n<pre><code>foo\n</code></pre>\n<hr />"));
    }

    // The first line can be indented more than four spaces:
    [Fact]
    public void LeafBlocksIndentedCodeBlocks_Example086()
    {
      // Example 86
      // Section: Leaf blocks / Indented code blocks
      //
      // The following Markdown:
      //           foo
      //       bar
      //
      // Should be rendered as:
      //   <pre><code>    foo
      //   bar
      //   </code></pre>

      Parser.Parse("        foo\n    bar").Is(Parser.Prettify("<pre><code>    foo\nbar\n</code></pre>"));

      Parser.DoubleParse("        foo\n    bar").Is(Parser.Prettify("<pre><code>    foo\nbar\n</code></pre>"));
    }

    // Blank lines preceding or following an indented code block
    // are not included in it:
    [Fact]
    public void LeafBlocksIndentedCodeBlocks_Example087()
    {
      // Example 87
      // Section: Leaf blocks / Indented code blocks
      //
      // The following Markdown:
      //  
      //       
      //       foo
      //       
      //  
      //
      // Should be rendered as:
      //   <pre><code>foo
      //   </code></pre>

      Parser.Parse("\n    \n    foo\n    \n").Is(Parser.Prettify("<pre><code>foo\n</code></pre>"));

      Parser.DoubleParse("\n    \n    foo\n    \n").Is(Parser.Prettify("<pre><code>foo\n</code></pre>"));
    }

    // Trailing spaces are included in the code block's content:
    [Fact]
    public void LeafBlocksIndentedCodeBlocks_Example088()
    {
      // Example 88
      // Section: Leaf blocks / Indented code blocks
      //
      // The following Markdown:
      //       foo  
      //
      // Should be rendered as:
      //   <pre><code>foo  
      //   </code></pre>

      Parser.Parse("    foo  ").Is(Parser.Prettify("<pre><code>foo  \n</code></pre>"));

      Parser.DoubleParse("    foo  ").Is(Parser.Prettify("<pre><code>foo  \n</code></pre>"));
    }
  }

  public class TestLeafBlocksFencedCodeBlocks
  {
    // ## Fenced code blocks
    //
    // A [code fence](@) is a sequence
    // of at least three consecutive backtick characters (`` ` ``) or
    // tildes (`~`).  (Tildes and backticks cannot be mixed.)
    // A [fenced code block](@)
    // begins with a code fence, indented no more than three spaces.
    //
    // The line with the opening code fence may optionally contain some text
    // following the code fence; this is trimmed of leading and trailing
    // whitespace and called the [info string](@). If the [info string] comes
    // after a backtick fence, it may not contain any backtick
    // characters.  (The reason for this restriction is that otherwise
    // some inline code would be incorrectly interpreted as the
    // beginning of a fenced code block.)
    //
    // The content of the code block consists of all subsequent lines, until
    // a closing [code fence] of the same type as the code block
    // began with (backticks or tildes), and with at least as many backticks
    // or tildes as the opening code fence.  If the leading code fence is
    // indented N spaces, then up to N spaces of indentation are removed from
    // each line of the content (if present).  (If a content line is not
    // indented, it is preserved unchanged.  If it is indented less than N
    // spaces, all of the indentation is removed.)
    //
    // The closing code fence may be indented up to three spaces, and may be
    // followed only by spaces, which are ignored.  If the end of the
    // containing block (or document) is reached and no closing code fence
    // has been found, the code block contains all of the lines after the
    // opening code fence until the end of the containing block (or
    // document).  (An alternative spec would require backtracking in the
    // event that a closing code fence is not found.  But this makes parsing
    // much less efficient, and there seems to be no real down side to the
    // behavior described here.)
    //
    // A fenced code block may interrupt a paragraph, and does not require
    // a blank line either before or after.
    //
    // The content of a code fence is treated as literal text, not parsed
    // as inlines.  The first word of the [info string] is typically used to
    // specify the language of the code sample, and rendered in the `class`
    // attribute of the `code` tag.  However, this spec does not mandate any
    // particular treatment of the [info string].
    //
    // Here is a simple example with backticks:
    [Fact]
    public void LeafBlocksFencedCodeBlocks_Example089()
    {
      // Example 89
      // Section: Leaf blocks / Fenced code blocks
      //
      // The following Markdown:
      //   ```
      //   <
      //    >
      //   ```
      //
      // Should be rendered as:
      //   <pre><code>&lt;
      //    &gt;
      //   </code></pre>

      Parser.Parse("```\n<\n >\n```").Is(Parser.Prettify("<pre><code>&lt;\n &gt;\n</code></pre>"));

      Parser.DoubleParse("```\n<\n >\n```").Is(Parser.Prettify("<pre><code>&lt;\n &gt;\n</code></pre>"));
    }

    // With tildes:
    [Fact]
    public void LeafBlocksFencedCodeBlocks_Example090()
    {
      // Example 90
      // Section: Leaf blocks / Fenced code blocks
      //
      // The following Markdown:
      //   ~~~
      //   <
      //    >
      //   ~~~
      //
      // Should be rendered as:
      //   <pre><code>&lt;
      //    &gt;
      //   </code></pre>

      Parser.Parse("~~~\n<\n >\n~~~").Is(Parser.Prettify("<pre><code>&lt;\n &gt;\n</code></pre>"));

      Parser.DoubleParse("~~~\n<\n >\n~~~").Is(Parser.Prettify("<pre><code>&lt;\n &gt;\n</code></pre>"));
    }

    // Fewer than three backticks is not enough:
    [Fact]
    public void LeafBlocksFencedCodeBlocks_Example091()
    {
      // Example 91
      // Section: Leaf blocks / Fenced code blocks
      //
      // The following Markdown:
      //   ``
      //   foo
      //   ``
      //
      // Should be rendered as:
      //   <p><code>foo</code></p>

      Parser.Parse("``\nfoo\n``").Is(Parser.Prettify("<p><code>foo</code></p>"));

      Parser.DoubleParse("``\nfoo\n``").Is(Parser.Prettify("<p><code>foo</code></p>"));
    }

    // The closing code fence must use the same character as the opening
    // fence:
    [Fact]
    public void LeafBlocksFencedCodeBlocks_Example092()
    {
      // Example 92
      // Section: Leaf blocks / Fenced code blocks
      //
      // The following Markdown:
      //   ```
      //   aaa
      //   ~~~
      //   ```
      //
      // Should be rendered as:
      //   <pre><code>aaa
      //   ~~~
      //   </code></pre>

      Parser.Parse("```\naaa\n~~~\n```").Is(Parser.Prettify("<pre><code>aaa\n~~~\n</code></pre>"));

      Parser.DoubleParse("```\naaa\n~~~\n```").Is(Parser.Prettify("<pre><code>aaa\n~~~\n</code></pre>"));
    }

    [Fact]
    public void LeafBlocksFencedCodeBlocks_Example093()
    {
      // Example 93
      // Section: Leaf blocks / Fenced code blocks
      //
      // The following Markdown:
      //   ~~~
      //   aaa
      //   ```
      //   ~~~
      //
      // Should be rendered as:
      //   <pre><code>aaa
      //   ```
      //   </code></pre>

      Parser.Parse("~~~\naaa\n```\n~~~").Is(Parser.Prettify("<pre><code>aaa\n```\n</code></pre>"));

      Parser.DoubleParse("~~~\naaa\n```\n~~~").Is(Parser.Prettify("<pre><code>aaa\n```\n</code></pre>"));
    }

    // The closing code fence must be at least as long as the opening fence:
    [Fact]
    public void LeafBlocksFencedCodeBlocks_Example094()
    {
      // Example 94
      // Section: Leaf blocks / Fenced code blocks
      //
      // The following Markdown:
      //   ````
      //   aaa
      //   ```
      //   ``````
      //
      // Should be rendered as:
      //   <pre><code>aaa
      //   ```
      //   </code></pre>

      Parser.Parse("````\naaa\n```\n``````").Is(Parser.Prettify("<pre><code>aaa\n```\n</code></pre>"));

      Parser.DoubleParse("````\naaa\n```\n``````").Is(Parser.Prettify("<pre><code>aaa\n```\n</code></pre>"));
    }

    [Fact]
    public void LeafBlocksFencedCodeBlocks_Example095()
    {
      // Example 95
      // Section: Leaf blocks / Fenced code blocks
      //
      // The following Markdown:
      //   ~~~~
      //   aaa
      //   ~~~
      //   ~~~~
      //
      // Should be rendered as:
      //   <pre><code>aaa
      //   ~~~
      //   </code></pre>

      Parser.Parse("~~~~\naaa\n~~~\n~~~~").Is(Parser.Prettify("<pre><code>aaa\n~~~\n</code></pre>"));

      Parser.DoubleParse("~~~~\naaa\n~~~\n~~~~").Is(Parser.Prettify("<pre><code>aaa\n~~~\n</code></pre>"));
    }

    // Unclosed code blocks are closed by the end of the document
    // (or the enclosing [block quote][block quotes] or [list item][list items]):
    [Fact]
    public void LeafBlocksFencedCodeBlocks_Example096()
    {
      // Example 96
      // Section: Leaf blocks / Fenced code blocks
      //
      // The following Markdown:
      //   ```
      //
      // Should be rendered as:
      //   <pre><code></code></pre>

      Parser.Parse("```").Is(Parser.Prettify("<pre><code></code></pre>"));

      Parser.DoubleParse("```").Is(Parser.Prettify("<pre><code></code></pre>"));
    }

    [Fact]
    public void LeafBlocksFencedCodeBlocks_Example097()
    {
      // Example 97
      // Section: Leaf blocks / Fenced code blocks
      //
      // The following Markdown:
      //   `````
      //  
      //   ```
      //   aaa
      //
      // Should be rendered as:
      //   <pre><code>
      //   ```
      //   aaa
      //   </code></pre>

      Parser.Parse("`````\n\n```\naaa").Is(Parser.Prettify("<pre><code>\n```\naaa\n</code></pre>"));

      Parser.DoubleParse("`````\n\n```\naaa").Is(Parser.Prettify("<pre><code>\n```\naaa\n</code></pre>"));
    }

    [Fact]
    public void LeafBlocksFencedCodeBlocks_Example098()
    {
      // Example 98
      // Section: Leaf blocks / Fenced code blocks
      //
      // The following Markdown:
      //   > ```
      //   > aaa
      //  
      //   bbb
      //
      // Should be rendered as:
      //   <blockquote>
      //   <pre><code>aaa
      //   </code></pre>
      //   </blockquote>
      //   <p>bbb</p>

      Parser.Parse("> ```\n> aaa\n\nbbb").Is(Parser.Prettify("<blockquote>\n<pre><code>aaa\n</code></pre>\n</blockquote>\n<p>bbb</p>"));

      Parser.DoubleParse("> ```\n> aaa\n\nbbb").Is(Parser.Prettify("<blockquote>\n<pre><code>aaa\n</code></pre>\n</blockquote>\n<p>bbb</p>"));
    }

    // A code block can have all empty lines as its content:
    [Fact]
    public void LeafBlocksFencedCodeBlocks_Example099()
    {
      // Example 99
      // Section: Leaf blocks / Fenced code blocks
      //
      // The following Markdown:
      //   ```
      //  
      //     
      //   ```
      //
      // Should be rendered as:
      //   <pre><code>
      //     
      //   </code></pre>

      Parser.Parse("```\n\n  \n```").Is(Parser.Prettify("<pre><code>\n  \n</code></pre>"));

      Parser.DoubleParse("```\n\n  \n```").Is(Parser.Prettify("<pre><code>\n  \n</code></pre>"));
    }

    // A code block can be empty:
    [Fact]
    public void LeafBlocksFencedCodeBlocks_Example100()
    {
      // Example 100
      // Section: Leaf blocks / Fenced code blocks
      //
      // The following Markdown:
      //   ```
      //   ```
      //
      // Should be rendered as:
      //   <pre><code></code></pre>

      Parser.Parse("```\n```").Is(Parser.Prettify("<pre><code></code></pre>"));

      Parser.DoubleParse("```\n```").Is(Parser.Prettify("<pre><code></code></pre>"));
    }

    // Fences can be indented.  If the opening fence is indented,
    // content lines will have equivalent opening indentation removed,
    // if present:
    [Fact]
    public void LeafBlocksFencedCodeBlocks_Example101()
    {
      // Example 101
      // Section: Leaf blocks / Fenced code blocks
      //
      // The following Markdown:
      //    ```
      //    aaa
      //   aaa
      //   ```
      //
      // Should be rendered as:
      //   <pre><code>aaa
      //   aaa
      //   </code></pre>

      Parser.Parse(" ```\n aaa\naaa\n```").Is(Parser.Prettify("<pre><code>aaa\naaa\n</code></pre>"));

      Parser.DoubleParse(" ```\n aaa\naaa\n```").Is(Parser.Prettify("<pre><code>aaa\naaa\n</code></pre>"));
    }

    [Fact]
    public void LeafBlocksFencedCodeBlocks_Example102()
    {
      // Example 102
      // Section: Leaf blocks / Fenced code blocks
      //
      // The following Markdown:
      //     ```
      //   aaa
      //     aaa
      //   aaa
      //     ```
      //
      // Should be rendered as:
      //   <pre><code>aaa
      //   aaa
      //   aaa
      //   </code></pre>

      Parser.Parse("  ```\naaa\n  aaa\naaa\n  ```").Is(Parser.Prettify("<pre><code>aaa\naaa\naaa\n</code></pre>"));

      Parser.DoubleParse("  ```\naaa\n  aaa\naaa\n  ```").Is(Parser.Prettify("<pre><code>aaa\naaa\naaa\n</code></pre>"));
    }

    [Fact]
    public void LeafBlocksFencedCodeBlocks_Example103()
    {
      // Example 103
      // Section: Leaf blocks / Fenced code blocks
      //
      // The following Markdown:
      //      ```
      //      aaa
      //       aaa
      //     aaa
      //      ```
      //
      // Should be rendered as:
      //   <pre><code>aaa
      //    aaa
      //   aaa
      //   </code></pre>

      Parser.Parse("   ```\n   aaa\n    aaa\n  aaa\n   ```").Is(Parser.Prettify("<pre><code>aaa\n aaa\naaa\n</code></pre>"));

      Parser.DoubleParse("   ```\n   aaa\n    aaa\n  aaa\n   ```").Is(Parser.Prettify("<pre><code>aaa\n aaa\naaa\n</code></pre>"));
    }

    // Four spaces indentation produces an indented code block:
    [Fact]
    public void LeafBlocksFencedCodeBlocks_Example104()
    {
      // Example 104
      // Section: Leaf blocks / Fenced code blocks
      //
      // The following Markdown:
      //       ```
      //       aaa
      //       ```
      //
      // Should be rendered as:
      //   <pre><code>```
      //   aaa
      //   ```
      //   </code></pre>

      Parser.Parse("    ```\n    aaa\n    ```").Is(Parser.Prettify("<pre><code>```\naaa\n```\n</code></pre>"));

      Parser.DoubleParse("    ```\n    aaa\n    ```").Is(Parser.Prettify("<pre><code>```\naaa\n```\n</code></pre>"));
    }

    // Closing fences may be indented by 0-3 spaces, and their indentation
    // need not match that of the opening fence:
    [Fact]
    public void LeafBlocksFencedCodeBlocks_Example105()
    {
      // Example 105
      // Section: Leaf blocks / Fenced code blocks
      //
      // The following Markdown:
      //   ```
      //   aaa
      //     ```
      //
      // Should be rendered as:
      //   <pre><code>aaa
      //   </code></pre>

      Parser.Parse("```\naaa\n  ```").Is(Parser.Prettify("<pre><code>aaa\n</code></pre>"));

      Parser.DoubleParse("```\naaa\n  ```").Is(Parser.Prettify("<pre><code>aaa\n</code></pre>"));
    }

    [Fact]
    public void LeafBlocksFencedCodeBlocks_Example106()
    {
      // Example 106
      // Section: Leaf blocks / Fenced code blocks
      //
      // The following Markdown:
      //      ```
      //   aaa
      //     ```
      //
      // Should be rendered as:
      //   <pre><code>aaa
      //   </code></pre>

      Parser.Parse("   ```\naaa\n  ```").Is(Parser.Prettify("<pre><code>aaa\n</code></pre>"));

      Parser.DoubleParse("   ```\naaa\n  ```").Is(Parser.Prettify("<pre><code>aaa\n</code></pre>"));
    }

    // This is not a closing fence, because it is indented 4 spaces:
    [Fact]
    public void LeafBlocksFencedCodeBlocks_Example107()
    {
      // Example 107
      // Section: Leaf blocks / Fenced code blocks
      //
      // The following Markdown:
      //   ```
      //   aaa
      //       ```
      //
      // Should be rendered as:
      //   <pre><code>aaa
      //       ```
      //   </code></pre>

      Parser.Parse("```\naaa\n    ```").Is(Parser.Prettify("<pre><code>aaa\n    ```\n</code></pre>"));

      Parser.DoubleParse("```\naaa\n    ```").Is(Parser.Prettify("<pre><code>aaa\n    ```\n</code></pre>"));
    }

    // Code fences (opening and closing) cannot contain internal spaces:
    [Fact]
    public void LeafBlocksFencedCodeBlocks_Example108()
    {
      // Example 108
      // Section: Leaf blocks / Fenced code blocks
      //
      // The following Markdown:
      //   ``` ```
      //   aaa
      //
      // Should be rendered as:
      //   <p><code> </code>
      //   aaa</p>

      Parser.Parse("``` ```\naaa").Is(Parser.Prettify("<p><code> </code>\naaa</p>"));

      Parser.DoubleParse("``` ```\naaa").Is(Parser.Prettify("<p><code> </code>\naaa</p>"));
    }

    [Fact]
    public void LeafBlocksFencedCodeBlocks_Example109()
    {
      // Example 109
      // Section: Leaf blocks / Fenced code blocks
      //
      // The following Markdown:
      //   ~~~~~~
      //   aaa
      //   ~~~ ~~
      //
      // Should be rendered as:
      //   <pre><code>aaa
      //   ~~~ ~~
      //   </code></pre>

      Parser.Parse("~~~~~~\naaa\n~~~ ~~").Is(Parser.Prettify("<pre><code>aaa\n~~~ ~~\n</code></pre>"));

      Parser.DoubleParse("~~~~~~\naaa\n~~~ ~~").Is(Parser.Prettify("<pre><code>aaa\n~~~ ~~\n</code></pre>"));
    }

    // Fenced code blocks can interrupt paragraphs, and can be followed
    // directly by paragraphs, without a blank line between:
    [Fact]
    public void LeafBlocksFencedCodeBlocks_Example110()
    {
      // Example 110
      // Section: Leaf blocks / Fenced code blocks
      //
      // The following Markdown:
      //   foo
      //   ```
      //   bar
      //   ```
      //   baz
      //
      // Should be rendered as:
      //   <p>foo</p>
      //   <pre><code>bar
      //   </code></pre>
      //   <p>baz</p>

      Parser.Parse("foo\n```\nbar\n```\nbaz").Is(Parser.Prettify("<p>foo</p>\n<pre><code>bar\n</code></pre>\n<p>baz</p>"));

      Parser.DoubleParse("foo\n```\nbar\n```\nbaz").Is(Parser.Prettify("<p>foo</p>\n<pre><code>bar\n</code></pre>\n<p>baz</p>"));
    }

    // Other blocks can also occur before and after fenced code blocks
    // without an intervening blank line:
    [Fact]
    public void LeafBlocksFencedCodeBlocks_Example111()
    {
      // Example 111
      // Section: Leaf blocks / Fenced code blocks
      //
      // The following Markdown:
      //   foo
      //   ---
      //   ~~~
      //   bar
      //   ~~~
      //   # baz
      //
      // Should be rendered as:
      //   <h2>foo</h2>
      //   <pre><code>bar
      //   </code></pre>
      //   <h1>baz</h1>

      Parser.Parse("foo\n---\n~~~\nbar\n~~~\n# baz").Is(Parser.Prettify("<h2>foo</h2>\n<pre><code>bar\n</code></pre>\n<h1>baz</h1>"));

      Parser.DoubleParse("foo\n---\n~~~\nbar\n~~~\n# baz").Is(Parser.Prettify("<h2>foo</h2>\n<pre><code>bar\n</code></pre>\n<h1>baz</h1>"));
    }

    // An [info string] can be provided after the opening code fence.
    // Although this spec doesn't mandate any particular treatment of
    // the info string, the first word is typically used to specify
    // the language of the code block. In HTML output, the language is
    // normally indicated by adding a class to the `code` element consisting
    // of `language-` followed by the language name.
    [Fact]
    public void LeafBlocksFencedCodeBlocks_Example112()
    {
      // Example 112
      // Section: Leaf blocks / Fenced code blocks
      //
      // The following Markdown:
      //   ```ruby
      //   def foo(x)
      //     return 3
      //   end
      //   ```
      //
      // Should be rendered as:
      //   <pre><code class="language-ruby">def foo(x)
      //     return 3
      //   end
      //   </code></pre>

      Parser.Parse("```ruby\ndef foo(x)\n  return 3\nend\n```").Is(Parser.Prettify("<pre><code class=\"language-ruby\">def foo(x)\n  return 3\nend\n</code></pre>"));

      Parser.DoubleParse("```ruby\ndef foo(x)\n  return 3\nend\n```").Is(Parser.Prettify("<pre><code class=\"language-ruby\">def foo(x)\n  return 3\nend\n</code></pre>"));
    }

    [Fact]
    public void LeafBlocksFencedCodeBlocks_Example113()
    {
      // Example 113
      // Section: Leaf blocks / Fenced code blocks
      //
      // The following Markdown:
      //   ~~~~    ruby startline=3 $%@#$
      //   def foo(x)
      //     return 3
      //   end
      //   ~~~~~~~
      //
      // Should be rendered as:
      //   <pre><code class="language-ruby">def foo(x)
      //     return 3
      //   end
      //   </code></pre>

      Parser.Parse("~~~~    ruby startline=3 $%@#$\ndef foo(x)\n  return 3\nend\n~~~~~~~").Is(Parser.Prettify("<pre><code class=\"language-ruby\">def foo(x)\n  return 3\nend\n</code></pre>"));

      Parser.DoubleParse("~~~~    ruby startline=3 $%@#$\ndef foo(x)\n  return 3\nend\n~~~~~~~").Is(Parser.Prettify("<pre><code class=\"language-ruby\">def foo(x)\n  return 3\nend\n</code></pre>"));
    }

    [Fact]
    public void LeafBlocksFencedCodeBlocks_Example114()
    {
      // Example 114
      // Section: Leaf blocks / Fenced code blocks
      //
      // The following Markdown:
      //   ````;
      //   ````
      //
      // Should be rendered as:
      //   <pre><code class="language-;"></code></pre>

      Parser.Parse("````;\n````").Is(Parser.Prettify("<pre><code class=\"language-;\"></code></pre>"));

      Parser.DoubleParse("````;\n````").Is(Parser.Prettify("<pre><code class=\"language-;\"></code></pre>"));
    }

    // [Info strings] for backtick code blocks cannot contain backticks:
    [Fact]
    public void LeafBlocksFencedCodeBlocks_Example115()
    {
      // Example 115
      // Section: Leaf blocks / Fenced code blocks
      //
      // The following Markdown:
      //   ``` aa ```
      //   foo
      //
      // Should be rendered as:
      //   <p><code>aa</code>
      //   foo</p>

      Parser.Parse("``` aa ```\nfoo").Is(Parser.Prettify("<p><code>aa</code>\nfoo</p>"));

      Parser.DoubleParse("``` aa ```\nfoo").Is(Parser.Prettify("<p><code>aa</code>\nfoo</p>"));
    }

    // [Info strings] for tilde code blocks can contain backticks and tildes:
    [Fact]
    public void LeafBlocksFencedCodeBlocks_Example116()
    {
      // Example 116
      // Section: Leaf blocks / Fenced code blocks
      //
      // The following Markdown:
      //   ~~~ aa ``` ~~~
      //   foo
      //   ~~~
      //
      // Should be rendered as:
      //   <pre><code class="language-aa">foo
      //   </code></pre>

      Parser.Parse("~~~ aa ``` ~~~\nfoo\n~~~").Is(Parser.Prettify("<pre><code class=\"language-aa\">foo\n</code></pre>"));

      Parser.DoubleParse("~~~ aa ``` ~~~\nfoo\n~~~").Is(Parser.Prettify("<pre><code class=\"language-aa\">foo\n</code></pre>"));
    }

    // Closing code fences cannot have [info strings]:
    [Fact]
    public void LeafBlocksFencedCodeBlocks_Example117()
    {
      // Example 117
      // Section: Leaf blocks / Fenced code blocks
      //
      // The following Markdown:
      //   ```
      //   ``` aaa
      //   ```
      //
      // Should be rendered as:
      //   <pre><code>``` aaa
      //   </code></pre>

      Parser.Parse("```\n``` aaa\n```").Is(Parser.Prettify("<pre><code>``` aaa\n</code></pre>"));

      Parser.DoubleParse("```\n``` aaa\n```").Is(Parser.Prettify("<pre><code>``` aaa\n</code></pre>"));
    }
  }

  public class TestLeafBlocksHTMLBlocks
  {
    // ## HTML blocks
    //
    // An [HTML block](@) is a group of lines that is treated
    // as raw HTML (and will not be escaped in HTML output).
    //
    // There are seven kinds of [HTML block], which can be defined by their
    // start and end conditions.  The block begins with a line that meets a
    // [start condition](@) (after up to three spaces optional indentation).
    // It ends with the first subsequent line that meets a matching [end
    // condition](@), or the last line of the document, or the last line of
    // the [container block](#container-blocks) containing the current HTML
    // block, if no line is encountered that meets the [end condition].  If
    // the first line meets both the [start condition] and the [end
    // condition], the block will contain just that line.
    //
    // 1.  **Start condition:**  line begins with the string `<script`,
    // `<pre`, or `<style` (case-insensitive), followed by whitespace,
    // the string `>`, or the end of the line.\
    // **End condition:**  line contains an end tag
    // `</script>`, `</pre>`, or `</style>` (case-insensitive; it
    // need not match the start tag).
    //
    // 2.  **Start condition:** line begins with the string `<!--`.\
    // **End condition:**  line contains the string `-->`.
    //
    // 3.  **Start condition:** line begins with the string `<?`.\
    // **End condition:** line contains the string `?>`.
    //
    // 4.  **Start condition:** line begins with the string `<!`
    // followed by an uppercase ASCII letter.\
    // **End condition:** line contains the character `>`.
    //
    // 5.  **Start condition:**  line begins with the string
    // `<![CDATA[`.\
    // **End condition:** line contains the string `]]>`.
    //
    // 6.  **Start condition:** line begins the string `<` or `</`
    // followed by one of the strings (case-insensitive) `address`,
    // `article`, `aside`, `base`, `basefont`, `blockquote`, `body`,
    // `caption`, `center`, `col`, `colgroup`, `dd`, `details`, `dialog`,
    // `dir`, `div`, `dl`, `dt`, `fieldset`, `figcaption`, `figure`,
    // `footer`, `form`, `frame`, `frameset`,
    // `h1`, `h2`, `h3`, `h4`, `h5`, `h6`, `head`, `header`, `hr`,
    // `html`, `iframe`, `legend`, `li`, `link`, `main`, `menu`, `menuitem`,
    // `nav`, `noframes`, `ol`, `optgroup`, `option`, `p`, `param`,
    // `section`, `source`, `summary`, `table`, `tbody`, `td`,
    // `tfoot`, `th`, `thead`, `title`, `tr`, `track`, `ul`, followed
    // by [whitespace], the end of the line, the string `>`, or
    // the string `/>`.\
    // **End condition:** line is followed by a [blank line].
    //
    // 7.  **Start condition:**  line begins with a complete [open tag]
    // (with any [tag name] other than `script`,
    // `style`, or `pre`) or a complete [closing tag],
    // followed only by [whitespace] or the end of the line.\
    // **End condition:** line is followed by a [blank line].
    //
    // HTML blocks continue until they are closed by their appropriate
    // [end condition], or the last line of the document or other [container
    // block](#container-blocks).  This means any HTML **within an HTML
    // block** that might otherwise be recognised as a start condition will
    // be ignored by the parser and passed through as-is, without changing
    // the parser's state.
    //
    // For instance, `<pre>` within a HTML block started by `<table>` will not affect
    // the parser state; as the HTML block was started in by start condition 6, it
    // will end at any blank line. This can be surprising:
    [Fact]
    public void LeafBlocksHTMLBlocks_Example118()
    {
      // Example 118
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   <table><tr><td>
      //   <pre>
      //   **Hello**,
      //  
      //   _world_.
      //   </pre>
      //   </td></tr></table>
      //
      // Should be rendered as:
      //   <table><tr><td>
      //   <pre>
      //   **Hello**,
      //   <p><em>world</em>.
      //   </pre></p>
      //   </td></tr></table>

      Parser.Parse("<table><tr><td>\n<pre>\n**Hello**,\n\n_world_.\n</pre>\n</td></tr></table>").Is(Parser.Prettify("<table><tr><td>\n<pre>\n**Hello**,\n<p><em>world</em>.\n</pre></p>\n</td></tr></table>"));

      Parser.DoubleParse("<table><tr><td>\n<pre>\n**Hello**,\n\n_world_.\n</pre>\n</td></tr></table>").Is(Parser.Prettify("<table><tr><td>\n<pre>\n**Hello**,\n<p><em>world</em>.\n</pre></p>\n</td></tr></table>"));
    }

    // In this case, the HTML block is terminated by the newline — the `**Hello**`
    // text remains verbatim — and regular parsing resumes, with a paragraph,
    // emphasised `world` and inline and block HTML following.
    //
    // All types of [HTML blocks] except type 7 may interrupt
    // a paragraph.  Blocks of type 7 may not interrupt a paragraph.
    // (This restriction is intended to prevent unwanted interpretation
    // of long tags inside a wrapped paragraph as starting HTML blocks.)
    //
    // Some simple examples follow.  Here are some basic HTML blocks
    // of type 6:
    [Fact]
    public void LeafBlocksHTMLBlocks_Example119()
    {
      // Example 119
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   <table>
      //     <tr>
      //       <td>
      //              hi
      //       </td>
      //     </tr>
      //   </table>
      //  
      //   okay.
      //
      // Should be rendered as:
      //   <table>
      //     <tr>
      //       <td>
      //              hi
      //       </td>
      //     </tr>
      //   </table>
      //   <p>okay.</p>

      Parser.Parse("<table>\n  <tr>\n    <td>\n           hi\n    </td>\n  </tr>\n</table>\n\nokay.").Is(Parser.Prettify("<table>\n  <tr>\n    <td>\n           hi\n    </td>\n  </tr>\n</table>\n<p>okay.</p>"));

      Parser.DoubleParse("<table>\n  <tr>\n    <td>\n           hi\n    </td>\n  </tr>\n</table>\n\nokay.").Is(Parser.Prettify("<table>\n  <tr>\n    <td>\n           hi\n    </td>\n  </tr>\n</table>\n<p>okay.</p>"));
    }

    [Fact]
    public void LeafBlocksHTMLBlocks_Example120()
    {
      // Example 120
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //    <div>
      //     *hello*
      //            <foo><a>
      //
      // Should be rendered as:
      //    <div>
      //     *hello*
      //            <foo><a>

      Parser.Parse(" <div>\n  *hello*\n         <foo><a>").Is(Parser.Prettify(" <div>\n  *hello*\n         <foo><a>"));

      Parser.DoubleParse(" <div>\n  *hello*\n         <foo><a>").Is(Parser.Prettify(" <div>\n  *hello*\n         <foo><a>"));
    }

    // A block can also start with a closing tag:
    [Fact]
    public void LeafBlocksHTMLBlocks_Example121()
    {
      // Example 121
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   </div>
      //   *foo*
      //
      // Should be rendered as:
      //   </div>
      //   *foo*

      Parser.Parse("</div>\n*foo*").Is(Parser.Prettify("</div>\n*foo*"));

      Parser.DoubleParse("</div>\n*foo*").Is(Parser.Prettify("</div>\n*foo*"));
    }

    // Here we have two HTML blocks with a Markdown paragraph between them:
    [Fact]
    public void LeafBlocksHTMLBlocks_Example122()
    {
      // Example 122
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   <DIV CLASS="foo">
      //  
      //   *Markdown*
      //  
      //   </DIV>
      //
      // Should be rendered as:
      //   <DIV CLASS="foo">
      //   <p><em>Markdown</em></p>
      //   </DIV>

      Parser.Parse("<DIV CLASS=\"foo\">\n\n*Markdown*\n\n</DIV>").Is(Parser.Prettify("<DIV CLASS=\"foo\">\n<p><em>Markdown</em></p>\n</DIV>"));

      Parser.DoubleParse("<DIV CLASS=\"foo\">\n\n*Markdown*\n\n</DIV>").Is(Parser.Prettify("<DIV CLASS=\"foo\">\n<p><em>Markdown</em></p>\n</DIV>"));
    }

    // The tag on the first line can be partial, as long
    // as it is split where there would be whitespace:
    [Fact]
    public void LeafBlocksHTMLBlocks_Example123()
    {
      // Example 123
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   <div id="foo"
      //     class="bar">
      //   </div>
      //
      // Should be rendered as:
      //   <div id="foo"
      //     class="bar">
      //   </div>

      Parser.Parse("<div id=\"foo\"\n  class=\"bar\">\n</div>").Is(Parser.Prettify("<div id=\"foo\"\n  class=\"bar\">\n</div>"));

      Parser.DoubleParse("<div id=\"foo\"\n  class=\"bar\">\n</div>").Is(Parser.Prettify("<div id=\"foo\"\n  class=\"bar\">\n</div>"));
    }

    [Fact]
    public void LeafBlocksHTMLBlocks_Example124()
    {
      // Example 124
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   <div id="foo" class="bar
      //     baz">
      //   </div>
      //
      // Should be rendered as:
      //   <div id="foo" class="bar
      //     baz">
      //   </div>

      Parser.Parse("<div id=\"foo\" class=\"bar\n  baz\">\n</div>").Is(Parser.Prettify("<div id=\"foo\" class=\"bar\n  baz\">\n</div>"));

      Parser.DoubleParse("<div id=\"foo\" class=\"bar\n  baz\">\n</div>").Is(Parser.Prettify("<div id=\"foo\" class=\"bar\n  baz\">\n</div>"));
    }

    // An open tag need not be closed:
    [Fact]
    public void LeafBlocksHTMLBlocks_Example125()
    {
      // Example 125
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   <div>
      //   *foo*
      //  
      //   *bar*
      //
      // Should be rendered as:
      //   <div>
      //   *foo*
      //   <p><em>bar</em></p>

      Parser.Parse("<div>\n*foo*\n\n*bar*").Is(Parser.Prettify("<div>\n*foo*\n<p><em>bar</em></p>"));

      Parser.DoubleParse("<div>\n*foo*\n\n*bar*").Is(Parser.Prettify("<div>\n*foo*\n<p><em>bar</em></p>"));
    }

    // A partial tag need not even be completed (garbage
    // in, garbage out):
    [Fact]
    public void LeafBlocksHTMLBlocks_Example126()
    {
      // Example 126
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   <div id="foo"
      //   *hi*
      //
      // Should be rendered as:
      //   <div id="foo"
      //   *hi*

      Parser.Parse("<div id=\"foo\"\n*hi*").Is(Parser.Prettify("<div id=\"foo\"\n*hi*"));

      Parser.DoubleParse("<div id=\"foo\"\n*hi*").Is(Parser.Prettify("<div id=\"foo\"\n*hi*"));
    }

    [Fact]
    public void LeafBlocksHTMLBlocks_Example127()
    {
      // Example 127
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   <div class
      //   foo
      //
      // Should be rendered as:
      //   <div class
      //   foo

      Parser.Parse("<div class\nfoo").Is(Parser.Prettify("<div class\nfoo"));

      Parser.DoubleParse("<div class\nfoo").Is(Parser.Prettify("<div class\nfoo"));
    }

    // The initial tag doesn't even need to be a valid
    // tag, as long as it starts like one:
    [Fact]
    public void LeafBlocksHTMLBlocks_Example128()
    {
      // Example 128
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   <div *???-&&&-<---
      //   *foo*
      //
      // Should be rendered as:
      //   <div *???-&&&-<---
      //   *foo*

      Parser.Parse("<div *???-&&&-<---\n*foo*").Is(Parser.Prettify("<div *???-&&&-<---\n*foo*"));

      Parser.DoubleParse("<div *???-&&&-<---\n*foo*").Is(Parser.Prettify("<div *???-&&&-<---\n*foo*"));
    }

    // In type 6 blocks, the initial tag need not be on a line by
    // itself:
    [Fact]
    public void LeafBlocksHTMLBlocks_Example129()
    {
      // Example 129
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   <div><a href="bar">*foo*</a></div>
      //
      // Should be rendered as:
      //   <div><a href="bar">*foo*</a></div>

      Parser.Parse("<div><a href=\"bar\">*foo*</a></div>").Is(Parser.Prettify("<div><a href=\"bar\">*foo*</a></div>"));

      Parser.DoubleParse("<div><a href=\"bar\">*foo*</a></div>").Is(Parser.Prettify("<div><a href=\"bar\">*foo*</a></div>"));
    }

    [Fact]
    public void LeafBlocksHTMLBlocks_Example130()
    {
      // Example 130
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   <table><tr><td>
      //   foo
      //   </td></tr></table>
      //
      // Should be rendered as:
      //   <table><tr><td>
      //   foo
      //   </td></tr></table>

      Parser.Parse("<table><tr><td>\nfoo\n</td></tr></table>").Is(Parser.Prettify("<table><tr><td>\nfoo\n</td></tr></table>"));

      Parser.DoubleParse("<table><tr><td>\nfoo\n</td></tr></table>").Is(Parser.Prettify("<table><tr><td>\nfoo\n</td></tr></table>"));
    }

    // Everything until the next blank line or end of document
    // gets included in the HTML block.  So, in the following
    // example, what looks like a Markdown code block
    // is actually part of the HTML block, which continues until a blank
    // line or the end of the document is reached:
    [Fact]
    public void LeafBlocksHTMLBlocks_Example131()
    {
      // Example 131
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   <div></div>
      //   ``` c
      //   int x = 33;
      //   ```
      //
      // Should be rendered as:
      //   <div></div>
      //   ``` c
      //   int x = 33;
      //   ```

      Parser.Parse("<div></div>\n``` c\nint x = 33;\n```").Is(Parser.Prettify("<div></div>\n``` c\nint x = 33;\n```"));

      Parser.DoubleParse("<div></div>\n``` c\nint x = 33;\n```").Is(Parser.Prettify("<div></div>\n``` c\nint x = 33;\n```"));
    }

    // To start an [HTML block] with a tag that is *not* in the
    // list of block-level tags in (6), you must put the tag by
    // itself on the first line (and it must be complete):
    [Fact]
    public void LeafBlocksHTMLBlocks_Example132()
    {
      // Example 132
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   <a href="foo">
      //   *bar*
      //   </a>
      //
      // Should be rendered as:
      //   <a href="foo">
      //   *bar*
      //   </a>

      Parser.Parse("<a href=\"foo\">\n*bar*\n</a>").Is(Parser.Prettify("<a href=\"foo\">\n*bar*\n</a>"));

      Parser.DoubleParse("<a href=\"foo\">\n*bar*\n</a>").Is(Parser.Prettify("<a href=\"foo\">\n*bar*\n</a>"));
    }

    // In type 7 blocks, the [tag name] can be anything:
    [Fact]
    public void LeafBlocksHTMLBlocks_Example133()
    {
      // Example 133
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   <Warning>
      //   *bar*
      //   </Warning>
      //
      // Should be rendered as:
      //   <Warning>
      //   *bar*
      //   </Warning>

      Parser.Parse("<Warning>\n*bar*\n</Warning>").Is(Parser.Prettify("<Warning>\n*bar*\n</Warning>"));

      Parser.DoubleParse("<Warning>\n*bar*\n</Warning>").Is(Parser.Prettify("<Warning>\n*bar*\n</Warning>"));
    }

    [Fact]
    public void LeafBlocksHTMLBlocks_Example134()
    {
      // Example 134
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   <i class="foo">
      //   *bar*
      //   </i>
      //
      // Should be rendered as:
      //   <i class="foo">
      //   *bar*
      //   </i>

      Parser.Parse("<i class=\"foo\">\n*bar*\n</i>").Is(Parser.Prettify("<i class=\"foo\">\n*bar*\n</i>"));

      Parser.DoubleParse("<i class=\"foo\">\n*bar*\n</i>").Is(Parser.Prettify("<i class=\"foo\">\n*bar*\n</i>"));
    }

    [Fact]
    public void LeafBlocksHTMLBlocks_Example135()
    {
      // Example 135
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   </ins>
      //   *bar*
      //
      // Should be rendered as:
      //   </ins>
      //   *bar*

      Parser.Parse("</ins>\n*bar*").Is(Parser.Prettify("</ins>\n*bar*"));

      Parser.DoubleParse("</ins>\n*bar*").Is(Parser.Prettify("</ins>\n*bar*"));
    }

    // These rules are designed to allow us to work with tags that
    // can function as either block-level or inline-level tags.
    // The `<del>` tag is a nice example.  We can surround content with
    // `<del>` tags in three different ways.  In this case, we get a raw
    // HTML block, because the `<del>` tag is on a line by itself:
    [Fact]
    public void LeafBlocksHTMLBlocks_Example136()
    {
      // Example 136
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   <del>
      //   *foo*
      //   </del>
      //
      // Should be rendered as:
      //   <del>
      //   *foo*
      //   </del>

      Parser.Parse("<del>\n*foo*\n</del>").Is(Parser.Prettify("<del>\n*foo*\n</del>"));

      Parser.DoubleParse("<del>\n*foo*\n</del>").Is(Parser.Prettify("<del>\n*foo*\n</del>"));
    }

    // In this case, we get a raw HTML block that just includes
    // the `<del>` tag (because it ends with the following blank
    // line).  So the contents get interpreted as CommonMark:
    [Fact]
    public void LeafBlocksHTMLBlocks_Example137()
    {
      // Example 137
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   <del>
      //  
      //   *foo*
      //  
      //   </del>
      //
      // Should be rendered as:
      //   <del>
      //   <p><em>foo</em></p>
      //   </del>

      Parser.Parse("<del>\n\n*foo*\n\n</del>").Is(Parser.Prettify("<del>\n<p><em>foo</em></p>\n</del>"));

      Parser.DoubleParse("<del>\n\n*foo*\n\n</del>").Is(Parser.Prettify("<del>\n<p><em>foo</em></p>\n</del>"));
    }

    // Finally, in this case, the `<del>` tags are interpreted
    // as [raw HTML] *inside* the CommonMark paragraph.  (Because
    // the tag is not on a line by itself, we get inline HTML
    // rather than an [HTML block].)
    [Fact]
    public void LeafBlocksHTMLBlocks_Example138()
    {
      // Example 138
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   <del>*foo*</del>
      //
      // Should be rendered as:
      //   <p><del><em>foo</em></del></p>

      Parser.Parse("<del>*foo*</del>").Is(Parser.Prettify("<p><del><em>foo</em></del></p>"));

      Parser.DoubleParse("<del>*foo*</del>").Is(Parser.Prettify("<p><del><em>foo</em></del></p>"));
    }

    // HTML tags designed to contain literal content
    // (`script`, `style`, `pre`), comments, processing instructions,
    // and declarations are treated somewhat differently.
    // Instead of ending at the first blank line, these blocks
    // end at the first line containing a corresponding end tag.
    // As a result, these blocks can contain blank lines:
    //
    // A pre tag (type 1):
    [Fact]
    public void LeafBlocksHTMLBlocks_Example139()
    {
      // Example 139
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   <pre language="haskell"><code>
      //   import Text.HTML.TagSoup
      //  
      //   main :: IO ()
      //   main = print $ parseTags tags
      //   </code></pre>
      //   okay
      //
      // Should be rendered as:
      //   <pre language="haskell"><code>
      //   import Text.HTML.TagSoup
      //  
      //   main :: IO ()
      //   main = print $ parseTags tags
      //   </code></pre>
      //   <p>okay</p>

      Parser.Parse("<pre language=\"haskell\"><code>\nimport Text.HTML.TagSoup\n\nmain :: IO ()\nmain = print $ parseTags tags\n</code></pre>\nokay").Is(Parser.Prettify("<pre language=\"haskell\"><code>\nimport Text.HTML.TagSoup\n\nmain :: IO ()\nmain = print $ parseTags tags\n</code></pre>\n<p>okay</p>"));

      Parser.DoubleParse("<pre language=\"haskell\"><code>\nimport Text.HTML.TagSoup\n\nmain :: IO ()\nmain = print $ parseTags tags\n</code></pre>\nokay").Is(Parser.Prettify("<pre language=\"haskell\"><code>\nimport Text.HTML.TagSoup\n\nmain :: IO ()\nmain = print $ parseTags tags\n</code></pre>\n<p>okay</p>"));
    }

    // A script tag (type 1):
    [Fact]
    public void LeafBlocksHTMLBlocks_Example140()
    {
      // Example 140
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   <script type="text/javascript">
      //   // JavaScript example
      //  
      //   document.getElementById("demo").innerHTML = "Hello JavaScript!";
      //   </script>
      //   okay
      //
      // Should be rendered as:
      //   <script type="text/javascript">
      //   // JavaScript example
      //  
      //   document.getElementById("demo").innerHTML = "Hello JavaScript!";
      //   </script>
      //   <p>okay</p>

      Parser.Parse("<script type=\"text/javascript\">\n// JavaScript example\n\ndocument.getElementById(\"demo\").innerHTML = \"Hello JavaScript!\";\n</script>\nokay").Is(Parser.Prettify("<script type=\"text/javascript\">\n// JavaScript example\n\ndocument.getElementById(\"demo\").innerHTML = \"Hello JavaScript!\";\n</script>\n<p>okay</p>"));

      Parser.DoubleParse("<script type=\"text/javascript\">\n// JavaScript example\n\ndocument.getElementById(\"demo\").innerHTML = \"Hello JavaScript!\";\n</script>\nokay").Is(Parser.Prettify("<script type=\"text/javascript\">\n// JavaScript example\n\ndocument.getElementById(\"demo\").innerHTML = \"Hello JavaScript!\";\n</script>\n<p>okay</p>"));
    }

    // A style tag (type 1):
    [Fact]
    public void LeafBlocksHTMLBlocks_Example141()
    {
      // Example 141
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   <style
      //     type="text/css">
      //   h1 {color:red;}
      //  
      //   p {color:blue;}
      //   </style>
      //   okay
      //
      // Should be rendered as:
      //   <style
      //     type="text/css">
      //   h1 {color:red;}
      //  
      //   p {color:blue;}
      //   </style>
      //   <p>okay</p>

      Parser.Parse("<style\n  type=\"text/css\">\nh1 {color:red;}\n\np {color:blue;}\n</style>\nokay").Is(Parser.Prettify("<style\n  type=\"text/css\">\nh1 {color:red;}\n\np {color:blue;}\n</style>\n<p>okay</p>"));

      Parser.DoubleParse("<style\n  type=\"text/css\">\nh1 {color:red;}\n\np {color:blue;}\n</style>\nokay").Is(Parser.Prettify("<style\n  type=\"text/css\">\nh1 {color:red;}\n\np {color:blue;}\n</style>\n<p>okay</p>"));
    }

    // If there is no matching end tag, the block will end at the
    // end of the document (or the enclosing [block quote][block quotes]
    // or [list item][list items]):
    [Fact]
    public void LeafBlocksHTMLBlocks_Example142()
    {
      // Example 142
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   <style
      //     type="text/css">
      //  
      //   foo
      //
      // Should be rendered as:
      //   <style
      //     type="text/css">
      //  
      //   foo

      Parser.Parse("<style\n  type=\"text/css\">\n\nfoo").Is(Parser.Prettify("<style\n  type=\"text/css\">\n\nfoo"));

      Parser.DoubleParse("<style\n  type=\"text/css\">\n\nfoo").Is(Parser.Prettify("<style\n  type=\"text/css\">\n\nfoo"));
    }

    [Fact]
    public void LeafBlocksHTMLBlocks_Example143()
    {
      // Example 143
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   > <div>
      //   > foo
      //  
      //   bar
      //
      // Should be rendered as:
      //   <blockquote>
      //   <div>
      //   foo
      //   </blockquote>
      //   <p>bar</p>

      Parser.Parse("> <div>\n> foo\n\nbar").Is(Parser.Prettify("<blockquote>\n<div>\nfoo\n</blockquote>\n<p>bar</p>"));

      Parser.DoubleParse("> <div>\n> foo\n\nbar").Is(Parser.Prettify("<blockquote>\n<div>\nfoo\n</blockquote>\n<p>bar</p>"));
    }

    [Fact]
    public void LeafBlocksHTMLBlocks_Example144()
    {
      // Example 144
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   - <div>
      //   - foo
      //
      // Should be rendered as:
      //   <ul>
      //   <li>
      //   <div>
      //   </li>
      //   <li>foo</li>
      //   </ul>

      Parser.Parse("- <div>\n- foo").Is(Parser.Prettify("<ul>\n<li>\n<div>\n</li>\n<li>foo</li>\n</ul>"));

      Parser.DoubleParse("- <div>\n- foo").Is(Parser.Prettify("<ul>\n<li>\n<div>\n</li>\n<li>foo</li>\n</ul>"));
    }

    // The end tag can occur on the same line as the start tag:
    [Fact]
    public void LeafBlocksHTMLBlocks_Example145()
    {
      // Example 145
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   <style>p{color:red;}</style>
      //   *foo*
      //
      // Should be rendered as:
      //   <style>p{color:red;}</style>
      //   <p><em>foo</em></p>

      Parser.Parse("<style>p{color:red;}</style>\n*foo*").Is(Parser.Prettify("<style>p{color:red;}</style>\n<p><em>foo</em></p>"));

      Parser.DoubleParse("<style>p{color:red;}</style>\n*foo*").Is(Parser.Prettify("<style>p{color:red;}</style>\n<p><em>foo</em></p>"));
    }

    [Fact]
    public void LeafBlocksHTMLBlocks_Example146()
    {
      // Example 146
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   <!-- foo -->*bar*
      //   *baz*
      //
      // Should be rendered as:
      //   <!-- foo -->*bar*
      //   <p><em>baz</em></p>

      Parser.Parse("<!-- foo -->*bar*\n*baz*").Is(Parser.Prettify("<!-- foo -->*bar*\n<p><em>baz</em></p>"));

      Parser.DoubleParse("<!-- foo -->*bar*\n*baz*").Is(Parser.Prettify("<!-- foo -->*bar*\n<p><em>baz</em></p>"));
    }

    // Note that anything on the last line after the
    // end tag will be included in the [HTML block]:
    [Fact]
    public void LeafBlocksHTMLBlocks_Example147()
    {
      // Example 147
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   <script>
      //   foo
      //   </script>1. *bar*
      //
      // Should be rendered as:
      //   <script>
      //   foo
      //   </script>1. *bar*

      Parser.Parse("<script>\nfoo\n</script>1. *bar*").Is(Parser.Prettify("<script>\nfoo\n</script>1. *bar*"));

      Parser.DoubleParse("<script>\nfoo\n</script>1. *bar*").Is(Parser.Prettify("<script>\nfoo\n</script>1. *bar*"));
    }

    // A comment (type 2):
    [Fact]
    public void LeafBlocksHTMLBlocks_Example148()
    {
      // Example 148
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   <!-- Foo
      //  
      //   bar
      //      baz -->
      //   okay
      //
      // Should be rendered as:
      //   <!-- Foo
      //  
      //   bar
      //      baz -->
      //   <p>okay</p>

      Parser.Parse("<!-- Foo\n\nbar\n   baz -->\nokay").Is(Parser.Prettify("<!-- Foo\n\nbar\n   baz -->\n<p>okay</p>"));

      Parser.DoubleParse("<!-- Foo\n\nbar\n   baz -->\nokay").Is(Parser.Prettify("<!-- Foo\n\nbar\n   baz -->\n<p>okay</p>"));
    }

    // A processing instruction (type 3):
    [Fact]
    public void LeafBlocksHTMLBlocks_Example149()
    {
      // Example 149
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   <?php
      //  
      //     echo '>';
      //  
      //   ?>
      //   okay
      //
      // Should be rendered as:
      //   <?php
      //  
      //     echo '>';
      //  
      //   ?>
      //   <p>okay</p>

      Parser.Parse("<?php\n\n  echo '>';\n\n?>\nokay").Is(Parser.Prettify("<?php\n\n  echo '>';\n\n?>\n<p>okay</p>"));

      Parser.DoubleParse("<?php\n\n  echo '>';\n\n?>\nokay").Is(Parser.Prettify("<?php\n\n  echo '>';\n\n?>\n<p>okay</p>"));
    }

    // A declaration (type 4):
    [Fact]
    public void LeafBlocksHTMLBlocks_Example150()
    {
      // Example 150
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   <!DOCTYPE html>
      //
      // Should be rendered as:
      //   <!DOCTYPE html>

      Parser.Parse("<!DOCTYPE html>").Is(Parser.Prettify("<!DOCTYPE html>"));

      Parser.DoubleParse("<!DOCTYPE html>").Is(Parser.Prettify("<!DOCTYPE html>"));
    }

    // CDATA (type 5):
    [Fact]
    public void LeafBlocksHTMLBlocks_Example151()
    {
      // Example 151
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   <![CDATA[
      //   function matchwo(a,b)
      //   {
      //     if (a < b && a < 0) then {
      //       return 1;
      //  
      //     } else {
      //  
      //       return 0;
      //     }
      //   }
      //   ]]>
      //   okay
      //
      // Should be rendered as:
      //   <![CDATA[
      //   function matchwo(a,b)
      //   {
      //     if (a < b && a < 0) then {
      //       return 1;
      //  
      //     } else {
      //  
      //       return 0;
      //     }
      //   }
      //   ]]>
      //   <p>okay</p>

      Parser.Parse("<![CDATA[\nfunction matchwo(a,b)\n{\n  if (a < b && a < 0) then {\n    return 1;\n\n  } else {\n\n    return 0;\n  }\n}\n]]>\nokay").Is(Parser.Prettify("<![CDATA[\nfunction matchwo(a,b)\n{\n  if (a < b && a < 0) then {\n    return 1;\n\n  } else {\n\n    return 0;\n  }\n}\n]]>\n<p>okay</p>"));

      Parser.DoubleParse("<![CDATA[\nfunction matchwo(a,b)\n{\n  if (a < b && a < 0) then {\n    return 1;\n\n  } else {\n\n    return 0;\n  }\n}\n]]>\nokay").Is(Parser.Prettify("<![CDATA[\nfunction matchwo(a,b)\n{\n  if (a < b && a < 0) then {\n    return 1;\n\n  } else {\n\n    return 0;\n  }\n}\n]]>\n<p>okay</p>"));
    }

    // The opening tag can be indented 1-3 spaces, but not 4:
    [Fact]
    public void LeafBlocksHTMLBlocks_Example152()
    {
      // Example 152
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //     <!-- foo -->
      //  
      //       <!-- foo -->
      //
      // Should be rendered as:
      //     <!-- foo -->
      //   <pre><code>&lt;!-- foo --&gt;
      //   </code></pre>

      Parser.Parse("  <!-- foo -->\n\n    <!-- foo -->").Is(Parser.Prettify("  <!-- foo -->\n<pre><code>&lt;!-- foo --&gt;\n</code></pre>"));

      Parser.DoubleParse("  <!-- foo -->\n\n    <!-- foo -->").Is(Parser.Prettify("  <!-- foo -->\n<pre><code>&lt;!-- foo --&gt;\n</code></pre>"));
    }

    [Fact]
    public void LeafBlocksHTMLBlocks_Example153()
    {
      // Example 153
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //     <div>
      //  
      //       <div>
      //
      // Should be rendered as:
      //     <div>
      //   <pre><code>&lt;div&gt;
      //   </code></pre>

      Parser.Parse("  <div>\n\n    <div>").Is(Parser.Prettify("  <div>\n<pre><code>&lt;div&gt;\n</code></pre>"));

      Parser.DoubleParse("  <div>\n\n    <div>").Is(Parser.Prettify("  <div>\n<pre><code>&lt;div&gt;\n</code></pre>"));
    }

    // An HTML block of types 1--6 can interrupt a paragraph, and need not be
    // preceded by a blank line.
    [Fact]
    public void LeafBlocksHTMLBlocks_Example154()
    {
      // Example 154
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   Foo
      //   <div>
      //   bar
      //   </div>
      //
      // Should be rendered as:
      //   <p>Foo</p>
      //   <div>
      //   bar
      //   </div>

      Parser.Parse("Foo\n<div>\nbar\n</div>").Is(Parser.Prettify("<p>Foo</p>\n<div>\nbar\n</div>"));

      Parser.DoubleParse("Foo\n<div>\nbar\n</div>").Is(Parser.Prettify("<p>Foo</p>\n<div>\nbar\n</div>"));
    }

    // However, a following blank line is needed, except at the end of
    // a document, and except for blocks of types 1--5, [above][HTML
    // block]:
    [Fact]
    public void LeafBlocksHTMLBlocks_Example155()
    {
      // Example 155
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   <div>
      //   bar
      //   </div>
      //   *foo*
      //
      // Should be rendered as:
      //   <div>
      //   bar
      //   </div>
      //   *foo*

      Parser.Parse("<div>\nbar\n</div>\n*foo*").Is(Parser.Prettify("<div>\nbar\n</div>\n*foo*"));

      Parser.DoubleParse("<div>\nbar\n</div>\n*foo*").Is(Parser.Prettify("<div>\nbar\n</div>\n*foo*"));
    }

    // HTML blocks of type 7 cannot interrupt a paragraph:
    [Fact]
    public void LeafBlocksHTMLBlocks_Example156()
    {
      // Example 156
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   Foo
      //   <a href="bar">
      //   baz
      //
      // Should be rendered as:
      //   <p>Foo
      //   <a href="bar">
      //   baz</p>

      Parser.Parse("Foo\n<a href=\"bar\">\nbaz").Is(Parser.Prettify("<p>Foo\n<a href=\"bar\">\nbaz</p>"));

      Parser.DoubleParse("Foo\n<a href=\"bar\">\nbaz").Is(Parser.Prettify("<p>Foo\n<a href=\"bar\">\nbaz</p>"));
    }

    // This rule differs from John Gruber's original Markdown syntax
    // specification, which says:
    //
    // > The only restrictions are that block-level HTML elements —
    // > e.g. `<div>`, `<table>`, `<pre>`, `<p>`, etc. — must be separated from
    // > surrounding content by blank lines, and the start and end tags of the
    // > block should not be indented with tabs or spaces.
    //
    // In some ways Gruber's rule is more restrictive than the one given
    // here:
    //
    // - It requires that an HTML block be preceded by a blank line.
    // - It does not allow the start tag to be indented.
    // - It requires a matching end tag, which it also does not allow to
    //   be indented.
    //
    // Most Markdown implementations (including some of Gruber's own) do not
    // respect all of these restrictions.
    //
    // There is one respect, however, in which Gruber's rule is more liberal
    // than the one given here, since it allows blank lines to occur inside
    // an HTML block.  There are two reasons for disallowing them here.
    // First, it removes the need to parse balanced tags, which is
    // expensive and can require backtracking from the end of the document
    // if no matching end tag is found. Second, it provides a very simple
    // and flexible way of including Markdown content inside HTML tags:
    // simply separate the Markdown from the HTML using blank lines:
    //
    // Compare:
    [Fact]
    public void LeafBlocksHTMLBlocks_Example157()
    {
      // Example 157
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   <div>
      //  
      //   *Emphasized* text.
      //  
      //   </div>
      //
      // Should be rendered as:
      //   <div>
      //   <p><em>Emphasized</em> text.</p>
      //   </div>

      Parser.Parse("<div>\n\n*Emphasized* text.\n\n</div>").Is(Parser.Prettify("<div>\n<p><em>Emphasized</em> text.</p>\n</div>"));

      Parser.DoubleParse("<div>\n\n*Emphasized* text.\n\n</div>").Is(Parser.Prettify("<div>\n<p><em>Emphasized</em> text.</p>\n</div>"));
    }

    [Fact]
    public void LeafBlocksHTMLBlocks_Example158()
    {
      // Example 158
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   <div>
      //   *Emphasized* text.
      //   </div>
      //
      // Should be rendered as:
      //   <div>
      //   *Emphasized* text.
      //   </div>

      Parser.Parse("<div>\n*Emphasized* text.\n</div>").Is(Parser.Prettify("<div>\n*Emphasized* text.\n</div>"));

      Parser.DoubleParse("<div>\n*Emphasized* text.\n</div>").Is(Parser.Prettify("<div>\n*Emphasized* text.\n</div>"));
    }

    // Some Markdown implementations have adopted a convention of
    // interpreting content inside tags as text if the open tag has
    // the attribute `markdown=1`.  The rule given above seems a simpler and
    // more elegant way of achieving the same expressive power, which is also
    // much simpler to parse.
    //
    // The main potential drawback is that one can no longer paste HTML
    // blocks into Markdown documents with 100% reliability.  However,
    // *in most cases* this will work fine, because the blank lines in
    // HTML are usually followed by HTML block tags.  For example:
    [Fact]
    public void LeafBlocksHTMLBlocks_Example159()
    {
      // Example 159
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   <table>
      //  
      //   <tr>
      //  
      //   <td>
      //   Hi
      //   </td>
      //  
      //   </tr>
      //  
      //   </table>
      //
      // Should be rendered as:
      //   <table>
      //   <tr>
      //   <td>
      //   Hi
      //   </td>
      //   </tr>
      //   </table>

      Parser.Parse("<table>\n\n<tr>\n\n<td>\nHi\n</td>\n\n</tr>\n\n</table>").Is(Parser.Prettify("<table>\n<tr>\n<td>\nHi\n</td>\n</tr>\n</table>"));

      Parser.DoubleParse("<table>\n\n<tr>\n\n<td>\nHi\n</td>\n\n</tr>\n\n</table>").Is(Parser.Prettify("<table>\n<tr>\n<td>\nHi\n</td>\n</tr>\n</table>"));
    }

    // There are problems, however, if the inner tags are indented
    // *and* separated by spaces, as then they will be interpreted as
    // an indented code block:
    [Fact]
    public void LeafBlocksHTMLBlocks_Example160()
    {
      // Example 160
      // Section: Leaf blocks / HTML blocks
      //
      // The following Markdown:
      //   <table>
      //  
      //     <tr>
      //  
      //       <td>
      //         Hi
      //       </td>
      //  
      //     </tr>
      //  
      //   </table>
      //
      // Should be rendered as:
      //   <table>
      //     <tr>
      //   <pre><code>&lt;td&gt;
      //     Hi
      //   &lt;/td&gt;
      //   </code></pre>
      //     </tr>
      //   </table>

      Parser.Parse("<table>\n\n  <tr>\n\n    <td>\n      Hi\n    </td>\n\n  </tr>\n\n</table>").Is(Parser.Prettify("<table>\n  <tr>\n<pre><code>&lt;td&gt;\n  Hi\n&lt;/td&gt;\n</code></pre>\n  </tr>\n</table>"));

      Parser.DoubleParse("<table>\n\n  <tr>\n\n    <td>\n      Hi\n    </td>\n\n  </tr>\n\n</table>").Is(Parser.Prettify("<table>\n  <tr>\n<pre><code>&lt;td&gt;\n  Hi\n&lt;/td&gt;\n</code></pre>\n  </tr>\n</table>"));
    }
  }

  public class TestLeafBlocksLinkReferenceDefinitions
  {
    // Fortunately, blank lines are usually not necessary and can be
    // deleted.  The exception is inside `<pre>` tags, but as described
    // [above][HTML blocks], raw HTML blocks starting with `<pre>`
    // *can* contain blank lines.
    //
    // ## Link reference definitions
    //
    // A [link reference definition](@)
    // consists of a [link label], indented up to three spaces, followed
    // by a colon (`:`), optional [whitespace] (including up to one
    // [line ending]), a [link destination],
    // optional [whitespace] (including up to one
    // [line ending]), and an optional [link
    // title], which if it is present must be separated
    // from the [link destination] by [whitespace].
    // No further [non-whitespace characters] may occur on the line.
    //
    // A [link reference definition]
    // does not correspond to a structural element of a document.  Instead, it
    // defines a label which can be used in [reference links]
    // and reference-style [images] elsewhere in the document.  [Link
    // reference definitions] can come either before or after the links that use
    // them.
    [Fact]
    public void LeafBlocksLinkReferenceDefinitions_Example161()
    {
      // Example 161
      // Section: Leaf blocks / Link reference definitions
      //
      // The following Markdown:
      //   [foo]: /url "title"
      //  
      //   [foo]
      //
      // Should be rendered as:
      //   <p><a href="/url" title="title">foo</a></p>

      Parser.Parse("[foo]: /url \"title\"\n\n[foo]").Is(Parser.Prettify("<p><a href=\"/url\" title=\"title\">foo</a></p>"));

      Parser.DoubleParse("[foo]: /url \"title\"\n\n[foo]").Is(Parser.Prettify("<p><a href=\"/url\" title=\"title\">foo</a></p>"));
    }

    [Fact]
    public void LeafBlocksLinkReferenceDefinitions_Example162()
    {
      // Example 162
      // Section: Leaf blocks / Link reference definitions
      //
      // The following Markdown:
      //      [foo]: 
      //         /url  
      //              'the title'  
      //  
      //   [foo]
      //
      // Should be rendered as:
      //   <p><a href="/url" title="the title">foo</a></p>

      Parser.Parse("   [foo]: \n      /url  \n           'the title'  \n\n[foo]").Is(Parser.Prettify("<p><a href=\"/url\" title=\"the title\">foo</a></p>"));

      Parser.DoubleParse("   [foo]: \n      /url  \n           'the title'  \n\n[foo]").Is(Parser.Prettify("<p><a href=\"/url\" title=\"the title\">foo</a></p>"));
    }

    [Fact]
    public void LeafBlocksLinkReferenceDefinitions_Example163()
    {
      // Example 163
      // Section: Leaf blocks / Link reference definitions
      //
      // The following Markdown:
      //   [Foo*bar\]]:my_(url) 'title (with parens)'
      //  
      //   [Foo*bar\]]
      //
      // Should be rendered as:
      //   <p><a href="my_(url)" title="title (with parens)">Foo*bar]</a></p>

      Parser.Parse("[Foo*bar\\]]:my_(url) 'title (with parens)'\n\n[Foo*bar\\]]").Is(Parser.Prettify("<p><a href=\"my_(url)\" title=\"title (with parens)\">Foo*bar]</a></p>"));

      Parser.DoubleParse("[Foo*bar\\]]:my_(url) 'title (with parens)'\n\n[Foo*bar\\]]").Is(Parser.Prettify("<p><a href=\"my_(url)\" title=\"title (with parens)\">Foo*bar]</a></p>"));
    }

    [Fact]
    public void LeafBlocksLinkReferenceDefinitions_Example164()
    {
      // Example 164
      // Section: Leaf blocks / Link reference definitions
      //
      // The following Markdown:
      //   [Foo bar]:
      //   <my url>
      //   'title'
      //  
      //   [Foo bar]
      //
      // Should be rendered as:
      //   <p><a href="my%20url" title="title">Foo bar</a></p>

      Parser.Parse("[Foo bar]:\n<my url>\n'title'\n\n[Foo bar]").Is(Parser.Prettify("<p><a href=\"my%20url\" title=\"title\">Foo bar</a></p>"));

      Parser.DoubleParse("[Foo bar]:\n<my url>\n'title'\n\n[Foo bar]").Is(Parser.Prettify("<p><a href=\"my%20url\" title=\"title\">Foo bar</a></p>"));
    }

    // The title may extend over multiple lines:
    [Fact]
    public void LeafBlocksLinkReferenceDefinitions_Example165()
    {
      // Example 165
      // Section: Leaf blocks / Link reference definitions
      //
      // The following Markdown:
      //   [foo]: /url '
      //   title
      //   line1
      //   line2
      //   '
      //  
      //   [foo]
      //
      // Should be rendered as:
      //   <p><a href="/url" title="
      //   title
      //   line1
      //   line2
      //   ">foo</a></p>

      Parser.Parse("[foo]: /url '\ntitle\nline1\nline2\n'\n\n[foo]").Is(Parser.Prettify("<p><a href=\"/url\" title=\"\ntitle\nline1\nline2\n\">foo</a></p>"));

      Parser.DoubleParse("[foo]: /url '\ntitle\nline1\nline2\n'\n\n[foo]").Is(Parser.Prettify("<p><a href=\"/url\" title=\"\ntitle\nline1\nline2\n\">foo</a></p>"));
    }

    // However, it may not contain a [blank line]:
    [Fact]
    public void LeafBlocksLinkReferenceDefinitions_Example166()
    {
      // Example 166
      // Section: Leaf blocks / Link reference definitions
      //
      // The following Markdown:
      //   [foo]: /url 'title
      //  
      //   with blank line'
      //  
      //   [foo]
      //
      // Should be rendered as:
      //   <p>[foo]: /url 'title</p>
      //   <p>with blank line'</p>
      //   <p>[foo]</p>

      Parser.Parse("[foo]: /url 'title\n\nwith blank line'\n\n[foo]").Is(Parser.Prettify("<p>[foo]: /url 'title</p>\n<p>with blank line'</p>\n<p>[foo]</p>"));

      Parser.DoubleParse("[foo]: /url 'title\n\nwith blank line'\n\n[foo]").Is(Parser.Prettify("<p>[foo]: /url 'title</p>\n<p>with blank line'</p>\n<p>[foo]</p>"));
    }

    // The title may be omitted:
    [Fact]
    public void LeafBlocksLinkReferenceDefinitions_Example167()
    {
      // Example 167
      // Section: Leaf blocks / Link reference definitions
      //
      // The following Markdown:
      //   [foo]:
      //   /url
      //  
      //   [foo]
      //
      // Should be rendered as:
      //   <p><a href="/url">foo</a></p>

      Parser.Parse("[foo]:\n/url\n\n[foo]").Is(Parser.Prettify("<p><a href=\"/url\">foo</a></p>"));

      Parser.DoubleParse("[foo]:\n/url\n\n[foo]").Is(Parser.Prettify("<p><a href=\"/url\">foo</a></p>"));
    }

    // The link destination may not be omitted:
    [Fact]
    public void LeafBlocksLinkReferenceDefinitions_Example168()
    {
      // Example 168
      // Section: Leaf blocks / Link reference definitions
      //
      // The following Markdown:
      //   [foo]:
      //  
      //   [foo]
      //
      // Should be rendered as:
      //   <p>[foo]:</p>
      //   <p>[foo]</p>

      Parser.Parse("[foo]:\n\n[foo]").Is(Parser.Prettify("<p>[foo]:</p>\n<p>[foo]</p>"));

      Parser.DoubleParse("[foo]:\n\n[foo]").Is(Parser.Prettify("<p>[foo]:</p>\n<p>[foo]</p>"));
    }

    //  However, an empty link destination may be specified using
    //  angle brackets:
    [Fact]
    public void LeafBlocksLinkReferenceDefinitions_Example169()
    {
      // Example 169
      // Section: Leaf blocks / Link reference definitions
      //
      // The following Markdown:
      //   [foo]: <>
      //  
      //   [foo]
      //
      // Should be rendered as:
      //   <p><a href="">foo</a></p>

      Parser.Parse("[foo]: <>\n\n[foo]").Is(Parser.Prettify("<p><a href=\"\">foo</a></p>"));

      Parser.DoubleParse("[foo]: <>\n\n[foo]").Is(Parser.Prettify("<p><a href=\"\">foo</a></p>"));
    }

    // The title must be separated from the link destination by
    // whitespace:
    [Fact]
    public void LeafBlocksLinkReferenceDefinitions_Example170()
    {
      // Example 170
      // Section: Leaf blocks / Link reference definitions
      //
      // The following Markdown:
      //   [foo]: <bar>(baz)
      //  
      //   [foo]
      //
      // Should be rendered as:
      //   <p>[foo]: <bar>(baz)</p>
      //   <p>[foo]</p>

      Parser.Parse("[foo]: <bar>(baz)\n\n[foo]").Is(Parser.Prettify("<p>[foo]: <bar>(baz)</p>\n<p>[foo]</p>"));

      Parser.DoubleParse("[foo]: <bar>(baz)\n\n[foo]").Is(Parser.Prettify("<p>[foo]: <bar>(baz)</p>\n<p>[foo]</p>"));
    }

    // Both title and destination can contain backslash escapes
    // and literal backslashes:
    [Fact]
    public void LeafBlocksLinkReferenceDefinitions_Example171()
    {
      // Example 171
      // Section: Leaf blocks / Link reference definitions
      //
      // The following Markdown:
      //   [foo]: /url\bar\*baz "foo\"bar\baz"
      //  
      //   [foo]
      //
      // Should be rendered as:
      //   <p><a href="/url%5Cbar*baz" title="foo&quot;bar\baz">foo</a></p>

      Parser.Parse("[foo]: /url\\bar\\*baz \"foo\\\"bar\\baz\"\n\n[foo]").Is(Parser.Prettify("<p><a href=\"/url%5Cbar*baz\" title=\"foo&quot;bar\\baz\">foo</a></p>"));

      Parser.DoubleParse("[foo]: /url\\bar\\*baz \"foo\\\"bar\\baz\"\n\n[foo]").Is(Parser.Prettify("<p><a href=\"/url%5Cbar*baz\" title=\"foo&quot;bar\\baz\">foo</a></p>"));
    }

    // A link can come before its corresponding definition:
    [Fact]
    public void LeafBlocksLinkReferenceDefinitions_Example172()
    {
      // Example 172
      // Section: Leaf blocks / Link reference definitions
      //
      // The following Markdown:
      //   [foo]
      //  
      //   [foo]: url
      //
      // Should be rendered as:
      //   <p><a href="url">foo</a></p>

      Parser.Parse("[foo]\n\n[foo]: url").Is(Parser.Prettify("<p><a href=\"url\">foo</a></p>"));

      Parser.DoubleParse("[foo]\n\n[foo]: url").Is(Parser.Prettify("<p><a href=\"url\">foo</a></p>"));
    }

    // If there are several matching definitions, the first one takes
    // precedence:
    [Fact]
    public void LeafBlocksLinkReferenceDefinitions_Example173()
    {
      // Example 173
      // Section: Leaf blocks / Link reference definitions
      //
      // The following Markdown:
      //   [foo]
      //  
      //   [foo]: first
      //   [foo]: second
      //
      // Should be rendered as:
      //   <p><a href="first">foo</a></p>

      Parser.Parse("[foo]\n\n[foo]: first\n[foo]: second").Is(Parser.Prettify("<p><a href=\"first\">foo</a></p>"));

      Parser.DoubleParse("[foo]\n\n[foo]: first\n[foo]: second").Is(Parser.Prettify("<p><a href=\"first\">foo</a></p>"));
    }

    // As noted in the section on [Links], matching of labels is
    // case-insensitive (see [matches]).
    [Fact]
    public void LeafBlocksLinkReferenceDefinitions_Example174()
    {
      // Example 174
      // Section: Leaf blocks / Link reference definitions
      //
      // The following Markdown:
      //   [FOO]: /url
      //  
      //   [Foo]
      //
      // Should be rendered as:
      //   <p><a href="/url">Foo</a></p>

      Parser.Parse("[FOO]: /url\n\n[Foo]").Is(Parser.Prettify("<p><a href=\"/url\">Foo</a></p>"));

      Parser.DoubleParse("[FOO]: /url\n\n[Foo]").Is(Parser.Prettify("<p><a href=\"/url\">Foo</a></p>"));
    }

    [Fact]
    public void LeafBlocksLinkReferenceDefinitions_Example175()
    {
      // Example 175
      // Section: Leaf blocks / Link reference definitions
      //
      // The following Markdown:
      //   [ΑΓΩ]: /φου
      //  
      //   [αγω]
      //
      // Should be rendered as:
      //   <p><a href="/%CF%86%CE%BF%CF%85">αγω</a></p>

      Parser.Parse("[ΑΓΩ]: /φου\n\n[αγω]").Is(Parser.Prettify("<p><a href=\"/%CF%86%CE%BF%CF%85\">αγω</a></p>"));

      Parser.DoubleParse("[ΑΓΩ]: /φου\n\n[αγω]").Is(Parser.Prettify("<p><a href=\"/%CF%86%CE%BF%CF%85\">αγω</a></p>"));
    }

    // Here is a link reference definition with no corresponding link.
    // It contributes nothing to the document.
    [Fact]
    public void LeafBlocksLinkReferenceDefinitions_Example176()
    {
      // Example 176
      // Section: Leaf blocks / Link reference definitions
      //
      // The following Markdown:
      //   [foo]: /url
      //
      // Should be rendered as:
      //
      Parser.Parse("[foo]: /url").Is(Parser.Prettify(""));

      Parser.DoubleParse("[foo]: /url").Is(Parser.Prettify(""));
    }

    // Here is another one:
    [Fact]
    public void LeafBlocksLinkReferenceDefinitions_Example177()
    {
      // Example 177
      // Section: Leaf blocks / Link reference definitions
      //
      // The following Markdown:
      //   [
      //   foo
      //   ]: /url
      //   bar
      //
      // Should be rendered as:
      //   <p>bar</p>

      Parser.Parse("[\nfoo\n]: /url\nbar").Is(Parser.Prettify("<p>bar</p>"));

      Parser.DoubleParse("[\nfoo\n]: /url\nbar").Is(Parser.Prettify("<p>bar</p>"));
    }

    // This is not a link reference definition, because there are
    // [non-whitespace characters] after the title:
    [Fact]
    public void LeafBlocksLinkReferenceDefinitions_Example178()
    {
      // Example 178
      // Section: Leaf blocks / Link reference definitions
      //
      // The following Markdown:
      //   [foo]: /url "title" ok
      //
      // Should be rendered as:
      //   <p>[foo]: /url &quot;title&quot; ok</p>

      Parser.Parse("[foo]: /url \"title\" ok").Is(Parser.Prettify("<p>[foo]: /url &quot;title&quot; ok</p>"));

      Parser.DoubleParse("[foo]: /url \"title\" ok").Is(Parser.Prettify("<p>[foo]: /url &quot;title&quot; ok</p>"));
    }

    // This is a link reference definition, but it has no title:
    [Fact]
    public void LeafBlocksLinkReferenceDefinitions_Example179()
    {
      // Example 179
      // Section: Leaf blocks / Link reference definitions
      //
      // The following Markdown:
      //   [foo]: /url
      //   "title" ok
      //
      // Should be rendered as:
      //   <p>&quot;title&quot; ok</p>

      Parser.Parse("[foo]: /url\n\"title\" ok").Is(Parser.Prettify("<p>&quot;title&quot; ok</p>"));

      Parser.DoubleParse("[foo]: /url\n\"title\" ok").Is(Parser.Prettify("<p>&quot;title&quot; ok</p>"));
    }

    // This is not a link reference definition, because it is indented
    // four spaces:
    [Fact]
    public void LeafBlocksLinkReferenceDefinitions_Example180()
    {
      // Example 180
      // Section: Leaf blocks / Link reference definitions
      //
      // The following Markdown:
      //       [foo]: /url "title"
      //  
      //   [foo]
      //
      // Should be rendered as:
      //   <pre><code>[foo]: /url &quot;title&quot;
      //   </code></pre>
      //   <p>[foo]</p>

      Parser.Parse("    [foo]: /url \"title\"\n\n[foo]").Is(Parser.Prettify("<pre><code>[foo]: /url &quot;title&quot;\n</code></pre>\n<p>[foo]</p>"));

      Parser.DoubleParse("    [foo]: /url \"title\"\n\n[foo]").Is(Parser.Prettify("<pre><code>[foo]: /url &quot;title&quot;\n</code></pre>\n<p>[foo]</p>"));
    }

    // This is not a link reference definition, because it occurs inside
    // a code block:
    [Fact]
    public void LeafBlocksLinkReferenceDefinitions_Example181()
    {
      // Example 181
      // Section: Leaf blocks / Link reference definitions
      //
      // The following Markdown:
      //   ```
      //   [foo]: /url
      //   ```
      //  
      //   [foo]
      //
      // Should be rendered as:
      //   <pre><code>[foo]: /url
      //   </code></pre>
      //   <p>[foo]</p>

      Parser.Parse("```\n[foo]: /url\n```\n\n[foo]").Is(Parser.Prettify("<pre><code>[foo]: /url\n</code></pre>\n<p>[foo]</p>"));

      Parser.DoubleParse("```\n[foo]: /url\n```\n\n[foo]").Is(Parser.Prettify("<pre><code>[foo]: /url\n</code></pre>\n<p>[foo]</p>"));
    }

    // A [link reference definition] cannot interrupt a paragraph.
    [Fact]
    public void LeafBlocksLinkReferenceDefinitions_Example182()
    {
      // Example 182
      // Section: Leaf blocks / Link reference definitions
      //
      // The following Markdown:
      //   Foo
      //   [bar]: /baz
      //  
      //   [bar]
      //
      // Should be rendered as:
      //   <p>Foo
      //   [bar]: /baz</p>
      //   <p>[bar]</p>

      Parser.Parse("Foo\n[bar]: /baz\n\n[bar]").Is(Parser.Prettify("<p>Foo\n[bar]: /baz</p>\n<p>[bar]</p>"));

      Parser.DoubleParse("Foo\n[bar]: /baz\n\n[bar]").Is(Parser.Prettify("<p>Foo\n[bar]: /baz</p>\n<p>[bar]</p>"));
    }

    // However, it can directly follow other block elements, such as headings
    // and thematic breaks, and it need not be followed by a blank line.
    [Fact]
    public void LeafBlocksLinkReferenceDefinitions_Example183()
    {
      // Example 183
      // Section: Leaf blocks / Link reference definitions
      //
      // The following Markdown:
      //   # [Foo]
      //   [foo]: /url
      //   > bar
      //
      // Should be rendered as:
      //   <h1><a href="/url">Foo</a></h1>
      //   <blockquote>
      //   <p>bar</p>
      //   </blockquote>

      Parser.Parse("# [Foo]\n[foo]: /url\n> bar").Is(Parser.Prettify("<h1><a href=\"/url\">Foo</a></h1>\n<blockquote>\n<p>bar</p>\n</blockquote>"));

      Parser.DoubleParse("# [Foo]\n[foo]: /url\n> bar").Is(Parser.Prettify("<h1><a href=\"/url\">Foo</a></h1>\n<blockquote>\n<p>bar</p>\n</blockquote>"));
    }

    [Fact]
    public void LeafBlocksLinkReferenceDefinitions_Example184()
    {
      // Example 184
      // Section: Leaf blocks / Link reference definitions
      //
      // The following Markdown:
      //   [foo]: /url
      //   bar
      //   ===
      //   [foo]
      //
      // Should be rendered as:
      //   <h1>bar</h1>
      //   <p><a href="/url">foo</a></p>

      Parser.Parse("[foo]: /url\nbar\n===\n[foo]").Is(Parser.Prettify("<h1>bar</h1>\n<p><a href=\"/url\">foo</a></p>"));

      Parser.DoubleParse("[foo]: /url\nbar\n===\n[foo]").Is(Parser.Prettify("<h1>bar</h1>\n<p><a href=\"/url\">foo</a></p>"));
    }

    [Fact]
    public void LeafBlocksLinkReferenceDefinitions_Example185()
    {
      // Example 185
      // Section: Leaf blocks / Link reference definitions
      //
      // The following Markdown:
      //   [foo]: /url
      //   ===
      //   [foo]
      //
      // Should be rendered as:
      //   <p>===
      //   <a href="/url">foo</a></p>

      Parser.Parse("[foo]: /url\n===\n[foo]").Is(Parser.Prettify("<p>===\n<a href=\"/url\">foo</a></p>"));

      Parser.DoubleParse("[foo]: /url\n===\n[foo]").Is(Parser.Prettify("<p>===\n<a href=\"/url\">foo</a></p>"));
    }

    // Several [link reference definitions]
    // can occur one after another, without intervening blank lines.
    [Fact]
    public void LeafBlocksLinkReferenceDefinitions_Example186()
    {
      // Example 186
      // Section: Leaf blocks / Link reference definitions
      //
      // The following Markdown:
      //   [foo]: /foo-url "foo"
      //   [bar]: /bar-url
      //     "bar"
      //   [baz]: /baz-url
      //  
      //   [foo],
      //   [bar],
      //   [baz]
      //
      // Should be rendered as:
      //   <p><a href="/foo-url" title="foo">foo</a>,
      //   <a href="/bar-url" title="bar">bar</a>,
      //   <a href="/baz-url">baz</a></p>

      Parser.Parse("[foo]: /foo-url \"foo\"\n[bar]: /bar-url\n  \"bar\"\n[baz]: /baz-url\n\n[foo],\n[bar],\n[baz]").Is(Parser.Prettify("<p><a href=\"/foo-url\" title=\"foo\">foo</a>,\n<a href=\"/bar-url\" title=\"bar\">bar</a>,\n<a href=\"/baz-url\">baz</a></p>"));

      Parser.DoubleParse("[foo]: /foo-url \"foo\"\n[bar]: /bar-url\n  \"bar\"\n[baz]: /baz-url\n\n[foo],\n[bar],\n[baz]").Is(Parser.Prettify("<p><a href=\"/foo-url\" title=\"foo\">foo</a>,\n<a href=\"/bar-url\" title=\"bar\">bar</a>,\n<a href=\"/baz-url\">baz</a></p>"));
    }

    // [Link reference definitions] can occur
    // inside block containers, like lists and block quotations.  They
    // affect the entire document, not just the container in which they
    // are defined:
    [Fact]
    public void LeafBlocksLinkReferenceDefinitions_Example187()
    {
      // Example 187
      // Section: Leaf blocks / Link reference definitions
      //
      // The following Markdown:
      //   [foo]
      //  
      //   > [foo]: /url
      //
      // Should be rendered as:
      //   <p><a href="/url">foo</a></p>
      //   <blockquote>
      //   </blockquote>

      Parser.Parse("[foo]\n\n> [foo]: /url").Is(Parser.Prettify("<p><a href=\"/url\">foo</a></p>\n<blockquote>\n</blockquote>"));

      Parser.DoubleParse("[foo]\n\n> [foo]: /url").Is(Parser.Prettify("<p><a href=\"/url\">foo</a></p>\n<blockquote>\n</blockquote>"));
    }

    // Whether something is a [link reference definition] is
    // independent of whether the link reference it defines is
    // used in the document.  Thus, for example, the following
    // document contains just a link reference definition, and
    // no visible content:
    [Fact]
    public void LeafBlocksLinkReferenceDefinitions_Example188()
    {
      // Example 188
      // Section: Leaf blocks / Link reference definitions
      //
      // The following Markdown:
      //   [foo]: /url
      //
      // Should be rendered as:
      //
      Parser.Parse("[foo]: /url").Is(Parser.Prettify(""));

      Parser.DoubleParse("[foo]: /url").Is(Parser.Prettify(""));
    }
  }

  public class TestLeafBlocksParagraphs
  {
    // ## Paragraphs
    //
    // A sequence of non-blank lines that cannot be interpreted as other
    // kinds of blocks forms a [paragraph](@).
    // The contents of the paragraph are the result of parsing the
    // paragraph's raw content as inlines.  The paragraph's raw content
    // is formed by concatenating the lines and removing initial and final
    // [whitespace].
    //
    // A simple example with two paragraphs:
    [Fact]
    public void LeafBlocksParagraphs_Example189()
    {
      // Example 189
      // Section: Leaf blocks / Paragraphs
      //
      // The following Markdown:
      //   aaa
      //  
      //   bbb
      //
      // Should be rendered as:
      //   <p>aaa</p>
      //   <p>bbb</p>

      Parser.Parse("aaa\n\nbbb").Is(Parser.Prettify("<p>aaa</p>\n<p>bbb</p>"));

      Parser.DoubleParse("aaa\n\nbbb").Is(Parser.Prettify("<p>aaa</p>\n<p>bbb</p>"));
    }

    // Paragraphs can contain multiple lines, but no blank lines:
    [Fact]
    public void LeafBlocksParagraphs_Example190()
    {
      // Example 190
      // Section: Leaf blocks / Paragraphs
      //
      // The following Markdown:
      //   aaa
      //   bbb
      //  
      //   ccc
      //   ddd
      //
      // Should be rendered as:
      //   <p>aaa
      //   bbb</p>
      //   <p>ccc
      //   ddd</p>

      Parser.Parse("aaa\nbbb\n\nccc\nddd").Is(Parser.Prettify("<p>aaa\nbbb</p>\n<p>ccc\nddd</p>"));

      Parser.DoubleParse("aaa\nbbb\n\nccc\nddd").Is(Parser.Prettify("<p>aaa\nbbb</p>\n<p>ccc\nddd</p>"));
    }

    // Multiple blank lines between paragraph have no effect:
    [Fact]
    public void LeafBlocksParagraphs_Example191()
    {
      // Example 191
      // Section: Leaf blocks / Paragraphs
      //
      // The following Markdown:
      //   aaa
      //  
      //  
      //   bbb
      //
      // Should be rendered as:
      //   <p>aaa</p>
      //   <p>bbb</p>

      Parser.Parse("aaa\n\n\nbbb").Is(Parser.Prettify("<p>aaa</p>\n<p>bbb</p>"));

      Parser.DoubleParse("aaa\n\n\nbbb").Is(Parser.Prettify("<p>aaa</p>\n<p>bbb</p>"));
    }

    // Leading spaces are skipped:
    [Fact]
    public void LeafBlocksParagraphs_Example192()
    {
      // Example 192
      // Section: Leaf blocks / Paragraphs
      //
      // The following Markdown:
      //     aaa
      //    bbb
      //
      // Should be rendered as:
      //   <p>aaa
      //   bbb</p>

      Parser.Parse("  aaa\n bbb").Is(Parser.Prettify("<p>aaa\nbbb</p>"));

      Parser.DoubleParse("  aaa\n bbb").Is(Parser.Prettify("<p>aaa\nbbb</p>"));
    }

    // Lines after the first may be indented any amount, since indented
    // code blocks cannot interrupt paragraphs.
    [Fact]
    public void LeafBlocksParagraphs_Example193()
    {
      // Example 193
      // Section: Leaf blocks / Paragraphs
      //
      // The following Markdown:
      //   aaa
      //                bbb
      //                                          ccc
      //
      // Should be rendered as:
      //   <p>aaa
      //   bbb
      //   ccc</p>

      Parser.Parse("aaa\n             bbb\n                                       ccc").Is(Parser.Prettify("<p>aaa\nbbb\nccc</p>"));

      Parser.DoubleParse("aaa\n             bbb\n                                       ccc").Is(Parser.Prettify("<p>aaa\nbbb\nccc</p>"));
    }

    // However, the first line may be indented at most three spaces,
    // or an indented code block will be triggered:
    [Fact]
    public void LeafBlocksParagraphs_Example194()
    {
      // Example 194
      // Section: Leaf blocks / Paragraphs
      //
      // The following Markdown:
      //      aaa
      //   bbb
      //
      // Should be rendered as:
      //   <p>aaa
      //   bbb</p>

      Parser.Parse("   aaa\nbbb").Is(Parser.Prettify("<p>aaa\nbbb</p>"));

      Parser.DoubleParse("   aaa\nbbb").Is(Parser.Prettify("<p>aaa\nbbb</p>"));
    }

    [Fact]
    public void LeafBlocksParagraphs_Example195()
    {
      // Example 195
      // Section: Leaf blocks / Paragraphs
      //
      // The following Markdown:
      //       aaa
      //   bbb
      //
      // Should be rendered as:
      //   <pre><code>aaa
      //   </code></pre>
      //   <p>bbb</p>

      Parser.Parse("    aaa\nbbb").Is(Parser.Prettify("<pre><code>aaa\n</code></pre>\n<p>bbb</p>"));

      Parser.DoubleParse("    aaa\nbbb").Is(Parser.Prettify("<pre><code>aaa\n</code></pre>\n<p>bbb</p>"));
    }

    // Final spaces are stripped before inline parsing, so a paragraph
    // that ends with two or more spaces will not end with a [hard line
    // break]:
    [Fact]
    public void LeafBlocksParagraphs_Example196()
    {
      // Example 196
      // Section: Leaf blocks / Paragraphs
      //
      // The following Markdown:
      //   aaa     
      //   bbb     
      //
      // Should be rendered as:
      //   <p>aaa<br />
      //   bbb</p>

      Parser.Parse("aaa     \nbbb     ").Is(Parser.Prettify("<p>aaa<br />\nbbb</p>"));

      Parser.DoubleParse("aaa     \nbbb     ").Is(Parser.Prettify("<p>aaa<br />\nbbb</p>"));
    }
  }

  public class TestLeafBlocksBlankLines
  {
    // ## Blank lines
    //
    // [Blank lines] between block-level elements are ignored,
    // except for the role they play in determining whether a [list]
    // is [tight] or [loose].
    //
    // Blank lines at the beginning and end of the document are also ignored.
    [Fact]
    public void LeafBlocksBlankLines_Example197()
    {
      // Example 197
      // Section: Leaf blocks / Blank lines
      //
      // The following Markdown:
      //     
      //  
      //   aaa
      //     
      //  
      //   # aaa
      //  
      //     
      //
      // Should be rendered as:
      //   <p>aaa</p>
      //   <h1>aaa</h1>

      Parser.Parse("  \n\naaa\n  \n\n# aaa\n\n  ").Is(Parser.Prettify("<p>aaa</p>\n<h1>aaa</h1>"));

      Parser.DoubleParse("  \n\naaa\n  \n\n# aaa\n\n  ").Is(Parser.Prettify("<p>aaa</p>\n<h1>aaa</h1>"));
    }
  }

  public class TestContainerBlocksBlockQuotes
  {
    // # Container blocks
    //
    // A [container block](#container-blocks) is a block that has other
    // blocks as its contents.  There are two basic kinds of container blocks:
    // [block quotes] and [list items].
    // [Lists] are meta-containers for [list items].
    //
    // We define the syntax for container blocks recursively.  The general
    // form of the definition is:
    //
    // > If X is a sequence of blocks, then the result of
    // > transforming X in such-and-such a way is a container of type Y
    // > with these blocks as its content.
    //
    // So, we explain what counts as a block quote or list item by explaining
    // how these can be *generated* from their contents. This should suffice
    // to define the syntax, although it does not give a recipe for *parsing*
    // these constructions.  (A recipe is provided below in the section entitled
    // [A parsing strategy](#appendix-a-parsing-strategy).)
    //
    // ## Block quotes
    //
    // A [block quote marker](@)
    // consists of 0-3 spaces of initial indent, plus (a) the character `>` together
    // with a following space, or (b) a single character `>` not followed by a space.
    //
    // The following rules define [block quotes]:
    //
    // 1.  **Basic case.**  If a string of lines *Ls* constitute a sequence
    //     of blocks *Bs*, then the result of prepending a [block quote
    //     marker] to the beginning of each line in *Ls*
    //     is a [block quote](#block-quotes) containing *Bs*.
    //
    // 2.  **Laziness.**  If a string of lines *Ls* constitute a [block
    //     quote](#block-quotes) with contents *Bs*, then the result of deleting
    //     the initial [block quote marker] from one or
    //     more lines in which the next [non-whitespace character] after the [block
    //     quote marker] is [paragraph continuation
    //     text] is a block quote with *Bs* as its content.
    //     [Paragraph continuation text](@) is text
    //     that will be parsed as part of the content of a paragraph, but does
    //     not occur at the beginning of the paragraph.
    //
    // 3.  **Consecutiveness.**  A document cannot contain two [block
    //     quotes] in a row unless there is a [blank line] between them.
    //
    // Nothing else counts as a [block quote](#block-quotes).
    //
    // Here is a simple example:
    [Fact]
    public void ContainerBlocksBlockQuotes_Example198()
    {
      // Example 198
      // Section: Container blocks / Block quotes
      //
      // The following Markdown:
      //   > # Foo
      //   > bar
      //   > baz
      //
      // Should be rendered as:
      //   <blockquote>
      //   <h1>Foo</h1>
      //   <p>bar
      //   baz</p>
      //   </blockquote>

      Parser.Parse("> # Foo\n> bar\n> baz").Is(Parser.Prettify("<blockquote>\n<h1>Foo</h1>\n<p>bar\nbaz</p>\n</blockquote>"));

      Parser.DoubleParse("> # Foo\n> bar\n> baz").Is(Parser.Prettify("<blockquote>\n<h1>Foo</h1>\n<p>bar\nbaz</p>\n</blockquote>"));
    }

    // The spaces after the `>` characters can be omitted:
    [Fact]
    public void ContainerBlocksBlockQuotes_Example199()
    {
      // Example 199
      // Section: Container blocks / Block quotes
      //
      // The following Markdown:
      //   ># Foo
      //   >bar
      //   > baz
      //
      // Should be rendered as:
      //   <blockquote>
      //   <h1>Foo</h1>
      //   <p>bar
      //   baz</p>
      //   </blockquote>

      Parser.Parse("># Foo\n>bar\n> baz").Is(Parser.Prettify("<blockquote>\n<h1>Foo</h1>\n<p>bar\nbaz</p>\n</blockquote>"));

      Parser.DoubleParse("># Foo\n>bar\n> baz").Is(Parser.Prettify("<blockquote>\n<h1>Foo</h1>\n<p>bar\nbaz</p>\n</blockquote>"));
    }

    // The `>` characters can be indented 1-3 spaces:
    [Fact]
    public void ContainerBlocksBlockQuotes_Example200()
    {
      // Example 200
      // Section: Container blocks / Block quotes
      //
      // The following Markdown:
      //      > # Foo
      //      > bar
      //    > baz
      //
      // Should be rendered as:
      //   <blockquote>
      //   <h1>Foo</h1>
      //   <p>bar
      //   baz</p>
      //   </blockquote>

      Parser.Parse("   > # Foo\n   > bar\n > baz").Is(Parser.Prettify("<blockquote>\n<h1>Foo</h1>\n<p>bar\nbaz</p>\n</blockquote>"));

      Parser.DoubleParse("   > # Foo\n   > bar\n > baz").Is(Parser.Prettify("<blockquote>\n<h1>Foo</h1>\n<p>bar\nbaz</p>\n</blockquote>"));
    }

    // Four spaces gives us a code block:
    [Fact]
    public void ContainerBlocksBlockQuotes_Example201()
    {
      // Example 201
      // Section: Container blocks / Block quotes
      //
      // The following Markdown:
      //       > # Foo
      //       > bar
      //       > baz
      //
      // Should be rendered as:
      //   <pre><code>&gt; # Foo
      //   &gt; bar
      //   &gt; baz
      //   </code></pre>

      Parser.Parse("    > # Foo\n    > bar\n    > baz").Is(Parser.Prettify("<pre><code>&gt; # Foo\n&gt; bar\n&gt; baz\n</code></pre>"));

      Parser.DoubleParse("    > # Foo\n    > bar\n    > baz").Is(Parser.Prettify("<pre><code>&gt; # Foo\n&gt; bar\n&gt; baz\n</code></pre>"));
    }

    // The Laziness clause allows us to omit the `>` before
    // [paragraph continuation text]:
    [Fact]
    public void ContainerBlocksBlockQuotes_Example202()
    {
      // Example 202
      // Section: Container blocks / Block quotes
      //
      // The following Markdown:
      //   > # Foo
      //   > bar
      //   baz
      //
      // Should be rendered as:
      //   <blockquote>
      //   <h1>Foo</h1>
      //   <p>bar
      //   baz</p>
      //   </blockquote>

      Parser.Parse("> # Foo\n> bar\nbaz").Is(Parser.Prettify("<blockquote>\n<h1>Foo</h1>\n<p>bar\nbaz</p>\n</blockquote>"));

      Parser.DoubleParse("> # Foo\n> bar\nbaz").Is(Parser.Prettify("<blockquote>\n<h1>Foo</h1>\n<p>bar\nbaz</p>\n</blockquote>"));
    }

    // A block quote can contain some lazy and some non-lazy
    // continuation lines:
    [Fact]
    public void ContainerBlocksBlockQuotes_Example203()
    {
      // Example 203
      // Section: Container blocks / Block quotes
      //
      // The following Markdown:
      //   > bar
      //   baz
      //   > foo
      //
      // Should be rendered as:
      //   <blockquote>
      //   <p>bar
      //   baz
      //   foo</p>
      //   </blockquote>

      Parser.Parse("> bar\nbaz\n> foo").Is(Parser.Prettify("<blockquote>\n<p>bar\nbaz\nfoo</p>\n</blockquote>"));

      Parser.DoubleParse("> bar\nbaz\n> foo").Is(Parser.Prettify("<blockquote>\n<p>bar\nbaz\nfoo</p>\n</blockquote>"));
    }

    // Laziness only applies to lines that would have been continuations of
    // paragraphs had they been prepended with [block quote markers].
    // For example, the `> ` cannot be omitted in the second line of
    //
    // ``` markdown
    // > foo
    // > ---
    // ```
    //
    // without changing the meaning:
    [Fact]
    public void ContainerBlocksBlockQuotes_Example204()
    {
      // Example 204
      // Section: Container blocks / Block quotes
      //
      // The following Markdown:
      //   > foo
      //   ---
      //
      // Should be rendered as:
      //   <blockquote>
      //   <p>foo</p>
      //   </blockquote>
      //   <hr />

      Parser.Parse("> foo\n---").Is(Parser.Prettify("<blockquote>\n<p>foo</p>\n</blockquote>\n<hr />"));

      Parser.DoubleParse("> foo\n---").Is(Parser.Prettify("<blockquote>\n<p>foo</p>\n</blockquote>\n<hr />"));
    }

    // Similarly, if we omit the `> ` in the second line of
    //
    // ``` markdown
    // > - foo
    // > - bar
    // ```
    //
    // then the block quote ends after the first line:
    [Fact]
    public void ContainerBlocksBlockQuotes_Example205()
    {
      // Example 205
      // Section: Container blocks / Block quotes
      //
      // The following Markdown:
      //   > - foo
      //   - bar
      //
      // Should be rendered as:
      //   <blockquote>
      //   <ul>
      //   <li>foo</li>
      //   </ul>
      //   </blockquote>
      //   <ul>
      //   <li>bar</li>
      //   </ul>

      Parser.Parse("> - foo\n- bar").Is(Parser.Prettify("<blockquote>\n<ul>\n<li>foo</li>\n</ul>\n</blockquote>\n<ul>\n<li>bar</li>\n</ul>"));

      Parser.DoubleParse("> - foo\n- bar").Is(Parser.Prettify("<blockquote>\n<ul>\n<li>foo</li>\n</ul>\n</blockquote>\n<ul>\n<li>bar</li>\n</ul>"));
    }

    // For the same reason, we can't omit the `> ` in front of
    // subsequent lines of an indented or fenced code block:
    [Fact]
    public void ContainerBlocksBlockQuotes_Example206()
    {
      // Example 206
      // Section: Container blocks / Block quotes
      //
      // The following Markdown:
      //   >     foo
      //       bar
      //
      // Should be rendered as:
      //   <blockquote>
      //   <pre><code>foo
      //   </code></pre>
      //   </blockquote>
      //   <pre><code>bar
      //   </code></pre>

      Parser.Parse(">     foo\n    bar").Is(Parser.Prettify("<blockquote>\n<pre><code>foo\n</code></pre>\n</blockquote>\n<pre><code>bar\n</code></pre>"));

      Parser.DoubleParse(">     foo\n    bar").Is(Parser.Prettify("<blockquote>\n<pre><code>foo\n</code></pre>\n</blockquote>\n<pre><code>bar\n</code></pre>"));
    }

    [Fact]
    public void ContainerBlocksBlockQuotes_Example207()
    {
      // Example 207
      // Section: Container blocks / Block quotes
      //
      // The following Markdown:
      //   > ```
      //   foo
      //   ```
      //
      // Should be rendered as:
      //   <blockquote>
      //   <pre><code></code></pre>
      //   </blockquote>
      //   <p>foo</p>
      //   <pre><code></code></pre>

      Parser.Parse("> ```\nfoo\n```").Is(Parser.Prettify("<blockquote>\n<pre><code></code></pre>\n</blockquote>\n<p>foo</p>\n<pre><code></code></pre>"));

      Parser.DoubleParse("> ```\nfoo\n```").Is(Parser.Prettify("<blockquote>\n<pre><code></code></pre>\n</blockquote>\n<p>foo</p>\n<pre><code></code></pre>"));
    }

    // Note that in the following case, we have a [lazy
    // continuation line]:
    [Fact]
    public void ContainerBlocksBlockQuotes_Example208()
    {
      // Example 208
      // Section: Container blocks / Block quotes
      //
      // The following Markdown:
      //   > foo
      //       - bar
      //
      // Should be rendered as:
      //   <blockquote>
      //   <p>foo
      //   - bar</p>
      //   </blockquote>

      Parser.Parse("> foo\n    - bar").Is(Parser.Prettify("<blockquote>\n<p>foo\n- bar</p>\n</blockquote>"));

      Parser.DoubleParse("> foo\n    - bar").Is(Parser.Prettify("<blockquote>\n<p>foo\n- bar</p>\n</blockquote>"));
    }

    // To see why, note that in
    //
    // ```markdown
    // > foo
    // >     - bar
    // ```
    //
    // the `- bar` is indented too far to start a list, and can't
    // be an indented code block because indented code blocks cannot
    // interrupt paragraphs, so it is [paragraph continuation text].
    //
    // A block quote can be empty:
    [Fact]
    public void ContainerBlocksBlockQuotes_Example209()
    {
      // Example 209
      // Section: Container blocks / Block quotes
      //
      // The following Markdown:
      //   >
      //
      // Should be rendered as:
      //   <blockquote>
      //   </blockquote>

      Parser.Parse(">").Is(Parser.Prettify("<blockquote>\n</blockquote>"));

      Parser.DoubleParse(">").Is(Parser.Prettify("<blockquote>\n</blockquote>"));
    }

    [Fact]
    public void ContainerBlocksBlockQuotes_Example210()
    {
      // Example 210
      // Section: Container blocks / Block quotes
      //
      // The following Markdown:
      //   >
      //   >  
      //   > 
      //
      // Should be rendered as:
      //   <blockquote>
      //   </blockquote>

      Parser.Parse(">\n>  \n> ").Is(Parser.Prettify("<blockquote>\n</blockquote>"));

      Parser.DoubleParse(">\n>  \n> ").Is(Parser.Prettify("<blockquote>\n</blockquote>"));
    }

    // A block quote can have initial or final blank lines:
    [Fact]
    public void ContainerBlocksBlockQuotes_Example211()
    {
      // Example 211
      // Section: Container blocks / Block quotes
      //
      // The following Markdown:
      //   >
      //   > foo
      //   >  
      //
      // Should be rendered as:
      //   <blockquote>
      //   <p>foo</p>
      //   </blockquote>

      Parser.Parse(">\n> foo\n>  ").Is(Parser.Prettify("<blockquote>\n<p>foo</p>\n</blockquote>"));

      Parser.DoubleParse(">\n> foo\n>  ").Is(Parser.Prettify("<blockquote>\n<p>foo</p>\n</blockquote>"));
    }

    // A blank line always separates block quotes:
    [Fact]
    public void ContainerBlocksBlockQuotes_Example212()
    {
      // Example 212
      // Section: Container blocks / Block quotes
      //
      // The following Markdown:
      //   > foo
      //  
      //   > bar
      //
      // Should be rendered as:
      //   <blockquote>
      //   <p>foo</p>
      //   </blockquote>
      //   <blockquote>
      //   <p>bar</p>
      //   </blockquote>

      Parser.Parse("> foo\n\n> bar").Is(Parser.Prettify("<blockquote>\n<p>foo</p>\n</blockquote>\n<blockquote>\n<p>bar</p>\n</blockquote>"));

      Parser.DoubleParse("> foo\n\n> bar").Is(Parser.Prettify("<blockquote>\n<p>foo</p>\n</blockquote>\n<blockquote>\n<p>bar</p>\n</blockquote>"));
    }

    // (Most current Markdown implementations, including John Gruber's
    // original `Markdown.pl`, will parse this example as a single block quote
    // with two paragraphs.  But it seems better to allow the author to decide
    // whether two block quotes or one are wanted.)
    //
    // Consecutiveness means that if we put these block quotes together,
    // we get a single block quote:
    [Fact]
    public void ContainerBlocksBlockQuotes_Example213()
    {
      // Example 213
      // Section: Container blocks / Block quotes
      //
      // The following Markdown:
      //   > foo
      //   > bar
      //
      // Should be rendered as:
      //   <blockquote>
      //   <p>foo
      //   bar</p>
      //   </blockquote>

      Parser.Parse("> foo\n> bar").Is(Parser.Prettify("<blockquote>\n<p>foo\nbar</p>\n</blockquote>"));

      Parser.DoubleParse("> foo\n> bar").Is(Parser.Prettify("<blockquote>\n<p>foo\nbar</p>\n</blockquote>"));
    }

    // To get a block quote with two paragraphs, use:
    [Fact]
    public void ContainerBlocksBlockQuotes_Example214()
    {
      // Example 214
      // Section: Container blocks / Block quotes
      //
      // The following Markdown:
      //   > foo
      //   >
      //   > bar
      //
      // Should be rendered as:
      //   <blockquote>
      //   <p>foo</p>
      //   <p>bar</p>
      //   </blockquote>

      Parser.Parse("> foo\n>\n> bar").Is(Parser.Prettify("<blockquote>\n<p>foo</p>\n<p>bar</p>\n</blockquote>"));

      Parser.DoubleParse("> foo\n>\n> bar").Is(Parser.Prettify("<blockquote>\n<p>foo</p>\n<p>bar</p>\n</blockquote>"));
    }

    // Block quotes can interrupt paragraphs:
    [Fact]
    public void ContainerBlocksBlockQuotes_Example215()
    {
      // Example 215
      // Section: Container blocks / Block quotes
      //
      // The following Markdown:
      //   foo
      //   > bar
      //
      // Should be rendered as:
      //   <p>foo</p>
      //   <blockquote>
      //   <p>bar</p>
      //   </blockquote>

      Parser.Parse("foo\n> bar").Is(Parser.Prettify("<p>foo</p>\n<blockquote>\n<p>bar</p>\n</blockquote>"));

      Parser.DoubleParse("foo\n> bar").Is(Parser.Prettify("<p>foo</p>\n<blockquote>\n<p>bar</p>\n</blockquote>"));
    }

    // In general, blank lines are not needed before or after block
    // quotes:
    [Fact]
    public void ContainerBlocksBlockQuotes_Example216()
    {
      // Example 216
      // Section: Container blocks / Block quotes
      //
      // The following Markdown:
      //   > aaa
      //   ***
      //   > bbb
      //
      // Should be rendered as:
      //   <blockquote>
      //   <p>aaa</p>
      //   </blockquote>
      //   <hr />
      //   <blockquote>
      //   <p>bbb</p>
      //   </blockquote>

      Parser.Parse("> aaa\n***\n> bbb").Is(Parser.Prettify("<blockquote>\n<p>aaa</p>\n</blockquote>\n<hr />\n<blockquote>\n<p>bbb</p>\n</blockquote>"));

      Parser.DoubleParse("> aaa\n***\n> bbb").Is(Parser.Prettify("<blockquote>\n<p>aaa</p>\n</blockquote>\n<hr />\n<blockquote>\n<p>bbb</p>\n</blockquote>"));
    }

    // However, because of laziness, a blank line is needed between
    // a block quote and a following paragraph:
    [Fact]
    public void ContainerBlocksBlockQuotes_Example217()
    {
      // Example 217
      // Section: Container blocks / Block quotes
      //
      // The following Markdown:
      //   > bar
      //   baz
      //
      // Should be rendered as:
      //   <blockquote>
      //   <p>bar
      //   baz</p>
      //   </blockquote>

      Parser.Parse("> bar\nbaz").Is(Parser.Prettify("<blockquote>\n<p>bar\nbaz</p>\n</blockquote>"));

      Parser.DoubleParse("> bar\nbaz").Is(Parser.Prettify("<blockquote>\n<p>bar\nbaz</p>\n</blockquote>"));
    }

    [Fact]
    public void ContainerBlocksBlockQuotes_Example218()
    {
      // Example 218
      // Section: Container blocks / Block quotes
      //
      // The following Markdown:
      //   > bar
      //  
      //   baz
      //
      // Should be rendered as:
      //   <blockquote>
      //   <p>bar</p>
      //   </blockquote>
      //   <p>baz</p>

      Parser.Parse("> bar\n\nbaz").Is(Parser.Prettify("<blockquote>\n<p>bar</p>\n</blockquote>\n<p>baz</p>"));

      Parser.DoubleParse("> bar\n\nbaz").Is(Parser.Prettify("<blockquote>\n<p>bar</p>\n</blockquote>\n<p>baz</p>"));
    }

    [Fact]
    public void ContainerBlocksBlockQuotes_Example219()
    {
      // Example 219
      // Section: Container blocks / Block quotes
      //
      // The following Markdown:
      //   > bar
      //   >
      //   baz
      //
      // Should be rendered as:
      //   <blockquote>
      //   <p>bar</p>
      //   </blockquote>
      //   <p>baz</p>

      Parser.Parse("> bar\n>\nbaz").Is(Parser.Prettify("<blockquote>\n<p>bar</p>\n</blockquote>\n<p>baz</p>"));

      Parser.DoubleParse("> bar\n>\nbaz").Is(Parser.Prettify("<blockquote>\n<p>bar</p>\n</blockquote>\n<p>baz</p>"));
    }

    // It is a consequence of the Laziness rule that any number
    // of initial `>`s may be omitted on a continuation line of a
    // nested block quote:
    [Fact]
    public void ContainerBlocksBlockQuotes_Example220()
    {
      // Example 220
      // Section: Container blocks / Block quotes
      //
      // The following Markdown:
      //   > > > foo
      //   bar
      //
      // Should be rendered as:
      //   <blockquote>
      //   <blockquote>
      //   <blockquote>
      //   <p>foo
      //   bar</p>
      //   </blockquote>
      //   </blockquote>
      //   </blockquote>

      Parser.Parse("> > > foo\nbar").Is(Parser.Prettify("<blockquote>\n<blockquote>\n<blockquote>\n<p>foo\nbar</p>\n</blockquote>\n</blockquote>\n</blockquote>"));

      Parser.DoubleParse("> > > foo\nbar").Is(Parser.Prettify("<blockquote>\n<blockquote>\n<blockquote>\n<p>foo\nbar</p>\n</blockquote>\n</blockquote>\n</blockquote>"));
    }

    [Fact]
    public void ContainerBlocksBlockQuotes_Example221()
    {
      // Example 221
      // Section: Container blocks / Block quotes
      //
      // The following Markdown:
      //   >>> foo
      //   > bar
      //   >>baz
      //
      // Should be rendered as:
      //   <blockquote>
      //   <blockquote>
      //   <blockquote>
      //   <p>foo
      //   bar
      //   baz</p>
      //   </blockquote>
      //   </blockquote>
      //   </blockquote>

      Parser.Parse(">>> foo\n> bar\n>>baz").Is(Parser.Prettify("<blockquote>\n<blockquote>\n<blockquote>\n<p>foo\nbar\nbaz</p>\n</blockquote>\n</blockquote>\n</blockquote>"));

      Parser.DoubleParse(">>> foo\n> bar\n>>baz").Is(Parser.Prettify("<blockquote>\n<blockquote>\n<blockquote>\n<p>foo\nbar\nbaz</p>\n</blockquote>\n</blockquote>\n</blockquote>"));
    }

    // When including an indented code block in a block quote,
    // remember that the [block quote marker] includes
    // both the `>` and a following space.  So *five spaces* are needed after
    // the `>`:
    [Fact]
    public void ContainerBlocksBlockQuotes_Example222()
    {
      // Example 222
      // Section: Container blocks / Block quotes
      //
      // The following Markdown:
      //   >     code
      //  
      //   >    not code
      //
      // Should be rendered as:
      //   <blockquote>
      //   <pre><code>code
      //   </code></pre>
      //   </blockquote>
      //   <blockquote>
      //   <p>not code</p>
      //   </blockquote>

      Parser.Parse(">     code\n\n>    not code").Is(Parser.Prettify("<blockquote>\n<pre><code>code\n</code></pre>\n</blockquote>\n<blockquote>\n<p>not code</p>\n</blockquote>"));

      Parser.DoubleParse(">     code\n\n>    not code").Is(Parser.Prettify("<blockquote>\n<pre><code>code\n</code></pre>\n</blockquote>\n<blockquote>\n<p>not code</p>\n</blockquote>"));
    }
  }

  public class TestContainerBlocksListItems
  {
    // ## List items
    //
    // A [list marker](@) is a
    // [bullet list marker] or an [ordered list marker].
    //
    // A [bullet list marker](@)
    // is a `-`, `+`, or `*` character.
    //
    // An [ordered list marker](@)
    // is a sequence of 1--9 arabic digits (`0-9`), followed by either a
    // `.` character or a `)` character.  (The reason for the length
    // limit is that with 10 digits we start seeing integer overflows
    // in some browsers.)
    //
    // The following rules define [list items]:
    //
    // 1.  **Basic case.**  If a sequence of lines *Ls* constitute a sequence of
    //     blocks *Bs* starting with a [non-whitespace character], and *M* is a
    //     list marker of width *W* followed by 1 ≤ *N* ≤ 4 spaces, then the result
    //     of prepending *M* and the following spaces to the first line of
    //     *Ls*, and indenting subsequent lines of *Ls* by *W + N* spaces, is a
    //     list item with *Bs* as its contents.  The type of the list item
    //     (bullet or ordered) is determined by the type of its list marker.
    //     If the list item is ordered, then it is also assigned a start
    //     number, based on the ordered list marker.
    //
    //     Exceptions:
    //
    //     1. When the first list item in a [list] interrupts
    //        a paragraph---that is, when it starts on a line that would
    //        otherwise count as [paragraph continuation text]---then (a)
    //        the lines *Ls* must not begin with a blank line, and (b) if
    //        the list item is ordered, the start number must be 1.
    //     2. If any line is a [thematic break][thematic breaks] then
    //        that line is not a list item.
    //
    // For example, let *Ls* be the lines
    [Fact]
    public void ContainerBlocksListItems_Example223()
    {
      // Example 223
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //   A paragraph
      //   with two lines.
      //  
      //       indented code
      //  
      //   > A block quote.
      //
      // Should be rendered as:
      //   <p>A paragraph
      //   with two lines.</p>
      //   <pre><code>indented code
      //   </code></pre>
      //   <blockquote>
      //   <p>A block quote.</p>
      //   </blockquote>

      Parser.Parse("A paragraph\nwith two lines.\n\n    indented code\n\n> A block quote.").Is(Parser.Prettify("<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>"));

      Parser.DoubleParse("A paragraph\nwith two lines.\n\n    indented code\n\n> A block quote.").Is(Parser.Prettify("<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>"));
    }

    // And let *M* be the marker `1.`, and *N* = 2.  Then rule #1 says
    // that the following is an ordered list item with start number 1,
    // and the same contents as *Ls*:
    [Fact]
    public void ContainerBlocksListItems_Example224()
    {
      // Example 224
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //   1.  A paragraph
      //       with two lines.
      //  
      //           indented code
      //  
      //       > A block quote.
      //
      // Should be rendered as:
      //   <ol>
      //   <li>
      //   <p>A paragraph
      //   with two lines.</p>
      //   <pre><code>indented code
      //   </code></pre>
      //   <blockquote>
      //   <p>A block quote.</p>
      //   </blockquote>
      //   </li>
      //   </ol>

      Parser.Parse("1.  A paragraph\n    with two lines.\n\n        indented code\n\n    > A block quote.").Is(Parser.Prettify("<ol>\n<li>\n<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>\n</li>\n</ol>"));

      Parser.DoubleParse("1.  A paragraph\n    with two lines.\n\n        indented code\n\n    > A block quote.").Is(Parser.Prettify("<ol>\n<li>\n<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>\n</li>\n</ol>"));
    }

    // The most important thing to notice is that the position of
    // the text after the list marker determines how much indentation
    // is needed in subsequent blocks in the list item.  If the list
    // marker takes up two spaces, and there are three spaces between
    // the list marker and the next [non-whitespace character], then blocks
    // must be indented five spaces in order to fall under the list
    // item.
    //
    // Here are some examples showing how far content must be indented to be
    // put under the list item:
    [Fact]
    public void ContainerBlocksListItems_Example225()
    {
      // Example 225
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //   - one
      //  
      //    two
      //
      // Should be rendered as:
      //   <ul>
      //   <li>one</li>
      //   </ul>
      //   <p>two</p>

      Parser.Parse("- one\n\n two").Is(Parser.Prettify("<ul>\n<li>one</li>\n</ul>\n<p>two</p>"));

      Parser.DoubleParse("- one\n\n two").Is(Parser.Prettify("<ul>\n<li>one</li>\n</ul>\n<p>two</p>"));
    }

    [Fact]
    public void ContainerBlocksListItems_Example226()
    {
      // Example 226
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //   - one
      //  
      //     two
      //
      // Should be rendered as:
      //   <ul>
      //   <li>
      //   <p>one</p>
      //   <p>two</p>
      //   </li>
      //   </ul>

      Parser.Parse("- one\n\n  two").Is(Parser.Prettify("<ul>\n<li>\n<p>one</p>\n<p>two</p>\n</li>\n</ul>"));

      Parser.DoubleParse("- one\n\n  two").Is(Parser.Prettify("<ul>\n<li>\n<p>one</p>\n<p>two</p>\n</li>\n</ul>"));
    }

    [Fact]
    public void ContainerBlocksListItems_Example227()
    {
      // Example 227
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //    -    one
      //  
      //        two
      //
      // Should be rendered as:
      //   <ul>
      //   <li>one</li>
      //   </ul>
      //   <pre><code> two
      //   </code></pre>

      Parser.Parse(" -    one\n\n     two").Is(Parser.Prettify("<ul>\n<li>one</li>\n</ul>\n<pre><code> two\n</code></pre>"));

      Parser.DoubleParse(" -    one\n\n     two").Is(Parser.Prettify("<ul>\n<li>one</li>\n</ul>\n<pre><code> two\n</code></pre>"));
    }

    [Fact]
    public void ContainerBlocksListItems_Example228()
    {
      // Example 228
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //    -    one
      //  
      //         two
      //
      // Should be rendered as:
      //   <ul>
      //   <li>
      //   <p>one</p>
      //   <p>two</p>
      //   </li>
      //   </ul>

      Parser.Parse(" -    one\n\n      two").Is(Parser.Prettify("<ul>\n<li>\n<p>one</p>\n<p>two</p>\n</li>\n</ul>"));

      Parser.DoubleParse(" -    one\n\n      two").Is(Parser.Prettify("<ul>\n<li>\n<p>one</p>\n<p>two</p>\n</li>\n</ul>"));
    }

    // It is tempting to think of this in terms of columns:  the continuation
    // blocks must be indented at least to the column of the first
    // [non-whitespace character] after the list marker. However, that is not quite right.
    // The spaces after the list marker determine how much relative indentation
    // is needed.  Which column this indentation reaches will depend on
    // how the list item is embedded in other constructions, as shown by
    // this example:
    [Fact]
    public void ContainerBlocksListItems_Example229()
    {
      // Example 229
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //      > > 1.  one
      //   >>
      //   >>     two
      //
      // Should be rendered as:
      //   <blockquote>
      //   <blockquote>
      //   <ol>
      //   <li>
      //   <p>one</p>
      //   <p>two</p>
      //   </li>
      //   </ol>
      //   </blockquote>
      //   </blockquote>

      Parser.Parse("   > > 1.  one\n>>\n>>     two").Is(Parser.Prettify("<blockquote>\n<blockquote>\n<ol>\n<li>\n<p>one</p>\n<p>two</p>\n</li>\n</ol>\n</blockquote>\n</blockquote>"));

      Parser.DoubleParse("   > > 1.  one\n>>\n>>     two").Is(Parser.Prettify("<blockquote>\n<blockquote>\n<ol>\n<li>\n<p>one</p>\n<p>two</p>\n</li>\n</ol>\n</blockquote>\n</blockquote>"));
    }

    // Here `two` occurs in the same column as the list marker `1.`,
    // but is actually contained in the list item, because there is
    // sufficient indentation after the last containing blockquote marker.
    //
    // The converse is also possible.  In the following example, the word `two`
    // occurs far to the right of the initial text of the list item, `one`, but
    // it is not considered part of the list item, because it is not indented
    // far enough past the blockquote marker:
    [Fact]
    public void ContainerBlocksListItems_Example230()
    {
      // Example 230
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //   >>- one
      //   >>
      //     >  > two
      //
      // Should be rendered as:
      //   <blockquote>
      //   <blockquote>
      //   <ul>
      //   <li>one</li>
      //   </ul>
      //   <p>two</p>
      //   </blockquote>
      //   </blockquote>

      Parser.Parse(">>- one\n>>\n  >  > two").Is(Parser.Prettify("<blockquote>\n<blockquote>\n<ul>\n<li>one</li>\n</ul>\n<p>two</p>\n</blockquote>\n</blockquote>"));

      Parser.DoubleParse(">>- one\n>>\n  >  > two").Is(Parser.Prettify("<blockquote>\n<blockquote>\n<ul>\n<li>one</li>\n</ul>\n<p>two</p>\n</blockquote>\n</blockquote>"));
    }

    // Note that at least one space is needed between the list marker and
    // any following content, so these are not list items:
    [Fact]
    public void ContainerBlocksListItems_Example231()
    {
      // Example 231
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //   -one
      //  
      //   2.two
      //
      // Should be rendered as:
      //   <p>-one</p>
      //   <p>2.two</p>

      Parser.Parse("-one\n\n2.two").Is(Parser.Prettify("<p>-one</p>\n<p>2.two</p>"));

      Parser.DoubleParse("-one\n\n2.two").Is(Parser.Prettify("<p>-one</p>\n<p>2.two</p>"));
    }

    // A list item may contain blocks that are separated by more than
    // one blank line.
    [Fact]
    public void ContainerBlocksListItems_Example232()
    {
      // Example 232
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //   - foo
      //  
      //  
      //     bar
      //
      // Should be rendered as:
      //   <ul>
      //   <li>
      //   <p>foo</p>
      //   <p>bar</p>
      //   </li>
      //   </ul>

      Parser.Parse("- foo\n\n\n  bar").Is(Parser.Prettify("<ul>\n<li>\n<p>foo</p>\n<p>bar</p>\n</li>\n</ul>"));

      Parser.DoubleParse("- foo\n\n\n  bar").Is(Parser.Prettify("<ul>\n<li>\n<p>foo</p>\n<p>bar</p>\n</li>\n</ul>"));
    }

    // A list item may contain any kind of block:
    [Fact]
    public void ContainerBlocksListItems_Example233()
    {
      // Example 233
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //   1.  foo
      //  
      //       ```
      //       bar
      //       ```
      //  
      //       baz
      //  
      //       > bam
      //
      // Should be rendered as:
      //   <ol>
      //   <li>
      //   <p>foo</p>
      //   <pre><code>bar
      //   </code></pre>
      //   <p>baz</p>
      //   <blockquote>
      //   <p>bam</p>
      //   </blockquote>
      //   </li>
      //   </ol>

      Parser.Parse("1.  foo\n\n    ```\n    bar\n    ```\n\n    baz\n\n    > bam").Is(Parser.Prettify("<ol>\n<li>\n<p>foo</p>\n<pre><code>bar\n</code></pre>\n<p>baz</p>\n<blockquote>\n<p>bam</p>\n</blockquote>\n</li>\n</ol>"));

      Parser.DoubleParse("1.  foo\n\n    ```\n    bar\n    ```\n\n    baz\n\n    > bam").Is(Parser.Prettify("<ol>\n<li>\n<p>foo</p>\n<pre><code>bar\n</code></pre>\n<p>baz</p>\n<blockquote>\n<p>bam</p>\n</blockquote>\n</li>\n</ol>"));
    }

    // A list item that contains an indented code block will preserve
    // empty lines within the code block verbatim.
    [Fact]
    public void ContainerBlocksListItems_Example234()
    {
      // Example 234
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //   - Foo
      //  
      //         bar
      //  
      //  
      //         baz
      //
      // Should be rendered as:
      //   <ul>
      //   <li>
      //   <p>Foo</p>
      //   <pre><code>bar
      //  
      //  
      //   baz
      //   </code></pre>
      //   </li>
      //   </ul>

      Parser.Parse("- Foo\n\n      bar\n\n\n      baz").Is(Parser.Prettify("<ul>\n<li>\n<p>Foo</p>\n<pre><code>bar\n\n\nbaz\n</code></pre>\n</li>\n</ul>"));

      Parser.DoubleParse("- Foo\n\n      bar\n\n\n      baz").Is(Parser.Prettify("<ul>\n<li>\n<p>Foo</p>\n<pre><code>bar\n\n\nbaz\n</code></pre>\n</li>\n</ul>"));
    }

    // Note that ordered list start numbers must be nine digits or less:
    [Fact]
    public void ContainerBlocksListItems_Example235()
    {
      // Example 235
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //   123456789. ok
      //
      // Should be rendered as:
      //   <ol start="123456789">
      //   <li>ok</li>
      //   </ol>

      Parser.Parse("123456789. ok").Is(Parser.Prettify("<ol start=\"123456789\">\n<li>ok</li>\n</ol>"));

      Parser.DoubleParse("123456789. ok").Is(Parser.Prettify("<ol start=\"123456789\">\n<li>ok</li>\n</ol>"));
    }

    [Fact]
    public void ContainerBlocksListItems_Example236()
    {
      // Example 236
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //   1234567890. not ok
      //
      // Should be rendered as:
      //   <p>1234567890. not ok</p>

      Parser.Parse("1234567890. not ok").Is(Parser.Prettify("<p>1234567890. not ok</p>"));

      Parser.DoubleParse("1234567890. not ok").Is(Parser.Prettify("<p>1234567890. not ok</p>"));
    }

    // A start number may begin with 0s:
    [Fact]
    public void ContainerBlocksListItems_Example237()
    {
      // Example 237
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //   0. ok
      //
      // Should be rendered as:
      //   <ol start="0">
      //   <li>ok</li>
      //   </ol>

      Parser.Parse("0. ok").Is(Parser.Prettify("<ol start=\"0\">\n<li>ok</li>\n</ol>"));

      Parser.DoubleParse("0. ok").Is(Parser.Prettify("<ol start=\"0\">\n<li>ok</li>\n</ol>"));
    }

    [Fact]
    public void ContainerBlocksListItems_Example238()
    {
      // Example 238
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //   003. ok
      //
      // Should be rendered as:
      //   <ol start="3">
      //   <li>ok</li>
      //   </ol>

      Parser.Parse("003. ok").Is(Parser.Prettify("<ol start=\"3\">\n<li>ok</li>\n</ol>"));

      Parser.DoubleParse("003. ok").Is(Parser.Prettify("<ol start=\"3\">\n<li>ok</li>\n</ol>"));
    }

    // A start number may not be negative:
    [Fact]
    public void ContainerBlocksListItems_Example239()
    {
      // Example 239
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //   -1. not ok
      //
      // Should be rendered as:
      //   <p>-1. not ok</p>

      Parser.Parse("-1. not ok").Is(Parser.Prettify("<p>-1. not ok</p>"));

      Parser.DoubleParse("-1. not ok").Is(Parser.Prettify("<p>-1. not ok</p>"));
    }

    // 2.  **Item starting with indented code.**  If a sequence of lines *Ls*
    //     constitute a sequence of blocks *Bs* starting with an indented code
    //     block, and *M* is a list marker of width *W* followed by
    //     one space, then the result of prepending *M* and the following
    //     space to the first line of *Ls*, and indenting subsequent lines of
    //     *Ls* by *W + 1* spaces, is a list item with *Bs* as its contents.
    //     If a line is empty, then it need not be indented.  The type of the
    //     list item (bullet or ordered) is determined by the type of its list
    //     marker.  If the list item is ordered, then it is also assigned a
    //     start number, based on the ordered list marker.
    //
    // An indented code block will have to be indented four spaces beyond
    // the edge of the region where text will be included in the list item.
    // In the following case that is 6 spaces:
    [Fact]
    public void ContainerBlocksListItems_Example240()
    {
      // Example 240
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //   - foo
      //  
      //         bar
      //
      // Should be rendered as:
      //   <ul>
      //   <li>
      //   <p>foo</p>
      //   <pre><code>bar
      //   </code></pre>
      //   </li>
      //   </ul>

      Parser.Parse("- foo\n\n      bar").Is(Parser.Prettify("<ul>\n<li>\n<p>foo</p>\n<pre><code>bar\n</code></pre>\n</li>\n</ul>"));

      Parser.DoubleParse("- foo\n\n      bar").Is(Parser.Prettify("<ul>\n<li>\n<p>foo</p>\n<pre><code>bar\n</code></pre>\n</li>\n</ul>"));
    }

    // And in this case it is 11 spaces:
    [Fact]
    public void ContainerBlocksListItems_Example241()
    {
      // Example 241
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //     10.  foo
      //  
      //              bar
      //
      // Should be rendered as:
      //   <ol start="10">
      //   <li>
      //   <p>foo</p>
      //   <pre><code>bar
      //   </code></pre>
      //   </li>
      //   </ol>

      Parser.Parse("  10.  foo\n\n           bar").Is(Parser.Prettify("<ol start=\"10\">\n<li>\n<p>foo</p>\n<pre><code>bar\n</code></pre>\n</li>\n</ol>"));

      Parser.DoubleParse("  10.  foo\n\n           bar").Is(Parser.Prettify("<ol start=\"10\">\n<li>\n<p>foo</p>\n<pre><code>bar\n</code></pre>\n</li>\n</ol>"));
    }

    // If the *first* block in the list item is an indented code block,
    // then by rule #2, the contents must be indented *one* space after the
    // list marker:
    [Fact]
    public void ContainerBlocksListItems_Example242()
    {
      // Example 242
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //       indented code
      //  
      //   paragraph
      //  
      //       more code
      //
      // Should be rendered as:
      //   <pre><code>indented code
      //   </code></pre>
      //   <p>paragraph</p>
      //   <pre><code>more code
      //   </code></pre>

      Parser.Parse("    indented code\n\nparagraph\n\n    more code").Is(Parser.Prettify("<pre><code>indented code\n</code></pre>\n<p>paragraph</p>\n<pre><code>more code\n</code></pre>"));

      Parser.DoubleParse("    indented code\n\nparagraph\n\n    more code").Is(Parser.Prettify("<pre><code>indented code\n</code></pre>\n<p>paragraph</p>\n<pre><code>more code\n</code></pre>"));
    }

    [Fact]
    public void ContainerBlocksListItems_Example243()
    {
      // Example 243
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //   1.     indented code
      //  
      //      paragraph
      //  
      //          more code
      //
      // Should be rendered as:
      //   <ol>
      //   <li>
      //   <pre><code>indented code
      //   </code></pre>
      //   <p>paragraph</p>
      //   <pre><code>more code
      //   </code></pre>
      //   </li>
      //   </ol>

      Parser.Parse("1.     indented code\n\n   paragraph\n\n       more code").Is(Parser.Prettify("<ol>\n<li>\n<pre><code>indented code\n</code></pre>\n<p>paragraph</p>\n<pre><code>more code\n</code></pre>\n</li>\n</ol>"));

      Parser.DoubleParse("1.     indented code\n\n   paragraph\n\n       more code").Is(Parser.Prettify("<ol>\n<li>\n<pre><code>indented code\n</code></pre>\n<p>paragraph</p>\n<pre><code>more code\n</code></pre>\n</li>\n</ol>"));
    }

    // Note that an additional space indent is interpreted as space
    // inside the code block:
    [Fact]
    public void ContainerBlocksListItems_Example244()
    {
      // Example 244
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //   1.      indented code
      //  
      //      paragraph
      //  
      //          more code
      //
      // Should be rendered as:
      //   <ol>
      //   <li>
      //   <pre><code> indented code
      //   </code></pre>
      //   <p>paragraph</p>
      //   <pre><code>more code
      //   </code></pre>
      //   </li>
      //   </ol>

      Parser.Parse("1.      indented code\n\n   paragraph\n\n       more code").Is(Parser.Prettify("<ol>\n<li>\n<pre><code> indented code\n</code></pre>\n<p>paragraph</p>\n<pre><code>more code\n</code></pre>\n</li>\n</ol>"));

      Parser.DoubleParse("1.      indented code\n\n   paragraph\n\n       more code").Is(Parser.Prettify("<ol>\n<li>\n<pre><code> indented code\n</code></pre>\n<p>paragraph</p>\n<pre><code>more code\n</code></pre>\n</li>\n</ol>"));
    }

    // Note that rules #1 and #2 only apply to two cases:  (a) cases
    // in which the lines to be included in a list item begin with a
    // [non-whitespace character], and (b) cases in which
    // they begin with an indented code
    // block.  In a case like the following, where the first block begins with
    // a three-space indent, the rules do not allow us to form a list item by
    // indenting the whole thing and prepending a list marker:
    [Fact]
    public void ContainerBlocksListItems_Example245()
    {
      // Example 245
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //      foo
      //  
      //   bar
      //
      // Should be rendered as:
      //   <p>foo</p>
      //   <p>bar</p>

      Parser.Parse("   foo\n\nbar").Is(Parser.Prettify("<p>foo</p>\n<p>bar</p>"));

      Parser.DoubleParse("   foo\n\nbar").Is(Parser.Prettify("<p>foo</p>\n<p>bar</p>"));
    }

    [Fact]
    public void ContainerBlocksListItems_Example246()
    {
      // Example 246
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //   -    foo
      //  
      //     bar
      //
      // Should be rendered as:
      //   <ul>
      //   <li>foo</li>
      //   </ul>
      //   <p>bar</p>

      Parser.Parse("-    foo\n\n  bar").Is(Parser.Prettify("<ul>\n<li>foo</li>\n</ul>\n<p>bar</p>"));

      Parser.DoubleParse("-    foo\n\n  bar").Is(Parser.Prettify("<ul>\n<li>foo</li>\n</ul>\n<p>bar</p>"));
    }

    // This is not a significant restriction, because when a block begins
    // with 1-3 spaces indent, the indentation can always be removed without
    // a change in interpretation, allowing rule #1 to be applied.  So, in
    // the above case:
    [Fact]
    public void ContainerBlocksListItems_Example247()
    {
      // Example 247
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //   -  foo
      //  
      //      bar
      //
      // Should be rendered as:
      //   <ul>
      //   <li>
      //   <p>foo</p>
      //   <p>bar</p>
      //   </li>
      //   </ul>

      Parser.Parse("-  foo\n\n   bar").Is(Parser.Prettify("<ul>\n<li>\n<p>foo</p>\n<p>bar</p>\n</li>\n</ul>"));

      Parser.DoubleParse("-  foo\n\n   bar").Is(Parser.Prettify("<ul>\n<li>\n<p>foo</p>\n<p>bar</p>\n</li>\n</ul>"));
    }

    // 3.  **Item starting with a blank line.**  If a sequence of lines *Ls*
    //     starting with a single [blank line] constitute a (possibly empty)
    //     sequence of blocks *Bs*, not separated from each other by more than
    //     one blank line, and *M* is a list marker of width *W*,
    //     then the result of prepending *M* to the first line of *Ls*, and
    //     indenting subsequent lines of *Ls* by *W + 1* spaces, is a list
    //     item with *Bs* as its contents.
    //     If a line is empty, then it need not be indented.  The type of the
    //     list item (bullet or ordered) is determined by the type of its list
    //     marker.  If the list item is ordered, then it is also assigned a
    //     start number, based on the ordered list marker.
    //
    // Here are some list items that start with a blank line but are not empty:
    [Fact]
    public void ContainerBlocksListItems_Example248()
    {
      // Example 248
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //   -
      //     foo
      //   -
      //     ```
      //     bar
      //     ```
      //   -
      //         baz
      //
      // Should be rendered as:
      //   <ul>
      //   <li>foo</li>
      //   <li>
      //   <pre><code>bar
      //   </code></pre>
      //   </li>
      //   <li>
      //   <pre><code>baz
      //   </code></pre>
      //   </li>
      //   </ul>

      Parser.Parse("-\n  foo\n-\n  ```\n  bar\n  ```\n-\n      baz").Is(Parser.Prettify("<ul>\n<li>foo</li>\n<li>\n<pre><code>bar\n</code></pre>\n</li>\n<li>\n<pre><code>baz\n</code></pre>\n</li>\n</ul>"));

      Parser.DoubleParse("-\n  foo\n-\n  ```\n  bar\n  ```\n-\n      baz").Is(Parser.Prettify("<ul>\n<li>foo</li>\n<li>\n<pre><code>bar\n</code></pre>\n</li>\n<li>\n<pre><code>baz\n</code></pre>\n</li>\n</ul>"));
    }

    // When the list item starts with a blank line, the number of spaces
    // following the list marker doesn't change the required indentation:
    [Fact]
    public void ContainerBlocksListItems_Example249()
    {
      // Example 249
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //   -   
      //     foo
      //
      // Should be rendered as:
      //   <ul>
      //   <li>foo</li>
      //   </ul>

      Parser.Parse("-   \n  foo").Is(Parser.Prettify("<ul>\n<li>foo</li>\n</ul>"));

      Parser.DoubleParse("-   \n  foo").Is(Parser.Prettify("<ul>\n<li>foo</li>\n</ul>"));
    }

    // A list item can begin with at most one blank line.
    // In the following example, `foo` is not part of the list
    // item:
    [Fact]
    public void ContainerBlocksListItems_Example250()
    {
      // Example 250
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //   -
      //  
      //     foo
      //
      // Should be rendered as:
      //   <ul>
      //   <li></li>
      //   </ul>
      //   <p>foo</p>

      Parser.Parse("-\n\n  foo").Is(Parser.Prettify("<ul>\n<li></li>\n</ul>\n<p>foo</p>"));

      Parser.DoubleParse("-\n\n  foo").Is(Parser.Prettify("<ul>\n<li></li>\n</ul>\n<p>foo</p>"));
    }

    // Here is an empty bullet list item:
    [Fact]
    public void ContainerBlocksListItems_Example251()
    {
      // Example 251
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //   - foo
      //   -
      //   - bar
      //
      // Should be rendered as:
      //   <ul>
      //   <li>foo</li>
      //   <li></li>
      //   <li>bar</li>
      //   </ul>

      Parser.Parse("- foo\n-\n- bar").Is(Parser.Prettify("<ul>\n<li>foo</li>\n<li></li>\n<li>bar</li>\n</ul>"));

      Parser.DoubleParse("- foo\n-\n- bar").Is(Parser.Prettify("<ul>\n<li>foo</li>\n<li></li>\n<li>bar</li>\n</ul>"));
    }

    // It does not matter whether there are spaces following the [list marker]:
    [Fact]
    public void ContainerBlocksListItems_Example252()
    {
      // Example 252
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //   - foo
      //   -   
      //   - bar
      //
      // Should be rendered as:
      //   <ul>
      //   <li>foo</li>
      //   <li></li>
      //   <li>bar</li>
      //   </ul>

      Parser.Parse("- foo\n-   \n- bar").Is(Parser.Prettify("<ul>\n<li>foo</li>\n<li></li>\n<li>bar</li>\n</ul>"));

      Parser.DoubleParse("- foo\n-   \n- bar").Is(Parser.Prettify("<ul>\n<li>foo</li>\n<li></li>\n<li>bar</li>\n</ul>"));
    }

    // Here is an empty ordered list item:
    [Fact]
    public void ContainerBlocksListItems_Example253()
    {
      // Example 253
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //   1. foo
      //   2.
      //   3. bar
      //
      // Should be rendered as:
      //   <ol>
      //   <li>foo</li>
      //   <li></li>
      //   <li>bar</li>
      //   </ol>

      Parser.Parse("1. foo\n2.\n3. bar").Is(Parser.Prettify("<ol>\n<li>foo</li>\n<li></li>\n<li>bar</li>\n</ol>"));

      Parser.DoubleParse("1. foo\n2.\n3. bar").Is(Parser.Prettify("<ol>\n<li>foo</li>\n<li></li>\n<li>bar</li>\n</ol>"));
    }

    // A list may start or end with an empty list item:
    [Fact]
    public void ContainerBlocksListItems_Example254()
    {
      // Example 254
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //   *
      //
      // Should be rendered as:
      //   <ul>
      //   <li></li>
      //   </ul>

      Parser.Parse("*").Is(Parser.Prettify("<ul>\n<li></li>\n</ul>"));

      Parser.DoubleParse("*").Is(Parser.Prettify("<ul>\n<li></li>\n</ul>"));
    }

    // However, an empty list item cannot interrupt a paragraph:
    [Fact]
    public void ContainerBlocksListItems_Example255()
    {
      // Example 255
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //   foo
      //   *
      //  
      //   foo
      //   1.
      //
      // Should be rendered as:
      //   <p>foo
      //   *</p>
      //   <p>foo
      //   1.</p>

      Parser.Parse("foo\n*\n\nfoo\n1.").Is(Parser.Prettify("<p>foo\n*</p>\n<p>foo\n1.</p>"));

      Parser.DoubleParse("foo\n*\n\nfoo\n1.").Is(Parser.Prettify("<p>foo\n*</p>\n<p>foo\n1.</p>"));
    }

    // 4.  **Indentation.**  If a sequence of lines *Ls* constitutes a list item
    //     according to rule #1, #2, or #3, then the result of indenting each line
    //     of *Ls* by 1-3 spaces (the same for each line) also constitutes a
    //     list item with the same contents and attributes.  If a line is
    //     empty, then it need not be indented.
    //
    // Indented one space:
    [Fact]
    public void ContainerBlocksListItems_Example256()
    {
      // Example 256
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //    1.  A paragraph
      //        with two lines.
      //  
      //            indented code
      //  
      //        > A block quote.
      //
      // Should be rendered as:
      //   <ol>
      //   <li>
      //   <p>A paragraph
      //   with two lines.</p>
      //   <pre><code>indented code
      //   </code></pre>
      //   <blockquote>
      //   <p>A block quote.</p>
      //   </blockquote>
      //   </li>
      //   </ol>

      Parser.Parse(" 1.  A paragraph\n     with two lines.\n\n         indented code\n\n     > A block quote.").Is(Parser.Prettify("<ol>\n<li>\n<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>\n</li>\n</ol>"));

      Parser.DoubleParse(" 1.  A paragraph\n     with two lines.\n\n         indented code\n\n     > A block quote.").Is(Parser.Prettify("<ol>\n<li>\n<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>\n</li>\n</ol>"));
    }

    // Indented two spaces:
    [Fact]
    public void ContainerBlocksListItems_Example257()
    {
      // Example 257
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //     1.  A paragraph
      //         with two lines.
      //  
      //             indented code
      //  
      //         > A block quote.
      //
      // Should be rendered as:
      //   <ol>
      //   <li>
      //   <p>A paragraph
      //   with two lines.</p>
      //   <pre><code>indented code
      //   </code></pre>
      //   <blockquote>
      //   <p>A block quote.</p>
      //   </blockquote>
      //   </li>
      //   </ol>

      Parser.Parse("  1.  A paragraph\n      with two lines.\n\n          indented code\n\n      > A block quote.").Is(Parser.Prettify("<ol>\n<li>\n<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>\n</li>\n</ol>"));

      Parser.DoubleParse("  1.  A paragraph\n      with two lines.\n\n          indented code\n\n      > A block quote.").Is(Parser.Prettify("<ol>\n<li>\n<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>\n</li>\n</ol>"));
    }

    // Indented three spaces:
    [Fact]
    public void ContainerBlocksListItems_Example258()
    {
      // Example 258
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //      1.  A paragraph
      //          with two lines.
      //  
      //              indented code
      //  
      //          > A block quote.
      //
      // Should be rendered as:
      //   <ol>
      //   <li>
      //   <p>A paragraph
      //   with two lines.</p>
      //   <pre><code>indented code
      //   </code></pre>
      //   <blockquote>
      //   <p>A block quote.</p>
      //   </blockquote>
      //   </li>
      //   </ol>

      Parser.Parse("   1.  A paragraph\n       with two lines.\n\n           indented code\n\n       > A block quote.").Is(Parser.Prettify("<ol>\n<li>\n<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>\n</li>\n</ol>"));

      Parser.DoubleParse("   1.  A paragraph\n       with two lines.\n\n           indented code\n\n       > A block quote.").Is(Parser.Prettify("<ol>\n<li>\n<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>\n</li>\n</ol>"));
    }

    // Four spaces indent gives a code block:
    [Fact]
    public void ContainerBlocksListItems_Example259()
    {
      // Example 259
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //       1.  A paragraph
      //           with two lines.
      //  
      //               indented code
      //  
      //           > A block quote.
      //
      // Should be rendered as:
      //   <pre><code>1.  A paragraph
      //       with two lines.
      //  
      //           indented code
      //  
      //       &gt; A block quote.
      //   </code></pre>

      Parser.Parse("    1.  A paragraph\n        with two lines.\n\n            indented code\n\n        > A block quote.").Is(Parser.Prettify("<pre><code>1.  A paragraph\n    with two lines.\n\n        indented code\n\n    &gt; A block quote.\n</code></pre>"));

      Parser.DoubleParse("    1.  A paragraph\n        with two lines.\n\n            indented code\n\n        > A block quote.").Is(Parser.Prettify("<pre><code>1.  A paragraph\n    with two lines.\n\n        indented code\n\n    &gt; A block quote.\n</code></pre>"));
    }

    // 5.  **Laziness.**  If a string of lines *Ls* constitute a [list
    //     item](#list-items) with contents *Bs*, then the result of deleting
    //     some or all of the indentation from one or more lines in which the
    //     next [non-whitespace character] after the indentation is
    //     [paragraph continuation text] is a
    //     list item with the same contents and attributes.  The unindented
    //     lines are called
    //     [lazy continuation line](@)s.
    //
    // Here is an example with [lazy continuation lines]:
    [Fact]
    public void ContainerBlocksListItems_Example260()
    {
      // Example 260
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //     1.  A paragraph
      //   with two lines.
      //  
      //             indented code
      //  
      //         > A block quote.
      //
      // Should be rendered as:
      //   <ol>
      //   <li>
      //   <p>A paragraph
      //   with two lines.</p>
      //   <pre><code>indented code
      //   </code></pre>
      //   <blockquote>
      //   <p>A block quote.</p>
      //   </blockquote>
      //   </li>
      //   </ol>

      Parser.Parse("  1.  A paragraph\nwith two lines.\n\n          indented code\n\n      > A block quote.").Is(Parser.Prettify("<ol>\n<li>\n<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>\n</li>\n</ol>"));

      Parser.DoubleParse("  1.  A paragraph\nwith two lines.\n\n          indented code\n\n      > A block quote.").Is(Parser.Prettify("<ol>\n<li>\n<p>A paragraph\nwith two lines.</p>\n<pre><code>indented code\n</code></pre>\n<blockquote>\n<p>A block quote.</p>\n</blockquote>\n</li>\n</ol>"));
    }

    // Indentation can be partially deleted:
    [Fact]
    public void ContainerBlocksListItems_Example261()
    {
      // Example 261
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //     1.  A paragraph
      //       with two lines.
      //
      // Should be rendered as:
      //   <ol>
      //   <li>A paragraph
      //   with two lines.</li>
      //   </ol>

      Parser.Parse("  1.  A paragraph\n    with two lines.").Is(Parser.Prettify("<ol>\n<li>A paragraph\nwith two lines.</li>\n</ol>"));

      Parser.DoubleParse("  1.  A paragraph\n    with two lines.").Is(Parser.Prettify("<ol>\n<li>A paragraph\nwith two lines.</li>\n</ol>"));
    }

    // These examples show how laziness can work in nested structures:
    [Fact]
    public void ContainerBlocksListItems_Example262()
    {
      // Example 262
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //   > 1. > Blockquote
      //   continued here.
      //
      // Should be rendered as:
      //   <blockquote>
      //   <ol>
      //   <li>
      //   <blockquote>
      //   <p>Blockquote
      //   continued here.</p>
      //   </blockquote>
      //   </li>
      //   </ol>
      //   </blockquote>

      Parser.Parse("> 1. > Blockquote\ncontinued here.").Is(Parser.Prettify("<blockquote>\n<ol>\n<li>\n<blockquote>\n<p>Blockquote\ncontinued here.</p>\n</blockquote>\n</li>\n</ol>\n</blockquote>"));

      Parser.DoubleParse("> 1. > Blockquote\ncontinued here.").Is(Parser.Prettify("<blockquote>\n<ol>\n<li>\n<blockquote>\n<p>Blockquote\ncontinued here.</p>\n</blockquote>\n</li>\n</ol>\n</blockquote>"));
    }

    [Fact]
    public void ContainerBlocksListItems_Example263()
    {
      // Example 263
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //   > 1. > Blockquote
      //   > continued here.
      //
      // Should be rendered as:
      //   <blockquote>
      //   <ol>
      //   <li>
      //   <blockquote>
      //   <p>Blockquote
      //   continued here.</p>
      //   </blockquote>
      //   </li>
      //   </ol>
      //   </blockquote>

      Parser.Parse("> 1. > Blockquote\n> continued here.").Is(Parser.Prettify("<blockquote>\n<ol>\n<li>\n<blockquote>\n<p>Blockquote\ncontinued here.</p>\n</blockquote>\n</li>\n</ol>\n</blockquote>"));

      Parser.DoubleParse("> 1. > Blockquote\n> continued here.").Is(Parser.Prettify("<blockquote>\n<ol>\n<li>\n<blockquote>\n<p>Blockquote\ncontinued here.</p>\n</blockquote>\n</li>\n</ol>\n</blockquote>"));
    }

    // 6.  **That's all.** Nothing that is not counted as a list item by rules
    //     #1--5 counts as a [list item](#list-items).
    //
    // The rules for sublists follow from the general rules
    // [above][List items].  A sublist must be indented the same number
    // of spaces a paragraph would need to be in order to be included
    // in the list item.
    //
    // So, in this case we need two spaces indent:
    [Fact]
    public void ContainerBlocksListItems_Example264()
    {
      // Example 264
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //   - foo
      //     - bar
      //       - baz
      //         - boo
      //
      // Should be rendered as:
      //   <ul>
      //   <li>foo
      //   <ul>
      //   <li>bar
      //   <ul>
      //   <li>baz
      //   <ul>
      //   <li>boo</li>
      //   </ul>
      //   </li>
      //   </ul>
      //   </li>
      //   </ul>
      //   </li>
      //   </ul>

      Parser.Parse("- foo\n  - bar\n    - baz\n      - boo").Is(Parser.Prettify("<ul>\n<li>foo\n<ul>\n<li>bar\n<ul>\n<li>baz\n<ul>\n<li>boo</li>\n</ul>\n</li>\n</ul>\n</li>\n</ul>\n</li>\n</ul>"));

      Parser.DoubleParse("- foo\n  - bar\n    - baz\n      - boo").Is(Parser.Prettify("<ul>\n<li>foo\n<ul>\n<li>bar\n<ul>\n<li>baz\n<ul>\n<li>boo</li>\n</ul>\n</li>\n</ul>\n</li>\n</ul>\n</li>\n</ul>"));
    }

    // One is not enough:
    [Fact]
    public void ContainerBlocksListItems_Example265()
    {
      // Example 265
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //   - foo
      //    - bar
      //     - baz
      //      - boo
      //
      // Should be rendered as:
      //   <ul>
      //   <li>foo</li>
      //   <li>bar</li>
      //   <li>baz</li>
      //   <li>boo</li>
      //   </ul>

      Parser.Parse("- foo\n - bar\n  - baz\n   - boo").Is(Parser.Prettify("<ul>\n<li>foo</li>\n<li>bar</li>\n<li>baz</li>\n<li>boo</li>\n</ul>"));

      Parser.DoubleParse("- foo\n - bar\n  - baz\n   - boo").Is(Parser.Prettify("<ul>\n<li>foo</li>\n<li>bar</li>\n<li>baz</li>\n<li>boo</li>\n</ul>"));
    }

    // Here we need four, because the list marker is wider:
    [Fact]
    public void ContainerBlocksListItems_Example266()
    {
      // Example 266
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //   10) foo
      //       - bar
      //
      // Should be rendered as:
      //   <ol start="10">
      //   <li>foo
      //   <ul>
      //   <li>bar</li>
      //   </ul>
      //   </li>
      //   </ol>

      Parser.Parse("10) foo\n    - bar").Is(Parser.Prettify("<ol start=\"10\">\n<li>foo\n<ul>\n<li>bar</li>\n</ul>\n</li>\n</ol>"));

      Parser.DoubleParse("10) foo\n    - bar").Is(Parser.Prettify("<ol start=\"10\">\n<li>foo\n<ul>\n<li>bar</li>\n</ul>\n</li>\n</ol>"));
    }

    // Three is not enough:
    [Fact]
    public void ContainerBlocksListItems_Example267()
    {
      // Example 267
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //   10) foo
      //      - bar
      //
      // Should be rendered as:
      //   <ol start="10">
      //   <li>foo</li>
      //   </ol>
      //   <ul>
      //   <li>bar</li>
      //   </ul>

      Parser.Parse("10) foo\n   - bar").Is(Parser.Prettify("<ol start=\"10\">\n<li>foo</li>\n</ol>\n<ul>\n<li>bar</li>\n</ul>"));

      Parser.DoubleParse("10) foo\n   - bar").Is(Parser.Prettify("<ol start=\"10\">\n<li>foo</li>\n</ol>\n<ul>\n<li>bar</li>\n</ul>"));
    }

    // A list may be the first block in a list item:
    [Fact]
    public void ContainerBlocksListItems_Example268()
    {
      // Example 268
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //   - - foo
      //
      // Should be rendered as:
      //   <ul>
      //   <li>
      //   <ul>
      //   <li>foo</li>
      //   </ul>
      //   </li>
      //   </ul>

      Parser.Parse("- - foo").Is(Parser.Prettify("<ul>\n<li>\n<ul>\n<li>foo</li>\n</ul>\n</li>\n</ul>"));

      Parser.DoubleParse("- - foo").Is(Parser.Prettify("<ul>\n<li>\n<ul>\n<li>foo</li>\n</ul>\n</li>\n</ul>"));
    }

    [Fact]
    public void ContainerBlocksListItems_Example269()
    {
      // Example 269
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //   1. - 2. foo
      //
      // Should be rendered as:
      //   <ol>
      //   <li>
      //   <ul>
      //   <li>
      //   <ol start="2">
      //   <li>foo</li>
      //   </ol>
      //   </li>
      //   </ul>
      //   </li>
      //   </ol>

      Parser.Parse("1. - 2. foo").Is(Parser.Prettify("<ol>\n<li>\n<ul>\n<li>\n<ol start=\"2\">\n<li>foo</li>\n</ol>\n</li>\n</ul>\n</li>\n</ol>"));

      Parser.DoubleParse("1. - 2. foo").Is(Parser.Prettify("<ol>\n<li>\n<ul>\n<li>\n<ol start=\"2\">\n<li>foo</li>\n</ol>\n</li>\n</ul>\n</li>\n</ol>"));
    }

    // A list item can contain a heading:
    [Fact]
    public void ContainerBlocksListItems_Example270()
    {
      // Example 270
      // Section: Container blocks / List items
      //
      // The following Markdown:
      //   - # Foo
      //   - Bar
      //     ---
      //     baz
      //
      // Should be rendered as:
      //   <ul>
      //   <li>
      //   <h1>Foo</h1>
      //   </li>
      //   <li>
      //   <h2>Bar</h2>
      //   baz</li>
      //   </ul>

      Parser.Parse("- # Foo\n- Bar\n  ---\n  baz").Is(Parser.Prettify("<ul>\n<li>\n<h1>Foo</h1>\n</li>\n<li>\n<h2>Bar</h2>\nbaz</li>\n</ul>"));

      Parser.DoubleParse("- # Foo\n- Bar\n  ---\n  baz").Is(Parser.Prettify("<ul>\n<li>\n<h1>Foo</h1>\n</li>\n<li>\n<h2>Bar</h2>\nbaz</li>\n</ul>"));
    }
  }

  public class TestContainerBlocksLists
  {
    // ### Motivation
    //
    // John Gruber's Markdown spec says the following about list items:
    //
    // 1. "List markers typically start at the left margin, but may be indented
    //    by up to three spaces. List markers must be followed by one or more
    //    spaces or a tab."
    //
    // 2. "To make lists look nice, you can wrap items with hanging indents....
    //    But if you don't want to, you don't have to."
    //
    // 3. "List items may consist of multiple paragraphs. Each subsequent
    //    paragraph in a list item must be indented by either 4 spaces or one
    //    tab."
    //
    // 4. "It looks nice if you indent every line of the subsequent paragraphs,
    //    but here again, Markdown will allow you to be lazy."
    //
    // 5. "To put a blockquote within a list item, the blockquote's `>`
    //    delimiters need to be indented."
    //
    // 6. "To put a code block within a list item, the code block needs to be
    //    indented twice — 8 spaces or two tabs."
    //
    // These rules specify that a paragraph under a list item must be indented
    // four spaces (presumably, from the left margin, rather than the start of
    // the list marker, but this is not said), and that code under a list item
    // must be indented eight spaces instead of the usual four.  They also say
    // that a block quote must be indented, but not by how much; however, the
    // example given has four spaces indentation.  Although nothing is said
    // about other kinds of block-level content, it is certainly reasonable to
    // infer that *all* block elements under a list item, including other
    // lists, must be indented four spaces.  This principle has been called the
    // *four-space rule*.
    //
    // The four-space rule is clear and principled, and if the reference
    // implementation `Markdown.pl` had followed it, it probably would have
    // become the standard.  However, `Markdown.pl` allowed paragraphs and
    // sublists to start with only two spaces indentation, at least on the
    // outer level.  Worse, its behavior was inconsistent: a sublist of an
    // outer-level list needed two spaces indentation, but a sublist of this
    // sublist needed three spaces.  It is not surprising, then, that different
    // implementations of Markdown have developed very different rules for
    // determining what comes under a list item.  (Pandoc and python-Markdown,
    // for example, stuck with Gruber's syntax description and the four-space
    // rule, while discount, redcarpet, marked, PHP Markdown, and others
    // followed `Markdown.pl`'s behavior more closely.)
    //
    // Unfortunately, given the divergences between implementations, there
    // is no way to give a spec for list items that will be guaranteed not
    // to break any existing documents.  However, the spec given here should
    // correctly handle lists formatted with either the four-space rule or
    // the more forgiving `Markdown.pl` behavior, provided they are laid out
    // in a way that is natural for a human to read.
    //
    // The strategy here is to let the width and indentation of the list marker
    // determine the indentation necessary for blocks to fall under the list
    // item, rather than having a fixed and arbitrary number.  The writer can
    // think of the body of the list item as a unit which gets indented to the
    // right enough to fit the list marker (and any indentation on the list
    // marker).  (The laziness rule, #5, then allows continuation lines to be
    // unindented if needed.)
    //
    // This rule is superior, we claim, to any rule requiring a fixed level of
    // indentation from the margin.  The four-space rule is clear but
    // unnatural. It is quite unintuitive that
    //
    // ``` markdown
    // - foo
    //
    //   bar
    //
    //   - baz
    // ```
    //
    // should be parsed as two lists with an intervening paragraph,
    //
    // ``` html
    // <ul>
    // <li>foo</li>
    // </ul>
    // <p>bar</p>
    // <ul>
    // <li>baz</li>
    // </ul>
    // ```
    //
    // as the four-space rule demands, rather than a single list,
    //
    // ``` html
    // <ul>
    // <li>
    // <p>foo</p>
    // <p>bar</p>
    // <ul>
    // <li>baz</li>
    // </ul>
    // </li>
    // </ul>
    // ```
    //
    // The choice of four spaces is arbitrary.  It can be learned, but it is
    // not likely to be guessed, and it trips up beginners regularly.
    //
    // Would it help to adopt a two-space rule?  The problem is that such
    // a rule, together with the rule allowing 1--3 spaces indentation of the
    // initial list marker, allows text that is indented *less than* the
    // original list marker to be included in the list item. For example,
    // `Markdown.pl` parses
    //
    // ``` markdown
    //    - one
    //
    //   two
    // ```
    //
    // as a single list item, with `two` a continuation paragraph:
    //
    // ``` html
    // <ul>
    // <li>
    // <p>one</p>
    // <p>two</p>
    // </li>
    // </ul>
    // ```
    //
    // and similarly
    //
    // ``` markdown
    // >   - one
    // >
    // >  two
    // ```
    //
    // as
    //
    // ``` html
    // <blockquote>
    // <ul>
    // <li>
    // <p>one</p>
    // <p>two</p>
    // </li>
    // </ul>
    // </blockquote>
    // ```
    //
    // This is extremely unintuitive.
    //
    // Rather than requiring a fixed indent from the margin, we could require
    // a fixed indent (say, two spaces, or even one space) from the list marker (which
    // may itself be indented).  This proposal would remove the last anomaly
    // discussed.  Unlike the spec presented above, it would count the following
    // as a list item with a subparagraph, even though the paragraph `bar`
    // is not indented as far as the first paragraph `foo`:
    //
    // ``` markdown
    //  10. foo
    //
    //    bar  
    // ```
    //
    // Arguably this text does read like a list item with `bar` as a subparagraph,
    // which may count in favor of the proposal.  However, on this proposal indented
    // code would have to be indented six spaces after the list marker.  And this
    // would break a lot of existing Markdown, which has the pattern:
    //
    // ``` markdown
    // 1.  foo
    //
    //         indented code
    // ```
    //
    // where the code is indented eight spaces.  The spec above, by contrast, will
    // parse this text as expected, since the code block's indentation is measured
    // from the beginning of `foo`.
    //
    // The one case that needs special treatment is a list item that *starts*
    // with indented code.  How much indentation is required in that case, since
    // we don't have a "first paragraph" to measure from?  Rule #2 simply stipulates
    // that in such cases, we require one space indentation from the list marker
    // (and then the normal four spaces for the indented code).  This will match the
    // four-space rule in cases where the list marker plus its initial indentation
    // takes four spaces (a common case), but diverge in other cases.
    //
    // ## Lists
    //
    // A [list](@) is a sequence of one or more
    // list items [of the same type].  The list items
    // may be separated by any number of blank lines.
    //
    // Two list items are [of the same type](@)
    // if they begin with a [list marker] of the same type.
    // Two list markers are of the
    // same type if (a) they are bullet list markers using the same character
    // (`-`, `+`, or `*`) or (b) they are ordered list numbers with the same
    // delimiter (either `.` or `)`).
    //
    // A list is an [ordered list](@)
    // if its constituent list items begin with
    // [ordered list markers], and a
    // [bullet list](@) if its constituent list
    // items begin with [bullet list markers].
    //
    // The [start number](@)
    // of an [ordered list] is determined by the list number of
    // its initial list item.  The numbers of subsequent list items are
    // disregarded.
    //
    // A list is [loose](@) if any of its constituent
    // list items are separated by blank lines, or if any of its constituent
    // list items directly contain two block-level elements with a blank line
    // between them.  Otherwise a list is [tight](@).
    // (The difference in HTML output is that paragraphs in a loose list are
    // wrapped in `<p>` tags, while paragraphs in a tight list are not.)
    //
    // Changing the bullet or ordered list delimiter starts a new list:
    [Fact]
    public void ContainerBlocksLists_Example271()
    {
      // Example 271
      // Section: Container blocks / Lists
      //
      // The following Markdown:
      //   - foo
      //   - bar
      //   + baz
      //
      // Should be rendered as:
      //   <ul>
      //   <li>foo</li>
      //   <li>bar</li>
      //   </ul>
      //   <ul>
      //   <li>baz</li>
      //   </ul>

      Parser.Parse("- foo\n- bar\n+ baz").Is(Parser.Prettify("<ul>\n<li>foo</li>\n<li>bar</li>\n</ul>\n<ul>\n<li>baz</li>\n</ul>"));

      Parser.DoubleParse("- foo\n- bar\n+ baz").Is(Parser.Prettify("<ul>\n<li>foo</li>\n<li>bar</li>\n</ul>\n<ul>\n<li>baz</li>\n</ul>"));
    }

    [Fact]
    public void ContainerBlocksLists_Example272()
    {
      // Example 272
      // Section: Container blocks / Lists
      //
      // The following Markdown:
      //   1. foo
      //   2. bar
      //   3) baz
      //
      // Should be rendered as:
      //   <ol>
      //   <li>foo</li>
      //   <li>bar</li>
      //   </ol>
      //   <ol start="3">
      //   <li>baz</li>
      //   </ol>

      Parser.Parse("1. foo\n2. bar\n3) baz").Is(Parser.Prettify("<ol>\n<li>foo</li>\n<li>bar</li>\n</ol>\n<ol start=\"3\">\n<li>baz</li>\n</ol>"));

      Parser.DoubleParse("1. foo\n2. bar\n3) baz").Is(Parser.Prettify("<ol>\n<li>foo</li>\n<li>bar</li>\n</ol>\n<ol start=\"3\">\n<li>baz</li>\n</ol>"));
    }

    // In CommonMark, a list can interrupt a paragraph. That is,
    // no blank line is needed to separate a paragraph from a following
    // list:
    [Fact]
    public void ContainerBlocksLists_Example273()
    {
      // Example 273
      // Section: Container blocks / Lists
      //
      // The following Markdown:
      //   Foo
      //   - bar
      //   - baz
      //
      // Should be rendered as:
      //   <p>Foo</p>
      //   <ul>
      //   <li>bar</li>
      //   <li>baz</li>
      //   </ul>

      Parser.Parse("Foo\n- bar\n- baz").Is(Parser.Prettify("<p>Foo</p>\n<ul>\n<li>bar</li>\n<li>baz</li>\n</ul>"));

      Parser.DoubleParse("Foo\n- bar\n- baz").Is(Parser.Prettify("<p>Foo</p>\n<ul>\n<li>bar</li>\n<li>baz</li>\n</ul>"));
    }

    // `Markdown.pl` does not allow this, through fear of triggering a list
    // via a numeral in a hard-wrapped line:
    //
    // ``` markdown
    // The number of windows in my house is
    // 14.  The number of doors is 6.
    // ```
    //
    // Oddly, though, `Markdown.pl` *does* allow a blockquote to
    // interrupt a paragraph, even though the same considerations might
    // apply.
    //
    // In CommonMark, we do allow lists to interrupt paragraphs, for
    // two reasons.  First, it is natural and not uncommon for people
    // to start lists without blank lines:
    //
    // ``` markdown
    // I need to buy
    // - new shoes
    // - a coat
    // - a plane ticket
    // ```
    //
    // Second, we are attracted to a
    //
    // > [principle of uniformity](@):
    // > if a chunk of text has a certain
    // > meaning, it will continue to have the same meaning when put into a
    // > container block (such as a list item or blockquote).
    //
    // (Indeed, the spec for [list items] and [block quotes] presupposes
    // this principle.) This principle implies that if
    //
    // ``` markdown
    //   * I need to buy
    //     - new shoes
    //     - a coat
    //     - a plane ticket
    // ```
    //
    // is a list item containing a paragraph followed by a nested sublist,
    // as all Markdown implementations agree it is (though the paragraph
    // may be rendered without `<p>` tags, since the list is "tight"),
    // then
    //
    // ``` markdown
    // I need to buy
    // - new shoes
    // - a coat
    // - a plane ticket
    // ```
    //
    // by itself should be a paragraph followed by a nested sublist.
    //
    // Since it is well established Markdown practice to allow lists to
    // interrupt paragraphs inside list items, the [principle of
    // uniformity] requires us to allow this outside list items as
    // well.  ([reStructuredText](http://docutils.sourceforge.net/rst.html)
    // takes a different approach, requiring blank lines before lists
    // even inside other list items.)
    //
    // In order to solve of unwanted lists in paragraphs with
    // hard-wrapped numerals, we allow only lists starting with `1` to
    // interrupt paragraphs.  Thus,
    [Fact]
    public void ContainerBlocksLists_Example274()
    {
      // Example 274
      // Section: Container blocks / Lists
      //
      // The following Markdown:
      //   The number of windows in my house is
      //   14.  The number of doors is 6.
      //
      // Should be rendered as:
      //   <p>The number of windows in my house is
      //   14.  The number of doors is 6.</p>

      Parser.Parse("The number of windows in my house is\n14.  The number of doors is 6.").Is(Parser.Prettify("<p>The number of windows in my house is\n14.  The number of doors is 6.</p>"));

      Parser.DoubleParse("The number of windows in my house is\n14.  The number of doors is 6.").Is(Parser.Prettify("<p>The number of windows in my house is\n14.  The number of doors is 6.</p>"));
    }

    // We may still get an unintended result in cases like
    [Fact]
    public void ContainerBlocksLists_Example275()
    {
      // Example 275
      // Section: Container blocks / Lists
      //
      // The following Markdown:
      //   The number of windows in my house is
      //   1.  The number of doors is 6.
      //
      // Should be rendered as:
      //   <p>The number of windows in my house is</p>
      //   <ol>
      //   <li>The number of doors is 6.</li>
      //   </ol>

      Parser.Parse("The number of windows in my house is\n1.  The number of doors is 6.").Is(Parser.Prettify("<p>The number of windows in my house is</p>\n<ol>\n<li>The number of doors is 6.</li>\n</ol>"));

      Parser.DoubleParse("The number of windows in my house is\n1.  The number of doors is 6.").Is(Parser.Prettify("<p>The number of windows in my house is</p>\n<ol>\n<li>The number of doors is 6.</li>\n</ol>"));
    }

    // but this rule should prevent most spurious list captures.
    //
    // There can be any number of blank lines between items:
    [Fact]
    public void ContainerBlocksLists_Example276()
    {
      // Example 276
      // Section: Container blocks / Lists
      //
      // The following Markdown:
      //   - foo
      //  
      //   - bar
      //  
      //  
      //   - baz
      //
      // Should be rendered as:
      //   <ul>
      //   <li>
      //   <p>foo</p>
      //   </li>
      //   <li>
      //   <p>bar</p>
      //   </li>
      //   <li>
      //   <p>baz</p>
      //   </li>
      //   </ul>

      Parser.Parse("- foo\n\n- bar\n\n\n- baz").Is(Parser.Prettify("<ul>\n<li>\n<p>foo</p>\n</li>\n<li>\n<p>bar</p>\n</li>\n<li>\n<p>baz</p>\n</li>\n</ul>"));

      Parser.DoubleParse("- foo\n\n- bar\n\n\n- baz").Is(Parser.Prettify("<ul>\n<li>\n<p>foo</p>\n</li>\n<li>\n<p>bar</p>\n</li>\n<li>\n<p>baz</p>\n</li>\n</ul>"));
    }

    [Fact]
    public void ContainerBlocksLists_Example277()
    {
      // Example 277
      // Section: Container blocks / Lists
      //
      // The following Markdown:
      //   - foo
      //     - bar
      //       - baz
      //  
      //  
      //         bim
      //
      // Should be rendered as:
      //   <ul>
      //   <li>foo
      //   <ul>
      //   <li>bar
      //   <ul>
      //   <li>
      //   <p>baz</p>
      //   <p>bim</p>
      //   </li>
      //   </ul>
      //   </li>
      //   </ul>
      //   </li>
      //   </ul>

      Parser.Parse("- foo\n  - bar\n    - baz\n\n\n      bim").Is(Parser.Prettify("<ul>\n<li>foo\n<ul>\n<li>bar\n<ul>\n<li>\n<p>baz</p>\n<p>bim</p>\n</li>\n</ul>\n</li>\n</ul>\n</li>\n</ul>"));

      Parser.DoubleParse("- foo\n  - bar\n    - baz\n\n\n      bim").Is(Parser.Prettify("<ul>\n<li>foo\n<ul>\n<li>bar\n<ul>\n<li>\n<p>baz</p>\n<p>bim</p>\n</li>\n</ul>\n</li>\n</ul>\n</li>\n</ul>"));
    }

    // To separate consecutive lists of the same type, or to separate a
    // list from an indented code block that would otherwise be parsed
    // as a subparagraph of the final list item, you can insert a blank HTML
    // comment:
    [Fact]
    public void ContainerBlocksLists_Example278()
    {
      // Example 278
      // Section: Container blocks / Lists
      //
      // The following Markdown:
      //   - foo
      //   - bar
      //  
      //   <!-- -->
      //  
      //   - baz
      //   - bim
      //
      // Should be rendered as:
      //   <ul>
      //   <li>foo</li>
      //   <li>bar</li>
      //   </ul>
      //   <!-- -->
      //   <ul>
      //   <li>baz</li>
      //   <li>bim</li>
      //   </ul>

      Parser.Parse("- foo\n- bar\n\n<!-- -->\n\n- baz\n- bim").Is(Parser.Prettify("<ul>\n<li>foo</li>\n<li>bar</li>\n</ul>\n<!-- -->\n<ul>\n<li>baz</li>\n<li>bim</li>\n</ul>"));

      Parser.DoubleParse("- foo\n- bar\n\n<!-- -->\n\n- baz\n- bim").Is(Parser.Prettify("<ul>\n<li>foo</li>\n<li>bar</li>\n</ul>\n<!-- -->\n<ul>\n<li>baz</li>\n<li>bim</li>\n</ul>"));
    }

    [Fact]
    public void ContainerBlocksLists_Example279()
    {
      // Example 279
      // Section: Container blocks / Lists
      //
      // The following Markdown:
      //   -   foo
      //  
      //       notcode
      //  
      //   -   foo
      //  
      //   <!-- -->
      //  
      //       code
      //
      // Should be rendered as:
      //   <ul>
      //   <li>
      //   <p>foo</p>
      //   <p>notcode</p>
      //   </li>
      //   <li>
      //   <p>foo</p>
      //   </li>
      //   </ul>
      //   <!-- -->
      //   <pre><code>code
      //   </code></pre>

      Parser.Parse("-   foo\n\n    notcode\n\n-   foo\n\n<!-- -->\n\n    code").Is(Parser.Prettify("<ul>\n<li>\n<p>foo</p>\n<p>notcode</p>\n</li>\n<li>\n<p>foo</p>\n</li>\n</ul>\n<!-- -->\n<pre><code>code\n</code></pre>"));

      Parser.DoubleParse("-   foo\n\n    notcode\n\n-   foo\n\n<!-- -->\n\n    code").Is(Parser.Prettify("<ul>\n<li>\n<p>foo</p>\n<p>notcode</p>\n</li>\n<li>\n<p>foo</p>\n</li>\n</ul>\n<!-- -->\n<pre><code>code\n</code></pre>"));
    }

    // List items need not be indented to the same level.  The following
    // list items will be treated as items at the same list level,
    // since none is indented enough to belong to the previous list
    // item:
    [Fact]
    public void ContainerBlocksLists_Example280()
    {
      // Example 280
      // Section: Container blocks / Lists
      //
      // The following Markdown:
      //   - a
      //    - b
      //     - c
      //      - d
      //     - e
      //    - f
      //   - g
      //
      // Should be rendered as:
      //   <ul>
      //   <li>a</li>
      //   <li>b</li>
      //   <li>c</li>
      //   <li>d</li>
      //   <li>e</li>
      //   <li>f</li>
      //   <li>g</li>
      //   </ul>

      Parser.Parse("- a\n - b\n  - c\n   - d\n  - e\n - f\n- g").Is(Parser.Prettify("<ul>\n<li>a</li>\n<li>b</li>\n<li>c</li>\n<li>d</li>\n<li>e</li>\n<li>f</li>\n<li>g</li>\n</ul>"));

      Parser.DoubleParse("- a\n - b\n  - c\n   - d\n  - e\n - f\n- g").Is(Parser.Prettify("<ul>\n<li>a</li>\n<li>b</li>\n<li>c</li>\n<li>d</li>\n<li>e</li>\n<li>f</li>\n<li>g</li>\n</ul>"));
    }

    [Fact]
    public void ContainerBlocksLists_Example281()
    {
      // Example 281
      // Section: Container blocks / Lists
      //
      // The following Markdown:
      //   1. a
      //  
      //     2. b
      //  
      //      3. c
      //
      // Should be rendered as:
      //   <ol>
      //   <li>
      //   <p>a</p>
      //   </li>
      //   <li>
      //   <p>b</p>
      //   </li>
      //   <li>
      //   <p>c</p>
      //   </li>
      //   </ol>

      Parser.Parse("1. a\n\n  2. b\n\n   3. c").Is(Parser.Prettify("<ol>\n<li>\n<p>a</p>\n</li>\n<li>\n<p>b</p>\n</li>\n<li>\n<p>c</p>\n</li>\n</ol>"));

      Parser.DoubleParse("1. a\n\n  2. b\n\n   3. c").Is(Parser.Prettify("<ol>\n<li>\n<p>a</p>\n</li>\n<li>\n<p>b</p>\n</li>\n<li>\n<p>c</p>\n</li>\n</ol>"));
    }

    // Note, however, that list items may not be indented more than
    // three spaces.  Here `- e` is treated as a paragraph continuation
    // line, because it is indented more than three spaces:
    [Fact]
    public void ContainerBlocksLists_Example282()
    {
      // Example 282
      // Section: Container blocks / Lists
      //
      // The following Markdown:
      //   - a
      //    - b
      //     - c
      //      - d
      //       - e
      //
      // Should be rendered as:
      //   <ul>
      //   <li>a</li>
      //   <li>b</li>
      //   <li>c</li>
      //   <li>d
      //   - e</li>
      //   </ul>

      Parser.Parse("- a\n - b\n  - c\n   - d\n    - e").Is(Parser.Prettify("<ul>\n<li>a</li>\n<li>b</li>\n<li>c</li>\n<li>d\n- e</li>\n</ul>"));

      Parser.DoubleParse("- a\n - b\n  - c\n   - d\n    - e").Is(Parser.Prettify("<ul>\n<li>a</li>\n<li>b</li>\n<li>c</li>\n<li>d\n- e</li>\n</ul>"));
    }

    // And here, `3. c` is treated as in indented code block,
    // because it is indented four spaces and preceded by a
    // blank line.
    [Fact]
    public void ContainerBlocksLists_Example283()
    {
      // Example 283
      // Section: Container blocks / Lists
      //
      // The following Markdown:
      //   1. a
      //  
      //     2. b
      //  
      //       3. c
      //
      // Should be rendered as:
      //   <ol>
      //   <li>
      //   <p>a</p>
      //   </li>
      //   <li>
      //   <p>b</p>
      //   </li>
      //   </ol>
      //   <pre><code>3. c
      //   </code></pre>

      Parser.Parse("1. a\n\n  2. b\n\n    3. c").Is(Parser.Prettify("<ol>\n<li>\n<p>a</p>\n</li>\n<li>\n<p>b</p>\n</li>\n</ol>\n<pre><code>3. c\n</code></pre>"));

      Parser.DoubleParse("1. a\n\n  2. b\n\n    3. c").Is(Parser.Prettify("<ol>\n<li>\n<p>a</p>\n</li>\n<li>\n<p>b</p>\n</li>\n</ol>\n<pre><code>3. c\n</code></pre>"));
    }

    // This is a loose list, because there is a blank line between
    // two of the list items:
    [Fact]
    public void ContainerBlocksLists_Example284()
    {
      // Example 284
      // Section: Container blocks / Lists
      //
      // The following Markdown:
      //   - a
      //   - b
      //  
      //   - c
      //
      // Should be rendered as:
      //   <ul>
      //   <li>
      //   <p>a</p>
      //   </li>
      //   <li>
      //   <p>b</p>
      //   </li>
      //   <li>
      //   <p>c</p>
      //   </li>
      //   </ul>

      Parser.Parse("- a\n- b\n\n- c").Is(Parser.Prettify("<ul>\n<li>\n<p>a</p>\n</li>\n<li>\n<p>b</p>\n</li>\n<li>\n<p>c</p>\n</li>\n</ul>"));

      Parser.DoubleParse("- a\n- b\n\n- c").Is(Parser.Prettify("<ul>\n<li>\n<p>a</p>\n</li>\n<li>\n<p>b</p>\n</li>\n<li>\n<p>c</p>\n</li>\n</ul>"));
    }

    // So is this, with a empty second item:
    [Fact]
    public void ContainerBlocksLists_Example285()
    {
      // Example 285
      // Section: Container blocks / Lists
      //
      // The following Markdown:
      //   * a
      //   *
      //  
      //   * c
      //
      // Should be rendered as:
      //   <ul>
      //   <li>
      //   <p>a</p>
      //   </li>
      //   <li></li>
      //   <li>
      //   <p>c</p>
      //   </li>
      //   </ul>

      Parser.Parse("* a\n*\n\n* c").Is(Parser.Prettify("<ul>\n<li>\n<p>a</p>\n</li>\n<li></li>\n<li>\n<p>c</p>\n</li>\n</ul>"));

      Parser.DoubleParse("* a\n*\n\n* c").Is(Parser.Prettify("<ul>\n<li>\n<p>a</p>\n</li>\n<li></li>\n<li>\n<p>c</p>\n</li>\n</ul>"));
    }

    // These are loose lists, even though there is no space between the items,
    // because one of the items directly contains two block-level elements
    // with a blank line between them:
    [Fact]
    public void ContainerBlocksLists_Example286()
    {
      // Example 286
      // Section: Container blocks / Lists
      //
      // The following Markdown:
      //   - a
      //   - b
      //  
      //     c
      //   - d
      //
      // Should be rendered as:
      //   <ul>
      //   <li>
      //   <p>a</p>
      //   </li>
      //   <li>
      //   <p>b</p>
      //   <p>c</p>
      //   </li>
      //   <li>
      //   <p>d</p>
      //   </li>
      //   </ul>

      Parser.Parse("- a\n- b\n\n  c\n- d").Is(Parser.Prettify("<ul>\n<li>\n<p>a</p>\n</li>\n<li>\n<p>b</p>\n<p>c</p>\n</li>\n<li>\n<p>d</p>\n</li>\n</ul>"));

      Parser.DoubleParse("- a\n- b\n\n  c\n- d").Is(Parser.Prettify("<ul>\n<li>\n<p>a</p>\n</li>\n<li>\n<p>b</p>\n<p>c</p>\n</li>\n<li>\n<p>d</p>\n</li>\n</ul>"));
    }

    [Fact]
    public void ContainerBlocksLists_Example287()
    {
      // Example 287
      // Section: Container blocks / Lists
      //
      // The following Markdown:
      //   - a
      //   - b
      //  
      //     [ref]: /url
      //   - d
      //
      // Should be rendered as:
      //   <ul>
      //   <li>
      //   <p>a</p>
      //   </li>
      //   <li>
      //   <p>b</p>
      //   </li>
      //   <li>
      //   <p>d</p>
      //   </li>
      //   </ul>

      Parser.Parse("- a\n- b\n\n  [ref]: /url\n- d").Is(Parser.Prettify("<ul>\n<li>\n<p>a</p>\n</li>\n<li>\n<p>b</p>\n</li>\n<li>\n<p>d</p>\n</li>\n</ul>"));

      Parser.DoubleParse("- a\n- b\n\n  [ref]: /url\n- d").Is(Parser.Prettify("<ul>\n<li>\n<p>a</p>\n</li>\n<li>\n<p>b</p>\n</li>\n<li>\n<p>d</p>\n</li>\n</ul>"));
    }

    // This is a tight list, because the blank lines are in a code block:
    [Fact]
    public void ContainerBlocksLists_Example288()
    {
      // Example 288
      // Section: Container blocks / Lists
      //
      // The following Markdown:
      //   - a
      //   - ```
      //     b
      //  
      //  
      //     ```
      //   - c
      //
      // Should be rendered as:
      //   <ul>
      //   <li>a</li>
      //   <li>
      //   <pre><code>b
      //  
      //  
      //   </code></pre>
      //   </li>
      //   <li>c</li>
      //   </ul>

      Parser.Parse("- a\n- ```\n  b\n\n\n  ```\n- c").Is(Parser.Prettify("<ul>\n<li>a</li>\n<li>\n<pre><code>b\n\n\n</code></pre>\n</li>\n<li>c</li>\n</ul>"));

      Parser.DoubleParse("- a\n- ```\n  b\n\n\n  ```\n- c").Is(Parser.Prettify("<ul>\n<li>a</li>\n<li>\n<pre><code>b\n\n\n</code></pre>\n</li>\n<li>c</li>\n</ul>"));
    }

    // This is a tight list, because the blank line is between two
    // paragraphs of a sublist.  So the sublist is loose while
    // the outer list is tight:
    [Fact]
    public void ContainerBlocksLists_Example289()
    {
      // Example 289
      // Section: Container blocks / Lists
      //
      // The following Markdown:
      //   - a
      //     - b
      //  
      //       c
      //   - d
      //
      // Should be rendered as:
      //   <ul>
      //   <li>a
      //   <ul>
      //   <li>
      //   <p>b</p>
      //   <p>c</p>
      //   </li>
      //   </ul>
      //   </li>
      //   <li>d</li>
      //   </ul>

      Parser.Parse("- a\n  - b\n\n    c\n- d").Is(Parser.Prettify("<ul>\n<li>a\n<ul>\n<li>\n<p>b</p>\n<p>c</p>\n</li>\n</ul>\n</li>\n<li>d</li>\n</ul>"));

      Parser.DoubleParse("- a\n  - b\n\n    c\n- d").Is(Parser.Prettify("<ul>\n<li>a\n<ul>\n<li>\n<p>b</p>\n<p>c</p>\n</li>\n</ul>\n</li>\n<li>d</li>\n</ul>"));
    }

    // This is a tight list, because the blank line is inside the
    // block quote:
    [Fact]
    public void ContainerBlocksLists_Example290()
    {
      // Example 290
      // Section: Container blocks / Lists
      //
      // The following Markdown:
      //   * a
      //     > b
      //     >
      //   * c
      //
      // Should be rendered as:
      //   <ul>
      //   <li>a
      //   <blockquote>
      //   <p>b</p>
      //   </blockquote>
      //   </li>
      //   <li>c</li>
      //   </ul>

      Parser.Parse("* a\n  > b\n  >\n* c").Is(Parser.Prettify("<ul>\n<li>a\n<blockquote>\n<p>b</p>\n</blockquote>\n</li>\n<li>c</li>\n</ul>"));

      Parser.DoubleParse("* a\n  > b\n  >\n* c").Is(Parser.Prettify("<ul>\n<li>a\n<blockquote>\n<p>b</p>\n</blockquote>\n</li>\n<li>c</li>\n</ul>"));
    }

    // This list is tight, because the consecutive block elements
    // are not separated by blank lines:
    [Fact]
    public void ContainerBlocksLists_Example291()
    {
      // Example 291
      // Section: Container blocks / Lists
      //
      // The following Markdown:
      //   - a
      //     > b
      //     ```
      //     c
      //     ```
      //   - d
      //
      // Should be rendered as:
      //   <ul>
      //   <li>a
      //   <blockquote>
      //   <p>b</p>
      //   </blockquote>
      //   <pre><code>c
      //   </code></pre>
      //   </li>
      //   <li>d</li>
      //   </ul>

      Parser.Parse("- a\n  > b\n  ```\n  c\n  ```\n- d").Is(Parser.Prettify("<ul>\n<li>a\n<blockquote>\n<p>b</p>\n</blockquote>\n<pre><code>c\n</code></pre>\n</li>\n<li>d</li>\n</ul>"));

      Parser.DoubleParse("- a\n  > b\n  ```\n  c\n  ```\n- d").Is(Parser.Prettify("<ul>\n<li>a\n<blockquote>\n<p>b</p>\n</blockquote>\n<pre><code>c\n</code></pre>\n</li>\n<li>d</li>\n</ul>"));
    }

    // A single-paragraph list is tight:
    [Fact]
    public void ContainerBlocksLists_Example292()
    {
      // Example 292
      // Section: Container blocks / Lists
      //
      // The following Markdown:
      //   - a
      //
      // Should be rendered as:
      //   <ul>
      //   <li>a</li>
      //   </ul>

      Parser.Parse("- a").Is(Parser.Prettify("<ul>\n<li>a</li>\n</ul>"));

      Parser.DoubleParse("- a").Is(Parser.Prettify("<ul>\n<li>a</li>\n</ul>"));
    }

    [Fact]
    public void ContainerBlocksLists_Example293()
    {
      // Example 293
      // Section: Container blocks / Lists
      //
      // The following Markdown:
      //   - a
      //     - b
      //
      // Should be rendered as:
      //   <ul>
      //   <li>a
      //   <ul>
      //   <li>b</li>
      //   </ul>
      //   </li>
      //   </ul>

      Parser.Parse("- a\n  - b").Is(Parser.Prettify("<ul>\n<li>a\n<ul>\n<li>b</li>\n</ul>\n</li>\n</ul>"));

      Parser.DoubleParse("- a\n  - b").Is(Parser.Prettify("<ul>\n<li>a\n<ul>\n<li>b</li>\n</ul>\n</li>\n</ul>"));
    }

    // This list is loose, because of the blank line between the
    // two block elements in the list item:
    [Fact]
    public void ContainerBlocksLists_Example294()
    {
      // Example 294
      // Section: Container blocks / Lists
      //
      // The following Markdown:
      //   1. ```
      //      foo
      //      ```
      //  
      //      bar
      //
      // Should be rendered as:
      //   <ol>
      //   <li>
      //   <pre><code>foo
      //   </code></pre>
      //   <p>bar</p>
      //   </li>
      //   </ol>

      Parser.Parse("1. ```\n   foo\n   ```\n\n   bar").Is(Parser.Prettify("<ol>\n<li>\n<pre><code>foo\n</code></pre>\n<p>bar</p>\n</li>\n</ol>"));

      Parser.DoubleParse("1. ```\n   foo\n   ```\n\n   bar").Is(Parser.Prettify("<ol>\n<li>\n<pre><code>foo\n</code></pre>\n<p>bar</p>\n</li>\n</ol>"));
    }

    // Here the outer list is loose, the inner list tight:
    [Fact]
    public void ContainerBlocksLists_Example295()
    {
      // Example 295
      // Section: Container blocks / Lists
      //
      // The following Markdown:
      //   * foo
      //     * bar
      //  
      //     baz
      //
      // Should be rendered as:
      //   <ul>
      //   <li>
      //   <p>foo</p>
      //   <ul>
      //   <li>bar</li>
      //   </ul>
      //   <p>baz</p>
      //   </li>
      //   </ul>

      Parser.Parse("* foo\n  * bar\n\n  baz").Is(Parser.Prettify("<ul>\n<li>\n<p>foo</p>\n<ul>\n<li>bar</li>\n</ul>\n<p>baz</p>\n</li>\n</ul>"));

      Parser.DoubleParse("* foo\n  * bar\n\n  baz").Is(Parser.Prettify("<ul>\n<li>\n<p>foo</p>\n<ul>\n<li>bar</li>\n</ul>\n<p>baz</p>\n</li>\n</ul>"));
    }

    [Fact]
    public void ContainerBlocksLists_Example296()
    {
      // Example 296
      // Section: Container blocks / Lists
      //
      // The following Markdown:
      //   - a
      //     - b
      //     - c
      //  
      //   - d
      //     - e
      //     - f
      //
      // Should be rendered as:
      //   <ul>
      //   <li>
      //   <p>a</p>
      //   <ul>
      //   <li>b</li>
      //   <li>c</li>
      //   </ul>
      //   </li>
      //   <li>
      //   <p>d</p>
      //   <ul>
      //   <li>e</li>
      //   <li>f</li>
      //   </ul>
      //   </li>
      //   </ul>

      Parser.Parse("- a\n  - b\n  - c\n\n- d\n  - e\n  - f").Is(Parser.Prettify("<ul>\n<li>\n<p>a</p>\n<ul>\n<li>b</li>\n<li>c</li>\n</ul>\n</li>\n<li>\n<p>d</p>\n<ul>\n<li>e</li>\n<li>f</li>\n</ul>\n</li>\n</ul>"));

      Parser.DoubleParse("- a\n  - b\n  - c\n\n- d\n  - e\n  - f").Is(Parser.Prettify("<ul>\n<li>\n<p>a</p>\n<ul>\n<li>b</li>\n<li>c</li>\n</ul>\n</li>\n<li>\n<p>d</p>\n<ul>\n<li>e</li>\n<li>f</li>\n</ul>\n</li>\n</ul>"));
    }
  }

  public class TestInlines
  {
    // # Inlines
    //
    // Inlines are parsed sequentially from the beginning of the character
    // stream to the end (left to right, in left-to-right languages).
    // Thus, for example, in
    [Fact]
    public void Inlines_Example297()
    {
      // Example 297
      // Section: Inlines
      //
      // The following Markdown:
      //   `hi`lo`
      //
      // Should be rendered as:
      //   <p><code>hi</code>lo`</p>

      Parser.Parse("`hi`lo`").Is(Parser.Prettify("<p><code>hi</code>lo`</p>"));

      Parser.DoubleParse("`hi`lo`").Is(Parser.Prettify("<p><code>hi</code>lo`</p>"));
    }
  }

  public class TestInlinesBackslashEscapes
  {
    // `hi` is parsed as code, leaving the backtick at the end as a literal
    // backtick.
    //
    //
    // ## Backslash escapes
    //
    // Any ASCII punctuation character may be backslash-escaped:
    [Fact]
    public void InlinesBackslashEscapes_Example298()
    {
      // Example 298
      // Section: Inlines / Backslash escapes
      //
      // The following Markdown:
      //   \!\"\#\$\%\&\'\(\)\*\+\,\-\.\/\:\;\<\=\>\?\@\[\\\]\^\_\`\{\|\}\~
      //
      // Should be rendered as:
      //   <p>!&quot;#$%&amp;'()*+,-./:;&lt;=&gt;?@[\]^_`{|}~</p>

      Parser.Parse("\\!\\\"\\#\\$\\%\\&\\'\\(\\)\\*\\+\\,\\-\\.\\/\\:\\;\\<\\=\\>\\?\\@\\[\\\\\\]\\^\\_\\`\\{\\|\\}\\~").Is(Parser.Prettify("<p>!&quot;#$%&amp;'()*+,-./:;&lt;=&gt;?@[\\]^_`{|}~</p>"));

      Parser.DoubleParse("\\!\\\"\\#\\$\\%\\&\\'\\(\\)\\*\\+\\,\\-\\.\\/\\:\\;\\<\\=\\>\\?\\@\\[\\\\\\]\\^\\_\\`\\{\\|\\}\\~").Is(Parser.Prettify("<p>!&quot;#$%&amp;'()*+,-./:;&lt;=&gt;?@[\\]^_`{|}~</p>"));
    }

    // Backslashes before other characters are treated as literal
    // backslashes:
    [Fact]
    public void InlinesBackslashEscapes_Example299()
    {
      // Example 299
      // Section: Inlines / Backslash escapes
      //
      // The following Markdown:
      //   \→\A\a\ \3\φ\«
      //
      // Should be rendered as:
      //   <p>\→\A\a\ \3\φ\«</p>

      Parser.Parse("\\\t\\A\\a\\ \\3\\φ\\«").Is(Parser.Prettify("<p>\\\t\\A\\a\\ \\3\\φ\\«</p>"));

      Parser.DoubleParse("\\\t\\A\\a\\ \\3\\φ\\«").Is(Parser.Prettify("<p>\\\t\\A\\a\\ \\3\\φ\\«</p>"));
    }

    // Escaped characters are treated as regular characters and do
    // not have their usual Markdown meanings:
    [Fact]
    public void InlinesBackslashEscapes_Example300()
    {
      // Example 300
      // Section: Inlines / Backslash escapes
      //
      // The following Markdown:
      //   \*not emphasized*
      //   \<br/> not a tag
      //   \[not a link](/foo)
      //   \`not code`
      //   1\. not a list
      //   \* not a list
      //   \# not a heading
      //   \[foo]: /url "not a reference"
      //   \&ouml; not a character entity
      //
      // Should be rendered as:
      //   <p>*not emphasized*
      //   &lt;br/&gt; not a tag
      //   [not a link](/foo)
      //   `not code`
      //   1. not a list
      //   * not a list
      //   # not a heading
      //   [foo]: /url &quot;not a reference&quot;
      //   &amp;ouml; not a character entity</p>

      Parser.Parse("\\*not emphasized*\n\\<br/> not a tag\n\\[not a link](/foo)\n\\`not code`\n1\\. not a list\n\\* not a list\n\\# not a heading\n\\[foo]: /url \"not a reference\"\n\\&ouml; not a character entity").Is(Parser.Prettify("<p>*not emphasized*\n&lt;br/&gt; not a tag\n[not a link](/foo)\n`not code`\n1. not a list\n* not a list\n# not a heading\n[foo]: /url &quot;not a reference&quot;\n&amp;ouml; not a character entity</p>"));

      Parser.DoubleParse("\\*not emphasized*\n\\<br/> not a tag\n\\[not a link](/foo)\n\\`not code`\n1\\. not a list\n\\* not a list\n\\# not a heading\n\\[foo]: /url \"not a reference\"\n\\&ouml; not a character entity").Is(Parser.Prettify("<p>*not emphasized*\n&lt;br/&gt; not a tag\n[not a link](/foo)\n`not code`\n1. not a list\n* not a list\n# not a heading\n[foo]: /url &quot;not a reference&quot;\n&amp;ouml; not a character entity</p>"));
    }

    // If a backslash is itself escaped, the following character is not:
    [Fact]
    public void InlinesBackslashEscapes_Example301()
    {
      // Example 301
      // Section: Inlines / Backslash escapes
      //
      // The following Markdown:
      //   \\*emphasis*
      //
      // Should be rendered as:
      //   <p>\<em>emphasis</em></p>

      Parser.Parse("\\\\*emphasis*").Is(Parser.Prettify("<p>\\<em>emphasis</em></p>"));

      Parser.DoubleParse("\\\\*emphasis*").Is(Parser.Prettify("<p>\\<em>emphasis</em></p>"));
    }

    // A backslash at the end of the line is a [hard line break]:
    [Fact]
    public void InlinesBackslashEscapes_Example302()
    {
      // Example 302
      // Section: Inlines / Backslash escapes
      //
      // The following Markdown:
      //   foo\
      //   bar
      //
      // Should be rendered as:
      //   <p>foo<br />
      //   bar</p>

      Parser.Parse("foo\\\nbar").Is(Parser.Prettify("<p>foo<br />\nbar</p>"));

      Parser.DoubleParse("foo\\\nbar").Is(Parser.Prettify("<p>foo<br />\nbar</p>"));
    }

    // Backslash escapes do not work in code blocks, code spans, autolinks, or
    // raw HTML:
    [Fact]
    public void InlinesBackslashEscapes_Example303()
    {
      // Example 303
      // Section: Inlines / Backslash escapes
      //
      // The following Markdown:
      //   `` \[\` ``
      //
      // Should be rendered as:
      //   <p><code>\[\`</code></p>

      Parser.Parse("`` \\[\\` ``").Is(Parser.Prettify("<p><code>\\[\\`</code></p>"));

      Parser.DoubleParse("`` \\[\\` ``").Is(Parser.Prettify("<p><code>\\[\\`</code></p>"));
    }

    [Fact]
    public void InlinesBackslashEscapes_Example304()
    {
      // Example 304
      // Section: Inlines / Backslash escapes
      //
      // The following Markdown:
      //       \[\]
      //
      // Should be rendered as:
      //   <pre><code>\[\]
      //   </code></pre>

      Parser.Parse("    \\[\\]").Is(Parser.Prettify("<pre><code>\\[\\]\n</code></pre>"));

      Parser.DoubleParse("    \\[\\]").Is(Parser.Prettify("<pre><code>\\[\\]\n</code></pre>"));
    }

    [Fact]
    public void InlinesBackslashEscapes_Example305()
    {
      // Example 305
      // Section: Inlines / Backslash escapes
      //
      // The following Markdown:
      //   ~~~
      //   \[\]
      //   ~~~
      //
      // Should be rendered as:
      //   <pre><code>\[\]
      //   </code></pre>

      Parser.Parse("~~~\n\\[\\]\n~~~").Is(Parser.Prettify("<pre><code>\\[\\]\n</code></pre>"));

      Parser.DoubleParse("~~~\n\\[\\]\n~~~").Is(Parser.Prettify("<pre><code>\\[\\]\n</code></pre>"));
    }

    [Fact]
    public void InlinesBackslashEscapes_Example306()
    {
      // Example 306
      // Section: Inlines / Backslash escapes
      //
      // The following Markdown:
      //   <http://example.com?find=\*>
      //
      // Should be rendered as:
      //   <p><a href="http://example.com?find=%5C*">http://example.com?find=\*</a></p>

      Parser.Parse("<http://example.com?find=\\*>").Is(Parser.Prettify("<p><a href=\"http://example.com?find=%5C*\">http://example.com?find=\\*</a></p>"));

      Parser.DoubleParse("<http://example.com?find=\\*>").Is(Parser.Prettify("<p><a href=\"http://example.com?find=%5C*\">http://example.com?find=\\*</a></p>"));
    }

    [Fact]
    public void InlinesBackslashEscapes_Example307()
    {
      // Example 307
      // Section: Inlines / Backslash escapes
      //
      // The following Markdown:
      //   <a href="/bar\/)">
      //
      // Should be rendered as:
      //   <a href="/bar\/)">

      Parser.Parse("<a href=\"/bar\\/)\">").Is(Parser.Prettify("<a href=\"/bar\\/)\">"));

      Parser.DoubleParse("<a href=\"/bar\\/)\">").Is(Parser.Prettify("<a href=\"/bar\\/)\">"));
    }

    // But they work in all other contexts, including URLs and link titles,
    // link references, and [info strings] in [fenced code blocks]:
    [Fact]
    public void InlinesBackslashEscapes_Example308()
    {
      // Example 308
      // Section: Inlines / Backslash escapes
      //
      // The following Markdown:
      //   [foo](/bar\* "ti\*tle")
      //
      // Should be rendered as:
      //   <p><a href="/bar*" title="ti*tle">foo</a></p>

      Parser.Parse("[foo](/bar\\* \"ti\\*tle\")").Is(Parser.Prettify("<p><a href=\"/bar*\" title=\"ti*tle\">foo</a></p>"));

      Parser.DoubleParse("[foo](/bar\\* \"ti\\*tle\")").Is(Parser.Prettify("<p><a href=\"/bar*\" title=\"ti*tle\">foo</a></p>"));
    }

    [Fact]
    public void InlinesBackslashEscapes_Example309()
    {
      // Example 309
      // Section: Inlines / Backslash escapes
      //
      // The following Markdown:
      //   [foo]
      //  
      //   [foo]: /bar\* "ti\*tle"
      //
      // Should be rendered as:
      //   <p><a href="/bar*" title="ti*tle">foo</a></p>

      Parser.Parse("[foo]\n\n[foo]: /bar\\* \"ti\\*tle\"").Is(Parser.Prettify("<p><a href=\"/bar*\" title=\"ti*tle\">foo</a></p>"));

      Parser.DoubleParse("[foo]\n\n[foo]: /bar\\* \"ti\\*tle\"").Is(Parser.Prettify("<p><a href=\"/bar*\" title=\"ti*tle\">foo</a></p>"));
    }

    [Fact]
    public void InlinesBackslashEscapes_Example310()
    {
      // Example 310
      // Section: Inlines / Backslash escapes
      //
      // The following Markdown:
      //   ``` foo\+bar
      //   foo
      //   ```
      //
      // Should be rendered as:
      //   <pre><code class="language-foo+bar">foo
      //   </code></pre>

      Parser.Parse("``` foo\\+bar\nfoo\n```").Is(Parser.Prettify("<pre><code class=\"language-foo+bar\">foo\n</code></pre>"));

      Parser.DoubleParse("``` foo\\+bar\nfoo\n```").Is(Parser.Prettify("<pre><code class=\"language-foo+bar\">foo\n</code></pre>"));
    }
  }

  public class TestInlinesEntityAndNumericCharacterReferences
  {
    // ## Entity and numeric character references
    //
    // Valid HTML entity references and numeric character references
    // can be used in place of the corresponding Unicode character,
    // with the following exceptions:
    //
    // - Entity and character references are not recognized in code
    //   blocks and code spans.
    //
    // - Entity and character references cannot stand in place of
    //   special characters that define structural elements in
    //   CommonMark.  For example, although `&#42;` can be used
    //   in place of a literal `*` character, `&#42;` cannot replace
    //   `*` in emphasis delimiters, bullet list markers, or thematic
    //   breaks.
    //
    // Conforming CommonMark parsers need not store information about
    // whether a particular character was represented in the source
    // using a Unicode character or an entity reference.
    //
    // [Entity references](@) consist of `&` + any of the valid
    // HTML5 entity names + `;`. The
    // document <https://html.spec.whatwg.org/multipage/entities.json>
    // is used as an authoritative source for the valid entity
    // references and their corresponding code points.
    [Fact]
    public void InlinesEntityAndNumericCharacterReferences_Example311()
    {
      // Example 311
      // Section: Inlines / Entity and numeric character references
      //
      // The following Markdown:
      //   &nbsp; &amp; &copy; &AElig; &Dcaron;
      //   &frac34; &HilbertSpace; &DifferentialD;
      //   &ClockwiseContourIntegral; &ngE;
      //
      // Should be rendered as:
      //   <p>  &amp; © Æ Ď
      //   ¾ ℋ ⅆ
      //   ∲ ≧̸</p>

      Parser.Parse("&nbsp; &amp; &copy; &AElig; &Dcaron;\n&frac34; &HilbertSpace; &DifferentialD;\n&ClockwiseContourIntegral; &ngE;").Is(Parser.Prettify("<p>  &amp; © Æ Ď\n¾ ℋ ⅆ\n∲ ≧̸</p>"));

      Parser.DoubleParse("&nbsp; &amp; &copy; &AElig; &Dcaron;\n&frac34; &HilbertSpace; &DifferentialD;\n&ClockwiseContourIntegral; &ngE;").Is(Parser.Prettify("<p>  &amp; © Æ Ď\n¾ ℋ ⅆ\n∲ ≧̸</p>"));
    }

    // [Decimal numeric character
    // references](@)
    // consist of `&#` + a string of 1--7 arabic digits + `;`. A
    // numeric character reference is parsed as the corresponding
    // Unicode character. Invalid Unicode code points will be replaced by
    // the REPLACEMENT CHARACTER (`U+FFFD`).  For security reasons,
    // the code point `U+0000` will also be replaced by `U+FFFD`.
    [Fact]
    public void InlinesEntityAndNumericCharacterReferences_Example312()
    {
      // Example 312
      // Section: Inlines / Entity and numeric character references
      //
      // The following Markdown:
      //   &#35; &#1234; &#992; &#0;
      //
      // Should be rendered as:
      //   <p># Ӓ Ϡ �</p>

      Parser.Parse("&#35; &#1234; &#992; &#0;").Is(Parser.Prettify("<p># Ӓ Ϡ �</p>"));

      Parser.DoubleParse("&#35; &#1234; &#992; &#0;").Is(Parser.Prettify("<p># Ӓ Ϡ �</p>"));
    }

    // [Hexadecimal numeric character
    // references](@) consist of `&#` +
    // either `X` or `x` + a string of 1-6 hexadecimal digits + `;`.
    // They too are parsed as the corresponding Unicode character (this
    // time specified with a hexadecimal numeral instead of decimal).
    [Fact]
    public void InlinesEntityAndNumericCharacterReferences_Example313()
    {
      // Example 313
      // Section: Inlines / Entity and numeric character references
      //
      // The following Markdown:
      //   &#X22; &#XD06; &#xcab;
      //
      // Should be rendered as:
      //   <p>&quot; ആ ಫ</p>

      Parser.Parse("&#X22; &#XD06; &#xcab;").Is(Parser.Prettify("<p>&quot; ആ ಫ</p>"));

      Parser.DoubleParse("&#X22; &#XD06; &#xcab;").Is(Parser.Prettify("<p>&quot; ആ ಫ</p>"));
    }

    // Here are some nonentities:
    [Fact]
    public void InlinesEntityAndNumericCharacterReferences_Example314()
    {
      // Example 314
      // Section: Inlines / Entity and numeric character references
      //
      // The following Markdown:
      //   &nbsp &x; &#; &#x;
      //   &#87654321;
      //   &#abcdef0;
      //   &ThisIsNotDefined; &hi?;
      //
      // Should be rendered as:
      //   <p>&amp;nbsp &amp;x; &amp;#; &amp;#x;
      //   &amp;#87654321;
      //   &amp;#abcdef0;
      //   &amp;ThisIsNotDefined; &amp;hi?;</p>

      Parser.Parse("&nbsp &x; &#; &#x;\n&#87654321;\n&#abcdef0;\n&ThisIsNotDefined; &hi?;").Is(Parser.Prettify("<p>&amp;nbsp &amp;x; &amp;#; &amp;#x;\n&amp;#87654321;\n&amp;#abcdef0;\n&amp;ThisIsNotDefined; &amp;hi?;</p>"));

      Parser.DoubleParse("&nbsp &x; &#; &#x;\n&#87654321;\n&#abcdef0;\n&ThisIsNotDefined; &hi?;").Is(Parser.Prettify("<p>&amp;nbsp &amp;x; &amp;#; &amp;#x;\n&amp;#87654321;\n&amp;#abcdef0;\n&amp;ThisIsNotDefined; &amp;hi?;</p>"));
    }

    // Although HTML5 does accept some entity references
    // without a trailing semicolon (such as `&copy`), these are not
    // recognized here, because it makes the grammar too ambiguous:
    [Fact]
    public void InlinesEntityAndNumericCharacterReferences_Example315()
    {
      // Example 315
      // Section: Inlines / Entity and numeric character references
      //
      // The following Markdown:
      //   &copy
      //
      // Should be rendered as:
      //   <p>&amp;copy</p>

      Parser.Parse("&copy").Is(Parser.Prettify("<p>&amp;copy</p>"));

      Parser.DoubleParse("&copy").Is(Parser.Prettify("<p>&amp;copy</p>"));
    }

    // Strings that are not on the list of HTML5 named entities are not
    // recognized as entity references either:
    [Fact]
    public void InlinesEntityAndNumericCharacterReferences_Example316()
    {
      // Example 316
      // Section: Inlines / Entity and numeric character references
      //
      // The following Markdown:
      //   &MadeUpEntity;
      //
      // Should be rendered as:
      //   <p>&amp;MadeUpEntity;</p>

      Parser.Parse("&MadeUpEntity;").Is(Parser.Prettify("<p>&amp;MadeUpEntity;</p>"));

      Parser.DoubleParse("&MadeUpEntity;").Is(Parser.Prettify("<p>&amp;MadeUpEntity;</p>"));
    }

    // Entity and numeric character references are recognized in any
    // context besides code spans or code blocks, including
    // URLs, [link titles], and [fenced code block][] [info strings]:
    [Fact]
    public void InlinesEntityAndNumericCharacterReferences_Example317()
    {
      // Example 317
      // Section: Inlines / Entity and numeric character references
      //
      // The following Markdown:
      //   <a href="&ouml;&ouml;.html">
      //
      // Should be rendered as:
      //   <a href="&ouml;&ouml;.html">

      Parser.Parse("<a href=\"&ouml;&ouml;.html\">").Is(Parser.Prettify("<a href=\"&ouml;&ouml;.html\">"));

      Parser.DoubleParse("<a href=\"&ouml;&ouml;.html\">").Is(Parser.Prettify("<a href=\"&ouml;&ouml;.html\">"));
    }

    [Fact]
    public void InlinesEntityAndNumericCharacterReferences_Example318()
    {
      // Example 318
      // Section: Inlines / Entity and numeric character references
      //
      // The following Markdown:
      //   [foo](/f&ouml;&ouml; "f&ouml;&ouml;")
      //
      // Should be rendered as:
      //   <p><a href="/f%C3%B6%C3%B6" title="föö">foo</a></p>

      Parser.Parse("[foo](/f&ouml;&ouml; \"f&ouml;&ouml;\")").Is(Parser.Prettify("<p><a href=\"/f%C3%B6%C3%B6\" title=\"föö\">foo</a></p>"));

      Parser.DoubleParse("[foo](/f&ouml;&ouml; \"f&ouml;&ouml;\")").Is(Parser.Prettify("<p><a href=\"/f%C3%B6%C3%B6\" title=\"föö\">foo</a></p>"));
    }

    [Fact]
    public void InlinesEntityAndNumericCharacterReferences_Example319()
    {
      // Example 319
      // Section: Inlines / Entity and numeric character references
      //
      // The following Markdown:
      //   [foo]
      //  
      //   [foo]: /f&ouml;&ouml; "f&ouml;&ouml;"
      //
      // Should be rendered as:
      //   <p><a href="/f%C3%B6%C3%B6" title="föö">foo</a></p>

      Parser.Parse("[foo]\n\n[foo]: /f&ouml;&ouml; \"f&ouml;&ouml;\"").Is(Parser.Prettify("<p><a href=\"/f%C3%B6%C3%B6\" title=\"föö\">foo</a></p>"));

      Parser.DoubleParse("[foo]\n\n[foo]: /f&ouml;&ouml; \"f&ouml;&ouml;\"").Is(Parser.Prettify("<p><a href=\"/f%C3%B6%C3%B6\" title=\"föö\">foo</a></p>"));
    }

    [Fact]
    public void InlinesEntityAndNumericCharacterReferences_Example320()
    {
      // Example 320
      // Section: Inlines / Entity and numeric character references
      //
      // The following Markdown:
      //   ``` f&ouml;&ouml;
      //   foo
      //   ```
      //
      // Should be rendered as:
      //   <pre><code class="language-föö">foo
      //   </code></pre>

      Parser.Parse("``` f&ouml;&ouml;\nfoo\n```").Is(Parser.Prettify("<pre><code class=\"language-föö\">foo\n</code></pre>"));

      Parser.DoubleParse("``` f&ouml;&ouml;\nfoo\n```").Is(Parser.Prettify("<pre><code class=\"language-föö\">foo\n</code></pre>"));
    }

    // Entity and numeric character references are treated as literal
    // text in code spans and code blocks:
    [Fact]
    public void InlinesEntityAndNumericCharacterReferences_Example321()
    {
      // Example 321
      // Section: Inlines / Entity and numeric character references
      //
      // The following Markdown:
      //   `f&ouml;&ouml;`
      //
      // Should be rendered as:
      //   <p><code>f&amp;ouml;&amp;ouml;</code></p>

      Parser.Parse("`f&ouml;&ouml;`").Is(Parser.Prettify("<p><code>f&amp;ouml;&amp;ouml;</code></p>"));

      Parser.DoubleParse("`f&ouml;&ouml;`").Is(Parser.Prettify("<p><code>f&amp;ouml;&amp;ouml;</code></p>"));
    }

    [Fact]
    public void InlinesEntityAndNumericCharacterReferences_Example322()
    {
      // Example 322
      // Section: Inlines / Entity and numeric character references
      //
      // The following Markdown:
      //       f&ouml;f&ouml;
      //
      // Should be rendered as:
      //   <pre><code>f&amp;ouml;f&amp;ouml;
      //   </code></pre>

      Parser.Parse("    f&ouml;f&ouml;").Is(Parser.Prettify("<pre><code>f&amp;ouml;f&amp;ouml;\n</code></pre>"));

      Parser.DoubleParse("    f&ouml;f&ouml;").Is(Parser.Prettify("<pre><code>f&amp;ouml;f&amp;ouml;\n</code></pre>"));
    }

    // Entity and numeric character references cannot be used
    // in place of symbols indicating structure in CommonMark
    // documents.
    [Fact]
    public void InlinesEntityAndNumericCharacterReferences_Example323()
    {
      // Example 323
      // Section: Inlines / Entity and numeric character references
      //
      // The following Markdown:
      //   &#42;foo&#42;
      //   *foo*
      //
      // Should be rendered as:
      //   <p>*foo*
      //   <em>foo</em></p>

      Parser.Parse("&#42;foo&#42;\n*foo*").Is(Parser.Prettify("<p>*foo*\n<em>foo</em></p>"));

      Parser.DoubleParse("&#42;foo&#42;\n*foo*").Is(Parser.Prettify("<p>*foo*\n<em>foo</em></p>"));
    }

    [Fact]
    public void InlinesEntityAndNumericCharacterReferences_Example324()
    {
      // Example 324
      // Section: Inlines / Entity and numeric character references
      //
      // The following Markdown:
      //   &#42; foo
      //  
      //   * foo
      //
      // Should be rendered as:
      //   <p>* foo</p>
      //   <ul>
      //   <li>foo</li>
      //   </ul>

      Parser.Parse("&#42; foo\n\n* foo").Is(Parser.Prettify("<p>* foo</p>\n<ul>\n<li>foo</li>\n</ul>"));

      Parser.DoubleParse("&#42; foo\n\n* foo").Is(Parser.Prettify("<p>* foo</p>\n<ul>\n<li>foo</li>\n</ul>"));
    }

    [Fact]
    public void InlinesEntityAndNumericCharacterReferences_Example325()
    {
      // Example 325
      // Section: Inlines / Entity and numeric character references
      //
      // The following Markdown:
      //   foo&#10;&#10;bar
      //
      // Should be rendered as:
      //   <p>foo
      //  
      //   bar</p>

      Parser.Parse("foo&#10;&#10;bar").Is(Parser.Prettify("<p>foo\n\nbar</p>"));

      Parser.DoubleParse("foo&#10;&#10;bar").Is(Parser.Prettify("<p>foo\n\nbar</p>"));
    }

    [Fact]
    public void InlinesEntityAndNumericCharacterReferences_Example326()
    {
      // Example 326
      // Section: Inlines / Entity and numeric character references
      //
      // The following Markdown:
      //   &#9;foo
      //
      // Should be rendered as:
      //   <p>→foo</p>

      Parser.Parse("&#9;foo").Is(Parser.Prettify("<p>\tfoo</p>"));

      Parser.DoubleParse("&#9;foo").Is(Parser.Prettify("<p>\tfoo</p>"));
    }

    [Fact]
    public void InlinesEntityAndNumericCharacterReferences_Example327()
    {
      // Example 327
      // Section: Inlines / Entity and numeric character references
      //
      // The following Markdown:
      //   [a](url &quot;tit&quot;)
      //
      // Should be rendered as:
      //   <p>[a](url &quot;tit&quot;)</p>

      Parser.Parse("[a](url &quot;tit&quot;)").Is(Parser.Prettify("<p>[a](url &quot;tit&quot;)</p>"));

      Parser.DoubleParse("[a](url &quot;tit&quot;)").Is(Parser.Prettify("<p>[a](url &quot;tit&quot;)</p>"));
    }
  }

  public class TestInlinesCodeSpans
  {
    // ## Code spans
    //
    // A [backtick string](@)
    // is a string of one or more backtick characters (`` ` ``) that is neither
    // preceded nor followed by a backtick.
    //
    // A [code span](@) begins with a backtick string and ends with
    // a backtick string of equal length.  The contents of the code span are
    // the characters between the two backtick strings, normalized in the
    // following ways:
    //
    // - First, [line endings] are converted to [spaces].
    // - If the resulting string both begins *and* ends with a [space]
    //   character, but does not consist entirely of [space]
    //   characters, a single [space] character is removed from the
    //   front and back.  This allows you to include code that begins
    //   or ends with backtick characters, which must be separated by
    //   whitespace from the opening or closing backtick strings.
    //
    // This is a simple code span:
    [Fact]
    public void InlinesCodeSpans_Example328()
    {
      // Example 328
      // Section: Inlines / Code spans
      //
      // The following Markdown:
      //   `foo`
      //
      // Should be rendered as:
      //   <p><code>foo</code></p>

      Parser.Parse("`foo`").Is(Parser.Prettify("<p><code>foo</code></p>"));

      Parser.DoubleParse("`foo`").Is(Parser.Prettify("<p><code>foo</code></p>"));
    }

    // Here two backticks are used, because the code contains a backtick.
    // This example also illustrates stripping of a single leading and
    // trailing space:
    [Fact]
    public void InlinesCodeSpans_Example329()
    {
      // Example 329
      // Section: Inlines / Code spans
      //
      // The following Markdown:
      //   `` foo ` bar ``
      //
      // Should be rendered as:
      //   <p><code>foo ` bar</code></p>

      Parser.Parse("`` foo ` bar ``").Is(Parser.Prettify("<p><code>foo ` bar</code></p>"));

      Parser.DoubleParse("`` foo ` bar ``").Is(Parser.Prettify("<p><code>foo ` bar</code></p>"));
    }

    // This example shows the motivation for stripping leading and trailing
    // spaces:
    [Fact]
    public void InlinesCodeSpans_Example330()
    {
      // Example 330
      // Section: Inlines / Code spans
      //
      // The following Markdown:
      //   ` `` `
      //
      // Should be rendered as:
      //   <p><code>``</code></p>

      Parser.Parse("` `` `").Is(Parser.Prettify("<p><code>``</code></p>"));

      Parser.DoubleParse("` `` `").Is(Parser.Prettify("<p><code>``</code></p>"));
    }

    // Note that only *one* space is stripped:
    [Fact]
    public void InlinesCodeSpans_Example331()
    {
      // Example 331
      // Section: Inlines / Code spans
      //
      // The following Markdown:
      //   `  ``  `
      //
      // Should be rendered as:
      //   <p><code> `` </code></p>

      Parser.Parse("`  ``  `").Is(Parser.Prettify("<p><code> `` </code></p>"));

      Parser.DoubleParse("`  ``  `").Is(Parser.Prettify("<p><code> `` </code></p>"));
    }

    // The stripping only happens if the space is on both
    // sides of the string:
    [Fact]
    public void InlinesCodeSpans_Example332()
    {
      // Example 332
      // Section: Inlines / Code spans
      //
      // The following Markdown:
      //   ` a`
      //
      // Should be rendered as:
      //   <p><code> a</code></p>

      Parser.Parse("` a`").Is(Parser.Prettify("<p><code> a</code></p>"));

      Parser.DoubleParse("` a`").Is(Parser.Prettify("<p><code> a</code></p>"));
    }

    // Only [spaces], and not [unicode whitespace] in general, are
    // stripped in this way:
    [Fact]
    public void InlinesCodeSpans_Example333()
    {
      // Example 333
      // Section: Inlines / Code spans
      //
      // The following Markdown:
      //   ` b `
      //
      // Should be rendered as:
      //   <p><code> b </code></p>

      Parser.Parse("` b `").Is(Parser.Prettify("<p><code> b </code></p>"));

      Parser.DoubleParse("` b `").Is(Parser.Prettify("<p><code> b </code></p>"));
    }

    // No stripping occurs if the code span contains only spaces:
    [Fact]
    public void InlinesCodeSpans_Example334()
    {
      // Example 334
      // Section: Inlines / Code spans
      //
      // The following Markdown:
      //   ` `
      //   `  `
      //
      // Should be rendered as:
      //   <p><code> </code>
      //   <code>  </code></p>

      Parser.Parse("` `\n`  `").Is(Parser.Prettify("<p><code> </code>\n<code>  </code></p>"));

      Parser.DoubleParse("` `\n`  `").Is(Parser.Prettify("<p><code> </code>\n<code>  </code></p>"));
    }

    // [Line endings] are treated like spaces:
    [Fact]
    public void InlinesCodeSpans_Example335()
    {
      // Example 335
      // Section: Inlines / Code spans
      //
      // The following Markdown:
      //   ``
      //   foo
      //   bar  
      //   baz
      //   ``
      //
      // Should be rendered as:
      //   <p><code>foo bar   baz</code></p>

      Parser.Parse("``\nfoo\nbar  \nbaz\n``").Is(Parser.Prettify("<p><code>foo bar   baz</code></p>"));

      Parser.DoubleParse("``\nfoo\nbar  \nbaz\n``").Is(Parser.Prettify("<p><code>foo bar   baz</code></p>"));
    }

    [Fact]
    public void InlinesCodeSpans_Example336()
    {
      // Example 336
      // Section: Inlines / Code spans
      //
      // The following Markdown:
      //   ``
      //   foo 
      //   ``
      //
      // Should be rendered as:
      //   <p><code>foo </code></p>

      Parser.Parse("``\nfoo \n``").Is(Parser.Prettify("<p><code>foo </code></p>"));

      Parser.DoubleParse("``\nfoo \n``").Is(Parser.Prettify("<p><code>foo </code></p>"));
    }

    // Interior spaces are not collapsed:
    [Fact]
    public void InlinesCodeSpans_Example337()
    {
      // Example 337
      // Section: Inlines / Code spans
      //
      // The following Markdown:
      //   `foo   bar 
      //   baz`
      //
      // Should be rendered as:
      //   <p><code>foo   bar  baz</code></p>

      Parser.Parse("`foo   bar \nbaz`").Is(Parser.Prettify("<p><code>foo   bar  baz</code></p>"));

      Parser.DoubleParse("`foo   bar \nbaz`").Is(Parser.Prettify("<p><code>foo   bar  baz</code></p>"));
    }

    // Note that browsers will typically collapse consecutive spaces
    // when rendering `<code>` elements, so it is recommended that
    // the following CSS be used:
    //
    //     code{white-space: pre-wrap;}
    //
    //
    // Note that backslash escapes do not work in code spans. All backslashes
    // are treated literally:
    [Fact]
    public void InlinesCodeSpans_Example338()
    {
      // Example 338
      // Section: Inlines / Code spans
      //
      // The following Markdown:
      //   `foo\`bar`
      //
      // Should be rendered as:
      //   <p><code>foo\</code>bar`</p>

      Parser.Parse("`foo\\`bar`").Is(Parser.Prettify("<p><code>foo\\</code>bar`</p>"));

      Parser.DoubleParse("`foo\\`bar`").Is(Parser.Prettify("<p><code>foo\\</code>bar`</p>"));
    }

    // Backslash escapes are never needed, because one can always choose a
    // string of *n* backtick characters as delimiters, where the code does
    // not contain any strings of exactly *n* backtick characters.
    [Fact]
    public void InlinesCodeSpans_Example339()
    {
      // Example 339
      // Section: Inlines / Code spans
      //
      // The following Markdown:
      //   ``foo`bar``
      //
      // Should be rendered as:
      //   <p><code>foo`bar</code></p>

      Parser.Parse("``foo`bar``").Is(Parser.Prettify("<p><code>foo`bar</code></p>"));

      Parser.DoubleParse("``foo`bar``").Is(Parser.Prettify("<p><code>foo`bar</code></p>"));
    }

    [Fact]
    public void InlinesCodeSpans_Example340()
    {
      // Example 340
      // Section: Inlines / Code spans
      //
      // The following Markdown:
      //   ` foo `` bar `
      //
      // Should be rendered as:
      //   <p><code>foo `` bar</code></p>

      Parser.Parse("` foo `` bar `").Is(Parser.Prettify("<p><code>foo `` bar</code></p>"));

      Parser.DoubleParse("` foo `` bar `").Is(Parser.Prettify("<p><code>foo `` bar</code></p>"));
    }

    // Code span backticks have higher precedence than any other inline
    // constructs except HTML tags and autolinks.  Thus, for example, this is
    // not parsed as emphasized text, since the second `*` is part of a code
    // span:
    [Fact]
    public void InlinesCodeSpans_Example341()
    {
      // Example 341
      // Section: Inlines / Code spans
      //
      // The following Markdown:
      //   *foo`*`
      //
      // Should be rendered as:
      //   <p>*foo<code>*</code></p>

      Parser.Parse("*foo`*`").Is(Parser.Prettify("<p>*foo<code>*</code></p>"));

      Parser.DoubleParse("*foo`*`").Is(Parser.Prettify("<p>*foo<code>*</code></p>"));
    }

    // And this is not parsed as a link:
    [Fact]
    public void InlinesCodeSpans_Example342()
    {
      // Example 342
      // Section: Inlines / Code spans
      //
      // The following Markdown:
      //   [not a `link](/foo`)
      //
      // Should be rendered as:
      //   <p>[not a <code>link](/foo</code>)</p>

      Parser.Parse("[not a `link](/foo`)").Is(Parser.Prettify("<p>[not a <code>link](/foo</code>)</p>"));

      Parser.DoubleParse("[not a `link](/foo`)").Is(Parser.Prettify("<p>[not a <code>link](/foo</code>)</p>"));
    }

    // Code spans, HTML tags, and autolinks have the same precedence.
    // Thus, this is code:
    [Fact]
    public void InlinesCodeSpans_Example343()
    {
      // Example 343
      // Section: Inlines / Code spans
      //
      // The following Markdown:
      //   `<a href="`">`
      //
      // Should be rendered as:
      //   <p><code>&lt;a href=&quot;</code>&quot;&gt;`</p>

      Parser.Parse("`<a href=\"`\">`").Is(Parser.Prettify("<p><code>&lt;a href=&quot;</code>&quot;&gt;`</p>"));

      Parser.DoubleParse("`<a href=\"`\">`").Is(Parser.Prettify("<p><code>&lt;a href=&quot;</code>&quot;&gt;`</p>"));
    }

    // But this is an HTML tag:
    [Fact]
    public void InlinesCodeSpans_Example344()
    {
      // Example 344
      // Section: Inlines / Code spans
      //
      // The following Markdown:
      //   <a href="`">`
      //
      // Should be rendered as:
      //   <p><a href="`">`</p>

      Parser.Parse("<a href=\"`\">`").Is(Parser.Prettify("<p><a href=\"`\">`</p>"));

      Parser.DoubleParse("<a href=\"`\">`").Is(Parser.Prettify("<p><a href=\"`\">`</p>"));
    }

    // And this is code:
    [Fact]
    public void InlinesCodeSpans_Example345()
    {
      // Example 345
      // Section: Inlines / Code spans
      //
      // The following Markdown:
      //   `<http://foo.bar.`baz>`
      //
      // Should be rendered as:
      //   <p><code>&lt;http://foo.bar.</code>baz&gt;`</p>

      Parser.Parse("`<http://foo.bar.`baz>`").Is(Parser.Prettify("<p><code>&lt;http://foo.bar.</code>baz&gt;`</p>"));

      Parser.DoubleParse("`<http://foo.bar.`baz>`").Is(Parser.Prettify("<p><code>&lt;http://foo.bar.</code>baz&gt;`</p>"));
    }

    // But this is an autolink:
    [Fact]
    public void InlinesCodeSpans_Example346()
    {
      // Example 346
      // Section: Inlines / Code spans
      //
      // The following Markdown:
      //   <http://foo.bar.`baz>`
      //
      // Should be rendered as:
      //   <p><a href="http://foo.bar.%60baz">http://foo.bar.`baz</a>`</p>

      Parser.Parse("<http://foo.bar.`baz>`").Is(Parser.Prettify("<p><a href=\"http://foo.bar.%60baz\">http://foo.bar.`baz</a>`</p>"));

      Parser.DoubleParse("<http://foo.bar.`baz>`").Is(Parser.Prettify("<p><a href=\"http://foo.bar.%60baz\">http://foo.bar.`baz</a>`</p>"));
    }

    // When a backtick string is not closed by a matching backtick string,
    // we just have literal backticks:
    [Fact]
    public void InlinesCodeSpans_Example347()
    {
      // Example 347
      // Section: Inlines / Code spans
      //
      // The following Markdown:
      //   ```foo``
      //
      // Should be rendered as:
      //   <p>```foo``</p>

      Parser.Parse("```foo``").Is(Parser.Prettify("<p>```foo``</p>"));

      Parser.DoubleParse("```foo``").Is(Parser.Prettify("<p>```foo``</p>"));
    }

    [Fact]
    public void InlinesCodeSpans_Example348()
    {
      // Example 348
      // Section: Inlines / Code spans
      //
      // The following Markdown:
      //   `foo
      //
      // Should be rendered as:
      //   <p>`foo</p>

      Parser.Parse("`foo").Is(Parser.Prettify("<p>`foo</p>"));

      Parser.DoubleParse("`foo").Is(Parser.Prettify("<p>`foo</p>"));
    }

    // The following case also illustrates the need for opening and
    // closing backtick strings to be equal in length:
    [Fact]
    public void InlinesCodeSpans_Example349()
    {
      // Example 349
      // Section: Inlines / Code spans
      //
      // The following Markdown:
      //   `foo``bar``
      //
      // Should be rendered as:
      //   <p>`foo<code>bar</code></p>

      Parser.Parse("`foo``bar``").Is(Parser.Prettify("<p>`foo<code>bar</code></p>"));

      Parser.DoubleParse("`foo``bar``").Is(Parser.Prettify("<p>`foo<code>bar</code></p>"));
    }
  }

  public class TestInlinesEmphasisAndStrongEmphasis
  {
    // ## Emphasis and strong emphasis
    //
    // John Gruber's original [Markdown syntax
    // description](http://daringfireball.net/projects/markdown/syntax#em) says:
    //
    // > Markdown treats asterisks (`*`) and underscores (`_`) as indicators of
    // > emphasis. Text wrapped with one `*` or `_` will be wrapped with an HTML
    // > `<em>` tag; double `*`'s or `_`'s will be wrapped with an HTML `<strong>`
    // > tag.
    //
    // This is enough for most users, but these rules leave much undecided,
    // especially when it comes to nested emphasis.  The original
    // `Markdown.pl` test suite makes it clear that triple `***` and
    // `___` delimiters can be used for strong emphasis, and most
    // implementations have also allowed the following patterns:
    //
    // ``` markdown
    // ***strong emph***
    // ***strong** in emph*
    // ***emph* in strong**
    // **in strong *emph***
    // *in emph **strong***
    // ```
    //
    // The following patterns are less widely supported, but the intent
    // is clear and they are useful (especially in contexts like bibliography
    // entries):
    //
    // ``` markdown
    // *emph *with emph* in it*
    // **strong **with strong** in it**
    // ```
    //
    // Many implementations have also restricted intraword emphasis to
    // the `*` forms, to avoid unwanted emphasis in words containing
    // internal underscores.  (It is best practice to put these in code
    // spans, but users often do not.)
    //
    // ``` markdown
    // internal emphasis: foo*bar*baz
    // no emphasis: foo_bar_baz
    // ```
    //
    // The rules given below capture all of these patterns, while allowing
    // for efficient parsing strategies that do not backtrack.
    //
    // First, some definitions.  A [delimiter run](@) is either
    // a sequence of one or more `*` characters that is not preceded or
    // followed by a non-backslash-escaped `*` character, or a sequence
    // of one or more `_` characters that is not preceded or followed by
    // a non-backslash-escaped `_` character.
    //
    // A [left-flanking delimiter run](@) is
    // a [delimiter run] that is (1) not followed by [Unicode whitespace],
    // and either (2a) not followed by a [punctuation character], or
    // (2b) followed by a [punctuation character] and
    // preceded by [Unicode whitespace] or a [punctuation character].
    // For purposes of this definition, the beginning and the end of
    // the line count as Unicode whitespace.
    //
    // A [right-flanking delimiter run](@) is
    // a [delimiter run] that is (1) not preceded by [Unicode whitespace],
    // and either (2a) not preceded by a [punctuation character], or
    // (2b) preceded by a [punctuation character] and
    // followed by [Unicode whitespace] or a [punctuation character].
    // For purposes of this definition, the beginning and the end of
    // the line count as Unicode whitespace.
    //
    // Here are some examples of delimiter runs.
    //
    //   - left-flanking but not right-flanking:
    //
    //     ```
    //     ***abc
    //       _abc
    //     **"abc"
    //      _"abc"
    //     ```
    //
    //   - right-flanking but not left-flanking:
    //
    //     ```
    //      abc***
    //      abc_
    //     "abc"**
    //     "abc"_
    //     ```
    //
    //   - Both left and right-flanking:
    //
    //     ```
    //      abc***def
    //     "abc"_"def"
    //     ```
    //
    //   - Neither left nor right-flanking:
    //
    //     ```
    //     abc *** def
    //     a _ b
    //     ```
    //
    // (The idea of distinguishing left-flanking and right-flanking
    // delimiter runs based on the character before and the character
    // after comes from Roopesh Chander's
    // [vfmd](http://www.vfmd.org/vfmd-spec/specification/#procedure-for-identifying-emphasis-tags).
    // vfmd uses the terminology "emphasis indicator string" instead of "delimiter
    // run," and its rules for distinguishing left- and right-flanking runs
    // are a bit more complex than the ones given here.)
    //
    // The following rules define emphasis and strong emphasis:
    //
    // 1.  A single `*` character [can open emphasis](@)
    //     iff (if and only if) it is part of a [left-flanking delimiter run].
    //
    // 2.  A single `_` character [can open emphasis] iff
    //     it is part of a [left-flanking delimiter run]
    //     and either (a) not part of a [right-flanking delimiter run]
    //     or (b) part of a [right-flanking delimiter run]
    //     preceded by punctuation.
    //
    // 3.  A single `*` character [can close emphasis](@)
    //     iff it is part of a [right-flanking delimiter run].
    //
    // 4.  A single `_` character [can close emphasis] iff
    //     it is part of a [right-flanking delimiter run]
    //     and either (a) not part of a [left-flanking delimiter run]
    //     or (b) part of a [left-flanking delimiter run]
    //     followed by punctuation.
    //
    // 5.  A double `**` [can open strong emphasis](@)
    //     iff it is part of a [left-flanking delimiter run].
    //
    // 6.  A double `__` [can open strong emphasis] iff
    //     it is part of a [left-flanking delimiter run]
    //     and either (a) not part of a [right-flanking delimiter run]
    //     or (b) part of a [right-flanking delimiter run]
    //     preceded by punctuation.
    //
    // 7.  A double `**` [can close strong emphasis](@)
    //     iff it is part of a [right-flanking delimiter run].
    //
    // 8.  A double `__` [can close strong emphasis] iff
    //     it is part of a [right-flanking delimiter run]
    //     and either (a) not part of a [left-flanking delimiter run]
    //     or (b) part of a [left-flanking delimiter run]
    //     followed by punctuation.
    //
    // 9.  Emphasis begins with a delimiter that [can open emphasis] and ends
    //     with a delimiter that [can close emphasis], and that uses the same
    //     character (`_` or `*`) as the opening delimiter.  The
    //     opening and closing delimiters must belong to separate
    //     [delimiter runs].  If one of the delimiters can both
    //     open and close emphasis, then the sum of the lengths of the
    //     delimiter runs containing the opening and closing delimiters
    //     must not be a multiple of 3 unless both lengths are
    //     multiples of 3.
    //
    // 10. Strong emphasis begins with a delimiter that
    //     [can open strong emphasis] and ends with a delimiter that
    //     [can close strong emphasis], and that uses the same character
    //     (`_` or `*`) as the opening delimiter.  The
    //     opening and closing delimiters must belong to separate
    //     [delimiter runs].  If one of the delimiters can both open
    //     and close strong emphasis, then the sum of the lengths of
    //     the delimiter runs containing the opening and closing
    //     delimiters must not be a multiple of 3 unless both lengths
    //     are multiples of 3.
    //
    // 11. A literal `*` character cannot occur at the beginning or end of
    //     `*`-delimited emphasis or `**`-delimited strong emphasis, unless it
    //     is backslash-escaped.
    //
    // 12. A literal `_` character cannot occur at the beginning or end of
    //     `_`-delimited emphasis or `__`-delimited strong emphasis, unless it
    //     is backslash-escaped.
    //
    // Where rules 1--12 above are compatible with multiple parsings,
    // the following principles resolve ambiguity:
    //
    // 13. The number of nestings should be minimized. Thus, for example,
    //     an interpretation `<strong>...</strong>` is always preferred to
    //     `<em><em>...</em></em>`.
    //
    // 14. An interpretation `<em><strong>...</strong></em>` is always
    //     preferred to `<strong><em>...</em></strong>`.
    //
    // 15. When two potential emphasis or strong emphasis spans overlap,
    //     so that the second begins before the first ends and ends after
    //     the first ends, the first takes precedence. Thus, for example,
    //     `*foo _bar* baz_` is parsed as `<em>foo _bar</em> baz_` rather
    //     than `*foo <em>bar* baz</em>`.
    //
    // 16. When there are two potential emphasis or strong emphasis spans
    //     with the same closing delimiter, the shorter one (the one that
    //     opens later) takes precedence. Thus, for example,
    //     `**foo **bar baz**` is parsed as `**foo <strong>bar baz</strong>`
    //     rather than `<strong>foo **bar baz</strong>`.
    //
    // 17. Inline code spans, links, images, and HTML tags group more tightly
    //     than emphasis.  So, when there is a choice between an interpretation
    //     that contains one of these elements and one that does not, the
    //     former always wins.  Thus, for example, `*[foo*](bar)` is
    //     parsed as `*<a href="bar">foo*</a>` rather than as
    //     `<em>[foo</em>](bar)`.
    //
    // These rules can be illustrated through a series of examples.
    //
    // Rule 1:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example350()
    {
      // Example 350
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   *foo bar*
      //
      // Should be rendered as:
      //   <p><em>foo bar</em></p>

      Parser.Parse("*foo bar*").Is(Parser.Prettify("<p><em>foo bar</em></p>"));

      Parser.DoubleParse("*foo bar*").Is(Parser.Prettify("<p><em>foo bar</em></p>"));
    }

    // This is not emphasis, because the opening `*` is followed by
    // whitespace, and hence not part of a [left-flanking delimiter run]:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example351()
    {
      // Example 351
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   a * foo bar*
      //
      // Should be rendered as:
      //   <p>a * foo bar*</p>

      Parser.Parse("a * foo bar*").Is(Parser.Prettify("<p>a * foo bar*</p>"));

      Parser.DoubleParse("a * foo bar*").Is(Parser.Prettify("<p>a * foo bar*</p>"));
    }

    // This is not emphasis, because the opening `*` is preceded
    // by an alphanumeric and followed by punctuation, and hence
    // not part of a [left-flanking delimiter run]:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example352()
    {
      // Example 352
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   a*"foo"*
      //
      // Should be rendered as:
      //   <p>a*&quot;foo&quot;*</p>

      Parser.Parse("a*\"foo\"*").Is(Parser.Prettify("<p>a*&quot;foo&quot;*</p>"));

      Parser.DoubleParse("a*\"foo\"*").Is(Parser.Prettify("<p>a*&quot;foo&quot;*</p>"));
    }

    // Unicode nonbreaking spaces count as whitespace, too:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example353()
    {
      // Example 353
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   * a *
      //
      // Should be rendered as:
      //   <p>* a *</p>

      Parser.Parse("* a *").Is(Parser.Prettify("<p>* a *</p>"));

      Parser.DoubleParse("* a *").Is(Parser.Prettify("<p>* a *</p>"));
    }

    // Intraword emphasis with `*` is permitted:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example354()
    {
      // Example 354
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   foo*bar*
      //
      // Should be rendered as:
      //   <p>foo<em>bar</em></p>

      Parser.Parse("foo*bar*").Is(Parser.Prettify("<p>foo<em>bar</em></p>"));

      Parser.DoubleParse("foo*bar*").Is(Parser.Prettify("<p>foo<em>bar</em></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example355()
    {
      // Example 355
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   5*6*78
      //
      // Should be rendered as:
      //   <p>5<em>6</em>78</p>

      Parser.Parse("5*6*78").Is(Parser.Prettify("<p>5<em>6</em>78</p>"));

      Parser.DoubleParse("5*6*78").Is(Parser.Prettify("<p>5<em>6</em>78</p>"));
    }

    // Rule 2:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example356()
    {
      // Example 356
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   _foo bar_
      //
      // Should be rendered as:
      //   <p><em>foo bar</em></p>

      Parser.Parse("_foo bar_").Is(Parser.Prettify("<p><em>foo bar</em></p>"));

      Parser.DoubleParse("_foo bar_").Is(Parser.Prettify("<p><em>foo bar</em></p>"));
    }

    // This is not emphasis, because the opening `_` is followed by
    // whitespace:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example357()
    {
      // Example 357
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   _ foo bar_
      //
      // Should be rendered as:
      //   <p>_ foo bar_</p>

      Parser.Parse("_ foo bar_").Is(Parser.Prettify("<p>_ foo bar_</p>"));

      Parser.DoubleParse("_ foo bar_").Is(Parser.Prettify("<p>_ foo bar_</p>"));
    }

    // This is not emphasis, because the opening `_` is preceded
    // by an alphanumeric and followed by punctuation:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example358()
    {
      // Example 358
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   a_"foo"_
      //
      // Should be rendered as:
      //   <p>a_&quot;foo&quot;_</p>

      Parser.Parse("a_\"foo\"_").Is(Parser.Prettify("<p>a_&quot;foo&quot;_</p>"));

      Parser.DoubleParse("a_\"foo\"_").Is(Parser.Prettify("<p>a_&quot;foo&quot;_</p>"));
    }

    // Emphasis with `_` is not allowed inside words:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example359()
    {
      // Example 359
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   foo_bar_
      //
      // Should be rendered as:
      //   <p>foo_bar_</p>

      Parser.Parse("foo_bar_").Is(Parser.Prettify("<p>foo_bar_</p>"));

      Parser.DoubleParse("foo_bar_").Is(Parser.Prettify("<p>foo_bar_</p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example360()
    {
      // Example 360
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   5_6_78
      //
      // Should be rendered as:
      //   <p>5_6_78</p>

      Parser.Parse("5_6_78").Is(Parser.Prettify("<p>5_6_78</p>"));

      Parser.DoubleParse("5_6_78").Is(Parser.Prettify("<p>5_6_78</p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example361()
    {
      // Example 361
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   пристаням_стремятся_
      //
      // Should be rendered as:
      //   <p>пристаням_стремятся_</p>

      Parser.Parse("пристаням_стремятся_").Is(Parser.Prettify("<p>пристаням_стремятся_</p>"));

      Parser.DoubleParse("пристаням_стремятся_").Is(Parser.Prettify("<p>пристаням_стремятся_</p>"));
    }

    // Here `_` does not generate emphasis, because the first delimiter run
    // is right-flanking and the second left-flanking:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example362()
    {
      // Example 362
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   aa_"bb"_cc
      //
      // Should be rendered as:
      //   <p>aa_&quot;bb&quot;_cc</p>

      Parser.Parse("aa_\"bb\"_cc").Is(Parser.Prettify("<p>aa_&quot;bb&quot;_cc</p>"));

      Parser.DoubleParse("aa_\"bb\"_cc").Is(Parser.Prettify("<p>aa_&quot;bb&quot;_cc</p>"));
    }

    // This is emphasis, even though the opening delimiter is
    // both left- and right-flanking, because it is preceded by
    // punctuation:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example363()
    {
      // Example 363
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   foo-_(bar)_
      //
      // Should be rendered as:
      //   <p>foo-<em>(bar)</em></p>

      Parser.Parse("foo-_(bar)_").Is(Parser.Prettify("<p>foo-<em>(bar)</em></p>"));

      Parser.DoubleParse("foo-_(bar)_").Is(Parser.Prettify("<p>foo-<em>(bar)</em></p>"));
    }

    // Rule 3:
    //
    // This is not emphasis, because the closing delimiter does
    // not match the opening delimiter:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example364()
    {
      // Example 364
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   _foo*
      //
      // Should be rendered as:
      //   <p>_foo*</p>

      Parser.Parse("_foo*").Is(Parser.Prettify("<p>_foo*</p>"));

      Parser.DoubleParse("_foo*").Is(Parser.Prettify("<p>_foo*</p>"));
    }

    // This is not emphasis, because the closing `*` is preceded by
    // whitespace:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example365()
    {
      // Example 365
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   *foo bar *
      //
      // Should be rendered as:
      //   <p>*foo bar *</p>

      Parser.Parse("*foo bar *").Is(Parser.Prettify("<p>*foo bar *</p>"));

      Parser.DoubleParse("*foo bar *").Is(Parser.Prettify("<p>*foo bar *</p>"));
    }

    // A newline also counts as whitespace:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example366()
    {
      // Example 366
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   *foo bar
      //   *
      //
      // Should be rendered as:
      //   <p>*foo bar
      //   *</p>

      Parser.Parse("*foo bar\n*").Is(Parser.Prettify("<p>*foo bar\n*</p>"));

      Parser.DoubleParse("*foo bar\n*").Is(Parser.Prettify("<p>*foo bar\n*</p>"));
    }

    // This is not emphasis, because the second `*` is
    // preceded by punctuation and followed by an alphanumeric
    // (hence it is not part of a [right-flanking delimiter run]:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example367()
    {
      // Example 367
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   *(*foo)
      //
      // Should be rendered as:
      //   <p>*(*foo)</p>

      Parser.Parse("*(*foo)").Is(Parser.Prettify("<p>*(*foo)</p>"));

      Parser.DoubleParse("*(*foo)").Is(Parser.Prettify("<p>*(*foo)</p>"));
    }

    // The point of this restriction is more easily appreciated
    // with this example:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example368()
    {
      // Example 368
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   *(*foo*)*
      //
      // Should be rendered as:
      //   <p><em>(<em>foo</em>)</em></p>

      Parser.Parse("*(*foo*)*").Is(Parser.Prettify("<p><em>(<em>foo</em>)</em></p>"));

      Parser.DoubleParse("*(*foo*)*").Is(Parser.Prettify("<p><em>(<em>foo</em>)</em></p>"));
    }

    // Intraword emphasis with `*` is allowed:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example369()
    {
      // Example 369
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   *foo*bar
      //
      // Should be rendered as:
      //   <p><em>foo</em>bar</p>

      Parser.Parse("*foo*bar").Is(Parser.Prettify("<p><em>foo</em>bar</p>"));

      Parser.DoubleParse("*foo*bar").Is(Parser.Prettify("<p><em>foo</em>bar</p>"));
    }

    // Rule 4:
    //
    // This is not emphasis, because the closing `_` is preceded by
    // whitespace:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example370()
    {
      // Example 370
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   _foo bar _
      //
      // Should be rendered as:
      //   <p>_foo bar _</p>

      Parser.Parse("_foo bar _").Is(Parser.Prettify("<p>_foo bar _</p>"));

      Parser.DoubleParse("_foo bar _").Is(Parser.Prettify("<p>_foo bar _</p>"));
    }

    // This is not emphasis, because the second `_` is
    // preceded by punctuation and followed by an alphanumeric:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example371()
    {
      // Example 371
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   _(_foo)
      //
      // Should be rendered as:
      //   <p>_(_foo)</p>

      Parser.Parse("_(_foo)").Is(Parser.Prettify("<p>_(_foo)</p>"));

      Parser.DoubleParse("_(_foo)").Is(Parser.Prettify("<p>_(_foo)</p>"));
    }

    // This is emphasis within emphasis:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example372()
    {
      // Example 372
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   _(_foo_)_
      //
      // Should be rendered as:
      //   <p><em>(<em>foo</em>)</em></p>

      Parser.Parse("_(_foo_)_").Is(Parser.Prettify("<p><em>(<em>foo</em>)</em></p>"));

      Parser.DoubleParse("_(_foo_)_").Is(Parser.Prettify("<p><em>(<em>foo</em>)</em></p>"));
    }

    // Intraword emphasis is disallowed for `_`:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example373()
    {
      // Example 373
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   _foo_bar
      //
      // Should be rendered as:
      //   <p>_foo_bar</p>

      Parser.Parse("_foo_bar").Is(Parser.Prettify("<p>_foo_bar</p>"));

      Parser.DoubleParse("_foo_bar").Is(Parser.Prettify("<p>_foo_bar</p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example374()
    {
      // Example 374
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   _пристаням_стремятся
      //
      // Should be rendered as:
      //   <p>_пристаням_стремятся</p>

      Parser.Parse("_пристаням_стремятся").Is(Parser.Prettify("<p>_пристаням_стремятся</p>"));

      Parser.DoubleParse("_пристаням_стремятся").Is(Parser.Prettify("<p>_пристаням_стремятся</p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example375()
    {
      // Example 375
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   _foo_bar_baz_
      //
      // Should be rendered as:
      //   <p><em>foo_bar_baz</em></p>

      Parser.Parse("_foo_bar_baz_").Is(Parser.Prettify("<p><em>foo_bar_baz</em></p>"));

      Parser.DoubleParse("_foo_bar_baz_").Is(Parser.Prettify("<p><em>foo_bar_baz</em></p>"));
    }

    // This is emphasis, even though the closing delimiter is
    // both left- and right-flanking, because it is followed by
    // punctuation:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example376()
    {
      // Example 376
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   _(bar)_.
      //
      // Should be rendered as:
      //   <p><em>(bar)</em>.</p>

      Parser.Parse("_(bar)_.").Is(Parser.Prettify("<p><em>(bar)</em>.</p>"));

      Parser.DoubleParse("_(bar)_.").Is(Parser.Prettify("<p><em>(bar)</em>.</p>"));
    }

    // Rule 5:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example377()
    {
      // Example 377
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   **foo bar**
      //
      // Should be rendered as:
      //   <p><strong>foo bar</strong></p>

      Parser.Parse("**foo bar**").Is(Parser.Prettify("<p><strong>foo bar</strong></p>"));

      Parser.DoubleParse("**foo bar**").Is(Parser.Prettify("<p><strong>foo bar</strong></p>"));
    }

    // This is not strong emphasis, because the opening delimiter is
    // followed by whitespace:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example378()
    {
      // Example 378
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   ** foo bar**
      //
      // Should be rendered as:
      //   <p>** foo bar**</p>

      Parser.Parse("** foo bar**").Is(Parser.Prettify("<p>** foo bar**</p>"));

      Parser.DoubleParse("** foo bar**").Is(Parser.Prettify("<p>** foo bar**</p>"));
    }

    // This is not strong emphasis, because the opening `**` is preceded
    // by an alphanumeric and followed by punctuation, and hence
    // not part of a [left-flanking delimiter run]:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example379()
    {
      // Example 379
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   a**"foo"**
      //
      // Should be rendered as:
      //   <p>a**&quot;foo&quot;**</p>

      Parser.Parse("a**\"foo\"**").Is(Parser.Prettify("<p>a**&quot;foo&quot;**</p>"));

      Parser.DoubleParse("a**\"foo\"**").Is(Parser.Prettify("<p>a**&quot;foo&quot;**</p>"));
    }

    // Intraword strong emphasis with `**` is permitted:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example380()
    {
      // Example 380
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   foo**bar**
      //
      // Should be rendered as:
      //   <p>foo<strong>bar</strong></p>

      Parser.Parse("foo**bar**").Is(Parser.Prettify("<p>foo<strong>bar</strong></p>"));

      Parser.DoubleParse("foo**bar**").Is(Parser.Prettify("<p>foo<strong>bar</strong></p>"));
    }

    // Rule 6:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example381()
    {
      // Example 381
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   __foo bar__
      //
      // Should be rendered as:
      //   <p><strong>foo bar</strong></p>

      Parser.Parse("__foo bar__").Is(Parser.Prettify("<p><strong>foo bar</strong></p>"));

      Parser.DoubleParse("__foo bar__").Is(Parser.Prettify("<p><strong>foo bar</strong></p>"));
    }

    // This is not strong emphasis, because the opening delimiter is
    // followed by whitespace:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example382()
    {
      // Example 382
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   __ foo bar__
      //
      // Should be rendered as:
      //   <p>__ foo bar__</p>

      Parser.Parse("__ foo bar__").Is(Parser.Prettify("<p>__ foo bar__</p>"));

      Parser.DoubleParse("__ foo bar__").Is(Parser.Prettify("<p>__ foo bar__</p>"));
    }

    // A newline counts as whitespace:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example383()
    {
      // Example 383
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   __
      //   foo bar__
      //
      // Should be rendered as:
      //   <p>__
      //   foo bar__</p>

      Parser.Parse("__\nfoo bar__").Is(Parser.Prettify("<p>__\nfoo bar__</p>"));

      Parser.DoubleParse("__\nfoo bar__").Is(Parser.Prettify("<p>__\nfoo bar__</p>"));
    }

    // This is not strong emphasis, because the opening `__` is preceded
    // by an alphanumeric and followed by punctuation:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example384()
    {
      // Example 384
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   a__"foo"__
      //
      // Should be rendered as:
      //   <p>a__&quot;foo&quot;__</p>

      Parser.Parse("a__\"foo\"__").Is(Parser.Prettify("<p>a__&quot;foo&quot;__</p>"));

      Parser.DoubleParse("a__\"foo\"__").Is(Parser.Prettify("<p>a__&quot;foo&quot;__</p>"));
    }

    // Intraword strong emphasis is forbidden with `__`:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example385()
    {
      // Example 385
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   foo__bar__
      //
      // Should be rendered as:
      //   <p>foo__bar__</p>

      Parser.Parse("foo__bar__").Is(Parser.Prettify("<p>foo__bar__</p>"));

      Parser.DoubleParse("foo__bar__").Is(Parser.Prettify("<p>foo__bar__</p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example386()
    {
      // Example 386
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   5__6__78
      //
      // Should be rendered as:
      //   <p>5__6__78</p>

      Parser.Parse("5__6__78").Is(Parser.Prettify("<p>5__6__78</p>"));

      Parser.DoubleParse("5__6__78").Is(Parser.Prettify("<p>5__6__78</p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example387()
    {
      // Example 387
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   пристаням__стремятся__
      //
      // Should be rendered as:
      //   <p>пристаням__стремятся__</p>

      Parser.Parse("пристаням__стремятся__").Is(Parser.Prettify("<p>пристаням__стремятся__</p>"));

      Parser.DoubleParse("пристаням__стремятся__").Is(Parser.Prettify("<p>пристаням__стремятся__</p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example388()
    {
      // Example 388
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   __foo, __bar__, baz__
      //
      // Should be rendered as:
      //   <p><strong>foo, <strong>bar</strong>, baz</strong></p>

      Parser.Parse("__foo, __bar__, baz__").Is(Parser.Prettify("<p><strong>foo, <strong>bar</strong>, baz</strong></p>"));

      Parser.DoubleParse("__foo, __bar__, baz__").Is(Parser.Prettify("<p><strong>foo, <strong>bar</strong>, baz</strong></p>"));
    }

    // This is strong emphasis, even though the opening delimiter is
    // both left- and right-flanking, because it is preceded by
    // punctuation:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example389()
    {
      // Example 389
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   foo-__(bar)__
      //
      // Should be rendered as:
      //   <p>foo-<strong>(bar)</strong></p>

      Parser.Parse("foo-__(bar)__").Is(Parser.Prettify("<p>foo-<strong>(bar)</strong></p>"));

      Parser.DoubleParse("foo-__(bar)__").Is(Parser.Prettify("<p>foo-<strong>(bar)</strong></p>"));
    }

    // Rule 7:
    //
    // This is not strong emphasis, because the closing delimiter is preceded
    // by whitespace:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example390()
    {
      // Example 390
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   **foo bar **
      //
      // Should be rendered as:
      //   <p>**foo bar **</p>

      Parser.Parse("**foo bar **").Is(Parser.Prettify("<p>**foo bar **</p>"));

      Parser.DoubleParse("**foo bar **").Is(Parser.Prettify("<p>**foo bar **</p>"));
    }

    // (Nor can it be interpreted as an emphasized `*foo bar *`, because of
    // Rule 11.)
    //
    // This is not strong emphasis, because the second `**` is
    // preceded by punctuation and followed by an alphanumeric:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example391()
    {
      // Example 391
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   **(**foo)
      //
      // Should be rendered as:
      //   <p>**(**foo)</p>

      Parser.Parse("**(**foo)").Is(Parser.Prettify("<p>**(**foo)</p>"));

      Parser.DoubleParse("**(**foo)").Is(Parser.Prettify("<p>**(**foo)</p>"));
    }

    // The point of this restriction is more easily appreciated
    // with these examples:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example392()
    {
      // Example 392
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   *(**foo**)*
      //
      // Should be rendered as:
      //   <p><em>(<strong>foo</strong>)</em></p>

      Parser.Parse("*(**foo**)*").Is(Parser.Prettify("<p><em>(<strong>foo</strong>)</em></p>"));

      Parser.DoubleParse("*(**foo**)*").Is(Parser.Prettify("<p><em>(<strong>foo</strong>)</em></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example393()
    {
      // Example 393
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   **Gomphocarpus (*Gomphocarpus physocarpus*, syn.
      //   *Asclepias physocarpa*)**
      //
      // Should be rendered as:
      //   <p><strong>Gomphocarpus (<em>Gomphocarpus physocarpus</em>, syn.
      //   <em>Asclepias physocarpa</em>)</strong></p>

      Parser.Parse("**Gomphocarpus (*Gomphocarpus physocarpus*, syn.\n*Asclepias physocarpa*)**").Is(Parser.Prettify("<p><strong>Gomphocarpus (<em>Gomphocarpus physocarpus</em>, syn.\n<em>Asclepias physocarpa</em>)</strong></p>"));

      Parser.DoubleParse("**Gomphocarpus (*Gomphocarpus physocarpus*, syn.\n*Asclepias physocarpa*)**").Is(Parser.Prettify("<p><strong>Gomphocarpus (<em>Gomphocarpus physocarpus</em>, syn.\n<em>Asclepias physocarpa</em>)</strong></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example394()
    {
      // Example 394
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   **foo "*bar*" foo**
      //
      // Should be rendered as:
      //   <p><strong>foo &quot;<em>bar</em>&quot; foo</strong></p>

      Parser.Parse("**foo \"*bar*\" foo**").Is(Parser.Prettify("<p><strong>foo &quot;<em>bar</em>&quot; foo</strong></p>"));

      Parser.DoubleParse("**foo \"*bar*\" foo**").Is(Parser.Prettify("<p><strong>foo &quot;<em>bar</em>&quot; foo</strong></p>"));
    }

    // Intraword emphasis:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example395()
    {
      // Example 395
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   **foo**bar
      //
      // Should be rendered as:
      //   <p><strong>foo</strong>bar</p>

      Parser.Parse("**foo**bar").Is(Parser.Prettify("<p><strong>foo</strong>bar</p>"));

      Parser.DoubleParse("**foo**bar").Is(Parser.Prettify("<p><strong>foo</strong>bar</p>"));
    }

    // Rule 8:
    //
    // This is not strong emphasis, because the closing delimiter is
    // preceded by whitespace:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example396()
    {
      // Example 396
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   __foo bar __
      //
      // Should be rendered as:
      //   <p>__foo bar __</p>

      Parser.Parse("__foo bar __").Is(Parser.Prettify("<p>__foo bar __</p>"));

      Parser.DoubleParse("__foo bar __").Is(Parser.Prettify("<p>__foo bar __</p>"));
    }

    // This is not strong emphasis, because the second `__` is
    // preceded by punctuation and followed by an alphanumeric:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example397()
    {
      // Example 397
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   __(__foo)
      //
      // Should be rendered as:
      //   <p>__(__foo)</p>

      Parser.Parse("__(__foo)").Is(Parser.Prettify("<p>__(__foo)</p>"));

      Parser.DoubleParse("__(__foo)").Is(Parser.Prettify("<p>__(__foo)</p>"));
    }

    // The point of this restriction is more easily appreciated
    // with this example:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example398()
    {
      // Example 398
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   _(__foo__)_
      //
      // Should be rendered as:
      //   <p><em>(<strong>foo</strong>)</em></p>

      Parser.Parse("_(__foo__)_").Is(Parser.Prettify("<p><em>(<strong>foo</strong>)</em></p>"));

      Parser.DoubleParse("_(__foo__)_").Is(Parser.Prettify("<p><em>(<strong>foo</strong>)</em></p>"));
    }

    // Intraword strong emphasis is forbidden with `__`:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example399()
    {
      // Example 399
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   __foo__bar
      //
      // Should be rendered as:
      //   <p>__foo__bar</p>

      Parser.Parse("__foo__bar").Is(Parser.Prettify("<p>__foo__bar</p>"));

      Parser.DoubleParse("__foo__bar").Is(Parser.Prettify("<p>__foo__bar</p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example400()
    {
      // Example 400
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   __пристаням__стремятся
      //
      // Should be rendered as:
      //   <p>__пристаням__стремятся</p>

      Parser.Parse("__пристаням__стремятся").Is(Parser.Prettify("<p>__пристаням__стремятся</p>"));

      Parser.DoubleParse("__пристаням__стремятся").Is(Parser.Prettify("<p>__пристаням__стремятся</p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example401()
    {
      // Example 401
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   __foo__bar__baz__
      //
      // Should be rendered as:
      //   <p><strong>foo__bar__baz</strong></p>

      Parser.Parse("__foo__bar__baz__").Is(Parser.Prettify("<p><strong>foo__bar__baz</strong></p>"));

      Parser.DoubleParse("__foo__bar__baz__").Is(Parser.Prettify("<p><strong>foo__bar__baz</strong></p>"));
    }

    // This is strong emphasis, even though the closing delimiter is
    // both left- and right-flanking, because it is followed by
    // punctuation:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example402()
    {
      // Example 402
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   __(bar)__.
      //
      // Should be rendered as:
      //   <p><strong>(bar)</strong>.</p>

      Parser.Parse("__(bar)__.").Is(Parser.Prettify("<p><strong>(bar)</strong>.</p>"));

      Parser.DoubleParse("__(bar)__.").Is(Parser.Prettify("<p><strong>(bar)</strong>.</p>"));
    }

    // Rule 9:
    //
    // Any nonempty sequence of inline elements can be the contents of an
    // emphasized span.
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example403()
    {
      // Example 403
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   *foo [bar](/url)*
      //
      // Should be rendered as:
      //   <p><em>foo <a href="/url">bar</a></em></p>

      Parser.Parse("*foo [bar](/url)*").Is(Parser.Prettify("<p><em>foo <a href=\"/url\">bar</a></em></p>"));

      Parser.DoubleParse("*foo [bar](/url)*").Is(Parser.Prettify("<p><em>foo <a href=\"/url\">bar</a></em></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example404()
    {
      // Example 404
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   *foo
      //   bar*
      //
      // Should be rendered as:
      //   <p><em>foo
      //   bar</em></p>

      Parser.Parse("*foo\nbar*").Is(Parser.Prettify("<p><em>foo\nbar</em></p>"));

      Parser.DoubleParse("*foo\nbar*").Is(Parser.Prettify("<p><em>foo\nbar</em></p>"));
    }

    // In particular, emphasis and strong emphasis can be nested
    // inside emphasis:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example405()
    {
      // Example 405
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   _foo __bar__ baz_
      //
      // Should be rendered as:
      //   <p><em>foo <strong>bar</strong> baz</em></p>

      Parser.Parse("_foo __bar__ baz_").Is(Parser.Prettify("<p><em>foo <strong>bar</strong> baz</em></p>"));

      Parser.DoubleParse("_foo __bar__ baz_").Is(Parser.Prettify("<p><em>foo <strong>bar</strong> baz</em></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example406()
    {
      // Example 406
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   _foo _bar_ baz_
      //
      // Should be rendered as:
      //   <p><em>foo <em>bar</em> baz</em></p>

      Parser.Parse("_foo _bar_ baz_").Is(Parser.Prettify("<p><em>foo <em>bar</em> baz</em></p>"));

      Parser.DoubleParse("_foo _bar_ baz_").Is(Parser.Prettify("<p><em>foo <em>bar</em> baz</em></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example407()
    {
      // Example 407
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   __foo_ bar_
      //
      // Should be rendered as:
      //   <p><em><em>foo</em> bar</em></p>

      Parser.Parse("__foo_ bar_").Is(Parser.Prettify("<p><em><em>foo</em> bar</em></p>"));

      Parser.DoubleParse("__foo_ bar_").Is(Parser.Prettify("<p><em><em>foo</em> bar</em></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example408()
    {
      // Example 408
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   *foo *bar**
      //
      // Should be rendered as:
      //   <p><em>foo <em>bar</em></em></p>

      Parser.Parse("*foo *bar**").Is(Parser.Prettify("<p><em>foo <em>bar</em></em></p>"));

      Parser.DoubleParse("*foo *bar**").Is(Parser.Prettify("<p><em>foo <em>bar</em></em></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example409()
    {
      // Example 409
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   *foo **bar** baz*
      //
      // Should be rendered as:
      //   <p><em>foo <strong>bar</strong> baz</em></p>

      Parser.Parse("*foo **bar** baz*").Is(Parser.Prettify("<p><em>foo <strong>bar</strong> baz</em></p>"));

      Parser.DoubleParse("*foo **bar** baz*").Is(Parser.Prettify("<p><em>foo <strong>bar</strong> baz</em></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example410()
    {
      // Example 410
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   *foo**bar**baz*
      //
      // Should be rendered as:
      //   <p><em>foo<strong>bar</strong>baz</em></p>

      Parser.Parse("*foo**bar**baz*").Is(Parser.Prettify("<p><em>foo<strong>bar</strong>baz</em></p>"));

      Parser.DoubleParse("*foo**bar**baz*").Is(Parser.Prettify("<p><em>foo<strong>bar</strong>baz</em></p>"));
    }

    // Note that in the preceding case, the interpretation
    //
    // ``` markdown
    // <p><em>foo</em><em>bar<em></em>baz</em></p>
    // ```
    //
    //
    // is precluded by the condition that a delimiter that
    // can both open and close (like the `*` after `foo`)
    // cannot form emphasis if the sum of the lengths of
    // the delimiter runs containing the opening and
    // closing delimiters is a multiple of 3 unless
    // both lengths are multiples of 3.
    //
    //
    // For the same reason, we don't get two consecutive
    // emphasis sections in this example:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example411()
    {
      // Example 411
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   *foo**bar*
      //
      // Should be rendered as:
      //   <p><em>foo**bar</em></p>

      Parser.Parse("*foo**bar*").Is(Parser.Prettify("<p><em>foo**bar</em></p>"));

      Parser.DoubleParse("*foo**bar*").Is(Parser.Prettify("<p><em>foo**bar</em></p>"));
    }

    // The same condition ensures that the following
    // cases are all strong emphasis nested inside
    // emphasis, even when the interior spaces are
    // omitted:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example412()
    {
      // Example 412
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   ***foo** bar*
      //
      // Should be rendered as:
      //   <p><em><strong>foo</strong> bar</em></p>

      Parser.Parse("***foo** bar*").Is(Parser.Prettify("<p><em><strong>foo</strong> bar</em></p>"));

      Parser.DoubleParse("***foo** bar*").Is(Parser.Prettify("<p><em><strong>foo</strong> bar</em></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example413()
    {
      // Example 413
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   *foo **bar***
      //
      // Should be rendered as:
      //   <p><em>foo <strong>bar</strong></em></p>

      Parser.Parse("*foo **bar***").Is(Parser.Prettify("<p><em>foo <strong>bar</strong></em></p>"));

      Parser.DoubleParse("*foo **bar***").Is(Parser.Prettify("<p><em>foo <strong>bar</strong></em></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example414()
    {
      // Example 414
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   *foo**bar***
      //
      // Should be rendered as:
      //   <p><em>foo<strong>bar</strong></em></p>

      Parser.Parse("*foo**bar***").Is(Parser.Prettify("<p><em>foo<strong>bar</strong></em></p>"));

      Parser.DoubleParse("*foo**bar***").Is(Parser.Prettify("<p><em>foo<strong>bar</strong></em></p>"));
    }

    // When the lengths of the interior closing and opening
    // delimiter runs are *both* multiples of 3, though,
    // they can match to create emphasis:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example415()
    {
      // Example 415
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   foo***bar***baz
      //
      // Should be rendered as:
      //   <p>foo<em><strong>bar</strong></em>baz</p>

      Parser.Parse("foo***bar***baz").Is(Parser.Prettify("<p>foo<em><strong>bar</strong></em>baz</p>"));

      Parser.DoubleParse("foo***bar***baz").Is(Parser.Prettify("<p>foo<em><strong>bar</strong></em>baz</p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example416()
    {
      // Example 416
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   foo******bar*********baz
      //
      // Should be rendered as:
      //   <p>foo<strong><strong><strong>bar</strong></strong></strong>***baz</p>

      Parser.Parse("foo******bar*********baz").Is(Parser.Prettify("<p>foo<strong><strong><strong>bar</strong></strong></strong>***baz</p>"));

      Parser.DoubleParse("foo******bar*********baz").Is(Parser.Prettify("<p>foo<strong><strong><strong>bar</strong></strong></strong>***baz</p>"));
    }

    // Indefinite levels of nesting are possible:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example417()
    {
      // Example 417
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   *foo **bar *baz* bim** bop*
      //
      // Should be rendered as:
      //   <p><em>foo <strong>bar <em>baz</em> bim</strong> bop</em></p>

      Parser.Parse("*foo **bar *baz* bim** bop*").Is(Parser.Prettify("<p><em>foo <strong>bar <em>baz</em> bim</strong> bop</em></p>"));

      Parser.DoubleParse("*foo **bar *baz* bim** bop*").Is(Parser.Prettify("<p><em>foo <strong>bar <em>baz</em> bim</strong> bop</em></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example418()
    {
      // Example 418
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   *foo [*bar*](/url)*
      //
      // Should be rendered as:
      //   <p><em>foo <a href="/url"><em>bar</em></a></em></p>

      Parser.Parse("*foo [*bar*](/url)*").Is(Parser.Prettify("<p><em>foo <a href=\"/url\"><em>bar</em></a></em></p>"));

      Parser.DoubleParse("*foo [*bar*](/url)*").Is(Parser.Prettify("<p><em>foo <a href=\"/url\"><em>bar</em></a></em></p>"));
    }

    // There can be no empty emphasis or strong emphasis:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example419()
    {
      // Example 419
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   ** is not an empty emphasis
      //
      // Should be rendered as:
      //   <p>** is not an empty emphasis</p>

      Parser.Parse("** is not an empty emphasis").Is(Parser.Prettify("<p>** is not an empty emphasis</p>"));

      Parser.DoubleParse("** is not an empty emphasis").Is(Parser.Prettify("<p>** is not an empty emphasis</p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example420()
    {
      // Example 420
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   **** is not an empty strong emphasis
      //
      // Should be rendered as:
      //   <p>**** is not an empty strong emphasis</p>

      Parser.Parse("**** is not an empty strong emphasis").Is(Parser.Prettify("<p>**** is not an empty strong emphasis</p>"));

      Parser.DoubleParse("**** is not an empty strong emphasis").Is(Parser.Prettify("<p>**** is not an empty strong emphasis</p>"));
    }

    // Rule 10:
    //
    // Any nonempty sequence of inline elements can be the contents of an
    // strongly emphasized span.
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example421()
    {
      // Example 421
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   **foo [bar](/url)**
      //
      // Should be rendered as:
      //   <p><strong>foo <a href="/url">bar</a></strong></p>

      Parser.Parse("**foo [bar](/url)**").Is(Parser.Prettify("<p><strong>foo <a href=\"/url\">bar</a></strong></p>"));

      Parser.DoubleParse("**foo [bar](/url)**").Is(Parser.Prettify("<p><strong>foo <a href=\"/url\">bar</a></strong></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example422()
    {
      // Example 422
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   **foo
      //   bar**
      //
      // Should be rendered as:
      //   <p><strong>foo
      //   bar</strong></p>

      Parser.Parse("**foo\nbar**").Is(Parser.Prettify("<p><strong>foo\nbar</strong></p>"));

      Parser.DoubleParse("**foo\nbar**").Is(Parser.Prettify("<p><strong>foo\nbar</strong></p>"));
    }

    // In particular, emphasis and strong emphasis can be nested
    // inside strong emphasis:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example423()
    {
      // Example 423
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   __foo _bar_ baz__
      //
      // Should be rendered as:
      //   <p><strong>foo <em>bar</em> baz</strong></p>

      Parser.Parse("__foo _bar_ baz__").Is(Parser.Prettify("<p><strong>foo <em>bar</em> baz</strong></p>"));

      Parser.DoubleParse("__foo _bar_ baz__").Is(Parser.Prettify("<p><strong>foo <em>bar</em> baz</strong></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example424()
    {
      // Example 424
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   __foo __bar__ baz__
      //
      // Should be rendered as:
      //   <p><strong>foo <strong>bar</strong> baz</strong></p>

      Parser.Parse("__foo __bar__ baz__").Is(Parser.Prettify("<p><strong>foo <strong>bar</strong> baz</strong></p>"));

      Parser.DoubleParse("__foo __bar__ baz__").Is(Parser.Prettify("<p><strong>foo <strong>bar</strong> baz</strong></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example425()
    {
      // Example 425
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   ____foo__ bar__
      //
      // Should be rendered as:
      //   <p><strong><strong>foo</strong> bar</strong></p>

      Parser.Parse("____foo__ bar__").Is(Parser.Prettify("<p><strong><strong>foo</strong> bar</strong></p>"));

      Parser.DoubleParse("____foo__ bar__").Is(Parser.Prettify("<p><strong><strong>foo</strong> bar</strong></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example426()
    {
      // Example 426
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   **foo **bar****
      //
      // Should be rendered as:
      //   <p><strong>foo <strong>bar</strong></strong></p>

      Parser.Parse("**foo **bar****").Is(Parser.Prettify("<p><strong>foo <strong>bar</strong></strong></p>"));

      Parser.DoubleParse("**foo **bar****").Is(Parser.Prettify("<p><strong>foo <strong>bar</strong></strong></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example427()
    {
      // Example 427
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   **foo *bar* baz**
      //
      // Should be rendered as:
      //   <p><strong>foo <em>bar</em> baz</strong></p>

      Parser.Parse("**foo *bar* baz**").Is(Parser.Prettify("<p><strong>foo <em>bar</em> baz</strong></p>"));

      Parser.DoubleParse("**foo *bar* baz**").Is(Parser.Prettify("<p><strong>foo <em>bar</em> baz</strong></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example428()
    {
      // Example 428
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   **foo*bar*baz**
      //
      // Should be rendered as:
      //   <p><strong>foo<em>bar</em>baz</strong></p>

      Parser.Parse("**foo*bar*baz**").Is(Parser.Prettify("<p><strong>foo<em>bar</em>baz</strong></p>"));

      Parser.DoubleParse("**foo*bar*baz**").Is(Parser.Prettify("<p><strong>foo<em>bar</em>baz</strong></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example429()
    {
      // Example 429
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   ***foo* bar**
      //
      // Should be rendered as:
      //   <p><strong><em>foo</em> bar</strong></p>

      Parser.Parse("***foo* bar**").Is(Parser.Prettify("<p><strong><em>foo</em> bar</strong></p>"));

      Parser.DoubleParse("***foo* bar**").Is(Parser.Prettify("<p><strong><em>foo</em> bar</strong></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example430()
    {
      // Example 430
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   **foo *bar***
      //
      // Should be rendered as:
      //   <p><strong>foo <em>bar</em></strong></p>

      Parser.Parse("**foo *bar***").Is(Parser.Prettify("<p><strong>foo <em>bar</em></strong></p>"));

      Parser.DoubleParse("**foo *bar***").Is(Parser.Prettify("<p><strong>foo <em>bar</em></strong></p>"));
    }

    // Indefinite levels of nesting are possible:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example431()
    {
      // Example 431
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   **foo *bar **baz**
      //   bim* bop**
      //
      // Should be rendered as:
      //   <p><strong>foo <em>bar <strong>baz</strong>
      //   bim</em> bop</strong></p>

      Parser.Parse("**foo *bar **baz**\nbim* bop**").Is(Parser.Prettify("<p><strong>foo <em>bar <strong>baz</strong>\nbim</em> bop</strong></p>"));

      Parser.DoubleParse("**foo *bar **baz**\nbim* bop**").Is(Parser.Prettify("<p><strong>foo <em>bar <strong>baz</strong>\nbim</em> bop</strong></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example432()
    {
      // Example 432
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   **foo [*bar*](/url)**
      //
      // Should be rendered as:
      //   <p><strong>foo <a href="/url"><em>bar</em></a></strong></p>

      Parser.Parse("**foo [*bar*](/url)**").Is(Parser.Prettify("<p><strong>foo <a href=\"/url\"><em>bar</em></a></strong></p>"));

      Parser.DoubleParse("**foo [*bar*](/url)**").Is(Parser.Prettify("<p><strong>foo <a href=\"/url\"><em>bar</em></a></strong></p>"));
    }

    // There can be no empty emphasis or strong emphasis:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example433()
    {
      // Example 433
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   __ is not an empty emphasis
      //
      // Should be rendered as:
      //   <p>__ is not an empty emphasis</p>

      Parser.Parse("__ is not an empty emphasis").Is(Parser.Prettify("<p>__ is not an empty emphasis</p>"));

      Parser.DoubleParse("__ is not an empty emphasis").Is(Parser.Prettify("<p>__ is not an empty emphasis</p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example434()
    {
      // Example 434
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   ____ is not an empty strong emphasis
      //
      // Should be rendered as:
      //   <p>____ is not an empty strong emphasis</p>

      Parser.Parse("____ is not an empty strong emphasis").Is(Parser.Prettify("<p>____ is not an empty strong emphasis</p>"));

      Parser.DoubleParse("____ is not an empty strong emphasis").Is(Parser.Prettify("<p>____ is not an empty strong emphasis</p>"));
    }

    // Rule 11:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example435()
    {
      // Example 435
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   foo ***
      //
      // Should be rendered as:
      //   <p>foo ***</p>

      Parser.Parse("foo ***").Is(Parser.Prettify("<p>foo ***</p>"));

      Parser.DoubleParse("foo ***").Is(Parser.Prettify("<p>foo ***</p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example436()
    {
      // Example 436
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   foo *\**
      //
      // Should be rendered as:
      //   <p>foo <em>*</em></p>

      Parser.Parse("foo *\\**").Is(Parser.Prettify("<p>foo <em>*</em></p>"));

      Parser.DoubleParse("foo *\\**").Is(Parser.Prettify("<p>foo <em>*</em></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example437()
    {
      // Example 437
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   foo *_*
      //
      // Should be rendered as:
      //   <p>foo <em>_</em></p>

      Parser.Parse("foo *_*").Is(Parser.Prettify("<p>foo <em>_</em></p>"));

      Parser.DoubleParse("foo *_*").Is(Parser.Prettify("<p>foo <em>_</em></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example438()
    {
      // Example 438
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   foo *****
      //
      // Should be rendered as:
      //   <p>foo *****</p>

      Parser.Parse("foo *****").Is(Parser.Prettify("<p>foo *****</p>"));

      Parser.DoubleParse("foo *****").Is(Parser.Prettify("<p>foo *****</p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example439()
    {
      // Example 439
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   foo **\***
      //
      // Should be rendered as:
      //   <p>foo <strong>*</strong></p>

      Parser.Parse("foo **\\***").Is(Parser.Prettify("<p>foo <strong>*</strong></p>"));

      Parser.DoubleParse("foo **\\***").Is(Parser.Prettify("<p>foo <strong>*</strong></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example440()
    {
      // Example 440
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   foo **_**
      //
      // Should be rendered as:
      //   <p>foo <strong>_</strong></p>

      Parser.Parse("foo **_**").Is(Parser.Prettify("<p>foo <strong>_</strong></p>"));

      Parser.DoubleParse("foo **_**").Is(Parser.Prettify("<p>foo <strong>_</strong></p>"));
    }

    // Note that when delimiters do not match evenly, Rule 11 determines
    // that the excess literal `*` characters will appear outside of the
    // emphasis, rather than inside it:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example441()
    {
      // Example 441
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   **foo*
      //
      // Should be rendered as:
      //   <p>*<em>foo</em></p>

      Parser.Parse("**foo*").Is(Parser.Prettify("<p>*<em>foo</em></p>"));

      Parser.DoubleParse("**foo*").Is(Parser.Prettify("<p>*<em>foo</em></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example442()
    {
      // Example 442
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   *foo**
      //
      // Should be rendered as:
      //   <p><em>foo</em>*</p>

      Parser.Parse("*foo**").Is(Parser.Prettify("<p><em>foo</em>*</p>"));

      Parser.DoubleParse("*foo**").Is(Parser.Prettify("<p><em>foo</em>*</p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example443()
    {
      // Example 443
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   ***foo**
      //
      // Should be rendered as:
      //   <p>*<strong>foo</strong></p>

      Parser.Parse("***foo**").Is(Parser.Prettify("<p>*<strong>foo</strong></p>"));

      Parser.DoubleParse("***foo**").Is(Parser.Prettify("<p>*<strong>foo</strong></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example444()
    {
      // Example 444
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   ****foo*
      //
      // Should be rendered as:
      //   <p>***<em>foo</em></p>

      Parser.Parse("****foo*").Is(Parser.Prettify("<p>***<em>foo</em></p>"));

      Parser.DoubleParse("****foo*").Is(Parser.Prettify("<p>***<em>foo</em></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example445()
    {
      // Example 445
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   **foo***
      //
      // Should be rendered as:
      //   <p><strong>foo</strong>*</p>

      Parser.Parse("**foo***").Is(Parser.Prettify("<p><strong>foo</strong>*</p>"));

      Parser.DoubleParse("**foo***").Is(Parser.Prettify("<p><strong>foo</strong>*</p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example446()
    {
      // Example 446
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   *foo****
      //
      // Should be rendered as:
      //   <p><em>foo</em>***</p>

      Parser.Parse("*foo****").Is(Parser.Prettify("<p><em>foo</em>***</p>"));

      Parser.DoubleParse("*foo****").Is(Parser.Prettify("<p><em>foo</em>***</p>"));
    }

    // Rule 12:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example447()
    {
      // Example 447
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   foo ___
      //
      // Should be rendered as:
      //   <p>foo ___</p>

      Parser.Parse("foo ___").Is(Parser.Prettify("<p>foo ___</p>"));

      Parser.DoubleParse("foo ___").Is(Parser.Prettify("<p>foo ___</p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example448()
    {
      // Example 448
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   foo _\__
      //
      // Should be rendered as:
      //   <p>foo <em>_</em></p>

      Parser.Parse("foo _\\__").Is(Parser.Prettify("<p>foo <em>_</em></p>"));

      Parser.DoubleParse("foo _\\__").Is(Parser.Prettify("<p>foo <em>_</em></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example449()
    {
      // Example 449
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   foo _*_
      //
      // Should be rendered as:
      //   <p>foo <em>*</em></p>

      Parser.Parse("foo _*_").Is(Parser.Prettify("<p>foo <em>*</em></p>"));

      Parser.DoubleParse("foo _*_").Is(Parser.Prettify("<p>foo <em>*</em></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example450()
    {
      // Example 450
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   foo _____
      //
      // Should be rendered as:
      //   <p>foo _____</p>

      Parser.Parse("foo _____").Is(Parser.Prettify("<p>foo _____</p>"));

      Parser.DoubleParse("foo _____").Is(Parser.Prettify("<p>foo _____</p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example451()
    {
      // Example 451
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   foo __\___
      //
      // Should be rendered as:
      //   <p>foo <strong>_</strong></p>

      Parser.Parse("foo __\\___").Is(Parser.Prettify("<p>foo <strong>_</strong></p>"));

      Parser.DoubleParse("foo __\\___").Is(Parser.Prettify("<p>foo <strong>_</strong></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example452()
    {
      // Example 452
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   foo __*__
      //
      // Should be rendered as:
      //   <p>foo <strong>*</strong></p>

      Parser.Parse("foo __*__").Is(Parser.Prettify("<p>foo <strong>*</strong></p>"));

      Parser.DoubleParse("foo __*__").Is(Parser.Prettify("<p>foo <strong>*</strong></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example453()
    {
      // Example 453
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   __foo_
      //
      // Should be rendered as:
      //   <p>_<em>foo</em></p>

      Parser.Parse("__foo_").Is(Parser.Prettify("<p>_<em>foo</em></p>"));

      Parser.DoubleParse("__foo_").Is(Parser.Prettify("<p>_<em>foo</em></p>"));
    }

    // Note that when delimiters do not match evenly, Rule 12 determines
    // that the excess literal `_` characters will appear outside of the
    // emphasis, rather than inside it:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example454()
    {
      // Example 454
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   _foo__
      //
      // Should be rendered as:
      //   <p><em>foo</em>_</p>

      Parser.Parse("_foo__").Is(Parser.Prettify("<p><em>foo</em>_</p>"));

      Parser.DoubleParse("_foo__").Is(Parser.Prettify("<p><em>foo</em>_</p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example455()
    {
      // Example 455
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   ___foo__
      //
      // Should be rendered as:
      //   <p>_<strong>foo</strong></p>

      Parser.Parse("___foo__").Is(Parser.Prettify("<p>_<strong>foo</strong></p>"));

      Parser.DoubleParse("___foo__").Is(Parser.Prettify("<p>_<strong>foo</strong></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example456()
    {
      // Example 456
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   ____foo_
      //
      // Should be rendered as:
      //   <p>___<em>foo</em></p>

      Parser.Parse("____foo_").Is(Parser.Prettify("<p>___<em>foo</em></p>"));

      Parser.DoubleParse("____foo_").Is(Parser.Prettify("<p>___<em>foo</em></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example457()
    {
      // Example 457
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   __foo___
      //
      // Should be rendered as:
      //   <p><strong>foo</strong>_</p>

      Parser.Parse("__foo___").Is(Parser.Prettify("<p><strong>foo</strong>_</p>"));

      Parser.DoubleParse("__foo___").Is(Parser.Prettify("<p><strong>foo</strong>_</p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example458()
    {
      // Example 458
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   _foo____
      //
      // Should be rendered as:
      //   <p><em>foo</em>___</p>

      Parser.Parse("_foo____").Is(Parser.Prettify("<p><em>foo</em>___</p>"));

      Parser.DoubleParse("_foo____").Is(Parser.Prettify("<p><em>foo</em>___</p>"));
    }

    // Rule 13 implies that if you want emphasis nested directly inside
    // emphasis, you must use different delimiters:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example459()
    {
      // Example 459
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   **foo**
      //
      // Should be rendered as:
      //   <p><strong>foo</strong></p>

      Parser.Parse("**foo**").Is(Parser.Prettify("<p><strong>foo</strong></p>"));

      Parser.DoubleParse("**foo**").Is(Parser.Prettify("<p><strong>foo</strong></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example460()
    {
      // Example 460
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   *_foo_*
      //
      // Should be rendered as:
      //   <p><em><em>foo</em></em></p>

      Parser.Parse("*_foo_*").Is(Parser.Prettify("<p><em><em>foo</em></em></p>"));

      Parser.DoubleParse("*_foo_*").Is(Parser.Prettify("<p><em><em>foo</em></em></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example461()
    {
      // Example 461
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   __foo__
      //
      // Should be rendered as:
      //   <p><strong>foo</strong></p>

      Parser.Parse("__foo__").Is(Parser.Prettify("<p><strong>foo</strong></p>"));

      Parser.DoubleParse("__foo__").Is(Parser.Prettify("<p><strong>foo</strong></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example462()
    {
      // Example 462
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   _*foo*_
      //
      // Should be rendered as:
      //   <p><em><em>foo</em></em></p>

      Parser.Parse("_*foo*_").Is(Parser.Prettify("<p><em><em>foo</em></em></p>"));

      Parser.DoubleParse("_*foo*_").Is(Parser.Prettify("<p><em><em>foo</em></em></p>"));
    }

    // However, strong emphasis within strong emphasis is possible without
    // switching delimiters:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example463()
    {
      // Example 463
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   ****foo****
      //
      // Should be rendered as:
      //   <p><strong><strong>foo</strong></strong></p>

      Parser.Parse("****foo****").Is(Parser.Prettify("<p><strong><strong>foo</strong></strong></p>"));

      Parser.DoubleParse("****foo****").Is(Parser.Prettify("<p><strong><strong>foo</strong></strong></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example464()
    {
      // Example 464
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   ____foo____
      //
      // Should be rendered as:
      //   <p><strong><strong>foo</strong></strong></p>

      Parser.Parse("____foo____").Is(Parser.Prettify("<p><strong><strong>foo</strong></strong></p>"));

      Parser.DoubleParse("____foo____").Is(Parser.Prettify("<p><strong><strong>foo</strong></strong></p>"));
    }

    // Rule 13 can be applied to arbitrarily long sequences of
    // delimiters:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example465()
    {
      // Example 465
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   ******foo******
      //
      // Should be rendered as:
      //   <p><strong><strong><strong>foo</strong></strong></strong></p>

      Parser.Parse("******foo******").Is(Parser.Prettify("<p><strong><strong><strong>foo</strong></strong></strong></p>"));

      Parser.DoubleParse("******foo******").Is(Parser.Prettify("<p><strong><strong><strong>foo</strong></strong></strong></p>"));
    }

    // Rule 14:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example466()
    {
      // Example 466
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   ***foo***
      //
      // Should be rendered as:
      //   <p><em><strong>foo</strong></em></p>

      Parser.Parse("***foo***").Is(Parser.Prettify("<p><em><strong>foo</strong></em></p>"));

      Parser.DoubleParse("***foo***").Is(Parser.Prettify("<p><em><strong>foo</strong></em></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example467()
    {
      // Example 467
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   _____foo_____
      //
      // Should be rendered as:
      //   <p><em><strong><strong>foo</strong></strong></em></p>

      Parser.Parse("_____foo_____").Is(Parser.Prettify("<p><em><strong><strong>foo</strong></strong></em></p>"));

      Parser.DoubleParse("_____foo_____").Is(Parser.Prettify("<p><em><strong><strong>foo</strong></strong></em></p>"));
    }

    // Rule 15:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example468()
    {
      // Example 468
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   *foo _bar* baz_
      //
      // Should be rendered as:
      //   <p><em>foo _bar</em> baz_</p>

      Parser.Parse("*foo _bar* baz_").Is(Parser.Prettify("<p><em>foo _bar</em> baz_</p>"));

      Parser.DoubleParse("*foo _bar* baz_").Is(Parser.Prettify("<p><em>foo _bar</em> baz_</p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example469()
    {
      // Example 469
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   *foo __bar *baz bim__ bam*
      //
      // Should be rendered as:
      //   <p><em>foo <strong>bar *baz bim</strong> bam</em></p>

      Parser.Parse("*foo __bar *baz bim__ bam*").Is(Parser.Prettify("<p><em>foo <strong>bar *baz bim</strong> bam</em></p>"));

      Parser.DoubleParse("*foo __bar *baz bim__ bam*").Is(Parser.Prettify("<p><em>foo <strong>bar *baz bim</strong> bam</em></p>"));
    }

    // Rule 16:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example470()
    {
      // Example 470
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   **foo **bar baz**
      //
      // Should be rendered as:
      //   <p>**foo <strong>bar baz</strong></p>

      Parser.Parse("**foo **bar baz**").Is(Parser.Prettify("<p>**foo <strong>bar baz</strong></p>"));

      Parser.DoubleParse("**foo **bar baz**").Is(Parser.Prettify("<p>**foo <strong>bar baz</strong></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example471()
    {
      // Example 471
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   *foo *bar baz*
      //
      // Should be rendered as:
      //   <p>*foo <em>bar baz</em></p>

      Parser.Parse("*foo *bar baz*").Is(Parser.Prettify("<p>*foo <em>bar baz</em></p>"));

      Parser.DoubleParse("*foo *bar baz*").Is(Parser.Prettify("<p>*foo <em>bar baz</em></p>"));
    }

    // Rule 17:
    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example472()
    {
      // Example 472
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   *[bar*](/url)
      //
      // Should be rendered as:
      //   <p>*<a href="/url">bar*</a></p>

      Parser.Parse("*[bar*](/url)").Is(Parser.Prettify("<p>*<a href=\"/url\">bar*</a></p>"));

      Parser.DoubleParse("*[bar*](/url)").Is(Parser.Prettify("<p>*<a href=\"/url\">bar*</a></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example473()
    {
      // Example 473
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   _foo [bar_](/url)
      //
      // Should be rendered as:
      //   <p>_foo <a href="/url">bar_</a></p>

      Parser.Parse("_foo [bar_](/url)").Is(Parser.Prettify("<p>_foo <a href=\"/url\">bar_</a></p>"));

      Parser.DoubleParse("_foo [bar_](/url)").Is(Parser.Prettify("<p>_foo <a href=\"/url\">bar_</a></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example474()
    {
      // Example 474
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   *<img src="foo" title="*"/>
      //
      // Should be rendered as:
      //   <p>*<img src="foo" title="*"/></p>

      Parser.Parse("*<img src=\"foo\" title=\"*\"/>").Is(Parser.Prettify("<p>*<img src=\"foo\" title=\"*\"/></p>"));

      Parser.DoubleParse("*<img src=\"foo\" title=\"*\"/>").Is(Parser.Prettify("<p>*<img src=\"foo\" title=\"*\"/></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example475()
    {
      // Example 475
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   **<a href="**">
      //
      // Should be rendered as:
      //   <p>**<a href="**"></p>

      Parser.Parse("**<a href=\"**\">").Is(Parser.Prettify("<p>**<a href=\"**\"></p>"));

      Parser.DoubleParse("**<a href=\"**\">").Is(Parser.Prettify("<p>**<a href=\"**\"></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example476()
    {
      // Example 476
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   __<a href="__">
      //
      // Should be rendered as:
      //   <p>__<a href="__"></p>

      Parser.Parse("__<a href=\"__\">").Is(Parser.Prettify("<p>__<a href=\"__\"></p>"));

      Parser.DoubleParse("__<a href=\"__\">").Is(Parser.Prettify("<p>__<a href=\"__\"></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example477()
    {
      // Example 477
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   *a `*`*
      //
      // Should be rendered as:
      //   <p><em>a <code>*</code></em></p>

      Parser.Parse("*a `*`*").Is(Parser.Prettify("<p><em>a <code>*</code></em></p>"));

      Parser.DoubleParse("*a `*`*").Is(Parser.Prettify("<p><em>a <code>*</code></em></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example478()
    {
      // Example 478
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   _a `_`_
      //
      // Should be rendered as:
      //   <p><em>a <code>_</code></em></p>

      Parser.Parse("_a `_`_").Is(Parser.Prettify("<p><em>a <code>_</code></em></p>"));

      Parser.DoubleParse("_a `_`_").Is(Parser.Prettify("<p><em>a <code>_</code></em></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example479()
    {
      // Example 479
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   **a<http://foo.bar/?q=**>
      //
      // Should be rendered as:
      //   <p>**a<a href="http://foo.bar/?q=**">http://foo.bar/?q=**</a></p>

      Parser.Parse("**a<http://foo.bar/?q=**>").Is(Parser.Prettify("<p>**a<a href=\"http://foo.bar/?q=**\">http://foo.bar/?q=**</a></p>"));

      Parser.DoubleParse("**a<http://foo.bar/?q=**>").Is(Parser.Prettify("<p>**a<a href=\"http://foo.bar/?q=**\">http://foo.bar/?q=**</a></p>"));
    }

    [Fact]
    public void InlinesEmphasisAndStrongEmphasis_Example480()
    {
      // Example 480
      // Section: Inlines / Emphasis and strong emphasis
      //
      // The following Markdown:
      //   __a<http://foo.bar/?q=__>
      //
      // Should be rendered as:
      //   <p>__a<a href="http://foo.bar/?q=__">http://foo.bar/?q=__</a></p>

      Parser.Parse("__a<http://foo.bar/?q=__>").Is(Parser.Prettify("<p>__a<a href=\"http://foo.bar/?q=__\">http://foo.bar/?q=__</a></p>"));

      Parser.DoubleParse("__a<http://foo.bar/?q=__>").Is(Parser.Prettify("<p>__a<a href=\"http://foo.bar/?q=__\">http://foo.bar/?q=__</a></p>"));
    }
  }

  public class TestInlinesLinks
  {
    // ## Links
    //
    // A link contains [link text] (the visible text), a [link destination]
    // (the URI that is the link destination), and optionally a [link title].
    // There are two basic kinds of links in Markdown.  In [inline links] the
    // destination and title are given immediately after the link text.  In
    // [reference links] the destination and title are defined elsewhere in
    // the document.
    //
    // A [link text](@) consists of a sequence of zero or more
    // inline elements enclosed by square brackets (`[` and `]`).  The
    // following rules apply:
    //
    // - Links may not contain other links, at any level of nesting. If
    //   multiple otherwise valid link definitions appear nested inside each
    //   other, the inner-most definition is used.
    //
    // - Brackets are allowed in the [link text] only if (a) they
    //   are backslash-escaped or (b) they appear as a matched pair of brackets,
    //   with an open bracket `[`, a sequence of zero or more inlines, and
    //   a close bracket `]`.
    //
    // - Backtick [code spans], [autolinks], and raw [HTML tags] bind more tightly
    //   than the brackets in link text.  Thus, for example,
    //   `` [foo`]` `` could not be a link text, since the second `]`
    //   is part of a code span.
    //
    // - The brackets in link text bind more tightly than markers for
    //   [emphasis and strong emphasis]. Thus, for example, `*[foo*](url)` is a link.
    //
    // A [link destination](@) consists of either
    //
    // - a sequence of zero or more characters between an opening `<` and a
    //   closing `>` that contains no line breaks or unescaped
    //   `<` or `>` characters, or
    //
    // - a nonempty sequence of characters that does not start with
    //   `<`, does not include ASCII space or control characters, and
    //   includes parentheses only if (a) they are backslash-escaped or
    //   (b) they are part of a balanced pair of unescaped parentheses.
    //   (Implementations may impose limits on parentheses nesting to
    //   avoid performance issues, but at least three levels of nesting
    //   should be supported.)
    //
    // A [link title](@)  consists of either
    //
    // - a sequence of zero or more characters between straight double-quote
    //   characters (`"`), including a `"` character only if it is
    //   backslash-escaped, or
    //
    // - a sequence of zero or more characters between straight single-quote
    //   characters (`'`), including a `'` character only if it is
    //   backslash-escaped, or
    //
    // - a sequence of zero or more characters between matching parentheses
    //   (`(...)`), including a `(` or `)` character only if it is
    //   backslash-escaped.
    //
    // Although [link titles] may span multiple lines, they may not contain
    // a [blank line].
    //
    // An [inline link](@) consists of a [link text] followed immediately
    // by a left parenthesis `(`, optional [whitespace], an optional
    // [link destination], an optional [link title] separated from the link
    // destination by [whitespace], optional [whitespace], and a right
    // parenthesis `)`. The link's text consists of the inlines contained
    // in the [link text] (excluding the enclosing square brackets).
    // The link's URI consists of the link destination, excluding enclosing
    // `<...>` if present, with backslash-escapes in effect as described
    // above.  The link's title consists of the link title, excluding its
    // enclosing delimiters, with backslash-escapes in effect as described
    // above.
    //
    // Here is a simple inline link:
    [Fact]
    public void InlinesLinks_Example481()
    {
      // Example 481
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [link](/uri "title")
      //
      // Should be rendered as:
      //   <p><a href="/uri" title="title">link</a></p>

      Parser.Parse("[link](/uri \"title\")").Is(Parser.Prettify("<p><a href=\"/uri\" title=\"title\">link</a></p>"));

      Parser.DoubleParse("[link](/uri \"title\")").Is(Parser.Prettify("<p><a href=\"/uri\" title=\"title\">link</a></p>"));
    }

    // The title may be omitted:
    [Fact]
    public void InlinesLinks_Example482()
    {
      // Example 482
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [link](/uri)
      //
      // Should be rendered as:
      //   <p><a href="/uri">link</a></p>

      Parser.Parse("[link](/uri)").Is(Parser.Prettify("<p><a href=\"/uri\">link</a></p>"));

      Parser.DoubleParse("[link](/uri)").Is(Parser.Prettify("<p><a href=\"/uri\">link</a></p>"));
    }

    // Both the title and the destination may be omitted:
    [Fact]
    public void InlinesLinks_Example483()
    {
      // Example 483
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [link]()
      //
      // Should be rendered as:
      //   <p><a href="">link</a></p>

      Parser.Parse("[link]()").Is(Parser.Prettify("<p><a href=\"\">link</a></p>"));

      Parser.DoubleParse("[link]()").Is(Parser.Prettify("<p><a href=\"\">link</a></p>"));
    }

    [Fact]
    public void InlinesLinks_Example484()
    {
      // Example 484
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [link](<>)
      //
      // Should be rendered as:
      //   <p><a href="">link</a></p>

      Parser.Parse("[link](<>)").Is(Parser.Prettify("<p><a href=\"\">link</a></p>"));

      Parser.DoubleParse("[link](<>)").Is(Parser.Prettify("<p><a href=\"\">link</a></p>"));
    }

    // The destination can only contain spaces if it is
    // enclosed in pointy brackets:
    [Fact]
    public void InlinesLinks_Example485()
    {
      // Example 485
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [link](/my uri)
      //
      // Should be rendered as:
      //   <p>[link](/my uri)</p>

      Parser.Parse("[link](/my uri)").Is(Parser.Prettify("<p>[link](/my uri)</p>"));

      Parser.DoubleParse("[link](/my uri)").Is(Parser.Prettify("<p>[link](/my uri)</p>"));
    }

    [Fact]
    public void InlinesLinks_Example486()
    {
      // Example 486
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [link](</my uri>)
      //
      // Should be rendered as:
      //   <p><a href="/my%20uri">link</a></p>

      Parser.Parse("[link](</my uri>)").Is(Parser.Prettify("<p><a href=\"/my%20uri\">link</a></p>"));

      Parser.DoubleParse("[link](</my uri>)").Is(Parser.Prettify("<p><a href=\"/my%20uri\">link</a></p>"));
    }

    // The destination cannot contain line breaks,
    // even if enclosed in pointy brackets:
    [Fact]
    public void InlinesLinks_Example487()
    {
      // Example 487
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [link](foo
      //   bar)
      //
      // Should be rendered as:
      //   <p>[link](foo
      //   bar)</p>

      Parser.Parse("[link](foo\nbar)").Is(Parser.Prettify("<p>[link](foo\nbar)</p>"));

      Parser.DoubleParse("[link](foo\nbar)").Is(Parser.Prettify("<p>[link](foo\nbar)</p>"));
    }

    [Fact]
    public void InlinesLinks_Example488()
    {
      // Example 488
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [link](<foo
      //   bar>)
      //
      // Should be rendered as:
      //   <p>[link](<foo
      //   bar>)</p>

      Parser.Parse("[link](<foo\nbar>)").Is(Parser.Prettify("<p>[link](<foo\nbar>)</p>"));

      Parser.DoubleParse("[link](<foo\nbar>)").Is(Parser.Prettify("<p>[link](<foo\nbar>)</p>"));
    }

    // The destination can contain `)` if it is enclosed
    // in pointy brackets:
    [Fact]
    public void InlinesLinks_Example489()
    {
      // Example 489
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [a](<b)c>)
      //
      // Should be rendered as:
      //   <p><a href="b)c">a</a></p>

      Parser.Parse("[a](<b)c>)").Is(Parser.Prettify("<p><a href=\"b)c\">a</a></p>"));

      Parser.DoubleParse("[a](<b)c>)").Is(Parser.Prettify("<p><a href=\"b)c\">a</a></p>"));
    }

    // Pointy brackets that enclose links must be unescaped:
    [Fact]
    public void InlinesLinks_Example490()
    {
      // Example 490
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [link](<foo\>)
      //
      // Should be rendered as:
      //   <p>[link](&lt;foo&gt;)</p>

      Parser.Parse("[link](<foo\\>)").Is(Parser.Prettify("<p>[link](&lt;foo&gt;)</p>"));

      Parser.DoubleParse("[link](<foo\\>)").Is(Parser.Prettify("<p>[link](&lt;foo&gt;)</p>"));
    }

    // These are not links, because the opening pointy bracket
    // is not matched properly:
    [Fact]
    public void InlinesLinks_Example491()
    {
      // Example 491
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [a](<b)c
      //   [a](<b)c>
      //   [a](<b>c)
      //
      // Should be rendered as:
      //   <p>[a](&lt;b)c
      //   [a](&lt;b)c&gt;
      //   [a](<b>c)</p>

      Parser.Parse("[a](<b)c\n[a](<b)c>\n[a](<b>c)").Is(Parser.Prettify("<p>[a](&lt;b)c\n[a](&lt;b)c&gt;\n[a](<b>c)</p>"));

      Parser.DoubleParse("[a](<b)c\n[a](<b)c>\n[a](<b>c)").Is(Parser.Prettify("<p>[a](&lt;b)c\n[a](&lt;b)c&gt;\n[a](<b>c)</p>"));
    }

    // Parentheses inside the link destination may be escaped:
    [Fact]
    public void InlinesLinks_Example492()
    {
      // Example 492
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [link](\(foo\))
      //
      // Should be rendered as:
      //   <p><a href="(foo)">link</a></p>

      Parser.Parse("[link](\\(foo\\))").Is(Parser.Prettify("<p><a href=\"(foo)\">link</a></p>"));

      Parser.DoubleParse("[link](\\(foo\\))").Is(Parser.Prettify("<p><a href=\"(foo)\">link</a></p>"));
    }

    // Any number of parentheses are allowed without escaping, as long as they are
    // balanced:
    [Fact]
    public void InlinesLinks_Example493()
    {
      // Example 493
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [link](foo(and(bar)))
      //
      // Should be rendered as:
      //   <p><a href="foo(and(bar))">link</a></p>

      Parser.Parse("[link](foo(and(bar)))").Is(Parser.Prettify("<p><a href=\"foo(and(bar))\">link</a></p>"));

      Parser.DoubleParse("[link](foo(and(bar)))").Is(Parser.Prettify("<p><a href=\"foo(and(bar))\">link</a></p>"));
    }

    // However, if you have unbalanced parentheses, you need to escape or use the
    // `<...>` form:
    [Fact]
    public void InlinesLinks_Example494()
    {
      // Example 494
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [link](foo\(and\(bar\))
      //
      // Should be rendered as:
      //   <p><a href="foo(and(bar)">link</a></p>

      Parser.Parse("[link](foo\\(and\\(bar\\))").Is(Parser.Prettify("<p><a href=\"foo(and(bar)\">link</a></p>"));

      Parser.DoubleParse("[link](foo\\(and\\(bar\\))").Is(Parser.Prettify("<p><a href=\"foo(and(bar)\">link</a></p>"));
    }

    [Fact]
    public void InlinesLinks_Example495()
    {
      // Example 495
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [link](<foo(and(bar)>)
      //
      // Should be rendered as:
      //   <p><a href="foo(and(bar)">link</a></p>

      Parser.Parse("[link](<foo(and(bar)>)").Is(Parser.Prettify("<p><a href=\"foo(and(bar)\">link</a></p>"));

      Parser.DoubleParse("[link](<foo(and(bar)>)").Is(Parser.Prettify("<p><a href=\"foo(and(bar)\">link</a></p>"));
    }

    // Parentheses and other symbols can also be escaped, as usual
    // in Markdown:
    [Fact]
    public void InlinesLinks_Example496()
    {
      // Example 496
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [link](foo\)\:)
      //
      // Should be rendered as:
      //   <p><a href="foo):">link</a></p>

      Parser.Parse("[link](foo\\)\\:)").Is(Parser.Prettify("<p><a href=\"foo):\">link</a></p>"));

      Parser.DoubleParse("[link](foo\\)\\:)").Is(Parser.Prettify("<p><a href=\"foo):\">link</a></p>"));
    }

    // A link can contain fragment identifiers and queries:
    [Fact]
    public void InlinesLinks_Example497()
    {
      // Example 497
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [link](#fragment)
      //  
      //   [link](http://example.com#fragment)
      //  
      //   [link](http://example.com?foo=3#frag)
      //
      // Should be rendered as:
      //   <p><a href="#fragment">link</a></p>
      //   <p><a href="http://example.com#fragment">link</a></p>
      //   <p><a href="http://example.com?foo=3#frag">link</a></p>

      Parser.Parse("[link](#fragment)\n\n[link](http://example.com#fragment)\n\n[link](http://example.com?foo=3#frag)").Is(Parser.Prettify("<p><a href=\"#fragment\">link</a></p>\n<p><a href=\"http://example.com#fragment\">link</a></p>\n<p><a href=\"http://example.com?foo=3#frag\">link</a></p>"));

      Parser.DoubleParse("[link](#fragment)\n\n[link](http://example.com#fragment)\n\n[link](http://example.com?foo=3#frag)").Is(Parser.Prettify("<p><a href=\"#fragment\">link</a></p>\n<p><a href=\"http://example.com#fragment\">link</a></p>\n<p><a href=\"http://example.com?foo=3#frag\">link</a></p>"));
    }

    // Note that a backslash before a non-escapable character is
    // just a backslash:
    [Fact]
    public void InlinesLinks_Example498()
    {
      // Example 498
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [link](foo\bar)
      //
      // Should be rendered as:
      //   <p><a href="foo%5Cbar">link</a></p>

      Parser.Parse("[link](foo\\bar)").Is(Parser.Prettify("<p><a href=\"foo%5Cbar\">link</a></p>"));

      Parser.DoubleParse("[link](foo\\bar)").Is(Parser.Prettify("<p><a href=\"foo%5Cbar\">link</a></p>"));
    }

    // URL-escaping should be left alone inside the destination, as all
    // URL-escaped characters are also valid URL characters. Entity and
    // numerical character references in the destination will be parsed
    // into the corresponding Unicode code points, as usual.  These may
    // be optionally URL-escaped when written as HTML, but this spec
    // does not enforce any particular policy for rendering URLs in
    // HTML or other formats.  Renderers may make different decisions
    // about how to escape or normalize URLs in the output.
    [Fact]
    public void InlinesLinks_Example499()
    {
      // Example 499
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [link](foo%20b&auml;)
      //
      // Should be rendered as:
      //   <p><a href="foo%20b%C3%A4">link</a></p>

      Parser.Parse("[link](foo%20b&auml;)").Is(Parser.Prettify("<p><a href=\"foo%20b%C3%A4\">link</a></p>"));

      Parser.DoubleParse("[link](foo%20b&auml;)").Is(Parser.Prettify("<p><a href=\"foo%20b%C3%A4\">link</a></p>"));
    }

    // Note that, because titles can often be parsed as destinations,
    // if you try to omit the destination and keep the title, you'll
    // get unexpected results:
    [Fact]
    public void InlinesLinks_Example500()
    {
      // Example 500
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [link]("title")
      //
      // Should be rendered as:
      //   <p><a href="%22title%22">link</a></p>

      Parser.Parse("[link](\"title\")").Is(Parser.Prettify("<p><a href=\"%22title%22\">link</a></p>"));

      Parser.DoubleParse("[link](\"title\")").Is(Parser.Prettify("<p><a href=\"%22title%22\">link</a></p>"));
    }

    // Titles may be in single quotes, double quotes, or parentheses:
    [Fact]
    public void InlinesLinks_Example501()
    {
      // Example 501
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [link](/url "title")
      //   [link](/url 'title')
      //   [link](/url (title))
      //
      // Should be rendered as:
      //   <p><a href="/url" title="title">link</a>
      //   <a href="/url" title="title">link</a>
      //   <a href="/url" title="title">link</a></p>

      Parser.Parse("[link](/url \"title\")\n[link](/url 'title')\n[link](/url (title))").Is(Parser.Prettify("<p><a href=\"/url\" title=\"title\">link</a>\n<a href=\"/url\" title=\"title\">link</a>\n<a href=\"/url\" title=\"title\">link</a></p>"));

      Parser.DoubleParse("[link](/url \"title\")\n[link](/url 'title')\n[link](/url (title))").Is(Parser.Prettify("<p><a href=\"/url\" title=\"title\">link</a>\n<a href=\"/url\" title=\"title\">link</a>\n<a href=\"/url\" title=\"title\">link</a></p>"));
    }

    // Backslash escapes and entity and numeric character references
    // may be used in titles:
    [Fact]
    public void InlinesLinks_Example502()
    {
      // Example 502
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [link](/url "title \"&quot;")
      //
      // Should be rendered as:
      //   <p><a href="/url" title="title &quot;&quot;">link</a></p>

      Parser.Parse("[link](/url \"title \\\"&quot;\")").Is(Parser.Prettify("<p><a href=\"/url\" title=\"title &quot;&quot;\">link</a></p>"));

      Parser.DoubleParse("[link](/url \"title \\\"&quot;\")").Is(Parser.Prettify("<p><a href=\"/url\" title=\"title &quot;&quot;\">link</a></p>"));
    }

    // Titles must be separated from the link using a [whitespace].
    // Other [Unicode whitespace] like non-breaking space doesn't work.
    [Fact]
    public void InlinesLinks_Example503()
    {
      // Example 503
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [link](/url "title")
      //
      // Should be rendered as:
      //   <p><a href="/url%C2%A0%22title%22">link</a></p>

      Parser.Parse("[link](/url \"title\")").Is(Parser.Prettify("<p><a href=\"/url%C2%A0%22title%22\">link</a></p>"));

      Parser.DoubleParse("[link](/url \"title\")").Is(Parser.Prettify("<p><a href=\"/url%C2%A0%22title%22\">link</a></p>"));
    }

    // Nested balanced quotes are not allowed without escaping:
    [Fact]
    public void InlinesLinks_Example504()
    {
      // Example 504
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [link](/url "title "and" title")
      //
      // Should be rendered as:
      //   <p>[link](/url &quot;title &quot;and&quot; title&quot;)</p>

      Parser.Parse("[link](/url \"title \"and\" title\")").Is(Parser.Prettify("<p>[link](/url &quot;title &quot;and&quot; title&quot;)</p>"));

      Parser.DoubleParse("[link](/url \"title \"and\" title\")").Is(Parser.Prettify("<p>[link](/url &quot;title &quot;and&quot; title&quot;)</p>"));
    }

    // But it is easy to work around this by using a different quote type:
    [Fact]
    public void InlinesLinks_Example505()
    {
      // Example 505
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [link](/url 'title "and" title')
      //
      // Should be rendered as:
      //   <p><a href="/url" title="title &quot;and&quot; title">link</a></p>

      Parser.Parse("[link](/url 'title \"and\" title')").Is(Parser.Prettify("<p><a href=\"/url\" title=\"title &quot;and&quot; title\">link</a></p>"));

      Parser.DoubleParse("[link](/url 'title \"and\" title')").Is(Parser.Prettify("<p><a href=\"/url\" title=\"title &quot;and&quot; title\">link</a></p>"));
    }

    // (Note:  `Markdown.pl` did allow double quotes inside a double-quoted
    // title, and its test suite included a test demonstrating this.
    // But it is hard to see a good rationale for the extra complexity this
    // brings, since there are already many ways---backslash escaping,
    // entity and numeric character references, or using a different
    // quote type for the enclosing title---to write titles containing
    // double quotes.  `Markdown.pl`'s handling of titles has a number
    // of other strange features.  For example, it allows single-quoted
    // titles in inline links, but not reference links.  And, in
    // reference links but not inline links, it allows a title to begin
    // with `"` and end with `)`.  `Markdown.pl` 1.0.1 even allows
    // titles with no closing quotation mark, though 1.0.2b8 does not.
    // It seems preferable to adopt a simple, rational rule that works
    // the same way in inline links and link reference definitions.)
    //
    // [Whitespace] is allowed around the destination and title:
    [Fact]
    public void InlinesLinks_Example506()
    {
      // Example 506
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [link](   /uri
      //     "title"  )
      //
      // Should be rendered as:
      //   <p><a href="/uri" title="title">link</a></p>

      Parser.Parse("[link](   /uri\n  \"title\"  )").Is(Parser.Prettify("<p><a href=\"/uri\" title=\"title\">link</a></p>"));

      Parser.DoubleParse("[link](   /uri\n  \"title\"  )").Is(Parser.Prettify("<p><a href=\"/uri\" title=\"title\">link</a></p>"));
    }

    // But it is not allowed between the link text and the
    // following parenthesis:
    [Fact]
    public void InlinesLinks_Example507()
    {
      // Example 507
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [link] (/uri)
      //
      // Should be rendered as:
      //   <p>[link] (/uri)</p>

      Parser.Parse("[link] (/uri)").Is(Parser.Prettify("<p>[link] (/uri)</p>"));

      Parser.DoubleParse("[link] (/uri)").Is(Parser.Prettify("<p>[link] (/uri)</p>"));
    }

    // The link text may contain balanced brackets, but not unbalanced ones,
    // unless they are escaped:
    [Fact]
    public void InlinesLinks_Example508()
    {
      // Example 508
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [link [foo [bar]]](/uri)
      //
      // Should be rendered as:
      //   <p><a href="/uri">link [foo [bar]]</a></p>

      Parser.Parse("[link [foo [bar]]](/uri)").Is(Parser.Prettify("<p><a href=\"/uri\">link [foo [bar]]</a></p>"));

      Parser.DoubleParse("[link [foo [bar]]](/uri)").Is(Parser.Prettify("<p><a href=\"/uri\">link [foo [bar]]</a></p>"));
    }

    [Fact]
    public void InlinesLinks_Example509()
    {
      // Example 509
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [link] bar](/uri)
      //
      // Should be rendered as:
      //   <p>[link] bar](/uri)</p>

      Parser.Parse("[link] bar](/uri)").Is(Parser.Prettify("<p>[link] bar](/uri)</p>"));

      Parser.DoubleParse("[link] bar](/uri)").Is(Parser.Prettify("<p>[link] bar](/uri)</p>"));
    }

    [Fact]
    public void InlinesLinks_Example510()
    {
      // Example 510
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [link [bar](/uri)
      //
      // Should be rendered as:
      //   <p>[link <a href="/uri">bar</a></p>

      Parser.Parse("[link [bar](/uri)").Is(Parser.Prettify("<p>[link <a href=\"/uri\">bar</a></p>"));

      Parser.DoubleParse("[link [bar](/uri)").Is(Parser.Prettify("<p>[link <a href=\"/uri\">bar</a></p>"));
    }

    [Fact]
    public void InlinesLinks_Example511()
    {
      // Example 511
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [link \[bar](/uri)
      //
      // Should be rendered as:
      //   <p><a href="/uri">link [bar</a></p>

      Parser.Parse("[link \\[bar](/uri)").Is(Parser.Prettify("<p><a href=\"/uri\">link [bar</a></p>"));

      Parser.DoubleParse("[link \\[bar](/uri)").Is(Parser.Prettify("<p><a href=\"/uri\">link [bar</a></p>"));
    }

    // The link text may contain inline content:
    [Fact]
    public void InlinesLinks_Example512()
    {
      // Example 512
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [link *foo **bar** `#`*](/uri)
      //
      // Should be rendered as:
      //   <p><a href="/uri">link <em>foo <strong>bar</strong> <code>#</code></em></a></p>

      Parser.Parse("[link *foo **bar** `#`*](/uri)").Is(Parser.Prettify("<p><a href=\"/uri\">link <em>foo <strong>bar</strong> <code>#</code></em></a></p>"));

      Parser.DoubleParse("[link *foo **bar** `#`*](/uri)").Is(Parser.Prettify("<p><a href=\"/uri\">link <em>foo <strong>bar</strong> <code>#</code></em></a></p>"));
    }

    [Fact]
    public void InlinesLinks_Example513()
    {
      // Example 513
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [![moon](moon.jpg)](/uri)
      //
      // Should be rendered as:
      //   <p><a href="/uri"><img src="moon.jpg" alt="moon" /></a></p>

      Parser.Parse("[![moon](moon.jpg)](/uri)").Is(Parser.Prettify("<p><a href=\"/uri\"><img src=\"moon.jpg\" alt=\"moon\" /></a></p>"));

      Parser.DoubleParse("[![moon](moon.jpg)](/uri)").Is(Parser.Prettify("<p><a href=\"/uri\"><img src=\"moon.jpg\" alt=\"moon\" /></a></p>"));
    }

    // However, links may not contain other links, at any level of nesting.
    [Fact]
    public void InlinesLinks_Example514()
    {
      // Example 514
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [foo [bar](/uri)](/uri)
      //
      // Should be rendered as:
      //   <p>[foo <a href="/uri">bar</a>](/uri)</p>

      Parser.Parse("[foo [bar](/uri)](/uri)").Is(Parser.Prettify("<p>[foo <a href=\"/uri\">bar</a>](/uri)</p>"));

      Parser.DoubleParse("[foo [bar](/uri)](/uri)").Is(Parser.Prettify("<p>[foo <a href=\"/uri\">bar</a>](/uri)</p>"));
    }

    [Fact]
    public void InlinesLinks_Example515()
    {
      // Example 515
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [foo *[bar [baz](/uri)](/uri)*](/uri)
      //
      // Should be rendered as:
      //   <p>[foo <em>[bar <a href="/uri">baz</a>](/uri)</em>](/uri)</p>

      Parser.Parse("[foo *[bar [baz](/uri)](/uri)*](/uri)").Is(Parser.Prettify("<p>[foo <em>[bar <a href=\"/uri\">baz</a>](/uri)</em>](/uri)</p>"));

      Parser.DoubleParse("[foo *[bar [baz](/uri)](/uri)*](/uri)").Is(Parser.Prettify("<p>[foo <em>[bar <a href=\"/uri\">baz</a>](/uri)</em>](/uri)</p>"));
    }

    [Fact]
    public void InlinesLinks_Example516()
    {
      // Example 516
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   ![[[foo](uri1)](uri2)](uri3)
      //
      // Should be rendered as:
      //   <p><img src="uri3" alt="[foo](uri2)" /></p>

      Parser.Parse("![[[foo](uri1)](uri2)](uri3)").Is(Parser.Prettify("<p><img src=\"uri3\" alt=\"[foo](uri2)\" /></p>"));

      Parser.DoubleParse("![[[foo](uri1)](uri2)](uri3)").Is(Parser.Prettify("<p><img src=\"uri3\" alt=\"[foo](uri2)\" /></p>"));
    }

    // These cases illustrate the precedence of link text grouping over
    // emphasis grouping:
    [Fact]
    public void InlinesLinks_Example517()
    {
      // Example 517
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   *[foo*](/uri)
      //
      // Should be rendered as:
      //   <p>*<a href="/uri">foo*</a></p>

      Parser.Parse("*[foo*](/uri)").Is(Parser.Prettify("<p>*<a href=\"/uri\">foo*</a></p>"));

      Parser.DoubleParse("*[foo*](/uri)").Is(Parser.Prettify("<p>*<a href=\"/uri\">foo*</a></p>"));
    }

    [Fact]
    public void InlinesLinks_Example518()
    {
      // Example 518
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [foo *bar](baz*)
      //
      // Should be rendered as:
      //   <p><a href="baz*">foo *bar</a></p>

      Parser.Parse("[foo *bar](baz*)").Is(Parser.Prettify("<p><a href=\"baz*\">foo *bar</a></p>"));

      Parser.DoubleParse("[foo *bar](baz*)").Is(Parser.Prettify("<p><a href=\"baz*\">foo *bar</a></p>"));
    }

    // Note that brackets that *aren't* part of links do not take
    // precedence:
    [Fact]
    public void InlinesLinks_Example519()
    {
      // Example 519
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   *foo [bar* baz]
      //
      // Should be rendered as:
      //   <p><em>foo [bar</em> baz]</p>

      Parser.Parse("*foo [bar* baz]").Is(Parser.Prettify("<p><em>foo [bar</em> baz]</p>"));

      Parser.DoubleParse("*foo [bar* baz]").Is(Parser.Prettify("<p><em>foo [bar</em> baz]</p>"));
    }

    // These cases illustrate the precedence of HTML tags, code spans,
    // and autolinks over link grouping:
    [Fact]
    public void InlinesLinks_Example520()
    {
      // Example 520
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [foo <bar attr="](baz)">
      //
      // Should be rendered as:
      //   <p>[foo <bar attr="](baz)"></p>

      Parser.Parse("[foo <bar attr=\"](baz)\">").Is(Parser.Prettify("<p>[foo <bar attr=\"](baz)\"></p>"));

      Parser.DoubleParse("[foo <bar attr=\"](baz)\">").Is(Parser.Prettify("<p>[foo <bar attr=\"](baz)\"></p>"));
    }

    [Fact]
    public void InlinesLinks_Example521()
    {
      // Example 521
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [foo`](/uri)`
      //
      // Should be rendered as:
      //   <p>[foo<code>](/uri)</code></p>

      Parser.Parse("[foo`](/uri)`").Is(Parser.Prettify("<p>[foo<code>](/uri)</code></p>"));

      Parser.DoubleParse("[foo`](/uri)`").Is(Parser.Prettify("<p>[foo<code>](/uri)</code></p>"));
    }

    [Fact]
    public void InlinesLinks_Example522()
    {
      // Example 522
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [foo<http://example.com/?search=](uri)>
      //
      // Should be rendered as:
      //   <p>[foo<a href="http://example.com/?search=%5D(uri)">http://example.com/?search=](uri)</a></p>

      Parser.Parse("[foo<http://example.com/?search=](uri)>").Is(Parser.Prettify("<p>[foo<a href=\"http://example.com/?search=%5D(uri)\">http://example.com/?search=](uri)</a></p>"));

      Parser.DoubleParse("[foo<http://example.com/?search=](uri)>").Is(Parser.Prettify("<p>[foo<a href=\"http://example.com/?search=%5D(uri)\">http://example.com/?search=](uri)</a></p>"));
    }

    // There are three kinds of [reference link](@)s:
    // [full](#full-reference-link), [collapsed](#collapsed-reference-link),
    // and [shortcut](#shortcut-reference-link).
    //
    // A [full reference link](@)
    // consists of a [link text] immediately followed by a [link label]
    // that [matches] a [link reference definition] elsewhere in the document.
    //
    // A [link label](@)  begins with a left bracket (`[`) and ends
    // with the first right bracket (`]`) that is not backslash-escaped.
    // Between these brackets there must be at least one [non-whitespace character].
    // Unescaped square bracket characters are not allowed inside the
    // opening and closing square brackets of [link labels].  A link
    // label can have at most 999 characters inside the square
    // brackets.
    //
    // One label [matches](@)
    // another just in case their normalized forms are equal.  To normalize a
    // label, strip off the opening and closing brackets,
    // perform the *Unicode case fold*, strip leading and trailing
    // [whitespace] and collapse consecutive internal
    // [whitespace] to a single space.  If there are multiple
    // matching reference link definitions, the one that comes first in the
    // document is used.  (It is desirable in such cases to emit a warning.)
    //
    // The contents of the first link label are parsed as inlines, which are
    // used as the link's text.  The link's URI and title are provided by the
    // matching [link reference definition].
    //
    // Here is a simple example:
    [Fact]
    public void InlinesLinks_Example523()
    {
      // Example 523
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [foo][bar]
      //  
      //   [bar]: /url "title"
      //
      // Should be rendered as:
      //   <p><a href="/url" title="title">foo</a></p>

      Parser.Parse("[foo][bar]\n\n[bar]: /url \"title\"").Is(Parser.Prettify("<p><a href=\"/url\" title=\"title\">foo</a></p>"));

      Parser.DoubleParse("[foo][bar]\n\n[bar]: /url \"title\"").Is(Parser.Prettify("<p><a href=\"/url\" title=\"title\">foo</a></p>"));
    }

    // The rules for the [link text] are the same as with
    // [inline links].  Thus:
    //
    // The link text may contain balanced brackets, but not unbalanced ones,
    // unless they are escaped:
    [Fact]
    public void InlinesLinks_Example524()
    {
      // Example 524
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [link [foo [bar]]][ref]
      //  
      //   [ref]: /uri
      //
      // Should be rendered as:
      //   <p><a href="/uri">link [foo [bar]]</a></p>

      Parser.Parse("[link [foo [bar]]][ref]\n\n[ref]: /uri").Is(Parser.Prettify("<p><a href=\"/uri\">link [foo [bar]]</a></p>"));

      Parser.DoubleParse("[link [foo [bar]]][ref]\n\n[ref]: /uri").Is(Parser.Prettify("<p><a href=\"/uri\">link [foo [bar]]</a></p>"));
    }

    [Fact]
    public void InlinesLinks_Example525()
    {
      // Example 525
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [link \[bar][ref]
      //  
      //   [ref]: /uri
      //
      // Should be rendered as:
      //   <p><a href="/uri">link [bar</a></p>

      Parser.Parse("[link \\[bar][ref]\n\n[ref]: /uri").Is(Parser.Prettify("<p><a href=\"/uri\">link [bar</a></p>"));

      Parser.DoubleParse("[link \\[bar][ref]\n\n[ref]: /uri").Is(Parser.Prettify("<p><a href=\"/uri\">link [bar</a></p>"));
    }

    // The link text may contain inline content:
    [Fact]
    public void InlinesLinks_Example526()
    {
      // Example 526
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [link *foo **bar** `#`*][ref]
      //  
      //   [ref]: /uri
      //
      // Should be rendered as:
      //   <p><a href="/uri">link <em>foo <strong>bar</strong> <code>#</code></em></a></p>

      Parser.Parse("[link *foo **bar** `#`*][ref]\n\n[ref]: /uri").Is(Parser.Prettify("<p><a href=\"/uri\">link <em>foo <strong>bar</strong> <code>#</code></em></a></p>"));

      Parser.DoubleParse("[link *foo **bar** `#`*][ref]\n\n[ref]: /uri").Is(Parser.Prettify("<p><a href=\"/uri\">link <em>foo <strong>bar</strong> <code>#</code></em></a></p>"));
    }

    [Fact]
    public void InlinesLinks_Example527()
    {
      // Example 527
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [![moon](moon.jpg)][ref]
      //  
      //   [ref]: /uri
      //
      // Should be rendered as:
      //   <p><a href="/uri"><img src="moon.jpg" alt="moon" /></a></p>

      Parser.Parse("[![moon](moon.jpg)][ref]\n\n[ref]: /uri").Is(Parser.Prettify("<p><a href=\"/uri\"><img src=\"moon.jpg\" alt=\"moon\" /></a></p>"));

      Parser.DoubleParse("[![moon](moon.jpg)][ref]\n\n[ref]: /uri").Is(Parser.Prettify("<p><a href=\"/uri\"><img src=\"moon.jpg\" alt=\"moon\" /></a></p>"));
    }

    // However, links may not contain other links, at any level of nesting.
    [Fact]
    public void InlinesLinks_Example528()
    {
      // Example 528
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [foo [bar](/uri)][ref]
      //  
      //   [ref]: /uri
      //
      // Should be rendered as:
      //   <p>[foo <a href="/uri">bar</a>]<a href="/uri">ref</a></p>

      Parser.Parse("[foo [bar](/uri)][ref]\n\n[ref]: /uri").Is(Parser.Prettify("<p>[foo <a href=\"/uri\">bar</a>]<a href=\"/uri\">ref</a></p>"));

      Parser.DoubleParse("[foo [bar](/uri)][ref]\n\n[ref]: /uri").Is(Parser.Prettify("<p>[foo <a href=\"/uri\">bar</a>]<a href=\"/uri\">ref</a></p>"));
    }

    [Fact]
    public void InlinesLinks_Example529()
    {
      // Example 529
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [foo *bar [baz][ref]*][ref]
      //  
      //   [ref]: /uri
      //
      // Should be rendered as:
      //   <p>[foo <em>bar <a href="/uri">baz</a></em>]<a href="/uri">ref</a></p>

      Parser.Parse("[foo *bar [baz][ref]*][ref]\n\n[ref]: /uri").Is(Parser.Prettify("<p>[foo <em>bar <a href=\"/uri\">baz</a></em>]<a href=\"/uri\">ref</a></p>"));

      Parser.DoubleParse("[foo *bar [baz][ref]*][ref]\n\n[ref]: /uri").Is(Parser.Prettify("<p>[foo <em>bar <a href=\"/uri\">baz</a></em>]<a href=\"/uri\">ref</a></p>"));
    }

    // (In the examples above, we have two [shortcut reference links]
    // instead of one [full reference link].)
    //
    // The following cases illustrate the precedence of link text grouping over
    // emphasis grouping:
    [Fact]
    public void InlinesLinks_Example530()
    {
      // Example 530
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   *[foo*][ref]
      //  
      //   [ref]: /uri
      //
      // Should be rendered as:
      //   <p>*<a href="/uri">foo*</a></p>

      Parser.Parse("*[foo*][ref]\n\n[ref]: /uri").Is(Parser.Prettify("<p>*<a href=\"/uri\">foo*</a></p>"));

      Parser.DoubleParse("*[foo*][ref]\n\n[ref]: /uri").Is(Parser.Prettify("<p>*<a href=\"/uri\">foo*</a></p>"));
    }

    [Fact]
    public void InlinesLinks_Example531()
    {
      // Example 531
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [foo *bar][ref]
      //  
      //   [ref]: /uri
      //
      // Should be rendered as:
      //   <p><a href="/uri">foo *bar</a></p>

      Parser.Parse("[foo *bar][ref]\n\n[ref]: /uri").Is(Parser.Prettify("<p><a href=\"/uri\">foo *bar</a></p>"));

      Parser.DoubleParse("[foo *bar][ref]\n\n[ref]: /uri").Is(Parser.Prettify("<p><a href=\"/uri\">foo *bar</a></p>"));
    }

    // These cases illustrate the precedence of HTML tags, code spans,
    // and autolinks over link grouping:
    [Fact]
    public void InlinesLinks_Example532()
    {
      // Example 532
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [foo <bar attr="][ref]">
      //  
      //   [ref]: /uri
      //
      // Should be rendered as:
      //   <p>[foo <bar attr="][ref]"></p>

      Parser.Parse("[foo <bar attr=\"][ref]\">\n\n[ref]: /uri").Is(Parser.Prettify("<p>[foo <bar attr=\"][ref]\"></p>"));

      Parser.DoubleParse("[foo <bar attr=\"][ref]\">\n\n[ref]: /uri").Is(Parser.Prettify("<p>[foo <bar attr=\"][ref]\"></p>"));
    }

    [Fact]
    public void InlinesLinks_Example533()
    {
      // Example 533
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [foo`][ref]`
      //  
      //   [ref]: /uri
      //
      // Should be rendered as:
      //   <p>[foo<code>][ref]</code></p>

      Parser.Parse("[foo`][ref]`\n\n[ref]: /uri").Is(Parser.Prettify("<p>[foo<code>][ref]</code></p>"));

      Parser.DoubleParse("[foo`][ref]`\n\n[ref]: /uri").Is(Parser.Prettify("<p>[foo<code>][ref]</code></p>"));
    }

    [Fact]
    public void InlinesLinks_Example534()
    {
      // Example 534
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [foo<http://example.com/?search=][ref]>
      //  
      //   [ref]: /uri
      //
      // Should be rendered as:
      //   <p>[foo<a href="http://example.com/?search=%5D%5Bref%5D">http://example.com/?search=][ref]</a></p>

      Parser.Parse("[foo<http://example.com/?search=][ref]>\n\n[ref]: /uri").Is(Parser.Prettify("<p>[foo<a href=\"http://example.com/?search=%5D%5Bref%5D\">http://example.com/?search=][ref]</a></p>"));

      Parser.DoubleParse("[foo<http://example.com/?search=][ref]>\n\n[ref]: /uri").Is(Parser.Prettify("<p>[foo<a href=\"http://example.com/?search=%5D%5Bref%5D\">http://example.com/?search=][ref]</a></p>"));
    }

    // Matching is case-insensitive:
    [Fact]
    public void InlinesLinks_Example535()
    {
      // Example 535
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [foo][BaR]
      //  
      //   [bar]: /url "title"
      //
      // Should be rendered as:
      //   <p><a href="/url" title="title">foo</a></p>

      Parser.Parse("[foo][BaR]\n\n[bar]: /url \"title\"").Is(Parser.Prettify("<p><a href=\"/url\" title=\"title\">foo</a></p>"));

      Parser.DoubleParse("[foo][BaR]\n\n[bar]: /url \"title\"").Is(Parser.Prettify("<p><a href=\"/url\" title=\"title\">foo</a></p>"));
    }

    // Unicode case fold is used:
    [Fact]
    public void InlinesLinks_Example536()
    {
      // Example 536
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [Толпой][Толпой] is a Russian word.
      //  
      //   [ТОЛПОЙ]: /url
      //
      // Should be rendered as:
      //   <p><a href="/url">Толпой</a> is a Russian word.</p>

      Parser.Parse("[Толпой][Толпой] is a Russian word.\n\n[ТОЛПОЙ]: /url").Is(Parser.Prettify("<p><a href=\"/url\">Толпой</a> is a Russian word.</p>"));

      Parser.DoubleParse("[Толпой][Толпой] is a Russian word.\n\n[ТОЛПОЙ]: /url").Is(Parser.Prettify("<p><a href=\"/url\">Толпой</a> is a Russian word.</p>"));
    }

    // Consecutive internal [whitespace] is treated as one space for
    // purposes of determining matching:
    [Fact]
    public void InlinesLinks_Example537()
    {
      // Example 537
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [Foo
      //     bar]: /url
      //  
      //   [Baz][Foo bar]
      //
      // Should be rendered as:
      //   <p><a href="/url">Baz</a></p>

      Parser.Parse("[Foo\n  bar]: /url\n\n[Baz][Foo bar]").Is(Parser.Prettify("<p><a href=\"/url\">Baz</a></p>"));

      Parser.DoubleParse("[Foo\n  bar]: /url\n\n[Baz][Foo bar]").Is(Parser.Prettify("<p><a href=\"/url\">Baz</a></p>"));
    }

    // No [whitespace] is allowed between the [link text] and the
    // [link label]:
    [Fact]
    public void InlinesLinks_Example538()
    {
      // Example 538
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [foo] [bar]
      //  
      //   [bar]: /url "title"
      //
      // Should be rendered as:
      //   <p>[foo] <a href="/url" title="title">bar</a></p>

      Parser.Parse("[foo] [bar]\n\n[bar]: /url \"title\"").Is(Parser.Prettify("<p>[foo] <a href=\"/url\" title=\"title\">bar</a></p>"));

      Parser.DoubleParse("[foo] [bar]\n\n[bar]: /url \"title\"").Is(Parser.Prettify("<p>[foo] <a href=\"/url\" title=\"title\">bar</a></p>"));
    }

    [Fact]
    public void InlinesLinks_Example539()
    {
      // Example 539
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [foo]
      //   [bar]
      //  
      //   [bar]: /url "title"
      //
      // Should be rendered as:
      //   <p>[foo]
      //   <a href="/url" title="title">bar</a></p>

      Parser.Parse("[foo]\n[bar]\n\n[bar]: /url \"title\"").Is(Parser.Prettify("<p>[foo]\n<a href=\"/url\" title=\"title\">bar</a></p>"));

      Parser.DoubleParse("[foo]\n[bar]\n\n[bar]: /url \"title\"").Is(Parser.Prettify("<p>[foo]\n<a href=\"/url\" title=\"title\">bar</a></p>"));
    }

    // This is a departure from John Gruber's original Markdown syntax
    // description, which explicitly allows whitespace between the link
    // text and the link label.  It brings reference links in line with
    // [inline links], which (according to both original Markdown and
    // this spec) cannot have whitespace after the link text.  More
    // importantly, it prevents inadvertent capture of consecutive
    // [shortcut reference links]. If whitespace is allowed between the
    // link text and the link label, then in the following we will have
    // a single reference link, not two shortcut reference links, as
    // intended:
    //
    // ``` markdown
    // [foo]
    // [bar]
    //
    // [foo]: /url1
    // [bar]: /url2
    // ```
    //
    // (Note that [shortcut reference links] were introduced by Gruber
    // himself in a beta version of `Markdown.pl`, but never included
    // in the official syntax description.  Without shortcut reference
    // links, it is harmless to allow space between the link text and
    // link label; but once shortcut references are introduced, it is
    // too dangerous to allow this, as it frequently leads to
    // unintended results.)
    //
    // When there are multiple matching [link reference definitions],
    // the first is used:
    [Fact]
    public void InlinesLinks_Example540()
    {
      // Example 540
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [foo]: /url1
      //  
      //   [foo]: /url2
      //  
      //   [bar][foo]
      //
      // Should be rendered as:
      //   <p><a href="/url1">bar</a></p>

      Parser.Parse("[foo]: /url1\n\n[foo]: /url2\n\n[bar][foo]").Is(Parser.Prettify("<p><a href=\"/url1\">bar</a></p>"));

      Parser.DoubleParse("[foo]: /url1\n\n[foo]: /url2\n\n[bar][foo]").Is(Parser.Prettify("<p><a href=\"/url1\">bar</a></p>"));
    }

    // Note that matching is performed on normalized strings, not parsed
    // inline content.  So the following does not match, even though the
    // labels define equivalent inline content:
    [Fact]
    public void InlinesLinks_Example541()
    {
      // Example 541
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [bar][foo\!]
      //  
      //   [foo!]: /url
      //
      // Should be rendered as:
      //   <p>[bar][foo!]</p>

      Parser.Parse("[bar][foo\\!]\n\n[foo!]: /url").Is(Parser.Prettify("<p>[bar][foo!]</p>"));

      Parser.DoubleParse("[bar][foo\\!]\n\n[foo!]: /url").Is(Parser.Prettify("<p>[bar][foo!]</p>"));
    }

    // [Link labels] cannot contain brackets, unless they are
    // backslash-escaped:
    [Fact]
    public void InlinesLinks_Example542()
    {
      // Example 542
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [foo][ref[]
      //  
      //   [ref[]: /uri
      //
      // Should be rendered as:
      //   <p>[foo][ref[]</p>
      //   <p>[ref[]: /uri</p>

      Parser.Parse("[foo][ref[]\n\n[ref[]: /uri").Is(Parser.Prettify("<p>[foo][ref[]</p>\n<p>[ref[]: /uri</p>"));

      Parser.DoubleParse("[foo][ref[]\n\n[ref[]: /uri").Is(Parser.Prettify("<p>[foo][ref[]</p>\n<p>[ref[]: /uri</p>"));
    }

    [Fact]
    public void InlinesLinks_Example543()
    {
      // Example 543
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [foo][ref[bar]]
      //  
      //   [ref[bar]]: /uri
      //
      // Should be rendered as:
      //   <p>[foo][ref[bar]]</p>
      //   <p>[ref[bar]]: /uri</p>

      Parser.Parse("[foo][ref[bar]]\n\n[ref[bar]]: /uri").Is(Parser.Prettify("<p>[foo][ref[bar]]</p>\n<p>[ref[bar]]: /uri</p>"));

      Parser.DoubleParse("[foo][ref[bar]]\n\n[ref[bar]]: /uri").Is(Parser.Prettify("<p>[foo][ref[bar]]</p>\n<p>[ref[bar]]: /uri</p>"));
    }

    [Fact]
    public void InlinesLinks_Example544()
    {
      // Example 544
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [[[foo]]]
      //  
      //   [[[foo]]]: /url
      //
      // Should be rendered as:
      //   <p>[[[foo]]]</p>
      //   <p>[[[foo]]]: /url</p>

      Parser.Parse("[[[foo]]]\n\n[[[foo]]]: /url").Is(Parser.Prettify("<p>[[[foo]]]</p>\n<p>[[[foo]]]: /url</p>"));

      Parser.DoubleParse("[[[foo]]]\n\n[[[foo]]]: /url").Is(Parser.Prettify("<p>[[[foo]]]</p>\n<p>[[[foo]]]: /url</p>"));
    }

    [Fact]
    public void InlinesLinks_Example545()
    {
      // Example 545
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [foo][ref\[]
      //  
      //   [ref\[]: /uri
      //
      // Should be rendered as:
      //   <p><a href="/uri">foo</a></p>

      Parser.Parse("[foo][ref\\[]\n\n[ref\\[]: /uri").Is(Parser.Prettify("<p><a href=\"/uri\">foo</a></p>"));

      Parser.DoubleParse("[foo][ref\\[]\n\n[ref\\[]: /uri").Is(Parser.Prettify("<p><a href=\"/uri\">foo</a></p>"));
    }

    // Note that in this example `]` is not backslash-escaped:
    [Fact]
    public void InlinesLinks_Example546()
    {
      // Example 546
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [bar\\]: /uri
      //  
      //   [bar\\]
      //
      // Should be rendered as:
      //   <p><a href="/uri">bar\</a></p>

      Parser.Parse("[bar\\\\]: /uri\n\n[bar\\\\]").Is(Parser.Prettify("<p><a href=\"/uri\">bar\\</a></p>"));

      Parser.DoubleParse("[bar\\\\]: /uri\n\n[bar\\\\]").Is(Parser.Prettify("<p><a href=\"/uri\">bar\\</a></p>"));
    }

    // A [link label] must contain at least one [non-whitespace character]:
    [Fact]
    public void InlinesLinks_Example547()
    {
      // Example 547
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   []
      //  
      //   []: /uri
      //
      // Should be rendered as:
      //   <p>[]</p>
      //   <p>[]: /uri</p>

      Parser.Parse("[]\n\n[]: /uri").Is(Parser.Prettify("<p>[]</p>\n<p>[]: /uri</p>"));

      Parser.DoubleParse("[]\n\n[]: /uri").Is(Parser.Prettify("<p>[]</p>\n<p>[]: /uri</p>"));
    }

    [Fact]
    public void InlinesLinks_Example548()
    {
      // Example 548
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [
      //    ]
      //  
      //   [
      //    ]: /uri
      //
      // Should be rendered as:
      //   <p>[
      //   ]</p>
      //   <p>[
      //   ]: /uri</p>

      Parser.Parse("[\n ]\n\n[\n ]: /uri").Is(Parser.Prettify("<p>[\n]</p>\n<p>[\n]: /uri</p>"));

      Parser.DoubleParse("[\n ]\n\n[\n ]: /uri").Is(Parser.Prettify("<p>[\n]</p>\n<p>[\n]: /uri</p>"));
    }

    // A [collapsed reference link](@)
    // consists of a [link label] that [matches] a
    // [link reference definition] elsewhere in the
    // document, followed by the string `[]`.
    // The contents of the first link label are parsed as inlines,
    // which are used as the link's text.  The link's URI and title are
    // provided by the matching reference link definition.  Thus,
    // `[foo][]` is equivalent to `[foo][foo]`.
    [Fact]
    public void InlinesLinks_Example549()
    {
      // Example 549
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [foo][]
      //  
      //   [foo]: /url "title"
      //
      // Should be rendered as:
      //   <p><a href="/url" title="title">foo</a></p>

      Parser.Parse("[foo][]\n\n[foo]: /url \"title\"").Is(Parser.Prettify("<p><a href=\"/url\" title=\"title\">foo</a></p>"));

      Parser.DoubleParse("[foo][]\n\n[foo]: /url \"title\"").Is(Parser.Prettify("<p><a href=\"/url\" title=\"title\">foo</a></p>"));
    }

    [Fact]
    public void InlinesLinks_Example550()
    {
      // Example 550
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [*foo* bar][]
      //  
      //   [*foo* bar]: /url "title"
      //
      // Should be rendered as:
      //   <p><a href="/url" title="title"><em>foo</em> bar</a></p>

      Parser.Parse("[*foo* bar][]\n\n[*foo* bar]: /url \"title\"").Is(Parser.Prettify("<p><a href=\"/url\" title=\"title\"><em>foo</em> bar</a></p>"));

      Parser.DoubleParse("[*foo* bar][]\n\n[*foo* bar]: /url \"title\"").Is(Parser.Prettify("<p><a href=\"/url\" title=\"title\"><em>foo</em> bar</a></p>"));
    }

    // The link labels are case-insensitive:
    [Fact]
    public void InlinesLinks_Example551()
    {
      // Example 551
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [Foo][]
      //  
      //   [foo]: /url "title"
      //
      // Should be rendered as:
      //   <p><a href="/url" title="title">Foo</a></p>

      Parser.Parse("[Foo][]\n\n[foo]: /url \"title\"").Is(Parser.Prettify("<p><a href=\"/url\" title=\"title\">Foo</a></p>"));

      Parser.DoubleParse("[Foo][]\n\n[foo]: /url \"title\"").Is(Parser.Prettify("<p><a href=\"/url\" title=\"title\">Foo</a></p>"));
    }

    // As with full reference links, [whitespace] is not
    // allowed between the two sets of brackets:
    [Fact]
    public void InlinesLinks_Example552()
    {
      // Example 552
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [foo] 
      //   []
      //  
      //   [foo]: /url "title"
      //
      // Should be rendered as:
      //   <p><a href="/url" title="title">foo</a>
      //   []</p>

      Parser.Parse("[foo] \n[]\n\n[foo]: /url \"title\"").Is(Parser.Prettify("<p><a href=\"/url\" title=\"title\">foo</a>\n[]</p>"));

      Parser.DoubleParse("[foo] \n[]\n\n[foo]: /url \"title\"").Is(Parser.Prettify("<p><a href=\"/url\" title=\"title\">foo</a>\n[]</p>"));
    }

    // A [shortcut reference link](@)
    // consists of a [link label] that [matches] a
    // [link reference definition] elsewhere in the
    // document and is not followed by `[]` or a link label.
    // The contents of the first link label are parsed as inlines,
    // which are used as the link's text.  The link's URI and title
    // are provided by the matching link reference definition.
    // Thus, `[foo]` is equivalent to `[foo][]`.
    [Fact]
    public void InlinesLinks_Example553()
    {
      // Example 553
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [foo]
      //  
      //   [foo]: /url "title"
      //
      // Should be rendered as:
      //   <p><a href="/url" title="title">foo</a></p>

      Parser.Parse("[foo]\n\n[foo]: /url \"title\"").Is(Parser.Prettify("<p><a href=\"/url\" title=\"title\">foo</a></p>"));

      Parser.DoubleParse("[foo]\n\n[foo]: /url \"title\"").Is(Parser.Prettify("<p><a href=\"/url\" title=\"title\">foo</a></p>"));
    }

    [Fact]
    public void InlinesLinks_Example554()
    {
      // Example 554
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [*foo* bar]
      //  
      //   [*foo* bar]: /url "title"
      //
      // Should be rendered as:
      //   <p><a href="/url" title="title"><em>foo</em> bar</a></p>

      Parser.Parse("[*foo* bar]\n\n[*foo* bar]: /url \"title\"").Is(Parser.Prettify("<p><a href=\"/url\" title=\"title\"><em>foo</em> bar</a></p>"));

      Parser.DoubleParse("[*foo* bar]\n\n[*foo* bar]: /url \"title\"").Is(Parser.Prettify("<p><a href=\"/url\" title=\"title\"><em>foo</em> bar</a></p>"));
    }

    [Fact]
    public void InlinesLinks_Example555()
    {
      // Example 555
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [[*foo* bar]]
      //  
      //   [*foo* bar]: /url "title"
      //
      // Should be rendered as:
      //   <p>[<a href="/url" title="title"><em>foo</em> bar</a>]</p>

      Parser.Parse("[[*foo* bar]]\n\n[*foo* bar]: /url \"title\"").Is(Parser.Prettify("<p>[<a href=\"/url\" title=\"title\"><em>foo</em> bar</a>]</p>"));

      Parser.DoubleParse("[[*foo* bar]]\n\n[*foo* bar]: /url \"title\"").Is(Parser.Prettify("<p>[<a href=\"/url\" title=\"title\"><em>foo</em> bar</a>]</p>"));
    }

    [Fact]
    public void InlinesLinks_Example556()
    {
      // Example 556
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [[bar [foo]
      //  
      //   [foo]: /url
      //
      // Should be rendered as:
      //   <p>[[bar <a href="/url">foo</a></p>

      Parser.Parse("[[bar [foo]\n\n[foo]: /url").Is(Parser.Prettify("<p>[[bar <a href=\"/url\">foo</a></p>"));

      Parser.DoubleParse("[[bar [foo]\n\n[foo]: /url").Is(Parser.Prettify("<p>[[bar <a href=\"/url\">foo</a></p>"));
    }

    // The link labels are case-insensitive:
    [Fact]
    public void InlinesLinks_Example557()
    {
      // Example 557
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [Foo]
      //  
      //   [foo]: /url "title"
      //
      // Should be rendered as:
      //   <p><a href="/url" title="title">Foo</a></p>

      Parser.Parse("[Foo]\n\n[foo]: /url \"title\"").Is(Parser.Prettify("<p><a href=\"/url\" title=\"title\">Foo</a></p>"));

      Parser.DoubleParse("[Foo]\n\n[foo]: /url \"title\"").Is(Parser.Prettify("<p><a href=\"/url\" title=\"title\">Foo</a></p>"));
    }

    // A space after the link text should be preserved:
    [Fact]
    public void InlinesLinks_Example558()
    {
      // Example 558
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [foo] bar
      //  
      //   [foo]: /url
      //
      // Should be rendered as:
      //   <p><a href="/url">foo</a> bar</p>

      Parser.Parse("[foo] bar\n\n[foo]: /url").Is(Parser.Prettify("<p><a href=\"/url\">foo</a> bar</p>"));

      Parser.DoubleParse("[foo] bar\n\n[foo]: /url").Is(Parser.Prettify("<p><a href=\"/url\">foo</a> bar</p>"));
    }

    // If you just want bracketed text, you can backslash-escape the
    // opening bracket to avoid links:
    [Fact]
    public void InlinesLinks_Example559()
    {
      // Example 559
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   \[foo]
      //  
      //   [foo]: /url "title"
      //
      // Should be rendered as:
      //   <p>[foo]</p>

      Parser.Parse("\\[foo]\n\n[foo]: /url \"title\"").Is(Parser.Prettify("<p>[foo]</p>"));

      Parser.DoubleParse("\\[foo]\n\n[foo]: /url \"title\"").Is(Parser.Prettify("<p>[foo]</p>"));
    }

    // Note that this is a link, because a link label ends with the first
    // following closing bracket:
    [Fact]
    public void InlinesLinks_Example560()
    {
      // Example 560
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [foo*]: /url
      //  
      //   *[foo*]
      //
      // Should be rendered as:
      //   <p>*<a href="/url">foo*</a></p>

      Parser.Parse("[foo*]: /url\n\n*[foo*]").Is(Parser.Prettify("<p>*<a href=\"/url\">foo*</a></p>"));

      Parser.DoubleParse("[foo*]: /url\n\n*[foo*]").Is(Parser.Prettify("<p>*<a href=\"/url\">foo*</a></p>"));
    }

    // Full and compact references take precedence over shortcut
    // references:
    [Fact]
    public void InlinesLinks_Example561()
    {
      // Example 561
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [foo][bar]
      //  
      //   [foo]: /url1
      //   [bar]: /url2
      //
      // Should be rendered as:
      //   <p><a href="/url2">foo</a></p>

      Parser.Parse("[foo][bar]\n\n[foo]: /url1\n[bar]: /url2").Is(Parser.Prettify("<p><a href=\"/url2\">foo</a></p>"));

      Parser.DoubleParse("[foo][bar]\n\n[foo]: /url1\n[bar]: /url2").Is(Parser.Prettify("<p><a href=\"/url2\">foo</a></p>"));
    }

    [Fact]
    public void InlinesLinks_Example562()
    {
      // Example 562
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [foo][]
      //  
      //   [foo]: /url1
      //
      // Should be rendered as:
      //   <p><a href="/url1">foo</a></p>

      Parser.Parse("[foo][]\n\n[foo]: /url1").Is(Parser.Prettify("<p><a href=\"/url1\">foo</a></p>"));

      Parser.DoubleParse("[foo][]\n\n[foo]: /url1").Is(Parser.Prettify("<p><a href=\"/url1\">foo</a></p>"));
    }

    // Inline links also take precedence:
    [Fact]
    public void InlinesLinks_Example563()
    {
      // Example 563
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [foo]()
      //  
      //   [foo]: /url1
      //
      // Should be rendered as:
      //   <p><a href="">foo</a></p>

      Parser.Parse("[foo]()\n\n[foo]: /url1").Is(Parser.Prettify("<p><a href=\"\">foo</a></p>"));

      Parser.DoubleParse("[foo]()\n\n[foo]: /url1").Is(Parser.Prettify("<p><a href=\"\">foo</a></p>"));
    }

    [Fact]
    public void InlinesLinks_Example564()
    {
      // Example 564
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [foo](not a link)
      //  
      //   [foo]: /url1
      //
      // Should be rendered as:
      //   <p><a href="/url1">foo</a>(not a link)</p>

      Parser.Parse("[foo](not a link)\n\n[foo]: /url1").Is(Parser.Prettify("<p><a href=\"/url1\">foo</a>(not a link)</p>"));

      Parser.DoubleParse("[foo](not a link)\n\n[foo]: /url1").Is(Parser.Prettify("<p><a href=\"/url1\">foo</a>(not a link)</p>"));
    }

    // In the following case `[bar][baz]` is parsed as a reference,
    // `[foo]` as normal text:
    [Fact]
    public void InlinesLinks_Example565()
    {
      // Example 565
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [foo][bar][baz]
      //  
      //   [baz]: /url
      //
      // Should be rendered as:
      //   <p>[foo]<a href="/url">bar</a></p>

      Parser.Parse("[foo][bar][baz]\n\n[baz]: /url").Is(Parser.Prettify("<p>[foo]<a href=\"/url\">bar</a></p>"));

      Parser.DoubleParse("[foo][bar][baz]\n\n[baz]: /url").Is(Parser.Prettify("<p>[foo]<a href=\"/url\">bar</a></p>"));
    }

    // Here, though, `[foo][bar]` is parsed as a reference, since
    // `[bar]` is defined:
    [Fact]
    public void InlinesLinks_Example566()
    {
      // Example 566
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [foo][bar][baz]
      //  
      //   [baz]: /url1
      //   [bar]: /url2
      //
      // Should be rendered as:
      //   <p><a href="/url2">foo</a><a href="/url1">baz</a></p>

      Parser.Parse("[foo][bar][baz]\n\n[baz]: /url1\n[bar]: /url2").Is(Parser.Prettify("<p><a href=\"/url2\">foo</a><a href=\"/url1\">baz</a></p>"));

      Parser.DoubleParse("[foo][bar][baz]\n\n[baz]: /url1\n[bar]: /url2").Is(Parser.Prettify("<p><a href=\"/url2\">foo</a><a href=\"/url1\">baz</a></p>"));
    }

    // Here `[foo]` is not parsed as a shortcut reference, because it
    // is followed by a link label (even though `[bar]` is not defined):
    [Fact]
    public void InlinesLinks_Example567()
    {
      // Example 567
      // Section: Inlines / Links
      //
      // The following Markdown:
      //   [foo][bar][baz]
      //  
      //   [baz]: /url1
      //   [foo]: /url2
      //
      // Should be rendered as:
      //   <p>[foo]<a href="/url1">bar</a></p>

      Parser.Parse("[foo][bar][baz]\n\n[baz]: /url1\n[foo]: /url2").Is(Parser.Prettify("<p>[foo]<a href=\"/url1\">bar</a></p>"));

      Parser.DoubleParse("[foo][bar][baz]\n\n[baz]: /url1\n[foo]: /url2").Is(Parser.Prettify("<p>[foo]<a href=\"/url1\">bar</a></p>"));
    }
  }

  public class TestInlinesImages
  {
    // ## Images
    //
    // Syntax for images is like the syntax for links, with one
    // difference. Instead of [link text], we have an
    // [image description](@).  The rules for this are the
    // same as for [link text], except that (a) an
    // image description starts with `![` rather than `[`, and
    // (b) an image description may contain links.
    // An image description has inline elements
    // as its contents.  When an image is rendered to HTML,
    // this is standardly used as the image's `alt` attribute.
    [Fact]
    public void InlinesImages_Example568()
    {
      // Example 568
      // Section: Inlines / Images
      //
      // The following Markdown:
      //   ![foo](/url "title")
      //
      // Should be rendered as:
      //   <p><img src="/url" alt="foo" title="title" /></p>

      Parser.Parse("![foo](/url \"title\")").Is(Parser.Prettify("<p><img src=\"/url\" alt=\"foo\" title=\"title\" /></p>"));

      Parser.DoubleParse("![foo](/url \"title\")").Is(Parser.Prettify("<p><img src=\"/url\" alt=\"foo\" title=\"title\" /></p>"));
    }

    [Fact]
    public void InlinesImages_Example569()
    {
      // Example 569
      // Section: Inlines / Images
      //
      // The following Markdown:
      //   ![foo *bar*]
      //  
      //   [foo *bar*]: train.jpg "train & tracks"
      //
      // Should be rendered as:
      //   <p><img src="train.jpg" alt="foo bar" title="train &amp; tracks" /></p>

      Parser.Parse("![foo *bar*]\n\n[foo *bar*]: train.jpg \"train & tracks\"").Is(Parser.Prettify("<p><img src=\"train.jpg\" alt=\"foo bar\" title=\"train &amp; tracks\" /></p>"));

      Parser.DoubleParse("![foo *bar*]\n\n[foo *bar*]: train.jpg \"train & tracks\"").Is(Parser.Prettify("<p><img src=\"train.jpg\" alt=\"foo bar\" title=\"train &amp; tracks\" /></p>"));
    }

    [Fact]
    public void InlinesImages_Example570()
    {
      // Example 570
      // Section: Inlines / Images
      //
      // The following Markdown:
      //   ![foo ![bar](/url)](/url2)
      //
      // Should be rendered as:
      //   <p><img src="/url2" alt="foo bar" /></p>

      Parser.Parse("![foo ![bar](/url)](/url2)").Is(Parser.Prettify("<p><img src=\"/url2\" alt=\"foo bar\" /></p>"));

      Parser.DoubleParse("![foo ![bar](/url)](/url2)").Is(Parser.Prettify("<p><img src=\"/url2\" alt=\"foo bar\" /></p>"));
    }

    [Fact]
    public void InlinesImages_Example571()
    {
      // Example 571
      // Section: Inlines / Images
      //
      // The following Markdown:
      //   ![foo [bar](/url)](/url2)
      //
      // Should be rendered as:
      //   <p><img src="/url2" alt="foo bar" /></p>

      Parser.Parse("![foo [bar](/url)](/url2)").Is(Parser.Prettify("<p><img src=\"/url2\" alt=\"foo bar\" /></p>"));

      Parser.DoubleParse("![foo [bar](/url)](/url2)").Is(Parser.Prettify("<p><img src=\"/url2\" alt=\"foo bar\" /></p>"));
    }

    // Though this spec is concerned with parsing, not rendering, it is
    // recommended that in rendering to HTML, only the plain string content
    // of the [image description] be used.  Note that in
    // the above example, the alt attribute's value is `foo bar`, not `foo
    // [bar](/url)` or `foo <a href="/url">bar</a>`.  Only the plain string
    // content is rendered, without formatting.
    [Fact]
    public void InlinesImages_Example572()
    {
      // Example 572
      // Section: Inlines / Images
      //
      // The following Markdown:
      //   ![foo *bar*][]
      //  
      //   [foo *bar*]: train.jpg "train & tracks"
      //
      // Should be rendered as:
      //   <p><img src="train.jpg" alt="foo bar" title="train &amp; tracks" /></p>

      Parser.Parse("![foo *bar*][]\n\n[foo *bar*]: train.jpg \"train & tracks\"").Is(Parser.Prettify("<p><img src=\"train.jpg\" alt=\"foo bar\" title=\"train &amp; tracks\" /></p>"));

      Parser.DoubleParse("![foo *bar*][]\n\n[foo *bar*]: train.jpg \"train & tracks\"").Is(Parser.Prettify("<p><img src=\"train.jpg\" alt=\"foo bar\" title=\"train &amp; tracks\" /></p>"));
    }

    [Fact]
    public void InlinesImages_Example573()
    {
      // Example 573
      // Section: Inlines / Images
      //
      // The following Markdown:
      //   ![foo *bar*][foobar]
      //  
      //   [FOOBAR]: train.jpg "train & tracks"
      //
      // Should be rendered as:
      //   <p><img src="train.jpg" alt="foo bar" title="train &amp; tracks" /></p>

      Parser.Parse("![foo *bar*][foobar]\n\n[FOOBAR]: train.jpg \"train & tracks\"").Is(Parser.Prettify("<p><img src=\"train.jpg\" alt=\"foo bar\" title=\"train &amp; tracks\" /></p>"));

      Parser.DoubleParse("![foo *bar*][foobar]\n\n[FOOBAR]: train.jpg \"train & tracks\"").Is(Parser.Prettify("<p><img src=\"train.jpg\" alt=\"foo bar\" title=\"train &amp; tracks\" /></p>"));
    }

    [Fact]
    public void InlinesImages_Example574()
    {
      // Example 574
      // Section: Inlines / Images
      //
      // The following Markdown:
      //   ![foo](train.jpg)
      //
      // Should be rendered as:
      //   <p><img src="train.jpg" alt="foo" /></p>

      Parser.Parse("![foo](train.jpg)").Is(Parser.Prettify("<p><img src=\"train.jpg\" alt=\"foo\" /></p>"));

      Parser.DoubleParse("![foo](train.jpg)").Is(Parser.Prettify("<p><img src=\"train.jpg\" alt=\"foo\" /></p>"));
    }

    [Fact]
    public void InlinesImages_Example575()
    {
      // Example 575
      // Section: Inlines / Images
      //
      // The following Markdown:
      //   My ![foo bar](/path/to/train.jpg  "title"   )
      //
      // Should be rendered as:
      //   <p>My <img src="/path/to/train.jpg" alt="foo bar" title="title" /></p>

      Parser.Parse("My ![foo bar](/path/to/train.jpg  \"title\"   )").Is(Parser.Prettify("<p>My <img src=\"/path/to/train.jpg\" alt=\"foo bar\" title=\"title\" /></p>"));

      Parser.DoubleParse("My ![foo bar](/path/to/train.jpg  \"title\"   )").Is(Parser.Prettify("<p>My <img src=\"/path/to/train.jpg\" alt=\"foo bar\" title=\"title\" /></p>"));
    }

    [Fact]
    public void InlinesImages_Example576()
    {
      // Example 576
      // Section: Inlines / Images
      //
      // The following Markdown:
      //   ![foo](<url>)
      //
      // Should be rendered as:
      //   <p><img src="url" alt="foo" /></p>

      Parser.Parse("![foo](<url>)").Is(Parser.Prettify("<p><img src=\"url\" alt=\"foo\" /></p>"));

      Parser.DoubleParse("![foo](<url>)").Is(Parser.Prettify("<p><img src=\"url\" alt=\"foo\" /></p>"));
    }

    [Fact]
    public void InlinesImages_Example577()
    {
      // Example 577
      // Section: Inlines / Images
      //
      // The following Markdown:
      //   ![](/url)
      //
      // Should be rendered as:
      //   <p><img src="/url" alt="" /></p>

      Parser.Parse("![](/url)").Is(Parser.Prettify("<p><img src=\"/url\" alt=\"\" /></p>"));

      Parser.DoubleParse("![](/url)").Is(Parser.Prettify("<p><img src=\"/url\" alt=\"\" /></p>"));
    }

    // Reference-style:
    [Fact]
    public void InlinesImages_Example578()
    {
      // Example 578
      // Section: Inlines / Images
      //
      // The following Markdown:
      //   ![foo][bar]
      //  
      //   [bar]: /url
      //
      // Should be rendered as:
      //   <p><img src="/url" alt="foo" /></p>

      Parser.Parse("![foo][bar]\n\n[bar]: /url").Is(Parser.Prettify("<p><img src=\"/url\" alt=\"foo\" /></p>"));

      Parser.DoubleParse("![foo][bar]\n\n[bar]: /url").Is(Parser.Prettify("<p><img src=\"/url\" alt=\"foo\" /></p>"));
    }

    [Fact]
    public void InlinesImages_Example579()
    {
      // Example 579
      // Section: Inlines / Images
      //
      // The following Markdown:
      //   ![foo][bar]
      //  
      //   [BAR]: /url
      //
      // Should be rendered as:
      //   <p><img src="/url" alt="foo" /></p>

      Parser.Parse("![foo][bar]\n\n[BAR]: /url").Is(Parser.Prettify("<p><img src=\"/url\" alt=\"foo\" /></p>"));

      Parser.DoubleParse("![foo][bar]\n\n[BAR]: /url").Is(Parser.Prettify("<p><img src=\"/url\" alt=\"foo\" /></p>"));
    }

    // Collapsed:
    [Fact]
    public void InlinesImages_Example580()
    {
      // Example 580
      // Section: Inlines / Images
      //
      // The following Markdown:
      //   ![foo][]
      //  
      //   [foo]: /url "title"
      //
      // Should be rendered as:
      //   <p><img src="/url" alt="foo" title="title" /></p>

      Parser.Parse("![foo][]\n\n[foo]: /url \"title\"").Is(Parser.Prettify("<p><img src=\"/url\" alt=\"foo\" title=\"title\" /></p>"));

      Parser.DoubleParse("![foo][]\n\n[foo]: /url \"title\"").Is(Parser.Prettify("<p><img src=\"/url\" alt=\"foo\" title=\"title\" /></p>"));
    }

    [Fact]
    public void InlinesImages_Example581()
    {
      // Example 581
      // Section: Inlines / Images
      //
      // The following Markdown:
      //   ![*foo* bar][]
      //  
      //   [*foo* bar]: /url "title"
      //
      // Should be rendered as:
      //   <p><img src="/url" alt="foo bar" title="title" /></p>

      Parser.Parse("![*foo* bar][]\n\n[*foo* bar]: /url \"title\"").Is(Parser.Prettify("<p><img src=\"/url\" alt=\"foo bar\" title=\"title\" /></p>"));

      Parser.DoubleParse("![*foo* bar][]\n\n[*foo* bar]: /url \"title\"").Is(Parser.Prettify("<p><img src=\"/url\" alt=\"foo bar\" title=\"title\" /></p>"));
    }

    // The labels are case-insensitive:
    [Fact]
    public void InlinesImages_Example582()
    {
      // Example 582
      // Section: Inlines / Images
      //
      // The following Markdown:
      //   ![Foo][]
      //  
      //   [foo]: /url "title"
      //
      // Should be rendered as:
      //   <p><img src="/url" alt="Foo" title="title" /></p>

      Parser.Parse("![Foo][]\n\n[foo]: /url \"title\"").Is(Parser.Prettify("<p><img src=\"/url\" alt=\"Foo\" title=\"title\" /></p>"));

      Parser.DoubleParse("![Foo][]\n\n[foo]: /url \"title\"").Is(Parser.Prettify("<p><img src=\"/url\" alt=\"Foo\" title=\"title\" /></p>"));
    }

    // As with reference links, [whitespace] is not allowed
    // between the two sets of brackets:
    [Fact]
    public void InlinesImages_Example583()
    {
      // Example 583
      // Section: Inlines / Images
      //
      // The following Markdown:
      //   ![foo] 
      //   []
      //  
      //   [foo]: /url "title"
      //
      // Should be rendered as:
      //   <p><img src="/url" alt="foo" title="title" />
      //   []</p>

      Parser.Parse("![foo] \n[]\n\n[foo]: /url \"title\"").Is(Parser.Prettify("<p><img src=\"/url\" alt=\"foo\" title=\"title\" />\n[]</p>"));

      Parser.DoubleParse("![foo] \n[]\n\n[foo]: /url \"title\"").Is(Parser.Prettify("<p><img src=\"/url\" alt=\"foo\" title=\"title\" />\n[]</p>"));
    }

    // Shortcut:
    [Fact]
    public void InlinesImages_Example584()
    {
      // Example 584
      // Section: Inlines / Images
      //
      // The following Markdown:
      //   ![foo]
      //  
      //   [foo]: /url "title"
      //
      // Should be rendered as:
      //   <p><img src="/url" alt="foo" title="title" /></p>

      Parser.Parse("![foo]\n\n[foo]: /url \"title\"").Is(Parser.Prettify("<p><img src=\"/url\" alt=\"foo\" title=\"title\" /></p>"));

      Parser.DoubleParse("![foo]\n\n[foo]: /url \"title\"").Is(Parser.Prettify("<p><img src=\"/url\" alt=\"foo\" title=\"title\" /></p>"));
    }

    [Fact]
    public void InlinesImages_Example585()
    {
      // Example 585
      // Section: Inlines / Images
      //
      // The following Markdown:
      //   ![*foo* bar]
      //  
      //   [*foo* bar]: /url "title"
      //
      // Should be rendered as:
      //   <p><img src="/url" alt="foo bar" title="title" /></p>

      Parser.Parse("![*foo* bar]\n\n[*foo* bar]: /url \"title\"").Is(Parser.Prettify("<p><img src=\"/url\" alt=\"foo bar\" title=\"title\" /></p>"));

      Parser.DoubleParse("![*foo* bar]\n\n[*foo* bar]: /url \"title\"").Is(Parser.Prettify("<p><img src=\"/url\" alt=\"foo bar\" title=\"title\" /></p>"));
    }

    // Note that link labels cannot contain unescaped brackets:
    [Fact]
    public void InlinesImages_Example586()
    {
      // Example 586
      // Section: Inlines / Images
      //
      // The following Markdown:
      //   ![[foo]]
      //  
      //   [[foo]]: /url "title"
      //
      // Should be rendered as:
      //   <p>![[foo]]</p>
      //   <p>[[foo]]: /url &quot;title&quot;</p>

      Parser.Parse("![[foo]]\n\n[[foo]]: /url \"title\"").Is(Parser.Prettify("<p>![[foo]]</p>\n<p>[[foo]]: /url &quot;title&quot;</p>"));

      Parser.DoubleParse("![[foo]]\n\n[[foo]]: /url \"title\"").Is(Parser.Prettify("<p>![[foo]]</p>\n<p>[[foo]]: /url &quot;title&quot;</p>"));
    }

    // The link labels are case-insensitive:
    [Fact]
    public void InlinesImages_Example587()
    {
      // Example 587
      // Section: Inlines / Images
      //
      // The following Markdown:
      //   ![Foo]
      //  
      //   [foo]: /url "title"
      //
      // Should be rendered as:
      //   <p><img src="/url" alt="Foo" title="title" /></p>

      Parser.Parse("![Foo]\n\n[foo]: /url \"title\"").Is(Parser.Prettify("<p><img src=\"/url\" alt=\"Foo\" title=\"title\" /></p>"));

      Parser.DoubleParse("![Foo]\n\n[foo]: /url \"title\"").Is(Parser.Prettify("<p><img src=\"/url\" alt=\"Foo\" title=\"title\" /></p>"));
    }

    // If you just want a literal `!` followed by bracketed text, you can
    // backslash-escape the opening `[`:
    [Fact]
    public void InlinesImages_Example588()
    {
      // Example 588
      // Section: Inlines / Images
      //
      // The following Markdown:
      //   !\[foo]
      //  
      //   [foo]: /url "title"
      //
      // Should be rendered as:
      //   <p>![foo]</p>

      Parser.Parse("!\\[foo]\n\n[foo]: /url \"title\"").Is(Parser.Prettify("<p>![foo]</p>"));

      Parser.DoubleParse("!\\[foo]\n\n[foo]: /url \"title\"").Is(Parser.Prettify("<p>![foo]</p>"));
    }

    // If you want a link after a literal `!`, backslash-escape the
    // `!`:
    [Fact]
    public void InlinesImages_Example589()
    {
      // Example 589
      // Section: Inlines / Images
      //
      // The following Markdown:
      //   \![foo]
      //  
      //   [foo]: /url "title"
      //
      // Should be rendered as:
      //   <p>!<a href="/url" title="title">foo</a></p>

      Parser.Parse("\\![foo]\n\n[foo]: /url \"title\"").Is(Parser.Prettify("<p>!<a href=\"/url\" title=\"title\">foo</a></p>"));

      Parser.DoubleParse("\\![foo]\n\n[foo]: /url \"title\"").Is(Parser.Prettify("<p>!<a href=\"/url\" title=\"title\">foo</a></p>"));
    }
  }

  public class TestInlinesAutolinks
  {
    // ## Autolinks
    //
    // [Autolink](@)s are absolute URIs and email addresses inside
    // `<` and `>`. They are parsed as links, with the URL or email address
    // as the link label.
    //
    // A [URI autolink](@) consists of `<`, followed by an
    // [absolute URI] followed by `>`.  It is parsed as
    // a link to the URI, with the URI as the link's label.
    //
    // An [absolute URI](@),
    // for these purposes, consists of a [scheme] followed by a colon (`:`)
    // followed by zero or more characters other than ASCII
    // [whitespace] and control characters, `<`, and `>`.  If
    // the URI includes these characters, they must be percent-encoded
    // (e.g. `%20` for a space).
    //
    // For purposes of this spec, a [scheme](@) is any sequence
    // of 2--32 characters beginning with an ASCII letter and followed
    // by any combination of ASCII letters, digits, or the symbols plus
    // ("+"), period ("."), or hyphen ("-").
    //
    // Here are some valid autolinks:
    [Fact]
    public void InlinesAutolinks_Example590()
    {
      // Example 590
      // Section: Inlines / Autolinks
      //
      // The following Markdown:
      //   <http://foo.bar.baz>
      //
      // Should be rendered as:
      //   <p><a href="http://foo.bar.baz">http://foo.bar.baz</a></p>

      Parser.Parse("<http://foo.bar.baz>").Is(Parser.Prettify("<p><a href=\"http://foo.bar.baz\">http://foo.bar.baz</a></p>"));

      Parser.DoubleParse("<http://foo.bar.baz>").Is(Parser.Prettify("<p><a href=\"http://foo.bar.baz\">http://foo.bar.baz</a></p>"));
    }

    [Fact]
    public void InlinesAutolinks_Example591()
    {
      // Example 591
      // Section: Inlines / Autolinks
      //
      // The following Markdown:
      //   <http://foo.bar.baz/test?q=hello&id=22&boolean>
      //
      // Should be rendered as:
      //   <p><a href="http://foo.bar.baz/test?q=hello&amp;id=22&amp;boolean">http://foo.bar.baz/test?q=hello&amp;id=22&amp;boolean</a></p>

      Parser.Parse("<http://foo.bar.baz/test?q=hello&id=22&boolean>").Is(Parser.Prettify("<p><a href=\"http://foo.bar.baz/test?q=hello&amp;id=22&amp;boolean\">http://foo.bar.baz/test?q=hello&amp;id=22&amp;boolean</a></p>"));

      Parser.DoubleParse("<http://foo.bar.baz/test?q=hello&id=22&boolean>").Is(Parser.Prettify("<p><a href=\"http://foo.bar.baz/test?q=hello&amp;id=22&amp;boolean\">http://foo.bar.baz/test?q=hello&amp;id=22&amp;boolean</a></p>"));
    }

    [Fact]
    public void InlinesAutolinks_Example592()
    {
      // Example 592
      // Section: Inlines / Autolinks
      //
      // The following Markdown:
      //   <irc://foo.bar:2233/baz>
      //
      // Should be rendered as:
      //   <p><a href="irc://foo.bar:2233/baz">irc://foo.bar:2233/baz</a></p>

      Parser.Parse("<irc://foo.bar:2233/baz>").Is(Parser.Prettify("<p><a href=\"irc://foo.bar:2233/baz\">irc://foo.bar:2233/baz</a></p>"));

      Parser.DoubleParse("<irc://foo.bar:2233/baz>").Is(Parser.Prettify("<p><a href=\"irc://foo.bar:2233/baz\">irc://foo.bar:2233/baz</a></p>"));
    }

    // Uppercase is also fine:
    [Fact]
    public void InlinesAutolinks_Example593()
    {
      // Example 593
      // Section: Inlines / Autolinks
      //
      // The following Markdown:
      //   <MAILTO:FOO@BAR.BAZ>
      //
      // Should be rendered as:
      //   <p><a href="MAILTO:FOO@BAR.BAZ">MAILTO:FOO@BAR.BAZ</a></p>

      Parser.Parse("<MAILTO:FOO@BAR.BAZ>").Is(Parser.Prettify("<p><a href=\"MAILTO:FOO@BAR.BAZ\">MAILTO:FOO@BAR.BAZ</a></p>"));

      Parser.DoubleParse("<MAILTO:FOO@BAR.BAZ>").Is(Parser.Prettify("<p><a href=\"MAILTO:FOO@BAR.BAZ\">MAILTO:FOO@BAR.BAZ</a></p>"));
    }

    // Note that many strings that count as [absolute URIs] for
    // purposes of this spec are not valid URIs, because their
    // schemes are not registered or because of other problems
    // with their syntax:
    [Fact]
    public void InlinesAutolinks_Example594()
    {
      // Example 594
      // Section: Inlines / Autolinks
      //
      // The following Markdown:
      //   <a+b+c:d>
      //
      // Should be rendered as:
      //   <p><a href="a+b+c:d">a+b+c:d</a></p>

      Parser.Parse("<a+b+c:d>").Is(Parser.Prettify("<p><a href=\"a+b+c:d\">a+b+c:d</a></p>"));

      Parser.DoubleParse("<a+b+c:d>").Is(Parser.Prettify("<p><a href=\"a+b+c:d\">a+b+c:d</a></p>"));
    }

    [Fact]
    public void InlinesAutolinks_Example595()
    {
      // Example 595
      // Section: Inlines / Autolinks
      //
      // The following Markdown:
      //   <made-up-scheme://foo,bar>
      //
      // Should be rendered as:
      //   <p><a href="made-up-scheme://foo,bar">made-up-scheme://foo,bar</a></p>

      Parser.Parse("<made-up-scheme://foo,bar>").Is(Parser.Prettify("<p><a href=\"made-up-scheme://foo,bar\">made-up-scheme://foo,bar</a></p>"));

      Parser.DoubleParse("<made-up-scheme://foo,bar>").Is(Parser.Prettify("<p><a href=\"made-up-scheme://foo,bar\">made-up-scheme://foo,bar</a></p>"));
    }

    [Fact]
    public void InlinesAutolinks_Example596()
    {
      // Example 596
      // Section: Inlines / Autolinks
      //
      // The following Markdown:
      //   <http://../>
      //
      // Should be rendered as:
      //   <p><a href="http://../">http://../</a></p>

      Parser.Parse("<http://../>").Is(Parser.Prettify("<p><a href=\"http://../\">http://../</a></p>"));

      Parser.DoubleParse("<http://../>").Is(Parser.Prettify("<p><a href=\"http://../\">http://../</a></p>"));
    }

    [Fact]
    public void InlinesAutolinks_Example597()
    {
      // Example 597
      // Section: Inlines / Autolinks
      //
      // The following Markdown:
      //   <localhost:5001/foo>
      //
      // Should be rendered as:
      //   <p><a href="localhost:5001/foo">localhost:5001/foo</a></p>

      Parser.Parse("<localhost:5001/foo>").Is(Parser.Prettify("<p><a href=\"localhost:5001/foo\">localhost:5001/foo</a></p>"));

      Parser.DoubleParse("<localhost:5001/foo>").Is(Parser.Prettify("<p><a href=\"localhost:5001/foo\">localhost:5001/foo</a></p>"));
    }

    // Spaces are not allowed in autolinks:
    [Fact]
    public void InlinesAutolinks_Example598()
    {
      // Example 598
      // Section: Inlines / Autolinks
      //
      // The following Markdown:
      //   <http://foo.bar/baz bim>
      //
      // Should be rendered as:
      //   <p>&lt;http://foo.bar/baz bim&gt;</p>

      Parser.Parse("<http://foo.bar/baz bim>").Is(Parser.Prettify("<p>&lt;http://foo.bar/baz bim&gt;</p>"));

      Parser.DoubleParse("<http://foo.bar/baz bim>").Is(Parser.Prettify("<p>&lt;http://foo.bar/baz bim&gt;</p>"));
    }

    // Backslash-escapes do not work inside autolinks:
    [Fact]
    public void InlinesAutolinks_Example599()
    {
      // Example 599
      // Section: Inlines / Autolinks
      //
      // The following Markdown:
      //   <http://example.com/\[\>
      //
      // Should be rendered as:
      //   <p><a href="http://example.com/%5C%5B%5C">http://example.com/\[\</a></p>

      Parser.Parse("<http://example.com/\\[\\>").Is(Parser.Prettify("<p><a href=\"http://example.com/%5C%5B%5C\">http://example.com/\\[\\</a></p>"));

      Parser.DoubleParse("<http://example.com/\\[\\>").Is(Parser.Prettify("<p><a href=\"http://example.com/%5C%5B%5C\">http://example.com/\\[\\</a></p>"));
    }

    // An [email autolink](@)
    // consists of `<`, followed by an [email address],
    // followed by `>`.  The link's label is the email address,
    // and the URL is `mailto:` followed by the email address.
    //
    // An [email address](@),
    // for these purposes, is anything that matches
    // the [non-normative regex from the HTML5
    // spec](https://html.spec.whatwg.org/multipage/forms.html#e-mail-state-(type=email)):
    //
    //     /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?
    //     (?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/
    //
    // Examples of email autolinks:
    [Fact]
    public void InlinesAutolinks_Example600()
    {
      // Example 600
      // Section: Inlines / Autolinks
      //
      // The following Markdown:
      //   <foo@bar.example.com>
      //
      // Should be rendered as:
      //   <p><a href="mailto:foo@bar.example.com">foo@bar.example.com</a></p>

      Parser.Parse("<foo@bar.example.com>").Is(Parser.Prettify("<p><a href=\"mailto:foo@bar.example.com\">foo@bar.example.com</a></p>"));

      Parser.DoubleParse("<foo@bar.example.com>").Is(Parser.Prettify("<p><a href=\"mailto:foo@bar.example.com\">foo@bar.example.com</a></p>"));
    }

    [Fact]
    public void InlinesAutolinks_Example601()
    {
      // Example 601
      // Section: Inlines / Autolinks
      //
      // The following Markdown:
      //   <foo+special@Bar.baz-bar0.com>
      //
      // Should be rendered as:
      //   <p><a href="mailto:foo+special@Bar.baz-bar0.com">foo+special@Bar.baz-bar0.com</a></p>

      Parser.Parse("<foo+special@Bar.baz-bar0.com>").Is(Parser.Prettify("<p><a href=\"mailto:foo+special@Bar.baz-bar0.com\">foo+special@Bar.baz-bar0.com</a></p>"));

      Parser.DoubleParse("<foo+special@Bar.baz-bar0.com>").Is(Parser.Prettify("<p><a href=\"mailto:foo+special@Bar.baz-bar0.com\">foo+special@Bar.baz-bar0.com</a></p>"));
    }

    // Backslash-escapes do not work inside email autolinks:
    [Fact]
    public void InlinesAutolinks_Example602()
    {
      // Example 602
      // Section: Inlines / Autolinks
      //
      // The following Markdown:
      //   <foo\+@bar.example.com>
      //
      // Should be rendered as:
      //   <p>&lt;foo+@bar.example.com&gt;</p>

      Parser.Parse("<foo\\+@bar.example.com>").Is(Parser.Prettify("<p>&lt;foo+@bar.example.com&gt;</p>"));

      Parser.DoubleParse("<foo\\+@bar.example.com>").Is(Parser.Prettify("<p>&lt;foo+@bar.example.com&gt;</p>"));
    }

    // These are not autolinks:
    [Fact]
    public void InlinesAutolinks_Example603()
    {
      // Example 603
      // Section: Inlines / Autolinks
      //
      // The following Markdown:
      //   <>
      //
      // Should be rendered as:
      //   <p>&lt;&gt;</p>

      Parser.Parse("<>").Is(Parser.Prettify("<p>&lt;&gt;</p>"));

      Parser.DoubleParse("<>").Is(Parser.Prettify("<p>&lt;&gt;</p>"));
    }

    [Fact]
    public void InlinesAutolinks_Example604()
    {
      // Example 604
      // Section: Inlines / Autolinks
      //
      // The following Markdown:
      //   < http://foo.bar >
      //
      // Should be rendered as:
      //   <p>&lt; http://foo.bar &gt;</p>

      Parser.Parse("< http://foo.bar >").Is(Parser.Prettify("<p>&lt; http://foo.bar &gt;</p>"));

      Parser.DoubleParse("< http://foo.bar >").Is(Parser.Prettify("<p>&lt; http://foo.bar &gt;</p>"));
    }

    [Fact]
    public void InlinesAutolinks_Example605()
    {
      // Example 605
      // Section: Inlines / Autolinks
      //
      // The following Markdown:
      //   <m:abc>
      //
      // Should be rendered as:
      //   <p>&lt;m:abc&gt;</p>

      Parser.Parse("<m:abc>").Is(Parser.Prettify("<p>&lt;m:abc&gt;</p>"));

      Parser.DoubleParse("<m:abc>").Is(Parser.Prettify("<p>&lt;m:abc&gt;</p>"));
    }

    [Fact]
    public void InlinesAutolinks_Example606()
    {
      // Example 606
      // Section: Inlines / Autolinks
      //
      // The following Markdown:
      //   <foo.bar.baz>
      //
      // Should be rendered as:
      //   <p>&lt;foo.bar.baz&gt;</p>

      Parser.Parse("<foo.bar.baz>").Is(Parser.Prettify("<p>&lt;foo.bar.baz&gt;</p>"));

      Parser.DoubleParse("<foo.bar.baz>").Is(Parser.Prettify("<p>&lt;foo.bar.baz&gt;</p>"));
    }

    [Fact]
    public void InlinesAutolinks_Example607()
    {
      // Example 607
      // Section: Inlines / Autolinks
      //
      // The following Markdown:
      //   http://example.com
      //
      // Should be rendered as:
      //   <p>http://example.com</p>

      Parser.Parse("http://example.com").Is(Parser.Prettify("<p>http://example.com</p>"));

      Parser.DoubleParse("http://example.com").Is(Parser.Prettify("<p>http://example.com</p>"));
    }

    [Fact]
    public void InlinesAutolinks_Example608()
    {
      // Example 608
      // Section: Inlines / Autolinks
      //
      // The following Markdown:
      //   foo@bar.example.com
      //
      // Should be rendered as:
      //   <p>foo@bar.example.com</p>

      Parser.Parse("foo@bar.example.com").Is(Parser.Prettify("<p>foo@bar.example.com</p>"));

      Parser.DoubleParse("foo@bar.example.com").Is(Parser.Prettify("<p>foo@bar.example.com</p>"));
    }
  }

  public class TestInlinesRawHTML
  {
    // ## Raw HTML
    //
    // Text between `<` and `>` that looks like an HTML tag is parsed as a
    // raw HTML tag and will be rendered in HTML without escaping.
    // Tag and attribute names are not limited to current HTML tags,
    // so custom tags (and even, say, DocBook tags) may be used.
    //
    // Here is the grammar for tags:
    //
    // A [tag name](@) consists of an ASCII letter
    // followed by zero or more ASCII letters, digits, or
    // hyphens (`-`).
    //
    // An [attribute](@) consists of [whitespace],
    // an [attribute name], and an optional
    // [attribute value specification].
    //
    // An [attribute name](@)
    // consists of an ASCII letter, `_`, or `:`, followed by zero or more ASCII
    // letters, digits, `_`, `.`, `:`, or `-`.  (Note:  This is the XML
    // specification restricted to ASCII.  HTML5 is laxer.)
    //
    // An [attribute value specification](@)
    // consists of optional [whitespace],
    // a `=` character, optional [whitespace], and an [attribute
    // value].
    //
    // An [attribute value](@)
    // consists of an [unquoted attribute value],
    // a [single-quoted attribute value], or a [double-quoted attribute value].
    //
    // An [unquoted attribute value](@)
    // is a nonempty string of characters not
    // including [whitespace], `"`, `'`, `=`, `<`, `>`, or `` ` ``.
    //
    // A [single-quoted attribute value](@)
    // consists of `'`, zero or more
    // characters not including `'`, and a final `'`.
    //
    // A [double-quoted attribute value](@)
    // consists of `"`, zero or more
    // characters not including `"`, and a final `"`.
    //
    // An [open tag](@) consists of a `<` character, a [tag name],
    // zero or more [attributes], optional [whitespace], an optional `/`
    // character, and a `>` character.
    //
    // A [closing tag](@) consists of the string `</`, a
    // [tag name], optional [whitespace], and the character `>`.
    //
    // An [HTML comment](@) consists of `<!--` + *text* + `-->`,
    // where *text* does not start with `>` or `->`, does not end with `-`,
    // and does not contain `--`.  (See the
    // [HTML5 spec](http://www.w3.org/TR/html5/syntax.html#comments).)
    //
    // A [processing instruction](@)
    // consists of the string `<?`, a string
    // of characters not including the string `?>`, and the string
    // `?>`.
    //
    // A [declaration](@) consists of the
    // string `<!`, a name consisting of one or more uppercase ASCII letters,
    // [whitespace], a string of characters not including the
    // character `>`, and the character `>`.
    //
    // A [CDATA section](@) consists of
    // the string `<![CDATA[`, a string of characters not including the string
    // `]]>`, and the string `]]>`.
    //
    // An [HTML tag](@) consists of an [open tag], a [closing tag],
    // an [HTML comment], a [processing instruction], a [declaration],
    // or a [CDATA section].
    //
    // Here are some simple open tags:
    [Fact]
    public void InlinesRawHTML_Example609()
    {
      // Example 609
      // Section: Inlines / Raw HTML
      //
      // The following Markdown:
      //   <a><bab><c2c>
      //
      // Should be rendered as:
      //   <p><a><bab><c2c></p>

      Parser.Parse("<a><bab><c2c>").Is(Parser.Prettify("<p><a><bab><c2c></p>"));

      Parser.DoubleParse("<a><bab><c2c>").Is(Parser.Prettify("<p><a><bab><c2c></p>"));
    }

    // Empty elements:
    [Fact]
    public void InlinesRawHTML_Example610()
    {
      // Example 610
      // Section: Inlines / Raw HTML
      //
      // The following Markdown:
      //   <a/><b2/>
      //
      // Should be rendered as:
      //   <p><a/><b2/></p>

      Parser.Parse("<a/><b2/>").Is(Parser.Prettify("<p><a/><b2/></p>"));

      Parser.DoubleParse("<a/><b2/>").Is(Parser.Prettify("<p><a/><b2/></p>"));
    }

    // [Whitespace] is allowed:
    [Fact]
    public void InlinesRawHTML_Example611()
    {
      // Example 611
      // Section: Inlines / Raw HTML
      //
      // The following Markdown:
      //   <a  /><b2
      //   data="foo" >
      //
      // Should be rendered as:
      //   <p><a  /><b2
      //   data="foo" ></p>

      Parser.Parse("<a  /><b2\ndata=\"foo\" >").Is(Parser.Prettify("<p><a  /><b2\ndata=\"foo\" ></p>"));

      Parser.DoubleParse("<a  /><b2\ndata=\"foo\" >").Is(Parser.Prettify("<p><a  /><b2\ndata=\"foo\" ></p>"));
    }

    // With attributes:
    [Fact]
    public void InlinesRawHTML_Example612()
    {
      // Example 612
      // Section: Inlines / Raw HTML
      //
      // The following Markdown:
      //   <a foo="bar" bam = 'baz <em>"</em>'
      //   _boolean zoop:33=zoop:33 />
      //
      // Should be rendered as:
      //   <p><a foo="bar" bam = 'baz <em>"</em>'
      //   _boolean zoop:33=zoop:33 /></p>

      Parser.Parse("<a foo=\"bar\" bam = 'baz <em>\"</em>'\n_boolean zoop:33=zoop:33 />").Is(Parser.Prettify("<p><a foo=\"bar\" bam = 'baz <em>\"</em>'\n_boolean zoop:33=zoop:33 /></p>"));

      Parser.DoubleParse("<a foo=\"bar\" bam = 'baz <em>\"</em>'\n_boolean zoop:33=zoop:33 />").Is(Parser.Prettify("<p><a foo=\"bar\" bam = 'baz <em>\"</em>'\n_boolean zoop:33=zoop:33 /></p>"));
    }

    // Custom tag names can be used:
    [Fact]
    public void InlinesRawHTML_Example613()
    {
      // Example 613
      // Section: Inlines / Raw HTML
      //
      // The following Markdown:
      //   Foo <responsive-image src="foo.jpg" />
      //
      // Should be rendered as:
      //   <p>Foo <responsive-image src="foo.jpg" /></p>

      Parser.Parse("Foo <responsive-image src=\"foo.jpg\" />").Is(Parser.Prettify("<p>Foo <responsive-image src=\"foo.jpg\" /></p>"));

      Parser.DoubleParse("Foo <responsive-image src=\"foo.jpg\" />").Is(Parser.Prettify("<p>Foo <responsive-image src=\"foo.jpg\" /></p>"));
    }

    // Illegal tag names, not parsed as HTML:
    [Fact]
    public void InlinesRawHTML_Example614()
    {
      // Example 614
      // Section: Inlines / Raw HTML
      //
      // The following Markdown:
      //   <33> <__>
      //
      // Should be rendered as:
      //   <p>&lt;33&gt; &lt;__&gt;</p>

      Parser.Parse("<33> <__>").Is(Parser.Prettify("<p>&lt;33&gt; &lt;__&gt;</p>"));

      Parser.DoubleParse("<33> <__>").Is(Parser.Prettify("<p>&lt;33&gt; &lt;__&gt;</p>"));
    }

    // Illegal attribute names:
    [Fact]
    public void InlinesRawHTML_Example615()
    {
      // Example 615
      // Section: Inlines / Raw HTML
      //
      // The following Markdown:
      //   <a h*#ref="hi">
      //
      // Should be rendered as:
      //   <p>&lt;a h*#ref=&quot;hi&quot;&gt;</p>

      Parser.Parse("<a h*#ref=\"hi\">").Is(Parser.Prettify("<p>&lt;a h*#ref=&quot;hi&quot;&gt;</p>"));

      Parser.DoubleParse("<a h*#ref=\"hi\">").Is(Parser.Prettify("<p>&lt;a h*#ref=&quot;hi&quot;&gt;</p>"));
    }

    // Illegal attribute values:
    [Fact]
    public void InlinesRawHTML_Example616()
    {
      // Example 616
      // Section: Inlines / Raw HTML
      //
      // The following Markdown:
      //   <a href="hi'> <a href=hi'>
      //
      // Should be rendered as:
      //   <p>&lt;a href=&quot;hi'&gt; &lt;a href=hi'&gt;</p>

      Parser.Parse("<a href=\"hi'> <a href=hi'>").Is(Parser.Prettify("<p>&lt;a href=&quot;hi'&gt; &lt;a href=hi'&gt;</p>"));

      Parser.DoubleParse("<a href=\"hi'> <a href=hi'>").Is(Parser.Prettify("<p>&lt;a href=&quot;hi'&gt; &lt;a href=hi'&gt;</p>"));
    }

    // Illegal [whitespace]:
    [Fact]
    public void InlinesRawHTML_Example617()
    {
      // Example 617
      // Section: Inlines / Raw HTML
      //
      // The following Markdown:
      //   < a><
      //   foo><bar/ >
      //   <foo bar=baz
      //   bim!bop />
      //
      // Should be rendered as:
      //   <p>&lt; a&gt;&lt;
      //   foo&gt;&lt;bar/ &gt;
      //   &lt;foo bar=baz
      //   bim!bop /&gt;</p>

      Parser.Parse("< a><\nfoo><bar/ >\n<foo bar=baz\nbim!bop />").Is(Parser.Prettify("<p>&lt; a&gt;&lt;\nfoo&gt;&lt;bar/ &gt;\n&lt;foo bar=baz\nbim!bop /&gt;</p>"));

      Parser.DoubleParse("< a><\nfoo><bar/ >\n<foo bar=baz\nbim!bop />").Is(Parser.Prettify("<p>&lt; a&gt;&lt;\nfoo&gt;&lt;bar/ &gt;\n&lt;foo bar=baz\nbim!bop /&gt;</p>"));
    }

    // Missing [whitespace]:
    [Fact]
    public void InlinesRawHTML_Example618()
    {
      // Example 618
      // Section: Inlines / Raw HTML
      //
      // The following Markdown:
      //   <a href='bar'title=title>
      //
      // Should be rendered as:
      //   <p>&lt;a href='bar'title=title&gt;</p>

      Parser.Parse("<a href='bar'title=title>").Is(Parser.Prettify("<p>&lt;a href='bar'title=title&gt;</p>"));

      Parser.DoubleParse("<a href='bar'title=title>").Is(Parser.Prettify("<p>&lt;a href='bar'title=title&gt;</p>"));
    }

    // Closing tags:
    [Fact]
    public void InlinesRawHTML_Example619()
    {
      // Example 619
      // Section: Inlines / Raw HTML
      //
      // The following Markdown:
      //   </a></foo >
      //
      // Should be rendered as:
      //   <p></a></foo ></p>

      Parser.Parse("</a></foo >").Is(Parser.Prettify("<p></a></foo ></p>"));

      Parser.DoubleParse("</a></foo >").Is(Parser.Prettify("<p></a></foo ></p>"));
    }

    // Illegal attributes in closing tag:
    [Fact]
    public void InlinesRawHTML_Example620()
    {
      // Example 620
      // Section: Inlines / Raw HTML
      //
      // The following Markdown:
      //   </a href="foo">
      //
      // Should be rendered as:
      //   <p>&lt;/a href=&quot;foo&quot;&gt;</p>

      Parser.Parse("</a href=\"foo\">").Is(Parser.Prettify("<p>&lt;/a href=&quot;foo&quot;&gt;</p>"));

      Parser.DoubleParse("</a href=\"foo\">").Is(Parser.Prettify("<p>&lt;/a href=&quot;foo&quot;&gt;</p>"));
    }

    // Comments:
    [Fact]
    public void InlinesRawHTML_Example621()
    {
      // Example 621
      // Section: Inlines / Raw HTML
      //
      // The following Markdown:
      //   foo <!-- this is a
      //   comment - with hyphen -->
      //
      // Should be rendered as:
      //   <p>foo <!-- this is a
      //   comment - with hyphen --></p>

      Parser.Parse("foo <!-- this is a\ncomment - with hyphen -->").Is(Parser.Prettify("<p>foo <!-- this is a\ncomment - with hyphen --></p>"));

      Parser.DoubleParse("foo <!-- this is a\ncomment - with hyphen -->").Is(Parser.Prettify("<p>foo <!-- this is a\ncomment - with hyphen --></p>"));
    }

    [Fact]
    public void InlinesRawHTML_Example622()
    {
      // Example 622
      // Section: Inlines / Raw HTML
      //
      // The following Markdown:
      //   foo <!-- not a comment -- two hyphens -->
      //
      // Should be rendered as:
      //   <p>foo &lt;!-- not a comment -- two hyphens --&gt;</p>

      Parser.Parse("foo <!-- not a comment -- two hyphens -->").Is(Parser.Prettify("<p>foo &lt;!-- not a comment -- two hyphens --&gt;</p>"));

      Parser.DoubleParse("foo <!-- not a comment -- two hyphens -->").Is(Parser.Prettify("<p>foo &lt;!-- not a comment -- two hyphens --&gt;</p>"));
    }

    // Not comments:
    [Fact]
    public void InlinesRawHTML_Example623()
    {
      // Example 623
      // Section: Inlines / Raw HTML
      //
      // The following Markdown:
      //   foo <!--> foo -->
      //  
      //   foo <!-- foo--->
      //
      // Should be rendered as:
      //   <p>foo &lt;!--&gt; foo --&gt;</p>
      //   <p>foo &lt;!-- foo---&gt;</p>

      Parser.Parse("foo <!--> foo -->\n\nfoo <!-- foo--->").Is(Parser.Prettify("<p>foo &lt;!--&gt; foo --&gt;</p>\n<p>foo &lt;!-- foo---&gt;</p>"));

      Parser.DoubleParse("foo <!--> foo -->\n\nfoo <!-- foo--->").Is(Parser.Prettify("<p>foo &lt;!--&gt; foo --&gt;</p>\n<p>foo &lt;!-- foo---&gt;</p>"));
    }

    // Processing instructions:
    [Fact]
    public void InlinesRawHTML_Example624()
    {
      // Example 624
      // Section: Inlines / Raw HTML
      //
      // The following Markdown:
      //   foo <?php echo $a; ?>
      //
      // Should be rendered as:
      //   <p>foo <?php echo $a; ?></p>

      Parser.Parse("foo <?php echo $a; ?>").Is(Parser.Prettify("<p>foo <?php echo $a; ?></p>"));

      Parser.DoubleParse("foo <?php echo $a; ?>").Is(Parser.Prettify("<p>foo <?php echo $a; ?></p>"));
    }

    // Declarations:
    [Fact]
    public void InlinesRawHTML_Example625()
    {
      // Example 625
      // Section: Inlines / Raw HTML
      //
      // The following Markdown:
      //   foo <!ELEMENT br EMPTY>
      //
      // Should be rendered as:
      //   <p>foo <!ELEMENT br EMPTY></p>

      Parser.Parse("foo <!ELEMENT br EMPTY>").Is(Parser.Prettify("<p>foo <!ELEMENT br EMPTY></p>"));

      Parser.DoubleParse("foo <!ELEMENT br EMPTY>").Is(Parser.Prettify("<p>foo <!ELEMENT br EMPTY></p>"));
    }

    // CDATA sections:
    [Fact]
    public void InlinesRawHTML_Example626()
    {
      // Example 626
      // Section: Inlines / Raw HTML
      //
      // The following Markdown:
      //   foo <![CDATA[>&<]]>
      //
      // Should be rendered as:
      //   <p>foo <![CDATA[>&<]]></p>

      Parser.Parse("foo <![CDATA[>&<]]>").Is(Parser.Prettify("<p>foo <![CDATA[>&<]]></p>"));

      Parser.DoubleParse("foo <![CDATA[>&<]]>").Is(Parser.Prettify("<p>foo <![CDATA[>&<]]></p>"));
    }

    // Entity and numeric character references are preserved in HTML
    // attributes:
    [Fact]
    public void InlinesRawHTML_Example627()
    {
      // Example 627
      // Section: Inlines / Raw HTML
      //
      // The following Markdown:
      //   foo <a href="&ouml;">
      //
      // Should be rendered as:
      //   <p>foo <a href="&ouml;"></p>

      Parser.Parse("foo <a href=\"&ouml;\">").Is(Parser.Prettify("<p>foo <a href=\"&ouml;\"></p>"));

      Parser.DoubleParse("foo <a href=\"&ouml;\">").Is(Parser.Prettify("<p>foo <a href=\"&ouml;\"></p>"));
    }

    // Backslash escapes do not work in HTML attributes:
    [Fact]
    public void InlinesRawHTML_Example628()
    {
      // Example 628
      // Section: Inlines / Raw HTML
      //
      // The following Markdown:
      //   foo <a href="\*">
      //
      // Should be rendered as:
      //   <p>foo <a href="\*"></p>

      Parser.Parse("foo <a href=\"\\*\">").Is(Parser.Prettify("<p>foo <a href=\"\\*\"></p>"));

      Parser.DoubleParse("foo <a href=\"\\*\">").Is(Parser.Prettify("<p>foo <a href=\"\\*\"></p>"));
    }

    [Fact]
    public void InlinesRawHTML_Example629()
    {
      // Example 629
      // Section: Inlines / Raw HTML
      //
      // The following Markdown:
      //   <a href="\"">
      //
      // Should be rendered as:
      //   <p>&lt;a href=&quot;&quot;&quot;&gt;</p>

      Parser.Parse("<a href=\"\\\"\">").Is(Parser.Prettify("<p>&lt;a href=&quot;&quot;&quot;&gt;</p>"));

      Parser.DoubleParse("<a href=\"\\\"\">").Is(Parser.Prettify("<p>&lt;a href=&quot;&quot;&quot;&gt;</p>"));
    }
  }

  public class TestInlinesHardLineBreaks
  {
    // ## Hard line breaks
    //
    // A line break (not in a code span or HTML tag) that is preceded
    // by two or more spaces and does not occur at the end of a block
    // is parsed as a [hard line break](@) (rendered
    // in HTML as a `<br />` tag):
    [Fact]
    public void InlinesHardLineBreaks_Example630()
    {
      // Example 630
      // Section: Inlines / Hard line breaks
      //
      // The following Markdown:
      //   foo  
      //   baz
      //
      // Should be rendered as:
      //   <p>foo<br />
      //   baz</p>

      Parser.Parse("foo  \nbaz").Is(Parser.Prettify("<p>foo<br />\nbaz</p>"));

      Parser.DoubleParse("foo  \nbaz").Is(Parser.Prettify("<p>foo<br />\nbaz</p>"));
    }

    // For a more visible alternative, a backslash before the
    // [line ending] may be used instead of two spaces:
    [Fact]
    public void InlinesHardLineBreaks_Example631()
    {
      // Example 631
      // Section: Inlines / Hard line breaks
      //
      // The following Markdown:
      //   foo\
      //   baz
      //
      // Should be rendered as:
      //   <p>foo<br />
      //   baz</p>

      Parser.Parse("foo\\\nbaz").Is(Parser.Prettify("<p>foo<br />\nbaz</p>"));

      Parser.DoubleParse("foo\\\nbaz").Is(Parser.Prettify("<p>foo<br />\nbaz</p>"));
    }

    // More than two spaces can be used:
    [Fact]
    public void InlinesHardLineBreaks_Example632()
    {
      // Example 632
      // Section: Inlines / Hard line breaks
      //
      // The following Markdown:
      //   foo       
      //   baz
      //
      // Should be rendered as:
      //   <p>foo<br />
      //   baz</p>

      Parser.Parse("foo       \nbaz").Is(Parser.Prettify("<p>foo<br />\nbaz</p>"));

      Parser.DoubleParse("foo       \nbaz").Is(Parser.Prettify("<p>foo<br />\nbaz</p>"));
    }

    // Leading spaces at the beginning of the next line are ignored:
    [Fact]
    public void InlinesHardLineBreaks_Example633()
    {
      // Example 633
      // Section: Inlines / Hard line breaks
      //
      // The following Markdown:
      //   foo  
      //        bar
      //
      // Should be rendered as:
      //   <p>foo<br />
      //   bar</p>

      Parser.Parse("foo  \n     bar").Is(Parser.Prettify("<p>foo<br />\nbar</p>"));

      Parser.DoubleParse("foo  \n     bar").Is(Parser.Prettify("<p>foo<br />\nbar</p>"));
    }

    [Fact]
    public void InlinesHardLineBreaks_Example634()
    {
      // Example 634
      // Section: Inlines / Hard line breaks
      //
      // The following Markdown:
      //   foo\
      //        bar
      //
      // Should be rendered as:
      //   <p>foo<br />
      //   bar</p>

      Parser.Parse("foo\\\n     bar").Is(Parser.Prettify("<p>foo<br />\nbar</p>"));

      Parser.DoubleParse("foo\\\n     bar").Is(Parser.Prettify("<p>foo<br />\nbar</p>"));
    }

    // Line breaks can occur inside emphasis, links, and other constructs
    // that allow inline content:
    [Fact]
    public void InlinesHardLineBreaks_Example635()
    {
      // Example 635
      // Section: Inlines / Hard line breaks
      //
      // The following Markdown:
      //   *foo  
      //   bar*
      //
      // Should be rendered as:
      //   <p><em>foo<br />
      //   bar</em></p>

      Parser.Parse("*foo  \nbar*").Is(Parser.Prettify("<p><em>foo<br />\nbar</em></p>"));

      Parser.DoubleParse("*foo  \nbar*").Is(Parser.Prettify("<p><em>foo<br />\nbar</em></p>"));
    }

    [Fact]
    public void InlinesHardLineBreaks_Example636()
    {
      // Example 636
      // Section: Inlines / Hard line breaks
      //
      // The following Markdown:
      //   *foo\
      //   bar*
      //
      // Should be rendered as:
      //   <p><em>foo<br />
      //   bar</em></p>

      Parser.Parse("*foo\\\nbar*").Is(Parser.Prettify("<p><em>foo<br />\nbar</em></p>"));

      Parser.DoubleParse("*foo\\\nbar*").Is(Parser.Prettify("<p><em>foo<br />\nbar</em></p>"));
    }

    // Line breaks do not occur inside code spans
    [Fact]
    public void InlinesHardLineBreaks_Example637()
    {
      // Example 637
      // Section: Inlines / Hard line breaks
      //
      // The following Markdown:
      //   `code 
      //   span`
      //
      // Should be rendered as:
      //   <p><code>code  span</code></p>

      Parser.Parse("`code \nspan`").Is(Parser.Prettify("<p><code>code  span</code></p>"));

      Parser.DoubleParse("`code \nspan`").Is(Parser.Prettify("<p><code>code  span</code></p>"));
    }

    [Fact]
    public void InlinesHardLineBreaks_Example638()
    {
      // Example 638
      // Section: Inlines / Hard line breaks
      //
      // The following Markdown:
      //   `code\
      //   span`
      //
      // Should be rendered as:
      //   <p><code>code\ span</code></p>

      Parser.Parse("`code\\\nspan`").Is(Parser.Prettify("<p><code>code\\ span</code></p>"));

      Parser.DoubleParse("`code\\\nspan`").Is(Parser.Prettify("<p><code>code\\ span</code></p>"));
    }

    // or HTML tags:
    [Fact]
    public void InlinesHardLineBreaks_Example639()
    {
      // Example 639
      // Section: Inlines / Hard line breaks
      //
      // The following Markdown:
      //   <a href="foo  
      //   bar">
      //
      // Should be rendered as:
      //   <p><a href="foo  
      //   bar"></p>

      Parser.Parse("<a href=\"foo  \nbar\">").Is(Parser.Prettify("<p><a href=\"foo  \nbar\"></p>"));

      Parser.DoubleParse("<a href=\"foo  \nbar\">").Is(Parser.Prettify("<p><a href=\"foo  \nbar\"></p>"));
    }

    [Fact]
    public void InlinesHardLineBreaks_Example640()
    {
      // Example 640
      // Section: Inlines / Hard line breaks
      //
      // The following Markdown:
      //   <a href="foo\
      //   bar">
      //
      // Should be rendered as:
      //   <p><a href="foo\
      //   bar"></p>

      Parser.Parse("<a href=\"foo\\\nbar\">").Is(Parser.Prettify("<p><a href=\"foo\\\nbar\"></p>"));

      Parser.DoubleParse("<a href=\"foo\\\nbar\">").Is(Parser.Prettify("<p><a href=\"foo\\\nbar\"></p>"));
    }

    // Hard line breaks are for separating inline content within a block.
    // Neither syntax for hard line breaks works at the end of a paragraph or
    // other block element:
    [Fact]
    public void InlinesHardLineBreaks_Example641()
    {
      // Example 641
      // Section: Inlines / Hard line breaks
      //
      // The following Markdown:
      //   foo\
      //
      // Should be rendered as:
      //   <p>foo\</p>

      Parser.Parse("foo\\").Is(Parser.Prettify("<p>foo\\</p>"));

      Parser.DoubleParse("foo\\").Is(Parser.Prettify("<p>foo\\</p>"));
    }

    [Fact]
    public void InlinesHardLineBreaks_Example642()
    {
      // Example 642
      // Section: Inlines / Hard line breaks
      //
      // The following Markdown:
      //   foo  
      //
      // Should be rendered as:
      //   <p>foo</p>

      Parser.Parse("foo  ").Is(Parser.Prettify("<p>foo</p>"));

      Parser.DoubleParse("foo  ").Is(Parser.Prettify("<p>foo</p>"));
    }

    [Fact]
    public void InlinesHardLineBreaks_Example643()
    {
      // Example 643
      // Section: Inlines / Hard line breaks
      //
      // The following Markdown:
      //   ### foo\
      //
      // Should be rendered as:
      //   <h3>foo\</h3>

      Parser.Parse("### foo\\").Is(Parser.Prettify("<h3>foo\\</h3>"));

      Parser.DoubleParse("### foo\\").Is(Parser.Prettify("<h3>foo\\</h3>"));
    }

    [Fact]
    public void InlinesHardLineBreaks_Example644()
    {
      // Example 644
      // Section: Inlines / Hard line breaks
      //
      // The following Markdown:
      //   ### foo  
      //
      // Should be rendered as:
      //   <h3>foo</h3>

      Parser.Parse("### foo  ").Is(Parser.Prettify("<h3>foo</h3>"));

      Parser.DoubleParse("### foo  ").Is(Parser.Prettify("<h3>foo</h3>"));
    }
  }

  public class TestInlinesSoftLineBreaks
  {
    // ## Soft line breaks
    //
    // A regular line break (not in a code span or HTML tag) that is not
    // preceded by two or more spaces or a backslash is parsed as a
    // [softbreak](@).  (A softbreak may be rendered in HTML either as a
    // [line ending] or as a space. The result will be the same in
    // browsers. In the examples here, a [line ending] will be used.)
    [Fact]
    public void InlinesSoftLineBreaks_Example645()
    {
      // Example 645
      // Section: Inlines / Soft line breaks
      //
      // The following Markdown:
      //   foo
      //   baz
      //
      // Should be rendered as:
      //   <p>foo
      //   baz</p>

      Parser.Parse("foo\nbaz").Is(Parser.Prettify("<p>foo\nbaz</p>"));

      Parser.DoubleParse("foo\nbaz").Is(Parser.Prettify("<p>foo\nbaz</p>"));
    }

    // Spaces at the end of the line and beginning of the next line are
    // removed:
    [Fact]
    public void InlinesSoftLineBreaks_Example646()
    {
      // Example 646
      // Section: Inlines / Soft line breaks
      //
      // The following Markdown:
      //   foo 
      //    baz
      //
      // Should be rendered as:
      //   <p>foo
      //   baz</p>

      Parser.Parse("foo \n baz").Is(Parser.Prettify("<p>foo\nbaz</p>"));

      Parser.DoubleParse("foo \n baz").Is(Parser.Prettify("<p>foo\nbaz</p>"));
    }
  }

  public class TestInlinesTextualContent
  {
    // A conforming parser may render a soft line break in HTML either as a
    // line break or as a space.
    //
    // A renderer may also provide an option to render soft line breaks
    // as hard line breaks.
    //
    // ## Textual content
    //
    // Any characters not given an interpretation by the above rules will
    // be parsed as plain textual content.
    [Fact]
    public void InlinesTextualContent_Example647()
    {
      // Example 647
      // Section: Inlines / Textual content
      //
      // The following Markdown:
      //   hello $.;'there
      //
      // Should be rendered as:
      //   <p>hello $.;'there</p>

      Parser.Parse("hello $.;'there").Is(Parser.Prettify("<p>hello $.;'there</p>"));

      Parser.DoubleParse("hello $.;'there").Is(Parser.Prettify("<p>hello $.;'there</p>"));
    }

    [Fact]
    public void InlinesTextualContent_Example648()
    {
      // Example 648
      // Section: Inlines / Textual content
      //
      // The following Markdown:
      //   Foo χρῆν
      //
      // Should be rendered as:
      //   <p>Foo χρῆν</p>

      Parser.Parse("Foo χρῆν").Is(Parser.Prettify("<p>Foo χρῆν</p>"));

      Parser.DoubleParse("Foo χρῆν").Is(Parser.Prettify("<p>Foo χρῆν</p>"));
    }

    // Internal spaces are preserved verbatim:
    [Fact]
    public void InlinesTextualContent_Example649()
    {
      // Example 649
      // Section: Inlines / Textual content
      //
      // The following Markdown:
      //   Multiple     spaces
      //
      // Should be rendered as:
      //   <p>Multiple     spaces</p>

      Parser.Parse("Multiple     spaces").Is(Parser.Prettify("<p>Multiple     spaces</p>"));

      Parser.DoubleParse("Multiple     spaces").Is(Parser.Prettify("<p>Multiple     spaces</p>"));
    }

    // Within a blockquote a setext heading takes precedence
    // over a thematic break:
    [Fact]
    public void InlinesTextualContent_Example650()
    {
      // Example 650
      // Section: Inlines / Textual content
      //
      // The following Markdown:
      //   > Foo
      //   > ---
      //   > bar
      //
      // Should be rendered as:
      //   <blockquote>
      //   <h2>Foo</h2>
      //   <p>bar</p>
      //   </blockquote>

      Parser.Parse("> Foo\n> ---\n> bar").Is(Parser.Prettify("<blockquote>\n<h2>Foo</h2>\n<p>bar</p>\n</blockquote>"));

      Parser.DoubleParse("> Foo\n> ---\n> bar").Is(Parser.Prettify("<blockquote>\n<h2>Foo</h2>\n<p>bar</p>\n</blockquote>"));
    }
    // <!-- END TESTS -->
    //
    // # Appendix: A parsing strategy
    //
    // In this appendix we describe some features of the parsing strategy
    // used in the CommonMark reference implementations.
    //
    // ## Overview
    //
    // Parsing has two phases:
    //
    // 1. In the first phase, lines of input are consumed and the block
    // structure of the document---its division into paragraphs, block quotes,
    // list items, and so on---is constructed.  Text is assigned to these
    // blocks but not parsed. Link reference definitions are parsed and a
    // map of links is constructed.
    //
    // 2. In the second phase, the raw text contents of paragraphs and headings
    // are parsed into sequences of Markdown inline elements (strings,
    // code spans, links, emphasis, and so on), using the map of link
    // references constructed in phase 1.
    //
    // At each point in processing, the document is represented as a tree of
    // **blocks**.  The root of the tree is a `document` block.  The `document`
    // may have any number of other blocks as **children**.  These children
    // may, in turn, have other blocks as children.  The last child of a block
    // is normally considered **open**, meaning that subsequent lines of input
    // can alter its contents.  (Blocks that are not open are **closed**.)
    // Here, for example, is a possible document tree, with the open blocks
    // marked by arrows:
    //
    // ``` tree
    // -> document
    //   -> block_quote
    //        paragraph
    //          "Lorem ipsum dolor\nsit amet."
    //     -> list (type=bullet tight=true bullet_char=-)
    //          list_item
    //            paragraph
    //              "Qui *quodsi iracundia*"
    //       -> list_item
    //         -> paragraph
    //              "aliquando id"
    // ```
    //
    // ## Phase 1: block structure
    //
    // Each line that is processed has an effect on this tree.  The line is
    // analyzed and, depending on its contents, the document may be altered
    // in one or more of the following ways:
    //
    // 1. One or more open blocks may be closed.
    // 2. One or more new blocks may be created as children of the
    //    last open block.
    // 3. Text may be added to the last (deepest) open block remaining
    //    on the tree.
    //
    // Once a line has been incorporated into the tree in this way,
    // it can be discarded, so input can be read in a stream.
    //
    // For each line, we follow this procedure:
    //
    // 1. First we iterate through the open blocks, starting with the
    // root document, and descending through last children down to the last
    // open block.  Each block imposes a condition that the line must satisfy
    // if the block is to remain open.  For example, a block quote requires a
    // `>` character.  A paragraph requires a non-blank line.
    // In this phase we may match all or just some of the open
    // blocks.  But we cannot close unmatched blocks yet, because we may have a
    // [lazy continuation line].
    //
    // 2.  Next, after consuming the continuation markers for existing
    // blocks, we look for new block starts (e.g. `>` for a block quote).
    // If we encounter a new block start, we close any blocks unmatched
    // in step 1 before creating the new block as a child of the last
    // matched block.
    //
    // 3.  Finally, we look at the remainder of the line (after block
    // markers like `>`, list markers, and indentation have been consumed).
    // This is text that can be incorporated into the last open
    // block (a paragraph, code block, heading, or raw HTML).
    //
    // Setext headings are formed when we see a line of a paragraph
    // that is a [setext heading underline].
    //
    // Reference link definitions are detected when a paragraph is closed;
    // the accumulated text lines are parsed to see if they begin with
    // one or more reference link definitions.  Any remainder becomes a
    // normal paragraph.
    //
    // We can see how this works by considering how the tree above is
    // generated by four lines of Markdown:
    //
    // ``` markdown
    // > Lorem ipsum dolor
    // sit amet.
    // > - Qui *quodsi iracundia*
    // > - aliquando id
    // ```
    //
    // At the outset, our document model is just
    //
    // ``` tree
    // -> document
    // ```
    //
    // The first line of our text,
    //
    // ``` markdown
    // > Lorem ipsum dolor
    // ```
    //
    // causes a `block_quote` block to be created as a child of our
    // open `document` block, and a `paragraph` block as a child of
    // the `block_quote`.  Then the text is added to the last open
    // block, the `paragraph`:
    //
    // ``` tree
    // -> document
    //   -> block_quote
    //     -> paragraph
    //          "Lorem ipsum dolor"
    // ```
    //
    // The next line,
    //
    // ``` markdown
    // sit amet.
    // ```
    //
    // is a "lazy continuation" of the open `paragraph`, so it gets added
    // to the paragraph's text:
    //
    // ``` tree
    // -> document
    //   -> block_quote
    //     -> paragraph
    //          "Lorem ipsum dolor\nsit amet."
    // ```
    //
    // The third line,
    //
    // ``` markdown
    // > - Qui *quodsi iracundia*
    // ```
    //
    // causes the `paragraph` block to be closed, and a new `list` block
    // opened as a child of the `block_quote`.  A `list_item` is also
    // added as a child of the `list`, and a `paragraph` as a child of
    // the `list_item`.  The text is then added to the new `paragraph`:
    //
    // ``` tree
    // -> document
    //   -> block_quote
    //        paragraph
    //          "Lorem ipsum dolor\nsit amet."
    //     -> list (type=bullet tight=true bullet_char=-)
    //       -> list_item
    //         -> paragraph
    //              "Qui *quodsi iracundia*"
    // ```
    //
    // The fourth line,
    //
    // ``` markdown
    // > - aliquando id
    // ```
    //
    // causes the `list_item` (and its child the `paragraph`) to be closed,
    // and a new `list_item` opened up as child of the `list`.  A `paragraph`
    // is added as a child of the new `list_item`, to contain the text.
    // We thus obtain the final tree:
    //
    // ``` tree
    // -> document
    //   -> block_quote
    //        paragraph
    //          "Lorem ipsum dolor\nsit amet."
    //     -> list (type=bullet tight=true bullet_char=-)
    //          list_item
    //            paragraph
    //              "Qui *quodsi iracundia*"
    //       -> list_item
    //         -> paragraph
    //              "aliquando id"
    // ```
    //
    // ## Phase 2: inline structure
    //
    // Once all of the input has been parsed, all open blocks are closed.
    //
    // We then "walk the tree," visiting every node, and parse raw
    // string contents of paragraphs and headings as inlines.  At this
    // point we have seen all the link reference definitions, so we can
    // resolve reference links as we go.
    //
    // ``` tree
    // document
    //   block_quote
    //     paragraph
    //       str "Lorem ipsum dolor"
    //       softbreak
    //       str "sit amet."
    //     list (type=bullet tight=true bullet_char=-)
    //       list_item
    //         paragraph
    //           str "Qui "
    //           emph
    //             str "quodsi iracundia"
    //       list_item
    //         paragraph
    //           str "aliquando id"
    // ```
    //
    // Notice how the [line ending] in the first paragraph has
    // been parsed as a `softbreak`, and the asterisks in the first list item
    // have become an `emph`.
    //
    // ### An algorithm for parsing nested emphasis and links
    //
    // By far the trickiest part of inline parsing is handling emphasis,
    // strong emphasis, links, and images.  This is done using the following
    // algorithm.
    //
    // When we're parsing inlines and we hit either
    //
    // - a run of `*` or `_` characters, or
    // - a `[` or `![`
    //
    // we insert a text node with these symbols as its literal content, and we
    // add a pointer to this text node to the [delimiter stack](@).
    //
    // The [delimiter stack] is a doubly linked list.  Each
    // element contains a pointer to a text node, plus information about
    //
    // - the type of delimiter (`[`, `![`, `*`, `_`)
    // - the number of delimiters,
    // - whether the delimiter is "active" (all are active to start), and
    // - whether the delimiter is a potential opener, a potential closer,
    //   or both (which depends on what sort of characters precede
    //   and follow the delimiters).
    //
    // When we hit a `]` character, we call the *look for link or image*
    // procedure (see below).
    //
    // When we hit the end of the input, we call the *process emphasis*
    // procedure (see below), with `stack_bottom` = NULL.
    //
    // #### *look for link or image*
    //
    // Starting at the top of the delimiter stack, we look backwards
    // through the stack for an opening `[` or `![` delimiter.
    //
    // - If we don't find one, we return a literal text node `]`.
    //
    // - If we do find one, but it's not *active*, we remove the inactive
    //   delimiter from the stack, and return a literal text node `]`.
    //
    // - If we find one and it's active, then we parse ahead to see if
    //   we have an inline link/image, reference link/image, compact reference
    //   link/image, or shortcut reference link/image.
    //
    //   + If we don't, then we remove the opening delimiter from the
    //     delimiter stack and return a literal text node `]`.
    //
    //   + If we do, then
    //
    //     * We return a link or image node whose children are the inlines
    //       after the text node pointed to by the opening delimiter.
    //
    //     * We run *process emphasis* on these inlines, with the `[` opener
    //       as `stack_bottom`.
    //
    //     * We remove the opening delimiter.
    //
    //     * If we have a link (and not an image), we also set all
    //       `[` delimiters before the opening delimiter to *inactive*.  (This
    //       will prevent us from getting links within links.)
    //
    // #### *process emphasis*
    //
    // Parameter `stack_bottom` sets a lower bound to how far we
    // descend in the [delimiter stack].  If it is NULL, we can
    // go all the way to the bottom.  Otherwise, we stop before
    // visiting `stack_bottom`.
    //
    // Let `current_position` point to the element on the [delimiter stack]
    // just above `stack_bottom` (or the first element if `stack_bottom`
    // is NULL).
    //
    // We keep track of the `openers_bottom` for each delimiter
    // type (`*`, `_`) and each length of the closing delimiter run
    // (modulo 3).  Initialize this to `stack_bottom`.
    //
    // Then we repeat the following until we run out of potential
    // closers:
    //
    // - Move `current_position` forward in the delimiter stack (if needed)
    //   until we find the first potential closer with delimiter `*` or `_`.
    //   (This will be the potential closer closest
    //   to the beginning of the input -- the first one in parse order.)
    //
    // - Now, look back in the stack (staying above `stack_bottom` and
    //   the `openers_bottom` for this delimiter type) for the
    //   first matching potential opener ("matching" means same delimiter).
    //
    // - If one is found:
    //
    //   + Figure out whether we have emphasis or strong emphasis:
    //     if both closer and opener spans have length >= 2, we have
    //     strong, otherwise regular.
    //
    //   + Insert an emph or strong emph node accordingly, after
    //     the text node corresponding to the opener.
    //
    //   + Remove any delimiters between the opener and closer from
    //     the delimiter stack.
    //
    //   + Remove 1 (for regular emph) or 2 (for strong emph) delimiters
    //     from the opening and closing text nodes.  If they become empty
    //     as a result, remove them and remove the corresponding element
    //     of the delimiter stack.  If the closing node is removed, reset
    //     `current_position` to the next element in the stack.
    //
    // - If none is found:
    //
    //   + Set `openers_bottom` to the element before `current_position`.
    //     (We know that there are no openers for this kind of closer up to and
    //     including this point, so this puts a lower bound on future searches.)
    //
    //   + If the closer at `current_position` is not a potential opener,
    //     remove it from the delimiter stack (since we know it can't
    //     be a closer either).
    //
    //   + Advance `current_position` to the next element in the stack.
    //
    // After we're done, we remove all delimiters above `stack_bottom` from the
    // delimiter stack.
  }
}
