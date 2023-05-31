using System.Collections.Generic;
using CodeHighlighter.Common;
using CodeHighlighter.Core;

namespace CodeHighlighter.Ancillary;

public readonly record struct TextHighlight(TextPosition Position, Color Color);

public interface ITextHighlighter
{
    event EventHandler? Changed;
    IReadOnlyCollection<TextHighlight> Highlights { get; }
    void Add(IEnumerable<TextHighlight> highlights);
    void Remove(IEnumerable<TextHighlight> highlights);
    IEnumerable<(TextHighlight, IEnumerable<TextSelectionLine>)> GetSelectedLines();
}

internal class TextHighlighter : ITextHighlighter
{
    private readonly List<TextHighlight> _highlights = new();
    private readonly ITextSelectionLineConverter _textSelectionLineConverter;

    public event EventHandler? Changed;

    public IReadOnlyCollection<TextHighlight> Highlights => _highlights;

    public TextHighlighter(ITextSelectionLineConverter textSelectionLineConverter)
    {
        _textSelectionLineConverter = textSelectionLineConverter;
    }

    public void Add(IEnumerable<TextHighlight> highlights)
    {
        _highlights.AddRange(highlights);
        Changed?.Invoke(this, EventArgs.Empty);
    }

    public void Remove(IEnumerable<TextHighlight> highlights)
    {
        highlights.Each(x => _highlights.Remove(x));
        Changed?.Invoke(this, EventArgs.Empty);
    }

    public IEnumerable<(TextHighlight, IEnumerable<TextSelectionLine>)> GetSelectedLines()
    {
        foreach (var highlight in _highlights)
        {
            var selectedLines = _textSelectionLineConverter.GetSelectedLines(
                new(highlight.Position.StartLineIndex, highlight.Position.StartColumnIndex),
                new(highlight.Position.EndLineIndex, highlight.Position.EndColumnIndex));

            yield return (highlight, selectedLines);
        }
    }
}
