using System.Linq;
using CodeHighlighter.Common;
using CodeHighlighter.Model;

namespace CodeHighlighter.Controllers;

public interface ILineFoldingPanelMouseController
{
    void LeftButtonDown(Point positionInControl);
}

internal class LineFoldingPanelMouseController : ILineFoldingPanelMouseController
{
    private readonly ILineFoldingPanelView _panel;
    private readonly ILineFoldingPanel _model;

    public LineFoldingPanelMouseController(
        ILineFoldingPanelView panel,
        ILineFoldingPanel model)
    {
        _panel = panel;
        _model = model;
    }

    public void LeftButtonDown(Point positionInControl)
    {
        var fold = _model
            .GetFolds(_panel.ActualHeight, _panel.VerticalScrollBarValue, _panel.TextLineHeight, _panel.TextLinesCount)
            .Where(x => x.OffsetY <= positionInControl.Y && positionInControl.Y <= x.OffsetY + _panel.TextLineHeight)
            .FirstOrDefault();

        if (!fold.Equals(default))
        {
            _model.Folds.Switch(fold.LineIndex);
        }
    }
}
