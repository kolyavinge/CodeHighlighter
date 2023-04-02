using System;
using System.Linq;
using CodeHighlighter.Core;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Core;

internal class TextTest
{
    private Text _text;

    [SetUp]
    public void Setup()
    {
        _text = new Text();
    }

    [Test]
    public void Created()
    {
        Assert.AreEqual("", _text.ToString());
        Assert.AreEqual(1, _text.LinesCount);
    }

    [Test]
    public void TextContent()
    {
        _text.TextContent = "123\r\n456";
        Assert.AreEqual("123\r\n456", _text.TextContent);
        Assert.AreEqual("123\r\n456", _text.ToString());
    }

    [Test]
    public void SetEmptyString()
    {
        SetText("");
        Assert.AreEqual("", _text.ToString());
        Assert.AreEqual(1, _text.LinesCount);
    }

    [Test]
    public void Spaces()
    {
        SetText(" ");
        Assert.AreEqual(" ", _text.ToString());
        Assert.AreEqual(1, _text.LinesCount);
    }

    [Test]
    public void NewLine_First()
    {
        SetText("123");
        _text.AppendNewLine(new(0, 0));
        Assert.AreEqual("\r\n123", _text.ToString());
    }

    [Test]
    public void NewLine_Middle()
    {
        SetText("123");
        _text.AppendNewLine(new(0, 2));
        Assert.AreEqual("12\r\n3", _text.ToString());
    }

    [Test]
    public void NewLine_Last()
    {
        SetText("123");
        _text.AppendNewLine(new(0, 3));
        Assert.AreEqual("123\r\n", _text.ToString());
    }

    [Test]
    public void NewLine_VirtualCursor()
    {
        SetText("    123\r\n");
        _text.AppendNewLine(new(1, 4, CursorPositionKind.Virtual));
        Assert.AreEqual("    123\r\n\r\n", _text.ToString());
    }

    [Test]
    public void AppendChar_First()
    {
        SetText("123");
        _text.AppendChar(new(0, 0), 'a');
        Assert.AreEqual("a123", _text.ToString());
    }

    [Test]
    public void AppendChar_Middle()
    {
        SetText("123");
        _text.AppendChar(new(0, 1), 'a');
        Assert.AreEqual("1a23", _text.ToString());
    }

    [Test]
    public void AppendChar_Last()
    {
        SetText("123");
        _text.AppendChar(new(0, 3), 'a');
        Assert.AreEqual("123a", _text.ToString());
    }

    [Test]
    public void AppendChar_Many()
    {
        SetText("123");
        _text.AppendChar(new(0, 3), 'a', 3);
        Assert.AreEqual("123aaa", _text.ToString());
    }

    [Test]
    public void AppendChar_NotAllowedSymbols()
    {
        try
        {
            _text.AppendChar(new(0, 0), '\n');
            Assert.Fail();
        }
        catch (ArgumentException e)
        {
            Assert.AreEqual("ch", e.Message);
        }

        try
        {
            _text.AppendChar(new(0, 0), '\r');
            Assert.Fail();
        }
        catch (ArgumentException e)
        {
            Assert.AreEqual("ch", e.Message);
        }

        try
        {
            _text.AppendChar(new(0, 0), '\b');
            Assert.Fail();
        }
        catch (ArgumentException e)
        {
            Assert.AreEqual("ch", e.Message);
        }

        try
        {
            _text.AppendChar(new(0, 0), '\u001B');
            Assert.Fail();
        }
        catch (ArgumentException e)
        {
            Assert.AreEqual("ch", e.Message);
        }
    }

    [Test]
    public void AppendChar_Many_NotAllowedSymbols()
    {
        try
        {
            _text.AppendChar(new(0, 0), '\n', 3);
            Assert.Fail();
        }
        catch (ArgumentException e)
        {
            Assert.AreEqual("ch", e.Message);
        }

        try
        {
            _text.AppendChar(new(0, 0), '\r', 3);
            Assert.Fail();
        }
        catch (ArgumentException e)
        {
            Assert.AreEqual("ch", e.Message);
        }

        try
        {
            _text.AppendChar(new(0, 0), '\b', 3);
            Assert.Fail();
        }
        catch (ArgumentException e)
        {
            Assert.AreEqual("ch", e.Message);
        }

        try
        {
            _text.AppendChar(new(0, 0), '\u001B', 3);
            Assert.Fail();
        }
        catch (ArgumentException e)
        {
            Assert.AreEqual("ch", e.Message);
        }
    }

