using System.Windows.Input;
using CodeHighlighter.CodeProvidering;
using CodeHighlighter.Controllers;
using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Controllers;

internal class KeyboardControllerIntegration
{
    private CodeTextBoxModel _model;
    private KeyboardController _controller;

    [SetUp]
    public void Setup()
    {
        _model = new CodeTextBoxModel(new EmptyCodeProvider());
        _controller = new KeyboardController();
    }

    [Test]
    public void KeyDownWithShift_Selection()
    {
        /* 12345
         * qwert
         * asdfg
         */
        _model.InputModel.SetText("12345\r\nqwert\r\nasdfg");
        _model.InputModel.MoveCursorTo(new(1, 3));

        KeyDownWithShift(Key.Left);
        Assert.AreEqual("e", _model.InputModel.GetSelectedText());

        KeyDownWithShift(Key.Home);
        Assert.AreEqual("qwe", _model.InputModel.GetSelectedText());

        KeyDownWithShift(Key.Right);
        Assert.AreEqual("we", _model.InputModel.GetSelectedText());

        KeyDownWithShift(Key.End);
        Assert.AreEqual("rt", _model.InputModel.GetSelectedText());

        _model.InputModel.MoveCursorTo(new(1, 3));
        KeyDownWithShift(Key.Up);
        Assert.AreEqual("45\r\nqwe", _model.InputModel.GetSelectedText());

        _model.InputModel.MoveCursorTo(new(1, 3));
        KeyDownWithShift(Key.Down);
        Assert.AreEqual("rt\r\nasd", _model.InputModel.GetSelectedText());

        KeyDownWithShift(Key.PageDown);
        Assert.AreEqual("rt\r\nasd", _model.InputModel.GetSelectedText());

        KeyDownWithShiftAndControl(Key.Home);
        Assert.AreEqual("12345\r\nqwe", _model.InputModel.GetSelectedText());

        KeyDownWithShiftAndControl(Key.End);
        Assert.AreEqual("rt\r\nasdfg", _model.InputModel.GetSelectedText());
    }

    [Test]
    public void KeyDownWithoutShift_SelectionReset()
    {
        _model.InputModel.SetText("0");

        _model.InputModel.SelectAll();
        KeyDownWithoutShift(Key.Left);
        Assert.AreEqual("", _model.InputModel.GetSelectedText());

        _model.InputModel.SelectAll();
        KeyDownWithoutShift(Key.Home);
        Assert.AreEqual("", _model.InputModel.GetSelectedText());

        _model.InputModel.SelectAll();
        KeyDownWithoutShift(Key.Right);
        Assert.AreEqual("", _model.InputModel.GetSelectedText());

        KeyDownWithoutShift(Key.End);
        Assert.AreEqual("", _model.InputModel.GetSelectedText());

        _model.InputModel.SelectAll();
        KeyDownWithoutShift(Key.Up);
        Assert.AreEqual("", _model.InputModel.GetSelectedText());

        _model.InputModel.SelectAll();
        KeyDownWithoutShift(Key.Down);
        Assert.AreEqual("", _model.InputModel.GetSelectedText());

        _model.InputModel.SelectAll();
        KeyDownWithoutShift(Key.PageDown);
        Assert.AreEqual("", _model.InputModel.GetSelectedText());

        _model.InputModel.SelectAll();
        KeyDownWithoutShift(Key.Home);
        Assert.AreEqual("", _model.InputModel.GetSelectedText());

        _model.InputModel.SelectAll();
        KeyDownWithoutShift(Key.End);
        Assert.AreEqual("", _model.InputModel.GetSelectedText());
    }

    private void KeyDownWithoutShift(Key key)
    {
        _controller.OnKeyDown(_model, key, false, false, false, true);
    }

    private void KeyDownWithShift(Key key)
    {
        _controller.OnKeyDown(_model, key, false, false, true, true);
    }

    private void KeyDownWithShiftAndControl(Key key)
    {
        _controller.OnKeyDown(_model, key, true, false, true, true);
    }
}
