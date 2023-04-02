using CodeHighlighter.Ancillary;
using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model;

internal class TextCursorTest
{
    private Text _text;
    private LineFolds _lineFolds;
    private TextCursorPositionCorrector _corrector;
    private TextCursor _textCursor;

    [SetUp]
    public void Setup()
    {
        _text = new Text("12345\n1234\n123");
        _lineFolds = new LineFolds();
        _corrector = new TextCursorPositionCorrector(_text, _lineFolds);
        _textCursor = new TextCursor(_text, _corrector);
    }

    [Test]
    public void InitPosition()
    {
        Assert.AreEqual(new CursorPosition(0, 0), _textCursor.Position);
    }

    [Test]
    public void PositionToString()
    {
        _text.TextContent = "12345\n1234\n    123\n";

        _textCursor.MoveTo(new(1, 1));
        Assert.AreEqual("1:1", _textCursor.Position.ToString());

        _textCursor.MoveTo(new(3, 4, CursorPositionKind.Virtual));
        Assert.AreEqual("[3:4]", _textCursor.Position.ToString());
    }

    [Test]
    public void MoveToEmptyText()
    {
        _textCursor = new TextCursor(new Text(""), new TextCursorPositionCorrector(new Text(""), new LineFolds()));
        _textCursor.MoveTo(new(1, 1));
        Assert.AreEqual(new CursorPosition(0, 0), _textCursor.Position);
    }

    [Test]
    public void MoveTo()
    {
        _textCursor.MoveTo(new(0, 0));
        Assert.AreEqual(new CursorPosition(0, 0), _textCursor.Position);

        _textCursor.MoveTo(new(-1, 0));
        Assert.AreEqual(new CursorPosition(0, 0), _textCursor.Position);

        _textCursor.MoveTo(new(0, -1));
        Assert.AreEqual(new CursorPosition(0, 0), _textCursor.Position);

        _textCursor.MoveTo(new(100, 0));
        Assert.AreEqual(new CursorPosition(2, 0), _textCursor.Position);

        _textCursor.MoveTo(new(0, 100));
        Assert.AreEqual(new CursorPosition(0, 5), _textCursor.Position);

        _textCursor.MoveTo(new(100, 100));
        Assert.AreEqual(new CursorPosition(2, 3), _textCursor.Position);

        _textCursor.MoveTo(new(1, 1));
        Assert.AreEqual(new CursorPosition(1, 1), _textCursor.Position);

        _textCursor.MoveTo(new(1, 100));
        Assert.AreEqual(new CursorPosition(1, 4), _textCursor.Position);
    }

    [Test]
    public void MoveTo_VirtualCursor()
    {
        _text.TextContent = "    123\r\n\r\n";
        _textCursor.MoveTo(new(2, 10, CursorPositionKind.Real));
        Assert.AreEqual(new CursorPosition(2, 4, CursorPositionKind.Virtual), _textCursor.Position);
    }

    [Test]
    public void MoveUp()
    {
        _textCursor.MoveTo(new(2, 1));

        _textCursor.MoveUp();
        Assert.AreEqual(new CursorPosition(1, 1), _textCursor.Position);

        _textCursor.MoveUp();
        Assert.AreEqual(new CursorPosition(0, 1), _textCursor.Position);

        _textCursor.MoveUp();
        Assert.AreEqual(new CursorPosition(0, 1), _textCursor.Position);
    }

    [Test]
    public void MoveDown()
    {
        _textCursor.MoveTo(new(0, 1));

        _textCursor.MoveDown();
        Assert.AreEqual(new CursorPosition(1, 1), _textCursor.Position);

        _textCursor.MoveDown();
        Assert.AreEqual(new CursorPosition(2, 1), _textCursor.Position);

        _textCursor.MoveDown();
        Assert.AreEqual(new CursorPosition(2, 1), _textCursor.Position);
    }

    [Test]
    public void MoveLeft()
    {
        _textCursor.MoveTo(new(2, 3));

        _textCursor.MoveLeft();
        Assert.AreEqual(new CursorPosition(2, 2), _textCursor.Position);

        _textCursor.MoveLeft();
        Assert.AreEqual(new CursorPosition(2, 1), _textCursor.Position);

        _textCursor.MoveLeft();
        Assert.AreEqual(new CursorPosition(2, 0), _textCursor.Position);

        _textCursor.MoveLeft();
        Assert.AreEqual(new CursorPosition(1, 4), _textCursor.Position);

        _textCursor.MoveLeft();
        Assert.AreEqual(new CursorPosition(1, 3), _textCursor.Position);

        _textCursor.MoveLeft();
        Assert.AreEqual(new CursorPosition(1, 2), _textCursor.Position);

        _textCursor.MoveLeft();
        Assert.AreEqual(new CursorPosition(1, 1), _textCursor.Position);

        _textCursor.MoveLeft();
        Assert.AreEqual(new CursorPosition(1, 0), _textCursor.Position);

        _textCursor.MoveLeft();
        Assert.AreEqual(new CursorPosition(0, 5), _textCursor.Position);

        _textCursor.MoveLeft();
        Assert.AreEqual(new CursorPosition(0, 4), _textCursor.Position);

        _textCursor.MoveLeft();
        Assert.AreEqual(new CursorPosition(0, 3), _textCursor.Position);

        _textCursor.MoveLeft();
        Assert.AreEqual(new CursorPosition(0, 2), _textCursor.Position);

        _textCursor.MoveLeft();
        Assert.AreEqual(new CursorPosition(0, 1), _textCursor.Position);

        _textCursor.MoveLeft();
        Assert.AreEqual(new CursorPosition(0, 0), _textCursor.Position);

        _textCursor.MoveLeft();
        Assert.AreEqual(new CursorPosition(0, 0), _textCursor.Position);
    }

