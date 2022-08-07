using System;
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

    public ICommand RedoCommand => new ActionCommand(Redo);

    public ICommand UndoCommand => new ActionCommand(Undo);

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

    private void Redo()
    {
        CodeTextBoxModel.History.Redo();
    }

    private void Undo()
    {
        CodeTextBoxModel.History.Undo();
    }
}
