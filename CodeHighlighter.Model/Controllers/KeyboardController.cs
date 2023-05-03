using System.Linq;
using CodeHighlighter.Core;
using CodeHighlighter.Model;

namespace CodeHighlighter.Controllers;

public interface IKeyboardController
{
    bool KeyDown(Key key, bool controlPressed, bool shiftPressed);
    bool KeyUp(bool shiftPressed);
    void TextInput(string inputText);
}

internal class KeyboardController : IKeyboardController
{
    private readonly ICodeTextBox _model;

    public KeyboardController(ICodeTextBox model)
    {
        _model = model;
    }

    public bool KeyDown(Key key, bool controlPressed, bool shiftPressed)
    {
        var isHandled = true;
        if (shiftPressed)
        {
            _model.ActivateSelection();
        }
        // with control pressed
        if (controlPressed && key == Key.Up)
        {
            _model.ScrollLineUp();
        }
        else if (controlPressed && key == Key.Down)
        {
            _model.ScrollLineDown();
        }
        else if (controlPressed && key == Key.Left)
        {
            _model.MoveToPrevToken();
        }
        else if (controlPressed && key == Key.Right)
        {
            _model.MoveToNextToken();
        }
        else if (controlPressed && key == Key.Home)
        {
            _model.MoveCursorTextBegin();
        }
        else if (controlPressed && key == Key.End)
        {
            _model.MoveCursorTextEnd();
        }
        else if (controlPressed && key == Key.Back)
        {
            _model.DeleteLeftToken();
        }
        else if (controlPressed && key == Key.Delete)
        {
            _model.DeleteRightToken();
        }
        else if (controlPressed && key == Key.A)
        {
            _model.SelectAll();
        }
        else if (controlPressed && key == Key.X)
        {
            _model.Cut();
        }
        else if (controlPressed && key == Key.C)
        {
            _model.Copy();
        }
        else if (controlPressed && key == Key.V)
        {
            _model.Paste();
        }
        // without any modifiers
        else if (key == Key.Up)
        {
            _model.MoveCursorUp();
        }
        else if (key == Key.Down)
        {
            _model.MoveCursorDown();
        }
        else if (key == Key.Left)
        {
            _model.MoveCursorLeft();
        }
        else if (key == Key.Right)
        {
            _model.MoveCursorRight();
        }
        else if (key == Key.Home)
        {
            _model.MoveCursorStartLine();
        }
        else if (key == Key.End)
        {
            _model.MoveCursorEndLine();
        }
        else if (key == Key.PageUp)
        {
            _model.MoveCursorPageUp();
        }
        else if (key == Key.PageDown)
        {
            _model.MoveCursorPageDown();
        }
        else if (key == Key.Return)
        {
            _model.AppendNewLine();
        }
        else if (key == Key.Back)
        {
            _model.LeftDelete();
        }
        else if (key == Key.Delete)
        {
            _model.RightDelete();
        }
        else if (key == Key.Tab)
        {
            _model.InsertText("    ");
        }
        else
        {
            isHandled = false;
        }

        return isHandled;
    }

    public bool KeyUp(bool shiftPressed)
    {
        var isHandled = true;
        if (!shiftPressed)
        {
            _model.CompleteSelection();
        }
        else
        {
            isHandled = false;
        }

        return isHandled;
    }

    public void TextInput(string inputText)
    {
        var inputTextList = inputText.Where(ch => !NotAllowedSymbols.Value.Contains(ch)).ToList();
        if (!inputTextList.Any()) return;
        foreach (var ch in inputText) _model.AppendChar(ch);
    }
}
