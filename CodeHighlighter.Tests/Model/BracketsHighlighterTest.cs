using CodeHighlighter.Model;
using Moq;
using NUnit.Framework;
using static CodeHighlighter.Model.BracketsHighlighter;

namespace CodeHighlighter.Tests.Model;

class BracketsHighlighterTest
{
    private Mock<IText> _text;
    private Mock<ITextCursor> _textCursor;
    private BracketsHighlighter _highlighter;
    private HighlightResult _result;

    [SetUp]
    public void Setup()
    {
        _text = new Mock<IText>();
        _textCursor = new Mock<ITextCursor>();
        _highlighter = new BracketsHighlighter("()", _text.Object, _textCursor.Object);
    }

    [Test]
    public void NoBracketsInHighlighter()
    {
        _highlighter = new BracketsHighlighter("", _text.Object, _textCursor.Object);

        _text.Setup(x => x.GetLine(0)).Returns(new TextLine("()"));
        _text.SetupGet(x => x.LinesCount).Returns(1);

        GetResult(0, 0);
        Assert.AreEqual(HighlightKind.NoHighlight, _result.Kind);
        Assert.AreEqual(default(BracketPosition), _result.Open);
        Assert.AreEqual(default(BracketPosition), _result.Close);
    }

    [Test]
    public void Empty()
    {
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine(""));
        _text.SetupGet(x => x.LinesCount).Returns(1);