    [Test]
    public void MoveLeft_VirtualCursor()
    {
        _text.TextContent = "    123\r\n";
        _textCursor.MoveTo(new(1, 4, CursorPositionKind.Virtual));

        _textCursor.MoveLeft();

        Assert.AreEqual(new CursorPosition(1, 0), _textCursor.Position);
    }

    [Test]
    public void MoveRight()
    {
        _textCursor.MoveRight();
        Assert.AreEqual(new CursorPosition(0, 1), _textCursor.Position);

        _textCursor.MoveRight();
        Assert.AreEqual(new CursorPosition(0, 2), _textCursor.Position);

        _textCursor.MoveRight();
        Assert.AreEqual(new CursorPosition(0, 3), _textCursor.Position);

        _textCursor.MoveRight();
        Assert.AreEqual(new CursorPosition(0, 4), _textCursor.Position);

        _textCursor.MoveRight();
        Assert.AreEqual(new CursorPosition(0, 5), _textCursor.Position);

        _textCursor.MoveRight();
        Assert.AreEqual(new CursorPosition(1, 0), _textCursor.Position);

        _textCursor.MoveRight();
        Assert.AreEqual(new CursorPosition(1, 1), _textCursor.Position);

        _textCursor.MoveRight();
        Assert.AreEqual(new CursorPosition(1, 2), _textCursor.Position);

        _textCursor.MoveRight();
        Assert.AreEqual(new CursorPosition(1, 3), _textCursor.Position);

        _textCursor.MoveRight();
        Assert.AreEqual(new CursorPosition(1, 4), _textCursor.Position);

        _textCursor.MoveRight();
        Assert.AreEqual(new CursorPosition(2, 0), _textCursor.Position);

        _textCursor.MoveRight();
        Assert.AreEqual(new CursorPosition(2, 1), _textCursor.Position);

        _textCursor.MoveRight();
        Assert.AreEqual(new CursorPosition(2, 2), _textCursor.Position);

        _textCursor.MoveRight();
        Assert.AreEqual(new CursorPosition(2, 3), _textCursor.Position);

        _textCursor.MoveRight();
        Assert.AreEqual(new CursorPosition(2, 3), _textCursor.Position);
    }

    [Test]
    public void MoveRight_VirtualCursor()
    {
        _text.TextContent = "    123\r\n\r\n";
        _textCursor.MoveTo(new(1, 4, CursorPositionKind.Virtual));

        _textCursor.MoveRight();

        Assert.AreEqual(new CursorPosition(2, 0), _textCursor.Position);
    }

    [Test]
    public void MoveStartLine()
    {
        _textCursor.MoveTo(new(0, 5));
        _textCursor.MoveStartLine();
        Assert.AreEqual(new CursorPosition(0, 0), _textCursor.Position);
        _textCursor.MoveStartLine();
        Assert.AreEqual(new CursorPosition(0, 0), _textCursor.Position);

        _textCursor.MoveTo(new(1, 5));
        _textCursor.MoveStartLine();
        Assert.AreEqual(new CursorPosition(1, 0), _textCursor.Position);
        _textCursor.MoveStartLine();
        Assert.AreEqual(new CursorPosition(1, 0), _textCursor.Position);
    }

    [Test]
    public void MoveStartLine_LeftSpaces()
    {
        _text.TextContent = "    0123456789";
        _textCursor.MoveTextEnd();

        _textCursor.MoveStartLine();
        Assert.AreEqual(0, _textCursor.LineIndex);
        Assert.AreEqual(4, _textCursor.ColumnIndex);

        _textCursor.MoveStartLine();
        Assert.AreEqual(0, _textCursor.LineIndex);
        Assert.AreEqual(0, _textCursor.ColumnIndex);

        _textCursor.MoveStartLine();
        Assert.AreEqual(0, _textCursor.LineIndex);
        Assert.AreEqual(4, _textCursor.ColumnIndex);

        _textCursor.MoveTo(new(0, 2));
        _textCursor.MoveStartLine();
        Assert.AreEqual(0, _textCursor.LineIndex);
        Assert.AreEqual(4, _textCursor.ColumnIndex);
    }

