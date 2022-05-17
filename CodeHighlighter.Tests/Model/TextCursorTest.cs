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
        Assert.AreEqual((0, 0), _textCursor.GetLineAndColumnIndex);
    }

    [Test]
    public void MoveToEmptyText()
    {
        _textCursor = new TextCursor(new Text(""));
        _textCursor.MoveTo(1, 1);
        Assert.AreEqual((0, 0), _textCursor.GetLineAndColumnIndex);
    }

    [Test]
    public void MoveTo()
    {
        _textCursor.MoveTo(0, 0);
        Assert.AreEqual((0, 0), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveTo(-1, 0);
        Assert.AreEqual((0, 0), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveTo(0, -1);
        Assert.AreEqual((0, 0), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveTo(100, 0);
        Assert.AreEqual((2, 0), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveTo(0, 100);
        Assert.AreEqual((0, 5), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveTo(100, 100);
        Assert.AreEqual((2, 3), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveTo(1, 1);
        Assert.AreEqual((1, 1), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveTo(1, 100);
        Assert.AreEqual((1, 4), _textCursor.GetLineAndColumnIndex);
    }

    [Test]
    public void MoveUp()
    {
        _textCursor.MoveTo(2, 1);

        _textCursor.MoveUp();
        Assert.AreEqual((1, 1), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveUp();
        Assert.AreEqual((0, 1), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveUp();
        Assert.AreEqual((0, 1), _textCursor.GetLineAndColumnIndex);
    }

    [Test]
    public void MoveDown()
    {
        _textCursor.MoveTo(0, 1);

        _textCursor.MoveDown();
        Assert.AreEqual((1, 1), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveDown();
        Assert.AreEqual((2, 1), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveDown();
        Assert.AreEqual((2, 1), _textCursor.GetLineAndColumnIndex);
    }

    [Test]
    public void MoveLeft()
    {
        _textCursor.MoveTo(2, 3);

        _textCursor.MoveLeft();
        Assert.AreEqual((2, 2), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveLeft();
        Assert.AreEqual((2, 1), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveLeft();
        Assert.AreEqual((2, 0), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveLeft();
        Assert.AreEqual((1, 4), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveLeft();
        Assert.AreEqual((1, 3), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveLeft();
        Assert.AreEqual((1, 2), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveLeft();
        Assert.AreEqual((1, 1), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveLeft();
        Assert.AreEqual((1, 0), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveLeft();
        Assert.AreEqual((0, 5), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveLeft();
        Assert.AreEqual((0, 4), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveLeft();
        Assert.AreEqual((0, 3), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveLeft();
        Assert.AreEqual((0, 2), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveLeft();
        Assert.AreEqual((0, 1), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveLeft();
        Assert.AreEqual((0, 0), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveLeft();
        Assert.AreEqual((0, 0), _textCursor.GetLineAndColumnIndex);
    }

    [Test]
    public void MoveRight()
    {
        _textCursor.MoveRight();
        Assert.AreEqual((0, 1), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveRight();
        Assert.AreEqual((0, 2), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveRight();
        Assert.AreEqual((0, 3), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveRight();
        Assert.AreEqual((0, 4), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveRight();
        Assert.AreEqual((0, 5), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveRight();
        Assert.AreEqual((1, 0), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveRight();
        Assert.AreEqual((1, 1), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveRight();
        Assert.AreEqual((1, 2), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveRight();
        Assert.AreEqual((1, 3), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveRight();
        Assert.AreEqual((1, 4), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveRight();
        Assert.AreEqual((2, 0), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveRight();
        Assert.AreEqual((2, 1), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveRight();
        Assert.AreEqual((2, 2), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveRight();
        Assert.AreEqual((2, 3), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveRight();
        Assert.AreEqual((2, 3), _textCursor.GetLineAndColumnIndex);
    }

    [Test]
    public void MoveStartLine()
    {
        _textCursor.MoveTo(0, 5);
        _textCursor.MoveStartLine();
        Assert.AreEqual((0, 0), _textCursor.GetLineAndColumnIndex);
        _textCursor.MoveStartLine();
        Assert.AreEqual((0, 0), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveTo(1, 5);
        _textCursor.MoveStartLine();
        Assert.AreEqual((1, 0), _textCursor.GetLineAndColumnIndex);
        _textCursor.MoveStartLine();
        Assert.AreEqual((1, 0), _textCursor.GetLineAndColumnIndex);
    }

    [Test]
    public void MoveEndLine()
    {
        _textCursor.MoveEndLine();
        Assert.AreEqual((0, 5), _textCursor.GetLineAndColumnIndex);
        _textCursor.MoveEndLine();
        Assert.AreEqual((0, 5), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveTo(1, 0);
        _textCursor.MoveEndLine();
        Assert.AreEqual((1, 4), _textCursor.GetLineAndColumnIndex);
        _textCursor.MoveEndLine();
        Assert.AreEqual((1, 4), _textCursor.GetLineAndColumnIndex);
    }

    [Test]
    public void MovePageUp()
    {
        _textCursor.MoveTo(2, 3);

        _textCursor.MovePageUp(2);
        Assert.AreEqual((0, 3), _textCursor.GetLineAndColumnIndex);

        _textCursor.MovePageUp(2);
        Assert.AreEqual((0, 3), _textCursor.GetLineAndColumnIndex);
    }

    [Test]
    public void MovePageDown()
    {
        _textCursor.MoveTo(0, 1);

        _textCursor.MovePageDown(2);
        Assert.AreEqual((2, 1), _textCursor.GetLineAndColumnIndex);

        _textCursor.MovePageDown(2);
        Assert.AreEqual((2, 1), _textCursor.GetLineAndColumnIndex);
    }

    [Test]
    public void MoveTextBegin()
    {
        _textCursor.MoveTo(2, 3);

        _textCursor.MoveTextBegin();
        Assert.AreEqual((0, 0), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveTextBegin();
        Assert.AreEqual((0, 0), _textCursor.GetLineAndColumnIndex);
    }

    [Test]
    public void MoveTextEnd()
    {
        _textCursor.MoveTextEnd();
        Assert.AreEqual((2, 3), _textCursor.GetLineAndColumnIndex);

        _textCursor.MoveTextEnd();
        Assert.AreEqual((2, 3), _textCursor.GetLineAndColumnIndex);
    }
}