        GetResult(0, 0);
        Assert.AreEqual(HighlightKind.NoHighlight, _result.Kind);
        Assert.AreEqual(default(BracketPosition), _result.Open);
        Assert.AreEqual(default(BracketPosition), _result.Close);
    }

    [Test]
    public void Forward()
    {
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine("()"));
        _text.SetupGet(x => x.LinesCount).Returns(1);

        GetResult(0, 0);
        Assert.AreEqual(HighlightKind.Highlighted, _result.Kind);
        Assert.AreEqual(new BracketPosition(0, 0), _result.Open);
        Assert.AreEqual(new BracketPosition(0, 1), _result.Close);

        GetResult(0, 1);
        Assert.AreEqual(HighlightKind.Highlighted, _result.Kind);
        Assert.AreEqual(new BracketPosition(0, 0), _result.Open);
        Assert.AreEqual(new BracketPosition(0, 1), _result.Close);

        GetResult(0, 2);
        Assert.AreEqual(HighlightKind.Highlighted, _result.Kind);
        Assert.AreEqual(new BracketPosition(0, 0), _result.Open);
        Assert.AreEqual(new BracketPosition(0, 1), _result.Close);
    }

    [Test]
    public void ForwardOneLine()
    {
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine(" ( ) "));
        _text.SetupGet(x => x.LinesCount).Returns(1);

        GetResult(0, 0);
        Assert.AreEqual(HighlightKind.NoHighlight, _result.Kind);
        Assert.AreEqual(default(BracketPosition), _result.Open);
        Assert.AreEqual(default(BracketPosition), _result.Close);

        GetResult(0, 1);
        Assert.AreEqual(HighlightKind.Highlighted, _result.Kind);
        Assert.AreEqual(new BracketPosition(0, 1), _result.Open);
        Assert.AreEqual(new BracketPosition(0, 3), _result.Close);

        GetResult(0, 2);
        Assert.AreEqual(HighlightKind.Highlighted, _result.Kind);
        Assert.AreEqual(new BracketPosition(0, 1), _result.Open);
        Assert.AreEqual(new BracketPosition(0, 3), _result.Close);

        GetResult(0, 3);
        Assert.AreEqual(HighlightKind.Highlighted, _result.Kind);
        Assert.AreEqual(new BracketPosition(0, 1), _result.Open);
        Assert.AreEqual(new BracketPosition(0, 3), _result.Close);

        GetResult(0, 4);
        Assert.AreEqual(HighlightKind.Highlighted, _result.Kind);
        Assert.AreEqual(new BracketPosition(0, 1), _result.Open);
        Assert.AreEqual(new BracketPosition(0, 3), _result.Close);

        GetResult(0, 5);
        Assert.AreEqual(HighlightKind.NoHighlight, _result.Kind);
        Assert.AreEqual(default(BracketPosition), _result.Open);
        Assert.AreEqual(default(BracketPosition), _result.Close);
    }

    [Test]
    public void ForwardTwoLine()
    {
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine(" ( "));
        _text.Setup(x => x.GetLine(1)).Returns(new TextLine(" ) "));
        _text.SetupGet(x => x.LinesCount).Returns(2);

        GetResult(0, 0);
        Assert.AreEqual(HighlightKind.NoHighlight, _result.Kind);
        Assert.AreEqual(default(BracketPosition), _result.Open);
        Assert.AreEqual(default(BracketPosition), _result.Close);

        GetResult(0, 1);
        Assert.AreEqual(HighlightKind.Highlighted, _result.Kind);
        Assert.AreEqual(new BracketPosition(0, 1), _result.Open);
        Assert.AreEqual(new BracketPosition(1, 1), _result.Close);

        GetResult(0, 2);
        Assert.AreEqual(HighlightKind.Highlighted, _result.Kind);
        Assert.AreEqual(new BracketPosition(0, 1), _result.Open);
        Assert.AreEqual(new BracketPosition(1, 1), _result.Close);

        GetResult(0, 3);
        Assert.AreEqual(HighlightKind.NoHighlight, _result.Kind);
        Assert.AreEqual(default(BracketPosition), _result.Open);
        Assert.AreEqual(default(BracketPosition), _result.Close);

        GetResult(1, 0);
        Assert.AreEqual(HighlightKind.NoHighlight, _result.Kind);
        Assert.AreEqual(default(BracketPosition), _result.Open);
        Assert.AreEqual(default(BracketPosition), _result.Close);

        GetResult(1, 1);
        Assert.AreEqual(HighlightKind.Highlighted, _result.Kind);
        Assert.AreEqual(new BracketPosition(0, 1), _result.Open);
        Assert.AreEqual(new BracketPosition(1, 1), _result.Close);

        GetResult(1, 2);
        Assert.AreEqual(HighlightKind.Highlighted, _result.Kind);
        Assert.AreEqual(new BracketPosition(0, 1), _result.Open);
        Assert.AreEqual(new BracketPosition(1, 1), _result.Close);

        GetResult(1, 3);
        Assert.AreEqual(HighlightKind.NoHighlight, _result.Kind);
        Assert.AreEqual(default(BracketPosition), _result.Open);
        Assert.AreEqual(default(BracketPosition), _result.Close);
    }

    [Test]
    public void Forward_Open_NoPair()
    {
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine(" (( ) "));
        _text.SetupGet(x => x.LinesCount).Returns(1);

        GetResult(0, 1);
        Assert.AreEqual(HighlightKind.NoPair, _result.Kind);
        Assert.AreEqual(new BracketPosition(0, 1), _result.Open);
        Assert.AreEqual(new BracketPosition(0, 1), _result.Close);
    }

    [Test]
    public void Forward_Close_NoPair()
    {
        _text.Setup(x => x.GetLine(0)).Returns(new TextLine(" ( )) "));
        _text.SetupGet(x => x.LinesCount).Returns(1);

        GetResult(0, 4);
        Assert.AreEqual(HighlightKind.NoPair, _result.Kind);
        Assert.AreEqual(new BracketPosition(0, 4), _result.Open);
        Assert.AreEqual(new BracketPosition(0, 4), _result.Close);
    }

    private void GetResult(int lineIndex, int columnIndex)
    {
        _textCursor.SetupGet(x => x.LineIndex).Returns(lineIndex);
        _textCursor.SetupGet(x => x.ColumnIndex).Returns(columnIndex);
        _result = _highlighter.GetHighlightedBrackets();
    }
}
