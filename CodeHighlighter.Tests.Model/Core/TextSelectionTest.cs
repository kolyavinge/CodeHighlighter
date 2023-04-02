using System.Linq;
using CodeHighlighter.Core;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Core;

internal class TextSelectionTest
{
    private Text _text;
    private TextSelection _textSelection;

    [SetUp]
    public void Setup()
    {
        _text = new Text();
        _textSelection = new TextSelection(_text);
    }


    [Test]
    public void Init()
    {
        Assert.False(_textSelection.IsExist);
        Assert.AreEqual(0, _textSelection.StartPosition.LineIndex);
        Assert.AreEqual(0, _textSelection.StartPosition.ColumnIndex);
        Assert.AreEqual(0, _textSelection.EndPosition.LineIndex);
        Assert.AreEqual(0, _textSelection.EndPosition.ColumnIndex);
    }

    [Test]
    public void GetTextSelectionLines_1()
    {
        _text.TextContent = "01234\n01234\n01234\n01234\n01234";
        _textSelection.StartPosition = new(0, 2);
        _textSelection.EndPosition = new(3, 4);

        var result = _textSelection.GetSelectedLines().ToList();

        Assert.True(_textSelection.IsExist);
        Assert.AreEqual(4, result.Count);

        Assert.AreEqual(0, result[0].LineIndex);
        Assert.AreEqual(2, result[0].LeftColumnIndex);
        Assert.AreEqual(5, result[0].RightColumnIndex);

        Assert.AreEqual(1, result[1].LineIndex);
        Assert.AreEqual(0, result[1].LeftColumnIndex);
        Assert.AreEqual(5, result[1].RightColumnIndex);

        Assert.AreEqual(2, result[2].LineIndex);
        Assert.AreEqual(0, result[2].LeftColumnIndex);
        Assert.AreEqual(5, result[2].RightColumnIndex);

        Assert.AreEqual(3, result[3].LineIndex);
        Assert.AreEqual(0, result[3].LeftColumnIndex);
        Assert.AreEqual(4, result[3].RightColumnIndex);
    }

    [Test]
    public void GetTextSelectionLines_2()
    {
        _text.TextContent = "01234\n01234\n01234\n01234\n01234";
        _textSelection.StartPosition = new(3, 4);
        _textSelection.EndPosition = new(0, 2);

        var result = _textSelection.GetSelectedLines().ToList();

        Assert.True(_textSelection.IsExist);
        Assert.AreEqual(4, result.Count);

        Assert.AreEqual(0, result[0].LineIndex);
        Assert.AreEqual(2, result[0].LeftColumnIndex);
        Assert.AreEqual(5, result[0].RightColumnIndex);

        Assert.AreEqual(1, result[1].LineIndex);
        Assert.AreEqual(0, result[1].LeftColumnIndex);
        Assert.AreEqual(5, result[1].RightColumnIndex);

        Assert.AreEqual(2, result[2].LineIndex);
        Assert.AreEqual(0, result[2].LeftColumnIndex);
        Assert.AreEqual(5, result[2].RightColumnIndex);

        Assert.AreEqual(3, result[3].LineIndex);
        Assert.AreEqual(0, result[3].LeftColumnIndex);
        Assert.AreEqual(4, result[3].RightColumnIndex);
    }

    [Test]
    public void SelectAll_Empty()
    {
        _text.TextContent = "";
        _textSelection.StartPosition = new();
        _textSelection.EndPosition = new();

        var result = _textSelection.GetSelectedLines().ToList();

        Assert.False(_textSelection.IsExist);
        Assert.AreEqual(0, result.Count);
    }

    [Test]
    public void SelectAll_Reset()
    {
        _textSelection.Reset();

        var result = _textSelection.GetSelectedLines().ToList();

        Assert.False(_textSelection.IsExist);
        Assert.AreEqual(0, result.Count);
        Assert.AreEqual(0, _textSelection.StartPosition.LineIndex);
        Assert.AreEqual(0, _textSelection.StartPosition.ColumnIndex);
        Assert.AreEqual(0, _textSelection.EndPosition.LineIndex);
        Assert.AreEqual(0, _textSelection.EndPosition.ColumnIndex);
    }

    [Test]
    public void VirtualCursor()
    {
        _text.TextContent = "    000\r\n\r\n111";
        _textSelection.StartPosition = new(1, 4, CursorPositionKind.Virtual);
        _textSelection.EndPosition = new(2, 0);

        var result = _textSelection.GetSelectedLines().ToList();

        Assert.True(_textSelection.IsExist);
        Assert.AreEqual(2, result.Count);

        Assert.AreEqual(1, result[0].LineIndex);
        Assert.AreEqual(4, result[0].LeftColumnIndex);
        Assert.AreEqual(4, result[0].RightColumnIndex);

        Assert.AreEqual(2, result[1].LineIndex);
        Assert.AreEqual(0, result[1].LeftColumnIndex);
        Assert.AreEqual(0, result[1].RightColumnIndex);
    }
}
