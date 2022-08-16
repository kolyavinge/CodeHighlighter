using System.IO;
using System.Windows;
using System.Windows.Input;
using CodeEditor.Mvvm;
using CodeHighlighter.CodeProvidering;
using CodeHighlighter.Model;

namespace CodeEditor.ViewModel;

public class MainViewModel
{
    public CodeTextBoxModel CodeTextBoxModel { get; }

    public ICodeProvider CodeProvider { get; }

    public ICommand CopyTextCommand => new ActionCommand(CopyText);

    public ICommand InsertLineCommand => new ActionCommand(InsertLine);

    public string? SelectedLineToGoto { get; set; }

    public ICommand GotoLineCommand => new ActionCommand(GotoLine);

    public ICommand KeyDownCommand { get; }

    private bool _isReadOnly;
    public bool IsReadOnly
    {
        get => _isReadOnly;
        set { _isReadOnly = value; CodeTextBoxModel.IsReadOnly = value; }
    }

    public MainViewModel()
    {
        CodeProvider = new SqlCodeProvider();
        CodeTextBoxModel = new CodeTextBoxModel(CodeProvider, new() { HighlighteredBrackets = "()[]" });
        CodeTextBoxModel.SetText(File.ReadAllText(@"D:\Projects\CodeHighlighter\CodeEditor\Examples\sql.txt"));
        KeyDownCommand = new ActionCommand<KeyEventArgs>(KeyDown);
    }

    private void CopyText()
    {
        Clipboard.SetText(CodeTextBoxModel.Text.ToString());
    }

    private void InsertLine()
    {
        CodeTextBoxModel.MoveCursorTextEnd();
        CodeTextBoxModel.AppendNewLine();
        CodeTextBoxModel.InsertText("new inserted line");
    }

    private void GotoLine()
    {
        if (Int32.TryParse(SelectedLineToGoto ?? "", out int gotoLine))
        {
            CodeTextBoxModel.GotoLine(gotoLine - 1);
        }
    }

    private void KeyDown(KeyEventArgs e)
    {
        var key = e.Key == Key.System ? e.SystemKey : e.Key;
        var controlPressed = (e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
        var altPressed = (e.KeyboardDevice.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt;
        var shiftPressed = (e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
        if (controlPressed && !altPressed && !shiftPressed && key == Key.L)
        {
            CodeTextBoxModel.DeleteSelectedLines();
            e.Handled = true;
        }
        else if (controlPressed && !altPressed && !shiftPressed && key == Key.Z)
        {
            CodeTextBoxModel.History.Undo();
            e.Handled = true;
        }
        else if (controlPressed && !altPressed && !shiftPressed && key == Key.Y)
        {
            CodeTextBoxModel.History.Redo();
            e.Handled = true;
        }
        else if (controlPressed && !altPressed && shiftPressed && key == Key.U)
        {
            CodeTextBoxModel.ToUpperCase();
            e.Handled = true;
        }
        else if (controlPressed && !altPressed && !shiftPressed && key == Key.U)
        {
            CodeTextBoxModel.ToLowerCase();
            e.Handled = true;
        }
        else if (!controlPressed && altPressed && !shiftPressed && key == Key.Up)
        {
            CodeTextBoxModel.MoveSelectedLinesUp();
            e.Handled = true;
        }
        else if (!controlPressed && altPressed && !shiftPressed && key == Key.Down)
        {
            CodeTextBoxModel.MoveSelectedLinesDown();
            e.Handled = true;
        }
    }
}
