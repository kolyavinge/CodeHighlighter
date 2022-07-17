using System.Windows;
using System.Windows.Input;
using CodeHighlighter.Model;

namespace CodeHighlighter.Controllers;

internal class KeyboardController
{
    public bool OnKeyDown(
        CodeTextBoxModel codeTextBoxModel, ITextSource textSource, ITextSelectionActivator selectionActivator, Key key, bool controlPressed, bool altPressed, bool shiftPressed, bool isReadOnly)
    {
        var isHandled = true;
        // with control and shift pressed
        if (controlPressed && shiftPressed && key == Key.U)
        {
            if (isReadOnly) return true;
            codeTextBoxModel.ToUpperCase();
        }
        // with control pressed
        else if (controlPressed && key == Key.Up)
        {
            codeTextBoxModel.ScrollLineUp();
        }
        else if (controlPressed && key == Key.Down)
        {
            codeTextBoxModel.ScrollLineDown();
        }
        else if (controlPressed && key == Key.Left)
        {
            ActivateOrCompleteSelection(selectionActivator, shiftPressed);
            codeTextBoxModel.MoveToPrevToken();
        }
        else if (controlPressed && key == Key.Right)
        {
            ActivateOrCompleteSelection(selectionActivator, shiftPressed);
            codeTextBoxModel.MoveToNextToken();
        }
        else if (controlPressed && key == Key.Home)
        {
            ActivateOrCompleteSelection(selectionActivator, shiftPressed);
            codeTextBoxModel.MoveCursorTextBegin();
        }
        else if (controlPressed && key == Key.End)
        {
            ActivateOrCompleteSelection(selectionActivator, shiftPressed);
            codeTextBoxModel.MoveCursorTextEnd();
        }
        else if (controlPressed && key == Key.Back)
        {
            codeTextBoxModel.DeleteLeftToken();
        }
        else if (controlPressed && key == Key.Delete)
        {
            codeTextBoxModel.DeleteRightToken();
        }
        else if (controlPressed && key == Key.A)
        {
            codeTextBoxModel.SelectAll();
        }
        else if (controlPressed && key == Key.X)
        {
            if (isReadOnly) return true;
            Clipboard.SetText(textSource.GetSelectedText());
            codeTextBoxModel.LeftDelete();
        }
        else if (controlPressed && key == Key.C)
        {
            Clipboard.SetText(textSource.GetSelectedText());
        }
        else if (controlPressed && key == Key.V)
        {
            if (isReadOnly) return true;
            codeTextBoxModel.InsertText(Clipboard.GetText());
        }
        else if (controlPressed && key == Key.L)
        {
            if (isReadOnly) return true;
            codeTextBoxModel.DeleteSelectedLines();
        }
        else if (controlPressed && key == Key.U)
        {
            if (isReadOnly) return true;
            codeTextBoxModel.ToLowerCase();
        }
        // with alt pressed
        else if (altPressed && key == Key.Up)
        {
            if (isReadOnly) return true;
            codeTextBoxModel.MoveSelectedLinesUp();
        }
        else if (altPressed && key == Key.Down)
        {
            if (isReadOnly) return true;
            codeTextBoxModel.MoveSelectedLinesDown();
        }
        // without any modifiers
        else if (key == Key.Up)
        {
            ActivateOrCompleteSelection(selectionActivator, shiftPressed);
            codeTextBoxModel.MoveCursorUp();
        }
        else if (key == Key.Down)
        {
            ActivateOrCompleteSelection(selectionActivator, shiftPressed);
            codeTextBoxModel.MoveCursorDown();
        }
        else if (key == Key.Left)
        {
            ActivateOrCompleteSelection(selectionActivator, shiftPressed);
            codeTextBoxModel.MoveCursorLeft();
        }
        else if (key == Key.Right)
        {
            ActivateOrCompleteSelection(selectionActivator, shiftPressed);
            codeTextBoxModel.MoveCursorRight();
        }
        else if (key == Key.Home)
        {
            ActivateOrCompleteSelection(selectionActivator, shiftPressed);
            codeTextBoxModel.MoveCursorStartLine();
        }
        else if (key == Key.End)
        {
            ActivateOrCompleteSelection(selectionActivator, shiftPressed);
            codeTextBoxModel.MoveCursorEndLine();
        }
        else if (key == Key.PageUp)
        {
            ActivateOrCompleteSelection(selectionActivator, shiftPressed);
            codeTextBoxModel.MoveCursorPageUp();
        }
        else if (key == Key.PageDown)
        {
            ActivateOrCompleteSelection(selectionActivator, shiftPressed);
            codeTextBoxModel.MoveCursorPageDown();
        }
        else if (key == Key.Return)
        {
            if (isReadOnly) return true;
            codeTextBoxModel.NewLine();
        }
        else if (key == Key.Back)
        {
            if (isReadOnly) return true;
            codeTextBoxModel.LeftDelete();
        }
        else if (key == Key.Delete)
        {
            if (isReadOnly) return true;
            codeTextBoxModel.RightDelete();
        }
        else if (key == Key.Tab)
        {
            if (isReadOnly) return true;
            codeTextBoxModel.InsertText("    ");
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

    private void ActivateOrCompleteSelection(ITextSelectionActivator selectionActivator, bool shiftPressed)
    {
        if (shiftPressed) selectionActivator.ActivateSelection();
        else selectionActivator.CompleteSelection();
    }
}
