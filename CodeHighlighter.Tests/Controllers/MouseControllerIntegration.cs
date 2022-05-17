using System.Windows.Input;
using CodeHighlighter.Controllers;
using CodeHighlighter.Model;
using Moq;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Controllers;

internal class MouseControllerIntegration
{
    private Mock<ICodeTextBox> _codeTextBox;
    private Mock<IViewportContext> _viewportContext;
    private TextMeasures _textMeasures;
    private Viewport _viewport;
    private CodeTextBoxModel _model;
    private MouseController _controller;

    [SetUp]
    public void Setup()
    {
        _codeTextBox = new Mock<ICodeTextBox>();
        _viewportContext = new Mock<IViewportContext>();
        _textMeasures = new TextMeasures(10, 4);
        _viewport = new Viewport(_viewportContext.Object, _textMeasures);
        _model = new CodeTextBoxModel();
        _controller = new MouseController(_codeTextBox.Object, _model, _model, _model, _viewport, _viewportContext.Object);
    }

    [Test]
    public void OnMouseDown()
    {
        /* 12345
         * qwert
         * asdfg
         */
        _model.SetText("12345\r\nqwert\r\nasdfg");

        _controller.OnMouseDown(new(0, 0), true);
        _controller.OnMouseDown(new(100, 100), true);
        Assert.AreEqual("12345\r\nqwert\r\nasdfg", _model.GetSelectedText());

        _controller.OnMouseDown(new(0, 0), false);
        Assert.AreEqual("", _model.GetSelectedText());
    }

    [Test]
    public void OnMouseMove()
    {
        /* 12345
         * qwert
         * asdfg
         */
        _model.SetText("12345\r\nqwert\r\nasdfg");

        _controller.OnMouseMove(new(0, 0), MouseButtonState.Pressed);
        _controller.OnMouseMove(new(100, 100), MouseButtonState.Pressed);
        Assert.AreEqual("12345\r\nqwert\r\nasdfg", _model.GetSelectedText());
    }
}
