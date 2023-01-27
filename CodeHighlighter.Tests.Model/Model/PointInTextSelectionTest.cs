using CodeHighlighter.Model;
using Moq;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model;

internal class PointInTextSelectionTest
{
    [Test]
    public void InSelection_1()
    {
        Assert.True(Check(new(2, 2), new(8, 2), new(5, 0)));
    }

    [Test]
    public void InSelection_2()
    {
        Assert.True(Check(new(2, 2), new(8, 2), new(5, 100)));
    }

    [Test]
    public void InSelection_OnStartLine_1()
    {
        Assert.True(Check(new(2, 2), new(8, 2), new(2, 3)));
    }

    [Test]
    public void InSelection_OnStartLine_2()
    {
        Assert.True(Check(new(2, 2), new(8, 2), new(2, 2)));
    }

    [Test]
    public void InSelection_OnEndLine_1()
    {
        Assert.True(Check(new(2, 2), new(8, 2), new(8, 0)));
    }

    [Test]
    public void InSelection_OnEndLine_2()
    {
        Assert.True(Check(new(2, 2), new(8, 2), new(8, 2)));
    }

    [Test]
    public void OutOfSelection_Above_1()
    {
        Assert.False(Check(new(2, 2), new(8, 2), new(2, 1)));
    }

    [Test]
    public void OutOfSelection_Above_2()
    {
        Assert.False(Check(new(2, 2), new(8, 2), new(1, 0)));
    }

    [Test]
    public void OutOfSelection_Below_1()
    {
        Assert.False(Check(new(2, 2), new(8, 2), new(8, 3)));
    }

    [Test]
    public void OutOfSelection_Below_2()
    {
        Assert.False(Check(new(2, 2), new(8, 2), new(9, 0)));
    }

    private bool Check(CursorPosition start, CursorPosition end, CursorPosition pos)
    {
        var textSelection = new Mock<ITextSelection>();
        textSelection.Setup(x => x.GetSortedPositions()).Returns((start, end));

        return new PointInTextSelection(textSelection.Object).Check(pos);
    }
}
