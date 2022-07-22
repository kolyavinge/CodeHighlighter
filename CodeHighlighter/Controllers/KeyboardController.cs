using System.Windows;
using System.Windows.Input;
using CodeHighlighter.Model;

namespace CodeHighlighter.Controllers;

internal class KeyboardController
{
    public bool OnKeyDown(CodeTextBoxModel model, Key key, bool controlPressed, bool altPressed, bool shiftPressed, bool isReadOnly)
    {
        var isHandled = true;
        // with control and shift pressed
        if (controlPressed && shiftPressed && key == Key.U)
        {
            if (isReadOnly) return true;
            model.ToUpperCase();
        }
        // with control pressed
        else if (controlPressed && key == Key.Up)
        {
            model.ScrollLineUp();
        }
        else if (controlPressed && key == Key.Down)
        {
            model.ScrollLineDown();
        }
        else if (controlPressed && key == Key.Left)
        {
            ActivateOrCompleteSelection(model.InputModel, shiftPressed);
            model.MoveToPrevToken();
        }
        else if (controlPressed && key == Key.Right)
        {
            ActivateOrCompleteSelection(model.InputModel, shiftPressed);
            model.MoveToNextToken();
        }
        else if (controlPressed && key == Key.Home)
        {
            ActivateOrCompleteSelection(model.InputModel, shiftPressed);
            model.MoveCursorTextBegin();
        }
        else if (controlPressed && key == Key.End)
        {
            ActivateOrCompleteSelection(model.InputModel, shiftPressed);
            model.MoveCursorTextEnd();
        }
        else if (controlPressed && key == Key.Back)
        {
            model.DeleteLeftToken();
        }
        else if (controlPressed && key == Key.Delete)
        {
            model.DeleteRightToken();
        }
        else if (controlPressed && key == Key.A)
        {
            model.SelectAll();
        }
        else if (controlPressed && key == Key.X)
        {
            if (isReadOnly) return true;
            Clipboard.SetText(model.InputModel.GetSelectedText());
            model.LeftDelete();
        }
        else if (controlPressed && key == Key.C)
        {
            Clipboard.SetText(model.InputModel.GetSelectedText());
        }
        else if (controlPressed && key == Key.V)
        {
            if (isReadOnly) return true;
            model.InsertText(Clipboard.GetText());
        }
        else if (controlPressed && key == Key.L)
        {
            if (isReadOnly) return true;
            model.DeleteSelectedLines();
        }
        else if (controlPressed && key == Key.U)
        {
            if (isReadOnly) return true;
            model.ToLowerCase();
        }
        // with alt pressed
        else if (altPressed && key == Key.Up)
        {
            if (isReadOnly) return true;
            model.MoveSelectedLinesUp();
        }
        else if (altPressed && key == Key.Down)
        {
            if (isReadOnly) return true;
            model.MoveSelectedLinesDown();
        }
        // without any modifiers
        else if (key == Key.Up)
        {
            ActivateOrCompleteSelection(model.InputModel, shiftPressed);
            model.MoveCursorUp();
        }
        else if (key == Key.Down)
        {
            ActivateOrCompleteSelection(model.InputModel, shiftPressed);
            model.MoveCursorDown();
        }
        else if (key == Key.Left)
        {
            ActivateOrCompleteSelection(model.InputModel, shiftPressed);
            model.MoveCursorLeft();
        }
        else if (key == Key.Right)
        {
            ActivateOrCompleteSelection(model.InputModel, shiftPressed);
            model.MoveCursorRight();
        }
        else if (key == Key.Home)
        {
            ActivateOrCompleteSelection(model.InputModel, shiftPressed);
            model.MoveCursorStartLine();
        }
        else if (key == Key.End)
        {
            ActivateOrCompleteSelection(model.InputModel, shiftPressed);
            model.MoveCursorEndLine();
        }
        else if (key == Key.PageUp)
        {
            ActivateOrCompleteSelection(model.InputModel, shiftPressed);
            model.MoveCursorPageUp();
        }
        else if (key == Key.PageDown)
        {
            ActivateOrCompleteSelection(model.InputModel, shiftPressed);
            model.MoveCursorPageDown();
        }
        else if (key == Key.Return)
        {
            if (isReadOnly) return true;
            model.NewLine();
        }
        else if (key == Key.Back)
        {
            if (isReadOnly) return true;
            model.LeftDelete();
        }
        else if (key == Key.Delete)
        {
            if (isReadOnly) return true;
            model.RightDelete();
        }
        else if (key == Key.Tab)
        {
            if (isReadOnly) return true;
            model.InsertText("    ");
        }
        else
        {
            isHandled = false;
        }

        return isHandled;
    }

    public void OnTextInput(CodeTextBoxModel codeTextBoxModel, string inputText, bool isReadOnly)
    {
        if (isReadOnly) return;
        codeTextBoxModel.TextInput(inputText);
    }

    private void ActivateOrCompleteSelection(InputModel inputModel, bool shiftPressed)
    {
        if (shiftPressed) inputModel.ActivateSelection();
        else inputModel.CompleteSelection();
    }
}
