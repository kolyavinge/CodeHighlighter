using System.Linq;
using CodeHighlighter.Ancillary;

namespace CodeHighlighter.Core;

internal interface IPageScroller
{
    int GetCursorLineIndexAfterScrollPageUp(int cursorLineIndex);
    int GetCursorLineIndexAfterScrollPageDown(int cursorLineIndex);
}

internal class PageScroller : IPageScroller
{
    private readonly IViewportInternal _viewport;
    private readonly ILineGapCollection _gaps;

    public PageScroller(
        IViewportInternal viewport,
        ILineGapCollection gaps)
    {
        _viewport = viewport;
        _gaps = gaps;
    }

    public int GetCursorLineIndexAfterScrollPageUp(int cursorLineIndex)
    {
        var endCursorLineIndex = cursorLineIndex - _viewport.GetLinesCountInViewport();
        if (endCursorLineIndex < 0) endCursorLineIndex = 0;
        if (_gaps.AnyItems)
        {
            endCursorLineIndex -= Enumerable.Range(endCursorLineIndex, cursorLineIndex - endCursorLineIndex).Sum(i => _gaps[i]?.CountBefore) ?? 0;
        }

        return endCursorLineIndex;
    }

    public int GetCursorLineIndexAfterScrollPageDown(int cursorLineIndex)
    {
        var endCursorLineIndex = cursorLineIndex + _viewport.GetLinesCountInViewport();
        if (_gaps.AnyItems)
        {
            endCursorLineIndex -= Enumerable.Range(cursorLineIndex, endCursorLineIndex - cursorLineIndex).Sum(i => _gaps[i]?.CountBefore) ?? 0;
        }

        return endCursorLineIndex;
    }
}
