using System.Collections.Generic;
using CodeHighlighter.Core;

namespace CodeHighlighter.Ancillary;

public struct SearchOptions
{
    public bool IgnoreCase;
}

internal interface ISearchLogic
{
    IEnumerable<TextPosition> DoSearch(IText text, string pattern, SearchOptions options);
}
