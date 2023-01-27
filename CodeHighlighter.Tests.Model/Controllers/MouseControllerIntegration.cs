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
    private IKeyboardController _keybaordController;
    private IMouseController _mouseController;

    [SetUp]
    public void Setup()
    {
        _codeTextBox = new Mock<ICodeTextBox>();
        _codeTextBox = new Mock<ICodeTextBox>();
        _model = CodeTextBoxModelFactory.MakeModel(new EmptyCodeProvider());
        _model.AttachCodeTextBox(_codeTextBox.Object);
        _codeTextBox.Raise(x => x.FontSettingsChanged += null, new FontSettingsChangedEventArgs(10, 10));
        _keybaordController = CodeTextBoxModelFactory.MakeKeyboardController(_model);
        _mouseController = CodeTextBoxModelFactory.MakeMouseController(_codeTextBox.Object, _model);
    }

    [Test]
    public void MouseDown()
    {
        /* 12345
         * qwert
         * asdfg
         */
        _model.Text = "12345\r\nqwert\r\nasdfg";

        _mouseController.LeftButtonDown(new(0, 0));
        _keybaordController.KeyDown(Key.LeftShift, false, true);
        _mouseController.LeftButtonDown(new(100, 100));
        Assert.AreEqual("12345\r\nqwert\r\nasdfg", _model.GetSelectedText());

        _keybaordController.KeyUp(false);
        _mouseController.LeftButtonDown(new(0, 0));
        Assert.AreEqual("", _model.GetSelectedText());
    }

    [Test]
    public void MouseMove()
    {
        /* 12345
         * qwert
         * asdfg
         */
        _model.Text = "12345\r\nqwert\r\nasdfg";

        _mouseController.LeftButtonMove(new(0, 0));
        _mouseController.LeftButtonMove(new(100, 100));
        Assert.AreEqual("12345\r\nqwert\r\nasdfg", _model.GetSelectedText());
    }
}
