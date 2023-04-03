using System.Collections.Generic;
using CodeHighlighter.Core;

namespace CodeHighlighter.Ancillary;

public readonly struct SearchEntry
{
    public readonly CursorPosition StartPosition;
    public readonly CursorPosition EndPosition;

    public SearchEntry(CursorPosition startPosition, CursorPosition endPosition)
    {
        StartPosition = startPosition;
        EndPosition = endPosition;
    }
}

public struct SearchOptions
{
    public bool IgnoreCase;
}

internal interface ISearchLogic
{
    IEnumerable<SearchEntry> DoSearch(IText text, string pattern, SearchOptions options);
}
