using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model;

public class TextTest
{
    private Text _text;
    private int _textChangedCount;

    [SetUp]
    public void Setup()
    {
        _text = new Text();
        _text.TextChanged += (s, e) => _textChangedCount++;
        _textChangedCount = 0;
    }

    [Test]
    public void Created()
    {
        Assert.AreEqual("", _text.ToString());
        Assert.AreEqual(1, _text.LinesCount);
        Assert.AreEqual(0, _text.VisibleLinesCount);
    }

    [Test]
    public void SetEmptyString()
    {
        SetText("");
        Assert.AreEqual("", _text.ToString());
        Assert.AreEqual(1, _text.LinesCount);
        Assert.AreEqual(0, _text.VisibleLinesCount);
    }

    [Test]
    public void Spaces()
    {
        SetText(" ");
        Assert.AreEqual(" ", _text.ToString());
        Assert.AreEqual(1, _text.LinesCount);
        Assert.AreEqual(1, _text.VisibleLinesCount);
    }

    [Test]
    public void NewLine_First()
    {
        SetText("123");
        _text.NewLine(0, 0);
        Assert.AreEqual("\r\n123", _text.ToString());
    }

    [Test]
    public void NewLine_Middle()
    {
        SetText("123");
        _text.NewLine(0, 2);
        Assert.AreEqual("12\r\n3", _text.ToString());
    }

    [Test]
    public void NewLine_Last()
    {
        SetText("123");
        _text.NewLine(0, 3);
        Assert.AreEqual("123\r\n", _text.ToString());
    }

    [Test]
    public void AppendChar_First()
    {
        SetText("123");
        _text.AppendChar(0, 0, 'a');
        Assert.AreEqual("a123", _text.ToString());
    }

    [Test]
    public void AppendChar_Middle()
    {
        SetText("123");
        _text.AppendChar(0, 1, 'a');
        Assert.AreEqual("1a23", _text.ToString());
    }

    [Test]
    public void AppendChar_Last()
    {
        SetText("123");
        _text.AppendChar(0, 3, 'a');
        Assert.AreEqual("123a", _text.ToString());
    }

    [Test]
    public void InsertOneLine_Begin()
    {
        SetText("345");
        _text.Insert(0, 0, new Text("12"));
        Assert.AreEqual("12345", _text.ToString());
    }

    [Test]
    public void InsertOneLine_Middle()
    {
        SetText("125");
        _text.Insert(0, 2, new Text("34"));
        Assert.AreEqual("12345", _text.ToString());
    }

    [Test]
    public void InsertOneLine_End()
    {
        SetText("123");
        _text.Insert(0, 3, new Text("45"));
        Assert.AreEqual("12345", _text.ToString());
    }

    [Test]
    public void InsertTwoLines_Begin()
    {
        SetText("45");
        _text.Insert(0, 0, new Text("12\n3"));
        Assert.AreEqual("12\r\n345", _text.ToString());
    }

    [Test]
    public void InsertTwoLines_Middle()
    {
        SetText("125");
        _text.Insert(0, 2, new Text("3\n4"));
        Assert.AreEqual("123\r\n45", _text.ToString());
    }

    [Test]
    public void InsertTwoLines_End()
    {
        SetText("12");
        _text.Insert(0, 2, new Text("3\n45"));
        Assert.AreEqual("123\r\n45", _text.ToString());
    }

    [Test]
    public void InsertMultyLines_End()
    {
        SetText("18");
        _text.Insert(0, 1, new Text("2\n34\n56\n7"));
        Assert.AreEqual("12\r\n34\r\n56\r\n78", _text.ToString());
    }

    [Test]
    public void LeftDelete_First()
    {
        SetText("123");
        _text.LeftDelete(0, 0);
        Assert.AreEqual("123", _text.ToString());
    }

    [Test]
    public void LeftDelete_Middle()
    {
        SetText("123");
        _text.LeftDelete(0, 1);
        Assert.AreEqual("23", _text.ToString());
    }

