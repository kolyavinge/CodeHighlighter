using System.Collections.Generic;
using CodeHighlighter.Model;
using Moq;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model;

internal class DefaultVerticalScrollBarMaximumValueStrategyTest
{
    private Mock<IText> _text;
    private Mock<ITextMeasuresInternal> _textMeasures;
    private Mock<ILineGapCollection> _gaps;
    private Mock<ILineFolds> _folds;
    private DefaultVerticalScrollBarMaximumValueStrategy _strategy;

    [SetUp]
    public void Setup()
    {
        _text = new Mock<IText>();
        _textMeasures = new Mock<ITextMeasuresInternal>();
        _gaps = new Mock<ILineGapCollection>();
        _folds = new Mock<ILineFolds>();
        _strategy = new DefaultVerticalScrollBarMaximumValueStrategy(_text.Object, _textMeasures.Object, _gaps.Object, _folds.Object);
    }

    [Test]
    public void GetValue()
    {
        _text.SetupGet(x => x.LinesCount).Returns(12);
        _textMeasures.SetupGet(x => x.LineHeight).Returns(10);
        _gaps.Setup(x => x.GetEnumerator()).Returns(new List<LineGap> { new(2), new(4) }.GetEnumerator());
        _folds.SetupGet(x => x.FoldedLinesCount).Returns(6);

        var result = _strategy.GetValue();

        Assert.That(result, Is.EqualTo((12 + (2 + 4) - 6) * 10));
    }
}
