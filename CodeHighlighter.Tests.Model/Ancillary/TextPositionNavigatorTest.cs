using CodeHighlighter.Ancillary;
using CodeHighlighter.Core;
using CodeHighlighter.Model;
using Moq;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Ancillary;

internal class TextPositionNavigatorTest
{
    private Mock<ICodeTextBoxModel> _codeTextBoxModel;
    private TextPositionNavigator _navigator;

    [SetUp]
    public void Setup()
    {
        _codeTextBoxModel = new Mock<ICodeTextBoxModel>();
        _navigator = new TextPositionNavigator(_codeTextBoxModel.Object);
    }

    [Test]
    public void GotoPrev_EmptyPositions()
    {
        _navigator.GotoPrev();

        _codeTextBoxModel.Verify(x => x.MoveCursorTo(It.IsAny<CursorPosition>()), Times.Never());
    }

    [Test]
    public void GotoNext_EmptyPositions()
    {
        _navigator.GotoNext();

        _codeTextBoxModel.Verify(x => x.MoveCursorTo(It.IsAny<CursorPosition>()), Times.Never());
    }

    [Test]
    public void GotoPrev_1()
    {
        _codeTextBoxModel.SetupGet(x => x.CursorPosition).Returns(new CursorPosition(10, 0));
        _navigator.SetPositions(new TextPosition[] { new(1, 2, 3, 4) });

        _navigator.GotoPrev();

        _codeTextBoxModel.Verify(x => x.MoveCursorTo(new(1, 2)), Times.Once());
        _codeTextBoxModel.Verify(x => x.Focus(), Times.Once());
    }

    [Test]
    public void GotoPrev_2()
    {
        _codeTextBoxModel.SetupGet(x => x.CursorPosition).Returns(new CursorPosition(5, 0));
        _navigator.SetPositions(new TextPosition[] { new(1, 2, 3, 4), new(10, 20, 30, 40) });

        _navigator.GotoPrev();

        _codeTextBoxModel.Verify(x => x.MoveCursorTo(new(1, 2)), Times.Once());
        _codeTextBoxModel.Verify(x => x.Focus(), Times.Once());
    }

    [Test]
    public void GotoPrev_3()
    {
        _codeTextBoxModel.SetupGet(x => x.CursorPosition).Returns(new CursorPosition(0, 0));
        _navigator.SetPositions(new TextPosition[] { new(1, 2, 3, 4), new(10, 20, 30, 40) });

        _navigator.GotoPrev();

        _codeTextBoxModel.Verify(x => x.MoveCursorTo(new(10, 20)), Times.Once());
        _codeTextBoxModel.Verify(x => x.Focus(), Times.Once());
    }

    [Test]
    public void GotoNext_1()
    {
        _codeTextBoxModel.SetupGet(x => x.CursorPosition).Returns(new CursorPosition(0, 0));
        _navigator.SetPositions(new TextPosition[] { new(1, 2, 3, 4) });

        _navigator.GotoNext();

        _codeTextBoxModel.Verify(x => x.MoveCursorTo(new(1, 2)), Times.Once());
        _codeTextBoxModel.Verify(x => x.Focus(), Times.Once());
    }

    [Test]
    public void GotoNext_2()
    {
        _codeTextBoxModel.SetupGet(x => x.CursorPosition).Returns(new CursorPosition(5, 0));
        _navigator.SetPositions(new TextPosition[] { new(1, 2, 3, 4), new(10, 20, 30, 40) });

        _navigator.GotoNext();

        _codeTextBoxModel.Verify(x => x.MoveCursorTo(new(10, 20)), Times.Once());
        _codeTextBoxModel.Verify(x => x.Focus(), Times.Once());
    }

    [Test]
    public void GotoNext_3()
    {
        _codeTextBoxModel.SetupGet(x => x.CursorPosition).Returns(new CursorPosition(50, 0));
        _navigator.SetPositions(new TextPosition[] { new(1, 2, 3, 4), new(10, 20, 30, 40) });

        _navigator.GotoNext();

        _codeTextBoxModel.Verify(x => x.MoveCursorTo(new(1, 2)), Times.Once());
        _codeTextBoxModel.Verify(x => x.Focus(), Times.Once());
    }
}
