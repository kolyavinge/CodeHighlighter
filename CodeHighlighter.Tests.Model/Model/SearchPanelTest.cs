﻿using CodeHighlighter.Ancillary;
using CodeHighlighter.Common;
using CodeHighlighter.Core;
using CodeHighlighter.Model;
using Moq;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model;

internal class SearchPanelTest
{
    private Mock<ITextEvents> _textEvents;
    private Mock<ITextHighlighter> _textHighlighter;
    private Mock<ICodeTextBox> _codeTextBoxModel;
    private Mock<ITextSearchLogic> _textSearchLogic;
    private Mock<IRegexSearchLogic> _regexSearchLogic;
    private Mock<IWholeWordLogic> _wholeWordLogic;
    private Mock<ITextPositionNavigatorInternal> _textPositionNavigator;
    private Mock<ISearchPanelView> _panel;
    private Color _highlightColor;
    private SearchPanel _model;

    [SetUp]
    public void Setup()
    {
        _textEvents = new Mock<ITextEvents>();
        _textHighlighter = new Mock<ITextHighlighter>();
        _codeTextBoxModel = new Mock<ICodeTextBox>();
        _codeTextBoxModel.SetupGet(x => x.TextEvents).Returns(_textEvents.Object);
        _codeTextBoxModel.SetupGet(x => x.TextHighlighter).Returns(_textHighlighter.Object);
        _textSearchLogic = new Mock<ITextSearchLogic>();
        _regexSearchLogic = new Mock<IRegexSearchLogic>();
        _wholeWordLogic = new Mock<IWholeWordLogic>();
        _textPositionNavigator = new Mock<ITextPositionNavigatorInternal>();
        _panel = new Mock<ISearchPanelView>();
        _highlightColor = Color.FromHex("123456");
        _model = new SearchPanel(
            _codeTextBoxModel.Object, _textSearchLogic.Object, _regexSearchLogic.Object, _wholeWordLogic.Object, _textPositionNavigator.Object);
        _model.HighlightColor = _highlightColor;
        _model.AttachSearchPanel(_panel.Object);
    }

    [Test]
    public void Contructor()
    {
        Assert.False(_model.HasResult);
        _textSearchLogic.Verify(x => x.DoSearch(It.IsAny<string>(), It.IsAny<bool>()), Times.Never());
        _regexSearchLogic.Verify(x => x.DoSearch(It.IsAny<string>(), It.IsAny<bool>()), Times.Never());
    }

    [Test]
    public void EmptyPattern()
    {
        _model.Pattern = "";

        _textSearchLogic.Verify(x => x.DoSearch(It.IsAny<string>(), It.IsAny<bool>()), Times.Never());
        _regexSearchLogic.Verify(x => x.DoSearch(It.IsAny<string>(), It.IsAny<bool>()), Times.Never());
    }

    [Test]
    public void Pattern()
    {
        _model.Pattern = "123";

        _textSearchLogic.Verify(x => x.DoSearch("123", false), Times.Once());
        _regexSearchLogic.Verify(x => x.DoSearch(It.IsAny<string>(), It.IsAny<bool>()), Times.Never());
    }

    [Test]
    public void IsRegex()
    {
        _model.Pattern = "123";
        _textSearchLogic.Verify(x => x.DoSearch("123", false), Times.Once());

        _model.IsRegex = true;
        _regexSearchLogic.Verify(x => x.DoSearch("123", false), Times.Once());
    }

    [Test]
    public void MatchCase()
    {
        _model.Pattern = "123";
        _textSearchLogic.Verify(x => x.DoSearch("123", false), Times.Once());

        _model.MatchCase = true;
        _textSearchLogic.Verify(x => x.DoSearch("123", true), Times.Once());
        _regexSearchLogic.Verify(x => x.DoSearch(It.IsAny<string>(), It.IsAny<bool>()), Times.Never());
    }

    [Test]
    public void RegexMatchCase()
    {
        _model.MatchCase = true;
        _model.IsRegex = true;
        _model.Pattern = "123";

        _regexSearchLogic.Verify(x => x.DoSearch("123", true), Times.Once());
    }

