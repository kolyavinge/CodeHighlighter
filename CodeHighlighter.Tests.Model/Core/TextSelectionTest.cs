using System.Linq;
using CodeHighlighter.Core;
using Moq;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Core;

internal class TextSelectionTest
{
    private Mock<ITextSelectionLineConverter> _textSelectionLineConverter;
    private TextSelection _textSelection;

    [SetUp]
    public void Setup()
    {
        _textSelectionLineConverter = new Mock<ITextSelectionLineConverter>();
        _textSelection = new TextSelection(_textSelectionLineConverter.Object);
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
    public void SelectAll_Empty()
    {
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
    public void GetTextSelectionLines_1()
    {
        // 01234\n01234\n01234\n01234\n01234
        _textSelection.StartPosition = new(0, 2);
        _textSelection.EndPosition = new(3, 4);

        var result = _textSelection.GetSelectedLines().ToList();

        _textSelectionLineConverter.Verify(x => x.GetSelectedLines(new(0, 2), new(3, 4)));
    }

    [Test]
    public void GetTextSelectionLines_2()
    {
        // 01234\n01234\n01234\n01234\n01234
        _textSelection.StartPosition = new(3, 4);
        _textSelection.EndPosition = new(0, 2);

        var result = _textSelection.GetSelectedLines().ToList();

        _textSelectionLineConverter.Verify(x => x.GetSelectedLines(new(0, 2), new(3, 4)));
    }

    [Test]
    public void VirtualCursor()
    {
        // "    000\r\n\r\n111"
        _textSelection.StartPosition = new(1, 4, CursorPositionKind.Virtual);
        _textSelection.EndPosition = new(2, 0);

        var result = _textSelection.GetSelectedLines().ToList();

        _textSelectionLineConverter.Verify(x => x.GetSelectedLines(new(1, 4, CursorPositionKind.Virtual), new(2, 0)));
    }
}