    [Test]
    public void InsertOneLine_Empty()
    {
        SetText("345");
        var result = _text.Insert(new(0, 0), new Text(""));
        Assert.AreEqual("345", _text.ToString());
        Assert.AreEqual(new CursorPosition(0, 0), result.StartPosition);
        Assert.AreEqual(new CursorPosition(0, 0), result.EndPosition);
        Assert.False(result.HasInserted);
    }

    [Test]
    public void InsertOneLine_Begin()
    {
        SetText("345");
        var result = _text.Insert(new(0, 0), new Text("12"));
        Assert.AreEqual("12345", _text.ToString());
        Assert.AreEqual(new CursorPosition(0, 0), result.StartPosition);
        Assert.AreEqual(new CursorPosition(0, 2), result.EndPosition);
        Assert.True(result.HasInserted);
    }

    [Test]
    public void InsertOneLine_Middle()
    {
        SetText("125");
        var result = _text.Insert(new(0, 2), new Text("34"));
        Assert.AreEqual("12345", _text.ToString());
        Assert.AreEqual(new CursorPosition(0, 2), result.StartPosition);
        Assert.AreEqual(new CursorPosition(0, 4), result.EndPosition);
        Assert.True(result.HasInserted);
    }

    [Test]
    public void InsertOneLine_End()
    {
        SetText("123");
        var result = _text.Insert(new(0, 3), new Text("45"));
        Assert.AreEqual("12345", _text.ToString());
        Assert.AreEqual(new CursorPosition(0, 3), result.StartPosition);
        Assert.AreEqual(new CursorPosition(0, 5), result.EndPosition);
        Assert.True(result.HasInserted);
    }

    [Test]
    public void InsertTwoLines_Begin()
    {
        SetText("45");
        var result = _text.Insert(new(0, 0), new Text("12\n3"));
        Assert.AreEqual("12\r\n345", _text.ToString());
        Assert.AreEqual(new CursorPosition(0, 0), result.StartPosition);
        Assert.AreEqual(new CursorPosition(1, 1), result.EndPosition);
    }

    [Test]
    public void InsertTwoLines_Middle()
    {
        SetText("125");
        var result = _text.Insert(new(0, 2), new Text("3\n4"));
        Assert.AreEqual("123\r\n45", _text.ToString());
        Assert.AreEqual(new CursorPosition(0, 2), result.StartPosition);
        Assert.AreEqual(new CursorPosition(1, 1), result.EndPosition);
    }

    [Test]
    public void InsertTwoLines_End()
    {
        SetText("12");
        var result = _text.Insert(new(0, 2), new Text("3\n45"));
        Assert.AreEqual("123\r\n45", _text.ToString());
        Assert.AreEqual(new CursorPosition(0, 2), result.StartPosition);
        Assert.AreEqual(new CursorPosition(1, 2), result.EndPosition);
    }

    [Test]
    public void InsertMultyLines_End()
    {
        SetText("18");
        var result = _text.Insert(new(0, 1), new Text("2\n34\n56\n7"));
        Assert.AreEqual("12\r\n34\r\n56\r\n78", _text.ToString());
        Assert.AreEqual(new CursorPosition(0, 1), result.StartPosition);
        Assert.AreEqual(new CursorPosition(3, 1), result.EndPosition);
    }

    [Test]
    public void LeftDelete_First()
    {
        SetText("123");
        var result = _text.LeftDelete(new(0, 0));
        Assert.AreEqual("123", _text.ToString());
        Assert.AreEqual(0, result.DeletedChar);
        Assert.False(result.IsLineDeleted);
        Assert.False(result.HasDeleted);
    }