    [Test]
    public void WholeWord()
    {
        _textSearchLogic.Setup(x => x.DoSearch("123", false)).Returns(new TextPosition[] { new(0, 1, 2, 3) });
        _model.IsWholeWord = true;
        _model.Pattern = "123";

        _wholeWordLogic.Verify(x => x.GetResult(new TextPosition[] { new(0, 1, 2, 3) }), Times.Once());
    }

    [Test]
    public void TextChangedEvent()
    {
        _model.Pattern = "123";
        _textEvents.Raise(x => x.TextChanged += null, new TextChangedEventArgs(new(), new()));
        _textSearchLogic.Verify(x => x.DoSearch("123", false), Times.Exactly(2));
    }

    [Test]
    public void RemoveOldTextHighlights()
    {
        _textSearchLogic.Setup(x => x.DoSearch("123", false)).Returns(new TextPosition[] { new(0, 1, 2, 3) });
        _model.Pattern = "123";

        _textSearchLogic.Setup(x => x.DoSearch("456", false)).Returns(new TextPosition[] { new(10, 11, 12, 13) });
        _model.Pattern = "456";

        _textHighlighter.Verify(x => x.Add(new TextHighlight[] { new(new(10, 11, 12, 13), _highlightColor) }));
        _textHighlighter.Verify(x => x.Remove(new TextHighlight[] { new(new(0, 1, 2, 3), _highlightColor) }));
    }

    [Test]
    public void HasResult()
    {
        _textSearchLogic.Setup(x => x.DoSearch("123", false)).Returns(new TextPosition[] { new(0, 1, 2, 3) });
        _model.Pattern = "123";
        Assert.True(_model.HasResult);

        _textSearchLogic.Setup(x => x.DoSearch("456", false)).Returns(new TextPosition[0]);
        _model.Pattern = "456";
        Assert.False(_model.HasResult);
    }

    [Test]
    public void TextPositionNavigatorSetPositions()
    {
        _textSearchLogic.Setup(x => x.DoSearch("123", false)).Returns(new TextPosition[] { new(0, 1, 2, 3) });

        _model.Pattern = "123";

        _textPositionNavigator.Verify(x => x.SetPositions(new TextPosition[] { new(0, 1, 2, 3) }), Times.Once());
    }

    [Test]
    public void ActivatePattern_SelectedTextNull()
    {
        _codeTextBoxModel.Setup(x => x.GetSelectedText()).Returns((string)null);

        _model.Pattern = "123";
        _model.ActivatePattern();

        Assert.That(_model.Pattern, Is.EqualTo("123"));
        _panel.Verify(x => x.FocusPattern(), Times.Once());
        _panel.Verify(x => x.SelectAllPattern(), Times.Once());
    }

    [Test]
    public void ActivatePattern_SelectedTextEmpty()
    {
        _codeTextBoxModel.Setup(x => x.GetSelectedText()).Returns("");

        _model.Pattern = "123";
        _model.ActivatePattern();

        Assert.That(_model.Pattern, Is.EqualTo("123"));
        _panel.Verify(x => x.FocusPattern(), Times.Once());
        _panel.Verify(x => x.SelectAllPattern(), Times.Once());
    }

    [Test]
    public void ActivatePattern_SelectedText()
    {
        _codeTextBoxModel.Setup(x => x.GetSelectedText()).Returns("456");

        _model.Pattern = "123";
        _model.ActivatePattern();

        Assert.That(_model.Pattern, Is.EqualTo("456"));
        _panel.Verify(x => x.FocusPattern(), Times.Once());
        _panel.Verify(x => x.SelectAllPattern(), Times.Once());
    }

    [Test]
    public void ActivatePattern_PanelIsNotAttached()
    {
        _model.AttachSearchPanel(null);
        _model.ActivatePattern();

        _panel.Verify(x => x.FocusPattern(), Times.Never());
        _panel.Verify(x => x.SelectAllPattern(), Times.Never());
    }
}
