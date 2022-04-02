using System.IO;
using CodeHighlighter;
using CodeHighlighter.CodeProviders;

namespace CodeEditor.ViewModel
{
    public class MainViewModel
    {
        public string Code { get; set; }

        public ICodeProvider CodeProvider { get; set; }

        public MainViewModel()
        {
            Code = File.ReadAllText(@"D:\Projects\CodeHighlighter\CodeEditor\Examples\sql.txt");
            CodeProvider = new SqlCodeProvider();
        }
    }
}
