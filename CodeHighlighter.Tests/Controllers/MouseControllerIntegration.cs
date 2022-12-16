﻿using System.Windows.Input;
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
    private MouseController _controller;

    [SetUp]
    public void Setup()
    {
        _codeTextBox = new Mock<ICodeTextBox>();
        _model = CodeTextBoxModelFactory.MakeModel(new EmptyCodeProvider());
        _model.TextMeasures.UpdateMeasures(10, 10);
        _model.AttachCodeTextBox(_codeTextBox.Object);
        _controller = new MouseController();
    }

    [Test]
    public void OnMouseDown()
    {
        /* 12345
         * qwert
         * asdfg
         */
        _model.Text = "12345\r\nqwert\r\nasdfg";

        _controller.OnMouseDown(_codeTextBox.Object, _model, new(0, 0), true);
        _controller.OnMouseDown(_codeTextBox.Object, _model, new(100, 100), true);
        Assert.AreEqual("12345\r\nqwert\r\nasdfg", _model.GetSelectedText());

        _controller.OnMouseDown(_codeTextBox.Object, _model, new(0, 0), false);
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

        _controller.OnMouseMove(_codeTextBox.Object, _model, new(0, 0), MouseButtonState.Pressed);
        _controller.OnMouseMove(_codeTextBox.Object, _model, new(100, 100), MouseButtonState.Pressed);
        Assert.AreEqual("12345\r\nqwert\r\nasdfg", _model.GetSelectedText());
    }
}
