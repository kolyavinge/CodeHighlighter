using CodeHighlighter.Common;
using CodeHighlighter.Core;
using Moq;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Core;

internal class TextCursorAbsolutePositionTest
{
    private Mock<ITextCursor> _textCursor;
    private Mock<ITextMeasuresInternal> _textMeasures;
    private Mock<IExtendedLineNumberGenerator> _generator;
    private TextCursorAbsolutePosition _absolutePosition;

    [SetUp]
    public void Setup()
    {
        _textCursor = new Mock<ITextCursor>();
        _textCursor.SetupGet(x => x.LineIndex).Returns(4);
        _textCursor.SetupGet(x => x.ColumnIndex).Returns(2);

        _textMeasures = new Mock<ITextMeasuresInternal>();
        _textMeasures.SetupGet(x => x.LetterWidth).Returns(10);
        _textMeasures.SetupGet(x => x.LineHeight).Returns(15);

        _generator = new Mock<IExtendedLineNumberGenerator>();
        _generator.Setup(x => x.GetLineOffsetY(4, 15)).Returns(4 * 15);

        _absolutePosition = new TextCursorAbsolutePosition(_textCursor.Object, _textMeasures.Object, _generator.Object);
    }

    [Test]
    public void Position()
    {
        Assert.That(_absolutePosition.Position, Is.EqualTo(new Point(2 * 10, 4 * 15)));
        _generator.Verify(x => x.GetLineOffsetY(4, 15), Times.Once());
    }
}
