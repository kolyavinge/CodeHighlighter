using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model;

public class TextCursorTest
{
    private TextCursor _textCursor;

    [SetUp]
    public void Setup()
    {
        _textCursor = new TextCursor(new Text("12345\n1234\n123"));
    }

    [Test]
    public void InitPosition()
    {
        Assert.AreEqual(new CursorPosition(0, 0), _textCursor.Position);
    }

    [Test]
    public void MoveToEmptyText()
    {
        _textCursor = new TextCursor(new Text(""));
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
    public void MoveTextEnd()
    {
        _textCursor.MoveTextEnd();
        Assert.AreEqual(new CursorPosition(2, 3), _textCursor.Position);

        _textCursor.MoveTextEnd();
        Assert.AreEqual(new CursorPosition(2, 3), _textCursor.Position);
    }
}
