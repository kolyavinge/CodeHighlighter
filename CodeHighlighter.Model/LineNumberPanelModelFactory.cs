using CodeHighlighter.Infrastructure;
using CodeHighlighter.Model;

namespace CodeHighlighter;

public static class LineNumberPanelModelFactory
{
    public static ILineNumberPanelModel MakeModel()
    {
        var container = new DependencyContainer();
        container.InitFromModules(new LineNumberPanelModelInjectModule());

        var model = container.Resolve<ILineNumberPanelModel>();

        return model;
    }
}
