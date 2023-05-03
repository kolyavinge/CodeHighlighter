using CodeHighlighter.Ancillary;
using CodeHighlighter.Core;
using CodeHighlighter.Model;

namespace CodeHighlighter;

public static class LineFoldingPanelFactory
{
    public static ILineFoldingPanel MakeModel(ICodeTextBox codeTextBoxModel)
    {
        return new LineFoldingPanel(
            codeTextBoxModel.Folds,
            new LineFoldsNumberGenerator(
                codeTextBoxModel.Folds,
                new ExtendedLineNumberGenerator(new LineNumberGenerator(), codeTextBoxModel.Gaps, codeTextBoxModel.Folds)));
    }
}