    [Test]
    public void LeftDelete_Middle()
    {
        SetText("123");
        var result = _text.LeftDelete(new(0, 1));
        Assert.AreEqual("23", _text.ToString());
        Assert.AreEqual('1', result.DeletedChar);
        Assert.False(result.IsLineDeleted);
        Assert.True(result.HasDeleted);
    }

    [Test]
    public void LeftDelete_Last()
    {
        SetText("123");
        var result = _text.LeftDelete(new(0, 3));
        Assert.AreEqual("12", _text.ToString());
        Assert.AreEqual('3', result.DeletedChar);
        Assert.False(result.IsLineDeleted);
        Assert.True(result.HasDeleted);
    }

    [Test]
    public void LeftDelete_Return()
    {
        SetText("123\n456");
        var result = _text.LeftDelete(new(1, 0));
        Assert.AreEqual("123456", _text.ToString());
        Assert.AreEqual('\n', result.DeletedChar);
        Assert.True(result.IsLineDeleted);
        Assert.True(result.HasDeleted);
    }

    [Test]
    public void RightDelete_First()
    {
        SetText("123");
        var result = _text.RightDelete(new(0, 0));
        Assert.AreEqual("23", _text.ToString());
        Assert.AreEqual('1', result.DeletedChar);
        Assert.False(result.IsLineDeleted);
        Assert.True(result.HasDeleted);
    }

    [Test]
    public void RightDelete_Middle()
    {
        SetText("123");
        var result = _text.RightDelete(new(0, 1));
        Assert.AreEqual("13", _text.ToString());
        Assert.AreEqual('2', result.DeletedChar);
        Assert.False(result.IsLineDeleted);
        Assert.True(result.HasDeleted);
    }

    [Test]
    public void RightDelete_Last()
    {
        SetText("123");
        var result = _text.RightDelete(new(0, 2));
        Assert.AreEqual("12", _text.ToString());
        Assert.AreEqual('3', result.DeletedChar);
        Assert.False(result.IsLineDeleted);
        Assert.True(result.HasDeleted);
    }

    [Test]
    public void RightDelete_EndLine()
    {
        SetText("123");
        var result = _text.RightDelete(new(0, 3));
        Assert.AreEqual("123", _text.ToString());
        Assert.AreEqual(0, result.DeletedChar);
        Assert.False(result.IsLineDeleted);
        Assert.False(result.HasDeleted);
    }

    [Test]
    public void RightDelete_Return()
    {
        SetText("123\n456");
        var result = _text.RightDelete(new(0, 3));
        Assert.AreEqual("123456", _text.ToString());
        Assert.True(result.IsLineDeleted);
        Assert.AreEqual('\n', result.DeletedChar);
        Assert.True(result.HasDeleted);
    }

    [Test]
    public void DeleteSelection_OneLine_Begin()
    {
        SetText("012345");
        var result = _text.DeleteSelection(new TextSelection(_text).Set(new(0, 0), new(0, 4)).GetSelectedLines());
        Assert.AreEqual("45", _text.ToString());
        Assert.AreEqual(0, result.FirstDeletedLineIndex);
        Assert.AreEqual(0, result.DeletedLinesCount);
    }

    [Test]
    public void DeleteSelection_OneLine_End()
    {
        SetText("012345");
        _text.DeleteSelection(new TextSelection(_text).Set(new(0, 1), new(0, 6)).GetSelectedLines());
        Assert.AreEqual("0", _text.ToString());
    }

    [Test]
    public void DeleteSelection_OneLine_Middle()
    {
        SetText("012345");
        _text.DeleteSelection(new TextSelection(_text).Set(new(0, 1), new(0, 5)).GetSelectedLines());
        Assert.AreEqual("05", _text.ToString());
    }

    [Test]
    public void DeleteSelection_OneLine_All()
    {
        SetText("012345");
        _text.DeleteSelection(new TextSelection(_text).Set(new(0, 0), new(0, 6)).GetSelectedLines());
        Assert.AreEqual("", _text.ToString());
    }

