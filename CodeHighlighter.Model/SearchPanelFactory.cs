using CodeHighlighter.Ancillary;
using CodeHighlighter.Core;
using CodeHighlighter.Infrastructure;
using CodeHighlighter.Model;

namespace CodeHighlighter;

public static class SearchPanelFactory
{
    public static ISearchPanel MakeModel(ICodeTextBox codeTextBoxModel)
    {
        var container = new DependencyContainer();
        container.BindSingleton<ITextSearchLogic, TextSearchLogic>();
        container.BindSingleton<IRegexSearchLogic, RegexSearchLogic>();
        container.BindSingleton<IWholeWordLogic, WholeWordLogic>();
        container.BindSingleton<ISearchPanel, SearchPanel>();
        container.BindSingleton<ITextPositionNavigatorInternal, TextPositionNavigator>();
        container.BindSingleton<ICodeTextBox>(codeTextBoxModel);
        container.BindSingleton<ITextLines>(codeTextBoxModel.TextLines);

        var model = container.Resolve<ISearchPanel>();

        return model;
    }
}
