using CodeHighlighter.Input;

namespace CodeHighlighter.Commands;

public class GotoLineCommandParameter
{
    public int LineIndex { get; }

    public GotoLineCommandParameter(int lineIndex)
    {
        LineIndex = lineIndex;
    }
}

internal class GotoLineCommand : InputCommand
{
    public GotoLineCommand(InputCommandContext context) : base(context) { }

    public override void Execute(object parameter)
    {
        var p = (GotoLineCommandParameter)parameter;
        var lineIndex = p.LineIndex < _context.Model.Text.LinesCount ? p.LineIndex : _context.Model.Text.LinesCount;
        _context.Model.MoveCursorTo(lineIndex, 0);
        var offsetLine = lineIndex - _context.Viewport.GetLinesCountInViewport() / 2;
        if (offsetLine < 0) offsetLine = 0;
        _context.ViewportContext.VerticalScrollBarValue = offsetLine * _context.TextMeasures.LineHeight;
        _context.TextBox.InvalidateVisual();
        _context.TextBox.Focus();
    }
}
