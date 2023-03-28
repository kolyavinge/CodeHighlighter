using CodeHighlighter.Model;

namespace CodeHighlighter;

public static class LineFoldingPanelModelFactory
{
    public static ILineFoldingPanelModel MakeModel(ICodeTextBoxModel codeTextBoxModel)
    {
        return new LineFoldingPanelModel(
            codeTextBoxModel.Folds,
            new LineFoldsNumberGenerator(
                codeTextBoxModel.Folds,
                new ExtendedLineNumberGenerator(new LineNumberGenerator(), codeTextBoxModel.Gaps, codeTextBoxModel.Folds)));
    }
}
