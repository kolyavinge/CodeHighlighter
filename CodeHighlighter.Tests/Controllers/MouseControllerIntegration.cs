using System.Windows.Input;
using CodeHighlighter.CodeProvidering;
using CodeHighlighter.Controllers;
using CodeHighlighter.Model;
using Moq;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Controllers;

internal class MouseControllerIntegration
{
    private Mock<ICodeTextBox> _codeTextBox;
    private ICodeTextBoxModel _model;
    private KeyboardController _keybaordController;
    private MouseController _mouseController;

    [SetUp]
    public void Setup()
    {
        _codeTextBox = new Mock<ICodeTextBox>();
        _codeTextBox = new Mock<ICodeTextBox>();
        _model = CodeTextBoxModelFactory.MakeModel(new EmptyCodeProvider());
        _model.AttachCodeTextBox(_codeTextBox.Object);
        _codeTextBox.Raise(x => x.FontSettingsChanged += null, new FontSettingsChangedEventArgs(10, 10));
        _keybaordController = new KeyboardController();
        _mouseController = new MouseController();
    }

    [Test]
    public void OnMouseDown()
    {
        /* 12345
         * qwert
         * asdfg
         */
        _model.Text = "12345\r\nqwert\r\nasdfg";

        _mouseController.OnMouseDown(_codeTextBox.Object, _model, new(0, 0));
        _keybaordController.OnKeyDown(_model, Key.LeftShift, false, true);
        _mouseController.OnMouseDown(_codeTextBox.Object, _model, new(100, 100));
        Assert.AreEqual("12345\r\nqwert\r\nasdfg", _model.GetSelectedText());

        _keybaordController.OnKeyUp(_model, false);
        _mouseController.OnMouseDown(_codeTextBox.Object, _model, new(0, 0));
        Assert.AreEqual("", _model.GetSelectedText());
    }

    [Test]
    public void OnMouseMove()
    {
        /* 12345
         * qwert
         * asdfg
         */
        _model.Text = "12345\r\nqwert\r\nasdfg";

        _mouseController.OnMouseMove(_codeTextBox.Object, _model, new(0, 0), MouseButtonState.Pressed);
        _mouseController.OnMouseMove(_codeTextBox.Object, _model, new(100, 100), MouseButtonState.Pressed);
        Assert.AreEqual("12345\r\nqwert\r\nasdfg", _model.GetSelectedText());
    }
}
