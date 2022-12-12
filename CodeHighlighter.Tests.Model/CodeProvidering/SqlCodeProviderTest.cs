using System.Collections.Generic;
using System.IO;
using System.Linq;
using CodeHighlighter.CodeProvidering;
using NUnit.Framework;

namespace CodeHighlighter.Tests.CodeProvidering;

internal class SqlCodeProviderTest
{
    private SqlCodeProvider _provider;

    [SetUp]
    public void Init()
    {
        _provider = new SqlCodeProvider();
    }

    [Test]
    public void Query()
    {
        var text = "select * from MyTable";
        var tokens = GetTokens(text);
        Assert.AreEqual(4, tokens.Count);
        int i = 0;
        Assert.AreEqual(0, tokens[i].StartColumnIndex);
        Assert.AreEqual(6, tokens[i].Length);
        Assert.AreEqual((byte)TokenKind.Keyword, tokens[i].Kind);
        i++;
        Assert.AreEqual(7, tokens[i].StartColumnIndex);
        Assert.AreEqual(1, tokens[i].Length);
        Assert.AreEqual((byte)TokenKind.Delimiter, tokens[i].Kind);
        i++;
        Assert.AreEqual(9, tokens[i].StartColumnIndex);
        Assert.AreEqual(4, tokens[i].Length);
        Assert.AreEqual((byte)TokenKind.Keyword, tokens[i].Kind);
        i++;
        Assert.AreEqual(14, tokens[i].StartColumnIndex);
        Assert.AreEqual(7, tokens[i].Length);
        Assert.AreEqual((byte)TokenKind.Identifier, tokens[i].Kind);
    }

    [Test]
    public void QueryTempTable()
    {
        var text = "select * from #TempTable";
        var tokens = GetTokens(text);
        Assert.AreEqual(4, tokens.Count);
        Assert.AreEqual((byte)TokenKind.Identifier, tokens[3].Kind);
    }

    [Test]
    public void QueryWithWhere()
    {
        var text = "select * from MyTable where a > 1";
        var tokens = GetTokens(text);
        Assert.AreEqual(8, tokens.Count);
        int i = 0;
        Assert.AreEqual(0, tokens[i].StartColumnIndex);
        Assert.AreEqual(6, tokens[i].Length);
        Assert.AreEqual((byte)TokenKind.Keyword, tokens[i].Kind);
        i++;
        Assert.AreEqual(7, tokens[i].StartColumnIndex);
        Assert.AreEqual(1, tokens[i].Length);
        Assert.AreEqual((byte)TokenKind.Delimiter, tokens[i].Kind);
        i++;
        Assert.AreEqual(9, tokens[i].StartColumnIndex);
        Assert.AreEqual(4, tokens[i].Length);
        Assert.AreEqual((byte)TokenKind.Keyword, tokens[i].Kind);
        i++;
        Assert.AreEqual(14, tokens[i].StartColumnIndex);
        Assert.AreEqual(7, tokens[i].Length);
        Assert.AreEqual((byte)TokenKind.Identifier, tokens[i].Kind);
        i++;
        Assert.AreEqual(22, tokens[i].StartColumnIndex);
        Assert.AreEqual(5, tokens[i].Length);
        Assert.AreEqual((byte)TokenKind.Keyword, tokens[i].Kind);
        i++;
        Assert.AreEqual(28, tokens[i].StartColumnIndex);
        Assert.AreEqual(1, tokens[i].Length);
        Assert.AreEqual((byte)TokenKind.Identifier, tokens[i].Kind);
        i++;
        Assert.AreEqual(30, tokens[i].StartColumnIndex);
        Assert.AreEqual(1, tokens[i].Length);
        Assert.AreEqual((byte)TokenKind.Delimiter, tokens[i].Kind);
        i++;
        Assert.AreEqual(32, tokens[i].StartColumnIndex);
        Assert.AreEqual(1, tokens[i].Length);
        Assert.AreEqual((byte)TokenKind.Other, tokens[i].Kind);
    }

    [Test]
    public void Unescape()
    {
        var text = "select [group] from MyTable";
        var tokens = GetTokens(text);
        Assert.AreEqual(4, tokens.Count);
        Assert.AreEqual(7, tokens[1].StartColumnIndex);
        Assert.AreEqual(7, tokens[1].Length);
        Assert.AreEqual((byte)TokenKind.Identifier, tokens[1].Kind);
    }

