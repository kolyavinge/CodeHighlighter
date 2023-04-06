using System.Linq;
using CodeHighlighter.Core;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Core;

internal class TextSelectionLineConverterTest
{
    private Text _text;
    private TextSelectionLineConverter _converter;

    [SetUp]
    public void Setup()
    {
        _text = new Text();
        _converter = new TextSelectionLineConverter(_text);
    }

    [Test]
    public void GetTextSelectionLines_1()
    {
        _text.TextContent = "01234\n01234\n01234\n01234\n01234";

        var result = _converter.GetSelectedLines(new(0, 0), new(0, 4)).ToList();

        Assert.AreEqual(1, result.Count);

        Assert.AreEqual(0, result[0].LineIndex);
        Assert.AreEqual(0, result[0].LeftColumnIndex);
        Assert.AreEqual(4, result[0].RightColumnIndex);
    }

    [Test]
    public void GetTextSelectionLines_2()
    {
        _text.TextContent = "01234\n01234\n01234\n01234\n01234";

        var result = _converter.GetSelectedLines(new(0, 2), new(3, 4)).ToList();

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
    public void VirtualCursor()
    {
        _text.TextContent = "    000\r\n\r\n111";

        var result = _converter.GetSelectedLines(new(1, 4, CursorPositionKind.Virtual), new(2, 0)).ToList();

        Assert.AreEqual(2, result.Count);

        Assert.AreEqual(1, result[0].LineIndex);
        Assert.AreEqual(4, result[0].LeftColumnIndex);
        Assert.AreEqual(4, result[0].RightColumnIndex);

        Assert.AreEqual(2, result[1].LineIndex);
        Assert.AreEqual(0, result[1].LeftColumnIndex);
        Assert.AreEqual(0, result[1].RightColumnIndex);
    }
}
