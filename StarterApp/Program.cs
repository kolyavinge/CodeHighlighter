using CodeHighlighter.Ancillary;
using CodeHighlighter.Core;

var bigtext = File.ReadAllText(@"D:\Projects\CodeHighlighter\CodeEditor\Examples\big_sql.txt");
var text = new Text(bigtext);
var searchLogic = new TextSearchLogic();
for (int i = 0; i < 100; i++)
{
    var result = searchLogic.DoSearch(text, "create", new() { IgnoreCase = true }).ToList();
}