    [Test]
    public void Unescape2()
    {
        var text = "select [group]from MyTable";
        var tokens = GetTokens(text);

        Assert.AreEqual(4, tokens.Count);

        Assert.AreEqual(7, tokens[1].StartColumnIndex);
        Assert.AreEqual(7, tokens[1].Length);
        Assert.AreEqual((byte)TokenKind.Identifier, tokens[1].Kind);

        Assert.AreEqual(14, tokens[2].StartColumnIndex);
        Assert.AreEqual(4, tokens[2].Length);
        Assert.AreEqual((byte)TokenKind.Keyword, tokens[2].Kind);
    }

    [Test]
    public void UnescapeWithDelimiter()
    {
        var text = "select [group-1] from MyTable";
        var tokens = GetTokens(text);
        Assert.AreEqual(4, tokens.Count);
        Assert.AreEqual((byte)TokenKind.Identifier, tokens[1].Kind);
    }

    [Test]
    public void ReturnAfterIdentifier()
    {
        var text = "select * from [MyTable\n]";
        var tokens = GetTokens(text);
        Assert.AreEqual(4, tokens.Count);
        Assert.AreEqual((byte)TokenKind.Other, tokens[3].Kind);
    }

    [Test]
    public void Comments()
    {
        var text = "select * from MyTable -- comments";
        var tokens = GetTokens(text);
        Assert.AreEqual(5, tokens.Count);
        Assert.AreEqual(22, tokens[4].StartColumnIndex);
        Assert.AreEqual(11, tokens[4].Length);
        Assert.AreEqual((byte)TokenKind.Comment, tokens[4].Kind);
    }

    [Test]
    public void Strings()
    {
        var text = "'select [group] from MyTable'";
        var tokens = GetTokens(text);
        Assert.AreEqual(1, tokens.Count);
        Assert.AreEqual(0, tokens[0].StartColumnIndex);
        Assert.AreEqual(29, tokens[0].Length);
        Assert.AreEqual((byte)TokenKind.String, tokens[0].Kind);
    }

    [Test]
    public void StringsMultiline()
    {
        var text = @"'select [group]
from MyTable'";
        var tokens = GetTokens(text);
        Assert.AreEqual(1, tokens.Count);
        Assert.AreEqual(0, tokens[0].StartColumnIndex);
        Assert.AreEqual(29, tokens[0].Length);
        Assert.AreEqual((byte)TokenKind.String, tokens[0].Kind);
    }

    [Test]
    public void Variables()
    {
        var text = "declare @var int";
        var tokens = GetTokens(text);
        Assert.AreEqual(3, tokens.Count);
        Assert.AreEqual((byte)TokenKind.Keyword, tokens[0].Kind);
        Assert.AreEqual((byte)TokenKind.Variable, tokens[1].Kind);
        Assert.AreEqual((byte)TokenKind.Keyword, tokens[2].Kind);
    }

    [Test]
    public void N()
    {
        var text = "N'string'";
        var tokens = GetTokens(text);
        Assert.AreEqual(2, tokens.Count);
        Assert.AreEqual((byte)TokenKind.Other, tokens[0].Kind);
        Assert.AreEqual((byte)TokenKind.String, tokens[1].Kind);
    }

    [Test]
    public void SQLFile_1()
    {
        var text = File.ReadAllText(@"D:\Projects\CodeHighlighter\CodeHighlighter.Tests.Model\Examples\sql_1.sql");
        var tokens = GetTokens(text);
        Assert.AreEqual(637, tokens.Count);
        tokens.ForEach(x => Assert.IsTrue(x.StartColumnIndex <= text.Length));
    }

    [Test]
    public void SQLFile_2()
    {
        var text = File.ReadAllText(@"D:\Projects\CodeHighlighter\CodeHighlighter.Tests.Model\Examples\sql_2.sql");
        var tokens = GetTokens(text);
        Assert.AreEqual(1744, tokens.Count);
        tokens.ForEach(x => Assert.IsTrue(x.StartColumnIndex <= text.Length));
    }

    [Test]
    public void SQLFile_3()
    {
        var text = File.ReadAllText(@"D:\Projects\CodeHighlighter\CodeHighlighter.Tests.Model\Examples\sql_3.sql");
        var tokens = GetTokens(text);
        Assert.AreEqual(31058, tokens.Count);
        tokens.ForEach(x => Assert.IsTrue(x.StartColumnIndex <= text.Length));
    }

    private List<Token> GetTokens(string textString)
    {
        var text = new CodeHighlighter.Model.Text();
        text.TextContent = textString;
        return _provider.GetTokens(new CodeHighlighter.Model.ForwardTextIterator(text, 0, text.LinesCount - 1)).ToList();
    }
}
