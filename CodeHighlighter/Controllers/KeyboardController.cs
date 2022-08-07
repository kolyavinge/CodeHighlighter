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
        // with control and shift pressed
        if (controlPressed && shiftPressed && key == Key.U) // remove !
        {
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
            Clipboard.SetText(model.InputModel.GetSelectedText());
            model.LeftDelete();
        }
        else if (controlPressed && key == Key.C)
        {
            Clipboard.SetText(model.InputModel.GetSelectedText());
        }
        else if (controlPressed && key == Key.V)
        {
            model.InsertText(Clipboard.GetText());
        }
        else if (controlPressed && key == Key.L) // remove !
        {
            model.DeleteSelectedLines();
        }
        else if (controlPressed && key == Key.U) // remove !
        {
            model.ToLowerCase();
        }
        // with alt pressed
        else if (altPressed && key == Key.Up) // remove !
        {
            model.MoveSelectedLinesUp();
        }
        else if (altPressed && key == Key.Down) // remove !
        {
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

    private void ActivateOrCompleteSelection(InputModel inputModel, bool shiftPressed)
    {
        if (shiftPressed) inputModel.ActivateSelection();
        else inputModel.CompleteSelection();
    }
}
