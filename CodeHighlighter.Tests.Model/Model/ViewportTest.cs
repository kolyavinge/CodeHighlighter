using CodeHighlighter.Model;
using Moq;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model;

internal class ViewportTest
{
    private Mock<ITextMeasuresInternal> _textMeasures;
    private Mock<IViewportVerticalOffsetUpdater> _verticalOffsetUpdater;
    private Mock<IVerticalScrollBarMaximumValueStrategy> _verticalScrollBarMaximumValueStrategy;
    private Mock<IHorizontalScrollBarMaximumValueStrategy> _horizontalScrollBarMaximumValueStrategy;
    private Mock<IViewportContext> _context;
    private Viewport _viewport;

    [SetUp]
    public void Setup()
    {
        _textMeasures = new Mock<ITextMeasuresInternal>();
        _verticalOffsetUpdater = new Mock<IViewportVerticalOffsetUpdater>();
        _verticalScrollBarMaximumValueStrategy = new Mock<IVerticalScrollBarMaximumValueStrategy>();
        _horizontalScrollBarMaximumValueStrategy = new Mock<IHorizontalScrollBarMaximumValueStrategy>();
        _context = new Mock<IViewportContext>();
        _viewport = new Viewport(
            _textMeasures.Object,
            _verticalOffsetUpdater.Object,
            _verticalScrollBarMaximumValueStrategy.Object,
            _horizontalScrollBarMaximumValueStrategy.Object);
        _viewport.SetContext(_context.Object);
    }

    [Test]
    public void VerticalScrollBarValue()
    {
        _viewport.VerticalScrollBarValue = 5;
        _context.VerifySet(x => x.VerticalScrollBarValue = 5);

        _viewport.VerticalScrollBarValue = -10;
        _context.VerifySet(x => x.VerticalScrollBarValue = 0);
    }

    [Test]
    public void HorizontalScrollBarValue()
    {
        _viewport.HorizontalScrollBarValue = 5;
        _context.VerifySet(x => x.HorizontalScrollBarValue = 5);

        _viewport.HorizontalScrollBarValue = -10;
        _context.VerifySet(x => x.HorizontalScrollBarValue = 0);
    }
}
