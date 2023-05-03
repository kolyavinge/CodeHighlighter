using CodeHighlighter.Ancillary;
using CodeHighlighter.Infrastructure;
using CodeHighlighter.Model;
using CodeHighlighter.Rendering;

namespace CodeHighlighter;

public static class RenderingModelFactory
{
    public static ICodeTextBoxRendering MakeModel(ICodeTextBox model, ICodeTextBoxRenderingContext renderingContext)
    {
        var container = new DependencyContainer();
        container.InitFromModules(new RenderingInjectModule());
        container.BindSingleton<ICodeTextBox>(model);
        container.BindSingleton<ILineGapCollection>(model.Gaps);
        container.BindSingleton<ILineFolds>(model.Folds);
        container.BindSingleton<ICodeTextBoxRenderingContext>(renderingContext);

        var renderingModel = container.Resolve<ICodeTextBoxRendering>();

        return renderingModel;
    }

    public static INumberRendering MakeNumberRendering(ILineNumberPanelRenderingContext context)
    {
        return new NumberRendering(context);
    }

    public static ILineFoldsRendering MakeLineFoldsRendering(ILineFoldingPanelRenderingContext context)
    {
        return new LineFoldsRendering(context);
    }
}