    [Test]
    public void LeftDelete_Last()
    {
        SetText("123");
        _text.LeftDelete(0, 3);
        Assert.AreEqual("12", _text.ToString());
    }

    [Test]
    public void LeftDelete_Return()
    {
        SetText("123\n456");
        var result = _text.LeftDelete(1, 0);
        Assert.AreEqual("123456", _text.ToString());
        Assert.True(result.IsLineDeleted);
    }

    [Test]
    public void RightDelete_First()
    {
        SetText("123");
        _text.RightDelete(0, 0);
        Assert.AreEqual("23", _text.ToString());
    }

    [Test]
    public void RightDelete_Middle()
    {
        SetText("123");
        _text.RightDelete(0, 1);
        Assert.AreEqual("13", _text.ToString());
    }

    [Test]
    public void RightDelete_Last()
    {
        SetText("123");
        _text.RightDelete(0, 2);
        Assert.AreEqual("12", _text.ToString());
    }

    [Test]
    public void RightDelete_EndLine()
    {
        SetText("123");
        _text.RightDelete(0, 3);
        Assert.AreEqual("123", _text.ToString());
    }

    [Test]
    public void RightDelete_Return()
    {
        SetText("123\n456");
        var result = _text.RightDelete(0, 3);
        Assert.AreEqual("123456", _text.ToString());
        Assert.True(result.IsLineDeleted);
    }

    [Test]
    public void DeleteSelection_OneLine_Begin()
    {
        SetText("012345");
        var result = _text.DeleteSelection(new TextSelection(0, 0, 0, 4));
        Assert.AreEqual("45", _text.ToString());
        Assert.AreEqual(0, result.FirstDeletedLineIndex);
        Assert.AreEqual(0, result.DeletedLinesCount);
    }

    [Test]
    public void DeleteSelection_OneLine_End()
    {
        SetText("012345");
        _text.DeleteSelection(new TextSelection(0, 1, 0, 6));
        Assert.AreEqual("0", _text.ToString());
    }

    [Test]
    public void DeleteSelection_OneLine_Middle()
    {
        SetText("012345");
        _text.DeleteSelection(new TextSelection(0, 1, 0, 5));
        Assert.AreEqual("05", _text.ToString());
    }

    [Test]
    public void DeleteSelection_OneLine_All()
    {
        SetText("012345");
        _text.DeleteSelection(new TextSelection(0, 0, 0, 6));
        Assert.AreEqual("", _text.ToString());
    }

    [Test]
    public void DeleteSelection_ManyLines_Begin()
    {
        SetText("012345\n012345");
        var result = _text.DeleteSelection(new TextSelection(0, 0, 1, 4));
        Assert.AreEqual("45", _text.ToString());
        Assert.AreEqual(1, result.FirstDeletedLineIndex);
        Assert.AreEqual(1, result.DeletedLinesCount);
    }

    [Test]
    public void DeleteSelection_ManyLines_Middle()
    {
        SetText("012345\n012345");
        var result = _text.DeleteSelection(new TextSelection(0, 2, 1, 4));
        Assert.AreEqual("0145", _text.ToString());
        Assert.AreEqual(1, result.FirstDeletedLineIndex);
        Assert.AreEqual(1, result.DeletedLinesCount);
    }

    [Test]
    public void DeleteSelection_ManyLines_End()
    {
        SetText("012345\n012345");
        var result = _text.DeleteSelection(new TextSelection(0, 2, 1, 6));
        Assert.AreEqual("01", _text.ToString());
        Assert.AreEqual(1, result.FirstDeletedLineIndex);
        Assert.AreEqual(1, result.DeletedLinesCount);
    }

    [Test]
    public void DeleteSelection_ManyLines_DeleteFirst()
    {
        SetText("012345\n555555");
        var result = _text.DeleteSelection(new TextSelection(0, 0, 0, 6));
        Assert.AreEqual("\r\n555555", _text.ToString());
        Assert.AreEqual(0, result.FirstDeletedLineIndex);
        Assert.AreEqual(0, result.DeletedLinesCount);
    }

