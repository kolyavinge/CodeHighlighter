using System.IO;
using System.Windows;
using System.Windows.Input;
using CodeEditor.Mvvm;
using CodeHighlighter;
using CodeHighlighter.CodeProviders;

namespace CodeEditor.ViewModel
{
    public class MainViewModel
    {
        public TextHolder TextHolder { get; set; }

        public ICodeProvider CodeProvider { get; set; }

        public ICommand CopyTextCommand => new ActionCommand(CopyText);

        public MainViewModel()
        {
            TextHolder = new TextHolder(File.ReadAllText(@"D:\Projects\CodeHighlighter\CodeEditor\Examples\sql.txt"));
            CodeProvider = new SqlCodeProvider();
        }

        private void CopyText()
        {
            Clipboard.SetText(TextHolder.TextValue);
        }
    }
}
