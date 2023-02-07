using CodeHighlighter.Controllers;
using CodeHighlighter.Model;

namespace CodeHighlighter;

public static class ControllerFactory
{
    public static IKeyboardController MakeKeyboardController(ICodeTextBoxModel model)
    {
        return new KeyboardController(model);
    }

    public static IMouseController MakeMouseController(ICodeTextBox codeTextBox, ICodeTextBoxModel model)
    {
        return new MouseController(
            codeTextBox,
            model,
            new PointInTextSelection(model.TextSelection),
            new MouseCursorPosition(model.Viewport, model.TextMeasures, new ExtendedLineNumberGenerator(new LineNumberGenerator(), model.Gaps)));
    }
}