    [Test]
    public void MoveStartLine_LeftSpaces_AllSpaces()
    {
        _text.TextContent = "    ";
        _textCursor.MoveTextEnd();

        _textCursor.MoveStartLine();
        Assert.AreEqual(0, _textCursor.LineIndex);
        Assert.AreEqual(0, _textCursor.ColumnIndex);

        _textCursor.MoveStartLine();
        Assert.AreEqual(0, _textCursor.LineIndex);
        Assert.AreEqual(0, _textCursor.ColumnIndex);

        _textCursor.MoveTo(new(0, 2));
        _textCursor.MoveStartLine();
        Assert.AreEqual(0, _textCursor.LineIndex);
        Assert.AreEqual(0, _textCursor.ColumnIndex);
    }

    [Test]
    public void MoveEndLine()
    {
        _textCursor.MoveEndLine();
        Assert.AreEqual(new CursorPosition(0, 5), _textCursor.Position);
        _textCursor.MoveEndLine();
        Assert.AreEqual(new CursorPosition(0, 5), _textCursor.Position);

        _textCursor.MoveTo(new(1, 0));
        _textCursor.MoveEndLine();
        Assert.AreEqual(new CursorPosition(1, 4), _textCursor.Position);
        _textCursor.MoveEndLine();
        Assert.AreEqual(new CursorPosition(1, 4), _textCursor.Position);
    }

    [Test]
    public void MovePageUp()
    {
        _textCursor.MoveTo(new(2, 3));

        _textCursor.MovePageUp(2);
        Assert.AreEqual(new CursorPosition(0, 3), _textCursor.Position);

        _textCursor.MovePageUp(2);
        Assert.AreEqual(new CursorPosition(0, 3), _textCursor.Position);
    }

    [Test]
    public void MovePageDown()
    {
        _textCursor.MoveTo(new(0, 1));

        _textCursor.MovePageDown(2);
        Assert.AreEqual(new CursorPosition(2, 1), _textCursor.Position);

        _textCursor.MovePageDown(2);
        Assert.AreEqual(new CursorPosition(2, 1), _textCursor.Position);
    }

    [Test]
    public void MoveTextBegin()
    {
        _textCursor.MoveTo(new(2, 3));

        _textCursor.MoveTextBegin();
        Assert.AreEqual(new CursorPosition(0, 0), _textCursor.Position);

        _textCursor.MoveTextBegin();
        Assert.AreEqual(new CursorPosition(0, 0), _textCursor.Position);
    }

    [Test]
    public void MoveTextBegin_AfterVertualVursor()
    {
        _text.TextContent = "    123\r\n\r\n";
        _textCursor.MoveTo(new(1, 4, CursorPositionKind.Virtual));

        _textCursor.MoveTextBegin();

        Assert.AreEqual(new CursorPosition(0, 0), _textCursor.Position);
    }

    [Test]
    public void MoveTextEnd()
    {
        _textCursor.MoveTextEnd();
        Assert.AreEqual(new CursorPosition(2, 3), _textCursor.Position);

        _textCursor.MoveTextEnd();
        Assert.AreEqual(new CursorPosition(2, 3), _textCursor.Position);
    }

    [Test]
    public void MoveTextEnd_AfterVertualVursor()
    {
        _text.TextContent = "    123\r\n\r\n";
        _textCursor.MoveTo(new(1, 4, CursorPositionKind.Virtual));

        _textCursor.MoveTextEnd();

        Assert.AreEqual(new CursorPosition(2, 0), _textCursor.Position);
    }

    [Test]
    public void MoveUp_FoldedLines()
    {
        _text.TextContent = "\n\n\n\n";
        _textCursor.MoveTo(new(2, 0));
        _lineFolds.Items = new LineFold[] { new(1, 3) };
        _lineFolds.Activate(new[] { 1 });

        _textCursor.MoveUp();

        Assert.AreEqual(new CursorPosition(1, 0), _textCursor.Position);
    }

    [Test]
    public void MoveDown_FoldedLines()
    {
        _text.TextContent = "\n\n\n\n";
        _lineFolds.Items = new LineFold[] { new(0, 3) };
        _lineFolds.Activate(new[] { 0 });

        _textCursor.MoveDown();

        Assert.AreEqual(new CursorPosition(4, 0), _textCursor.Position);
    }

    [Test]
    public void MovePageUp_FoldedLines()
    {
        _text.TextContent = "\n\n\n\n";
        _textCursor.MoveTo(new(2, 0));
        _lineFolds.Items = new LineFold[] { new(1, 3) };
        _lineFolds.Activate(new[] { 1 });

        _textCursor.MovePageUp(1);

        Assert.AreEqual(new CursorPosition(1, 0), _textCursor.Position);
    }

    [Test]
    public void MovePageDown_FoldedLines()
    {
        _text.TextContent = "\n\n\n\n";
        _lineFolds.Items = new LineFold[] { new(0, 3) };
        _lineFolds.Activate(new[] { 0 });

        _textCursor.MovePageDown(1);

        Assert.AreEqual(new CursorPosition(4, 0), _textCursor.Position);
    }
}
