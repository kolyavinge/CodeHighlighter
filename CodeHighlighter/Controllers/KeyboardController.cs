using System.Linq;
using System.Windows;
using System.Windows.Input;
using CodeHighlighter.Model;

namespace CodeHighlighter.Controllers;

internal class KeyboardController
{
    public bool OnKeyDown(ICodeTextBoxModel model, Key key, bool controlPressed, bool shiftPressed)
    {
        var isHandled = true;
        if (shiftPressed)
        {
            model.ActivateSelection();
        }
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
            model.MoveToPrevToken();
        }
        else if (controlPressed && key == Key.Right)
        {
            model.MoveToNextToken();
        }
        else if (controlPressed && key == Key.Home)
        {
            model.MoveCursorTextBegin();
        }
        else if (controlPressed && key == Key.End)
        {
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
            model.MoveCursorUp();
        }
        else if (key == Key.Down)
        {
            model.MoveCursorDown();
        }
        else if (key == Key.Left)
        {
            model.MoveCursorLeft();
        }
        else if (key == Key.Right)
        {
            model.MoveCursorRight();
        }
        else if (key == Key.Home)
        {
            model.MoveCursorStartLine();
        }
        else if (key == Key.End)
        {
            model.MoveCursorEndLine();
        }
        else if (key == Key.PageUp)
        {
            model.MoveCursorPageUp();
        }
        else if (key == Key.PageDown)
        {
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

    public bool OnKeyUp(ICodeTextBoxModel model, bool shiftPressed)
    {
        var isHandled = true;
        if (!shiftPressed)
        {
            model.CompleteSelection();
        }
        else
        {
            isHandled = false;
        }

        return isHandled;
    }

    public void OnTextInput(ICodeTextBoxModel codeTextBoxModel, string inputText)
    {
        var inputTextList = inputText.Where(ch => !NotAllowedSymbols.Value.Contains(ch)).ToList();
        if (!inputTextList.Any()) return;
        foreach (var ch in inputText) codeTextBoxModel.AppendChar(ch);
    }
}
