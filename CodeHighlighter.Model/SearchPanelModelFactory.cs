using CodeHighlighter.Ancillary;
using CodeHighlighter.Core;
using CodeHighlighter.Infrastructure;
using CodeHighlighter.Model;

namespace CodeHighlighter;

public static class SearchPanelModelFactory
{
    public static ISearchPanelModel MakeModel(ICodeTextBoxModel codeTextBoxModel)
    {
        var container = new DependencyContainer();
        container.BindSingleton<ITextSearchLogic, TextSearchLogic>();
        container.BindSingleton<IRegexSearchLogic, RegexSearchLogic>();
        container.BindSingleton<ISearchPanelModel, SearchPanelModel>();
        container.BindSingleton<ICodeTextBoxModel>(codeTextBoxModel);
        container.BindSingleton<ITextLines>(codeTextBoxModel.TextLines);

        var model = container.Resolve<ISearchPanelModel>();

        return model;
    }
}
