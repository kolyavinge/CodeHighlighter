using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using CodeEditor.Mvvm;
using CodeHighlighter;
using CodeHighlighter.CodeProviders;
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

    public MainViewModel()
    {
        CodeProvider = new SqlCodeProvider();
        CodeTextBoxModel = new CodeTextBoxModel();
        CodeTextBoxModel.SetCodeProvider(CodeProvider);
        CodeTextBoxModel.SetText(File.ReadAllText(@"D:\Projects\CodeHighlighter\CodeEditor\Examples\sql.txt"));
    }

    private void CopyText()
    {
        Clipboard.SetText(CodeTextBoxModel.Text.TextContent);
    }

    private void InsertLine()
    {
        CodeTextBoxModel.MoveCursorTextEnd();
        CodeTextBoxModel.NewLine();
        CodeTextBoxModel.InsertText("new inserted line");
    }

    private void GotoLine()
    {
        if (Int32.TryParse(SelectedLineToGoto ?? "", out int gotoLine))
        {
            CodeTextBoxModel.GotoLine(gotoLine - 1);
        }
    }
}
