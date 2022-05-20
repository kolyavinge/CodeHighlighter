using System.Windows.Input;
using CodeHighlighter.Controllers;
using CodeHighlighter.Input;
using CodeHighlighter.Model;
using Moq;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Controllers;

internal class KeyboardControllerIntegration
{
    private Mock<ICodeTextBox> _codeTextBox;
    private Mock<IViewportContext> _viewportContext;
    private TextMeasures _textMeasures;
    private Viewport _viewport;
    private CodeTextBoxModel _model;
    private CodeTextBoxCommands _commands;
    private KeyboardController _controller;

    [SetUp]
    public void Setup()
    {
        _codeTextBox = new Mock<ICodeTextBox>();
        _viewportContext = new Mock<IViewportContext>();
        _textMeasures = new TextMeasures(10, 4);
        _viewport = new Viewport(_viewportContext.Object, _textMeasures);
        _model = new CodeTextBoxModel();
        _commands = new CodeTextBoxCommands();
        _commands.Init(new InputCommandContext(_codeTextBox.Object, _model, _viewport));
        _controller = new KeyboardController(_commands, _model, _model);
    }

    [Test]
    public void KeyDownWithShift_Selection()
    {
        /* 12345
         * qwert
         * asdfg
         */
        _model.SetText("12345\r\nqwert\r\nasdfg");
        _model.MoveCursorTo(1, 3);

        KeyDownWithShift(Key.Left);
        Assert.AreEqual("e", _model.GetSelectedText());

        KeyDownWithShift(Key.Home);
        Assert.AreEqual("qwe", _model.GetSelectedText());

        KeyDownWithShift(Key.Right);
        Assert.AreEqual("we", _model.GetSelectedText());

        KeyDownWithShift(Key.End);
        Assert.AreEqual("rt", _model.GetSelectedText());

        _model.MoveCursorTo(1, 3);
        KeyDownWithShift(Key.Up);
        Assert.AreEqual("45\r\nqwe", _model.GetSelectedText());

        _model.MoveCursorTo(1, 3);
        KeyDownWithShift(Key.Down);
        Assert.AreEqual("rt\r\nasd", _model.GetSelectedText());

        KeyDownWithShift(Key.PageDown);
        Assert.AreEqual("rt\r\nasd", _model.GetSelectedText());

        KeyDownWithShiftAndControl(Key.Home);
        Assert.AreEqual("12345\r\nqwe", _model.GetSelectedText());

        KeyDownWithShiftAndControl(Key.End);
        Assert.AreEqual("rt\r\nasdfg", _model.GetSelectedText());
    }

    [Test]
    public void KeyDownWithoutShift_SelectionReset()
    {
        _model.SetText("0");

        _model.SelectAll();
        KeyDownWithoutShift(Key.Left);
        Assert.AreEqual("", _model.GetSelectedText());

        _model.SelectAll();
        KeyDownWithoutShift(Key.Home);
        Assert.AreEqual("", _model.GetSelectedText());

        _model.SelectAll();
        KeyDownWithoutShift(Key.Right);
        Assert.AreEqual("", _model.GetSelectedText());

        KeyDownWithoutShift(Key.End);
        Assert.AreEqual("", _model.GetSelectedText());

        _model.SelectAll();
        KeyDownWithoutShift(Key.Up);
        Assert.AreEqual("", _model.GetSelectedText());

        _model.SelectAll();
        KeyDownWithoutShift(Key.Down);
        Assert.AreEqual("", _model.GetSelectedText());

        _model.SelectAll();
        KeyDownWithoutShift(Key.PageDown);
        Assert.AreEqual("", _model.GetSelectedText());

        _model.SelectAll();
        KeyDownWithoutShift(Key.Home);
        Assert.AreEqual("", _model.GetSelectedText());

        _model.SelectAll();
        KeyDownWithoutShift(Key.End);
        Assert.AreEqual("", _model.GetSelectedText());
    }

    private void KeyDownWithoutShift(Key key)
    {
        _controller.OnKeyDown(key, false, false, false, true);
    }

    private void KeyDownWithShift(Key key)
    {
        _controller.OnKeyDown(key, false, false, true, true);
    }

    private void KeyDownWithShiftAndControl(Key key)
    {
        _controller.OnKeyDown(key, true, false, true, true);
    }
}