    [Test]
    public void DeleteSelection_ManyLines_DeleteLast()
    {
        SetText("012345\n555555");
        var result = _text.DeleteSelection(new TextSelection(1, 0, 1, 6));
        Assert.AreEqual("012345\r\n", _text.ToString());
        Assert.AreEqual(0, result.FirstDeletedLineIndex);
        Assert.AreEqual(0, result.DeletedLinesCount);
    }

    [Test]
    public void DeleteSelection_ManyLines_All()
    {
        SetText("012345\n012345");
        var result = _text.DeleteSelection(new TextSelection(0, 0, 1, 6));
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
        _text.SetSelectedTextCase(new TextSelection(0, 0, 1, 1), TextCase.Upper);
        Assert.AreEqual("ABC\r\nXyz", _text.ToString());

        SetText("ABC\nXYZ");
        _text.SetSelectedTextCase(new TextSelection(0, 0, 1, 1), TextCase.Lower);
        Assert.AreEqual("abc\r\nxYZ", _text.ToString());
    }

    [Test]
    public void SetText_RaiseTextChanged()
    {
        _text.SetText("123");
        Assert.AreEqual(1, _textChangedCount);
    }

    [Test]
    public void NewLine_RaiseTextChanged()
    {
        _text.NewLine(0, 0);
        Assert.AreEqual(1, _textChangedCount);
    }

    [Test]
    public void AppendChar_RaiseTextChanged()
    {
        _text.AppendChar(0, 0, '1');
        Assert.AreEqual(1, _textChangedCount);
    }

    [Test]
    public void Insert_OneLine_RaiseTextChanged()
    {
        _text.Insert(0, 0, new Text("123"));
        Assert.AreEqual(1, _textChangedCount);
    }

    [Test]
    public void Insert_TwoLines_RaiseTextChanged()
    {
        _text.Insert(0, 0, new Text("123\n123"));
        Assert.AreEqual(2, _textChangedCount);
    }

    [Test]
    public void LeftDelete_RaiseTextChanged()
    {
        _text.SetText("123\n123");

        _text.LeftDelete(0, 0);
        Assert.AreEqual(1, _textChangedCount); // no changes

        _text.LeftDelete(0, 1);
        Assert.AreEqual(2, _textChangedCount);

        _text.LeftDelete(1, 0);
        Assert.AreEqual(3, _textChangedCount);
    }

    [Test]
    public void RightDelete_RaiseTextChanged()
    {
        _text.SetText("123\n123");

        _text.RightDelete(1, 0);
        Assert.AreEqual(2, _textChangedCount); // no changes

        _text.RightDelete(0, 0);
        Assert.AreEqual(3, _textChangedCount);

        _text.RightDelete(1, 0);
        Assert.AreEqual(4, _textChangedCount);
    }

    [Test]
    public void DeleteSelection_RaiseTextChanged()
    {
        _text.SetText("123\n123");

        _text.DeleteSelection(new TextSelection(0, 0, 0, 1));
        Assert.AreEqual(2, _textChangedCount);

        _text.DeleteSelection(new TextSelection(0, 0, 1, 0));
        Assert.AreEqual(3, _textChangedCount);
    }

    [Test]
    public void DeleteLine_RaiseTextChanged()
    {
        _text.SetText("123\n123");
        _text.DeleteLine(0);
        Assert.AreEqual(2, _textChangedCount);
    }

    [Test]
    public void DeleteLines_RaiseTextChanged()
    {
        _text.SetText("123\n123");
        _text.DeleteLines(0, 1);
        Assert.AreEqual(2, _textChangedCount);
    }

    [Test]
    public void ReplaceLines_RaiseTextChanged()
    {
        _text.SetText("123\n123");
        _text.ReplaceLines(0, 1);
        Assert.AreEqual(2, _textChangedCount);
    }

    private void SetText(string textString)
    {
        _text.SetText(textString);
    }
}