    [Test]
    public void DeleteSelection_ManyLines_Begin()
    {
        SetText("012345\n012345");
        var result = _text.DeleteSelection(new TextSelection(_text).Set(new(0, 0), new(1, 4)).GetSelectedLines());
        Assert.AreEqual("45", _text.ToString());
        Assert.AreEqual(1, result.FirstDeletedLineIndex);
        Assert.AreEqual(1, result.DeletedLinesCount);
    }

    [Test]
    public void DeleteSelection_ManyLines_Middle()
    {
        SetText("012345\n012345");
        var result = _text.DeleteSelection(new TextSelection(_text).Set(new(0, 2), new(1, 4)).GetSelectedLines());
        Assert.AreEqual("0145", _text.ToString());
        Assert.AreEqual(1, result.FirstDeletedLineIndex);
        Assert.AreEqual(1, result.DeletedLinesCount);
    }

    [Test]
    public void DeleteSelection_ManyLines_End()
    {
        SetText("012345\n012345");
        var result = _text.DeleteSelection(new TextSelection(_text).Set(new(0, 2), new(1, 6)).GetSelectedLines());
        Assert.AreEqual("01", _text.ToString());
        Assert.AreEqual(1, result.FirstDeletedLineIndex);
        Assert.AreEqual(1, result.DeletedLinesCount);
    }

    [Test]
    public void DeleteSelection_ManyLines_DeleteFirst()
    {
        SetText("012345\n555555");
        var result = _text.DeleteSelection(new TextSelection(_text).Set(new(0, 0), new(0, 6)).GetSelectedLines());
        Assert.AreEqual("\r\n555555", _text.ToString());
        Assert.AreEqual(0, result.FirstDeletedLineIndex);
        Assert.AreEqual(0, result.DeletedLinesCount);
    }

    [Test]
    public void DeleteSelection_ManyLines_DeleteLast()
    {
        SetText("012345\n555555");
        var result = _text.DeleteSelection(new TextSelection(_text).Set(new(1, 0), new(1, 6)).GetSelectedLines());
        Assert.AreEqual("012345\r\n", _text.ToString());
        Assert.AreEqual(0, result.FirstDeletedLineIndex);
        Assert.AreEqual(0, result.DeletedLinesCount);
    }

    [Test]
    public void DeleteSelection_ManyLines_All()
    {
        SetText("012345\n012345");
        var result = _text.DeleteSelection(new TextSelection(_text).Set(new(0, 0), new(1, 6)).GetSelectedLines());
        Assert.AreEqual("", _text.ToString());
        Assert.AreEqual(1, result.FirstDeletedLineIndex);
        Assert.AreEqual(1, result.DeletedLinesCount);
    }

    [Test]
    public void DeleteLine()
    {
        SetText("123\n456");
        _text.DeleteLine(1);
        Assert.AreEqual("123", _text.ToString());
        _text.DeleteLine(0);
        Assert.AreEqual("", _text.ToString());
        _text.DeleteLine(0);
        Assert.AreEqual("", _text.ToString());
    }

    [Test]
    public void DeleteLines()
    {
        SetText("123\n456");
        _text.DeleteLines(0, 2);
        Assert.AreEqual("", _text.ToString());
        Assert.AreEqual(1, _text.Lines.Count());
        Assert.AreEqual(0, _text.Lines.First().Length);
    }

    [Test]
    public void ReplaceLines()
    {
        SetText("123\n456\n789");
        _text.ReplaceLines(0, 2);
        Assert.AreEqual("456\r\n789\r\n123", _text.ToString());
    }

    [Test]
    public void SetSelectedTextCase()
    {
        SetText("abc\nxyz");
        _text.SetSelectedTextCase(new TextSelection(_text).Set(new(0, 0), new(1, 1)).GetSelectedLines(), TextCase.Upper);
        Assert.AreEqual("ABC\r\nXyz", _text.ToString());

        SetText("ABC\nXYZ");
        _text.SetSelectedTextCase(new TextSelection(_text).Set(new(0, 0), new(1, 1)).GetSelectedLines(), TextCase.Lower);
        Assert.AreEqual("abc\r\nxYZ", _text.ToString());
    }

    private void SetText(string textString)
    {
        _text.TextContent = textString;
    }
}
