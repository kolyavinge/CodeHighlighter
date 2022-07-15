namespace CodeHighlighter.Contracts;

public interface ITextCursor
{
    int LineIndex { get; }

    int ColumnIndex { get; }
}
