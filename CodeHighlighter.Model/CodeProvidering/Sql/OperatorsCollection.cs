using System.Collections;
using System.Collections.Generic;

namespace CodeHighlighter.CodeProvidering.Sql;

internal class OperatorsCollection : IEnumerable<string>
{
    private readonly List<string> _keywords = new()
    {
        "AND",
        "OR",
        "NOT",
        "NULL",
        "IN",
        "IS",
        "EXISTS"
    };

    public IEnumerator<string> GetEnumerator() => _keywords.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _keywords.GetEnumerator();
}
