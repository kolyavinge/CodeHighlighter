using CodeHighlighter.Controllers;
using CodeHighlighter.Core;
using CodeHighlighter.Model;

namespace CodeHighlighter;

public static class ControllerFactory
{
    public static IKeyboardController MakeKeyboardController(ICodeTextBox model)
    {
        return new KeyboardController(model);
    }

    public static IMouseController MakeMouseController(ICodeTextBoxView codeTextBox, ICodeTextBox model)
    {
        return new MouseController(
            codeTextBox,
            model,
            new PointInTextSelection(model.TextSelection),
            new MouseCursorPosition(model.Viewport, model.TextMeasures, new ExtendedLineNumberGenerator(new LineNumberGenerator(), model.Gaps, model.Folds)));
    }

    public static ILineFoldingPanelMouseController MakeMouseController(ILineFoldingPanelView panel, ILineFoldingPanel model)
    {
        return new LineFoldingPanelMouseController(panel, model);
    }
}
