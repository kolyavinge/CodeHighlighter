using System.Linq;
using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model;

public class TextSelectionTest
{
    private Text _text;
    private TextSelection _textSelection;

    [SetUp]
    public void Setup()
    {
        _text = new Text();
        _textSelection = new TextSelection();
    }


    [Test]
    public void Init()
    {
        Assert.AreEqual(0, _textSelection.StartLineIndex);
        Assert.AreEqual(0, _textSelection.StartCursorColumnIndex);
        Assert.AreEqual(0, _textSelection.EndLineIndex);
        Assert.AreEqual(0, _textSelection.EndCursorColumnIndex);
    }

    [Test]
    public void GetTextSelectionLines_1()
    {
        _text.SetText("01234\n01234\n01234\n01234\n01234");
        _textSelection.StartLineIndex = 0;
        _textSelection.StartCursorColumnIndex = 2;
        _textSelection.EndLineIndex = 3;
        _textSelection.EndCursorColumnIndex = 4;

        var result = _textSelection.GetSelectedLines(_text).ToList();

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
        _text.SetText("01234\n01234\n01234\n01234\n01234");
        _textSelection.StartLineIndex = 3;
        _textSelection.StartCursorColumnIndex = 4;
        _textSelection.EndLineIndex = 0;
        _textSelection.EndCursorColumnIndex = 2;

        var result = _textSelection.GetSelectedLines(_text).ToList();

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
    public void SelectAll()
    {
        _text.SetText("");
        _textSelection.StartLineIndex = 0;
        _textSelection.StartCursorColumnIndex = 0;
        _textSelection.EndLineIndex = 0;
        _textSelection.EndCursorColumnIndex = 0;

        var result = _textSelection.GetSelectedLines(_text).ToList();

        Assert.AreEqual(false, _textSelection.IsExist);
        Assert.AreEqual(0, result.Count);
    }
}
