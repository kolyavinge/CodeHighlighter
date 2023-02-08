﻿using CodeHighlighter.Common;
using CodeHighlighter.Model;

namespace CodeHighlighter.Controllers;

public interface IMouseController
{
    void LeftButtonDown(Point positionInControl);
    void RightButtonDown(Point positionInControl);
    void Move(Point positionInControl);
    void LeftButtonUp();
    void ScrollWheel(int pages, bool up);
    void LeftButtonDoubleClick(Point positionInControl);
}

internal class MouseController : IMouseController
{
    private readonly ICodeTextBox _codeTextBox;
    private readonly ICodeTextBoxModel _model;
    private readonly IPointInTextSelection _pointInTextSelection;
    private readonly IMouseCursorPosition _mouseCursorPosition;
    private bool _isLeftButtonPressed;

    public MouseController(
        ICodeTextBox codeTextBox,
        ICodeTextBoxModel model,
        IPointInTextSelection pointInTextSelection,
        IMouseCursorPosition mouseCursorPosition)
    {
        _codeTextBox = codeTextBox;
        _model = model;
        _pointInTextSelection = pointInTextSelection;
        _mouseCursorPosition = mouseCursorPosition;
    }

    public void LeftButtonDown(Point positionInControl)
    {
        _isLeftButtonPressed = true;
        _codeTextBox.Focus();
        var pos = _mouseCursorPosition.GetCursorPosition(positionInControl);
        _model.MoveCursorTo(pos);
        _codeTextBox.InvalidateVisual();
    }

    public void RightButtonDown(Point positionInControl)
    {
        _codeTextBox.Focus();
        var pos = _mouseCursorPosition.GetCursorPosition(positionInControl);
        if (_model.TextSelection.IsExist && _pointInTextSelection.Check(pos))
        {
            return;
        }
        else
        {
            _model.MoveCursorTo(pos);
            _codeTextBox.InvalidateVisual();
        }
    }

    public void Move(Point positionInControl)
    {
        if (_isLeftButtonPressed)
        {
            _model.ActivateSelection();
            var pos = _mouseCursorPosition.GetCursorPosition(positionInControl);
            _model.MoveCursorTo(pos);
            _codeTextBox.InvalidateVisual();
        }
    }

    public void LeftButtonUp()
    {
        _isLeftButtonPressed = false;
        _model.CompleteSelection();
    }

    public void ScrollWheel(int pages, bool up)
    {
        _model.Viewport.VerticalScrollBarValue += (up ? -1 : 1) * pages * _model.TextMeasures.LineHeight;
        _codeTextBox.InvalidateVisual();
    }

    public void LeftButtonDoubleClick(Point positionInControl)
    {
        var pos = _mouseCursorPosition.GetCursorPosition(positionInControl);
        _model.SelectToken(pos);
        _codeTextBox.InvalidateVisual();
    }
}