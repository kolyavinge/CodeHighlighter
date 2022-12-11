using System.Linq;
using System.Windows;
using System.Windows.Input;
using CodeHighlighter.Model;

namespace CodeHighlighter.Controllers;

internal class KeyboardController
{
    public bool OnKeyDown(CodeTextBoxModel model, Key key, bool controlPressed, bool altPressed, bool shiftPressed)
    {
        var isHandled = true;
        // with control pressed
        if (controlPressed && key == Key.Up)
        {
            model.ScrollLineUp();
        }
        else if (controlPressed && key == Key.Down)
        {
            model.ScrollLineDown();
        }
        else if (controlPressed && key == Key.Left)
        {
            ActivateOrCompleteSelection(model, shiftPressed);
            model.MoveToPrevToken();
        }
        else if (controlPressed && key == Key.Right)
        {
            ActivateOrCompleteSelection(model, shiftPressed);
            model.MoveToNextToken();
        }
        else if (controlPressed && key == Key.Home)
        {
            ActivateOrCompleteSelection(model, shiftPressed);
            model.MoveCursorTextBegin();
        }
        else if (controlPressed && key == Key.End)
        {
            ActivateOrCompleteSelection(model, shiftPressed);
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
            Clipboard.SetText(model.GetSelectedText());
            model.LeftDelete();
        }
        else if (controlPressed && key == Key.C)
        {
            Clipboard.SetText(model.GetSelectedText());
        }
        else if (controlPressed && key == Key.V)
        {
            model.InsertText(Clipboard.GetText());
        }
        // without any modifiers
        else if (key == Key.Up)
        {
            ActivateOrCompleteSelection(model, shiftPressed);
            model.MoveCursorUp();
        }
        else if (key == Key.Down)
        {
            ActivateOrCompleteSelection(model, shiftPressed);
            model.MoveCursorDown();
        }
        else if (key == Key.Left)
        {
            ActivateOrCompleteSelection(model, shiftPressed);
            model.MoveCursorLeft();
        }
        else if (key == Key.Right)
        {
            ActivateOrCompleteSelection(model, shiftPressed);
            model.MoveCursorRight();
        }
        else if (key == Key.Home)
        {
            ActivateOrCompleteSelection(model, shiftPressed);
            model.MoveCursorStartLine();
        }
        else if (key == Key.End)
        {
            ActivateOrCompleteSelection(model, shiftPressed);
            model.MoveCursorEndLine();
        }
        else if (key == Key.PageUp)
        {
            ActivateOrCompleteSelection(model, shiftPressed);
            model.MoveCursorPageUp();
        }
        else if (key == Key.PageDown)
        {
            ActivateOrCompleteSelection(model, shiftPressed);
            model.MoveCursorPageDown();
        }
        else if (key == Key.Return)
        {
            model.AppendNewLine();
        }
        else if (key == Key.Back)
        {
            model.LeftDelete();
        }
        else if (key == Key.Delete)
        {
            model.RightDelete();
        }
        else if (key == Key.Tab)
        {
            model.InsertText("    ");
        }
        else
        {
            isHandled = false;
        }

        return isHandled;
    }

    public void OnTextInput(CodeTextBoxModel codeTextBoxModel, string inputText)
    {
        var inputTextList = inputText.Where(ch => !Text.NotAllowedSymbols.Contains(ch)).ToList();
        if (!inputTextList.Any()) return;
        foreach (var ch in inputText) codeTextBoxModel.AppendChar(ch);
    }

    private void ActivateOrCompleteSelection(CodeTextBoxModel codeTextBoxModel, bool shiftPressed)
    {
        if (shiftPressed) codeTextBoxModel.ActivateSelection();
        else codeTextBoxModel.CompleteSelection();
    }
}
