using System.Windows;
using System.Windows.Input;
using CodeHighlighter.Commands;
using CodeHighlighter.Model;

namespace CodeHighlighter.Controllers;

internal class KeyboardController
{
    private readonly CodeTextBoxCommands _commands;
    private readonly ITextSource _textSource;
    private readonly ITextSelectionActivator _selectionActivator;

    public KeyboardController(CodeTextBoxCommands commands, ITextSource textSource, ITextSelectionActivator selectionActivator)
    {
        _commands = commands;
        _textSource = textSource;
        _selectionActivator = selectionActivator;
    }

    public bool OnKeyDown(Key key, bool controlPressed, bool altPressed, bool shiftPressed, bool isReadOnly)
    {
        var isHandled = true;
        // with control and shift pressed
        if (controlPressed && shiftPressed && key == Key.U)
        {
            if (isReadOnly) return true;
            _commands.ToUpperCaseCommand.Execute();
        }
        // with control pressed
        else if (controlPressed && key == Key.Up)
        {
            _commands.ScrollLineUpCommand.Execute();
        }
        else if (controlPressed && key == Key.Down)
        {
            _commands.ScrollLineDownCommand.Execute();
        }
        else if (controlPressed && key == Key.Left)
        {
            ActivateOrCompleteSelection(shiftPressed);
            _commands.MoveToPrevTokenCommand.Execute();
        }
        else if (controlPressed && key == Key.Right)
        {
            ActivateOrCompleteSelection(shiftPressed);
            _commands.MoveToNextTokenCommand.Execute();
        }
        else if (controlPressed && key == Key.Home)
        {
            ActivateOrCompleteSelection(shiftPressed);
            _commands.MoveCursorTextBeginCommand.Execute();
        }
        else if (controlPressed && key == Key.End)
        {
            ActivateOrCompleteSelection(shiftPressed);
            _commands.MoveCursorTextEndCommand.Execute();
        }
        else if (controlPressed && key == Key.Back)
        {
            _commands.DeleteLeftTokenCommand.Execute();
        }
        else if (controlPressed && key == Key.Delete)
        {
            _commands.DeleteRightTokenCommand.Execute();
        }
        else if (controlPressed && key == Key.A)
        {
            _commands.SelectAllCommand.Execute();
        }
        else if (controlPressed && key == Key.X)
        {
            if (isReadOnly) return true;
            Clipboard.SetText(_textSource.GetSelectedText());
            _commands.LeftDeleteCommand.Execute();
        }
        else if (controlPressed && key == Key.C)
        {
            Clipboard.SetText(_textSource.GetSelectedText());
        }
        else if (controlPressed && key == Key.V)
        {
            if (isReadOnly) return true;
            _commands.InsertTextCommand.Execute(new InsertTextCommandParameter(Clipboard.GetText()));
        }
        else if (controlPressed && key == Key.L)
        {
            if (isReadOnly) return true;
            _commands.DeleteSelectedLinesCommand.Execute();
        }
        else if (controlPressed && key == Key.U)
        {
            if (isReadOnly) return true;
            _commands.ToLowerCaseCommand.Execute();
        }
        // with alt pressed
        else if (altPressed && key == Key.Up)
        {
            if (isReadOnly) return true;
            _commands.MoveSelectedLinesUpCommand.Execute();
        }
        else if (altPressed && key == Key.Down)
        {
            if (isReadOnly) return true;
            _commands.MoveSelectedLinesDownCommand.Execute();
        }
        // without any modifiers
        else if (key == Key.Up)
        {
            ActivateOrCompleteSelection(shiftPressed);
            _commands.MoveCursorUpCommand.Execute();
        }
        else if (key == Key.Down)
        {
            ActivateOrCompleteSelection(shiftPressed);
            _commands.MoveCursorDownCommand.Execute();
        }
        else if (key == Key.Left)
        {
            ActivateOrCompleteSelection(shiftPressed);
            _commands.MoveCursorLeftCommand.Execute();
        }
        else if (key == Key.Right)
        {
            ActivateOrCompleteSelection(shiftPressed);
            _commands.MoveCursorRightCommand.Execute();
        }
        else if (key == Key.Home)
        {
            ActivateOrCompleteSelection(shiftPressed);
            _commands.MoveCursorStartLineCommand.Execute();
        }
        else if (key == Key.End)
        {
            ActivateOrCompleteSelection(shiftPressed);
            _commands.MoveCursorEndLineCommand.Execute();
        }
        else if (key == Key.PageUp)
        {
            ActivateOrCompleteSelection(shiftPressed);
            _commands.MoveCursorPageUpCommand.Execute();
        }
        else if (key == Key.PageDown)
        {
            ActivateOrCompleteSelection(shiftPressed);
            _commands.MoveCursorPageDownCommand.Execute();
        }
        else if (key == Key.Return)
        {
            if (isReadOnly) return true;
            _commands.NewLineCommand.Execute();
        }
        else if (key == Key.Back)
        {
            if (isReadOnly) return true;
            _commands.LeftDeleteCommand.Execute();
        }
        else if (key == Key.Delete)
        {
            if (isReadOnly) return true;
            _commands.RightDeleteCommand.Execute();
        }
        else if (key == Key.Tab)
        {
            if (isReadOnly) return true;
            _commands.InsertTextCommand.Execute(new InsertTextCommandParameter("    "));
        }
        else
        {
            isHandled = false;
        }

        return isHandled;
    }

    public void OnTextInput(string inputText, bool isReadOnly)
    {
        if (isReadOnly) return;
        _commands.TextInputCommand.Execute(new TextInputCommandParameter(inputText));
    }

    private void ActivateOrCompleteSelection(bool shiftPressed)
    {
        if (shiftPressed) _selectionActivator.ActivateSelection();
        else _selectionActivator.CompleteSelection();
    }
}
