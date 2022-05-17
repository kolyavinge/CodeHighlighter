using System.IO;
using System.Windows;
using System.Windows.Input;
using CodeEditor.Mvvm;
using CodeHighlighter;
using CodeHighlighter.CodeProviders;
using CodeHighlighter.Commands;

namespace CodeEditor.ViewModel;

public class MainViewModel
{
    public TextHolder TextHolder { get; set; }

    public CodeTextBoxCommands Commands { get; set; }

    public ICodeProvider CodeProvider { get; set; }

    public ICommand CopyTextCommand => new ActionCommand(CopyText);

    public ICommand InsertLineCommand => new ActionCommand(InsertLine);

    public MainViewModel()
    {
        TextHolder = new TextHolder(File.ReadAllText(@"D:\Projects\CodeHighlighter\CodeEditor\Examples\sql.txt"));
        Commands = new CodeTextBoxCommands();
        CodeProvider = new SqlCodeProvider();
    }

    private void CopyText()
    {
        Clipboard.SetText(TextHolder.TextValue);
    }

    private void InsertLine()
    {
        Commands.MoveCursorTextEndCommand.Execute();
        Commands.NewLineCommand.Execute();
        Commands.InsertTextCommand.Execute(new InsertTextCommandParameter("new inserted line"));
    }
}
