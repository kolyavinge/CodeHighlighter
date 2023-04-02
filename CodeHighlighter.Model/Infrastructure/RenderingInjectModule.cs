using CodeHighlighter.Ancillary;
using CodeHighlighter.Model;
using CodeHighlighter.Rendering;
using DependencyInjection;

namespace CodeHighlighter.Infrastructure;

internal class RenderingInjectModule : InjectModule
{
    public override void Init(IBindingProvider bindingProvider)
    {
        bindingProvider.Bind<ILineNumberGenerator, LineNumberGenerator>().ToSingleton();
        bindingProvider.Bind<IExtendedLineNumberGenerator, ExtendedLineNumberGenerator>().ToSingleton();
        bindingProvider.Bind<ILineFoldsNumberGenerator, LineFoldsNumberGenerator>().ToSingleton();
        bindingProvider.Bind<ITextSelectionRendering, TextSelectionRendering>().ToSingleton();
        bindingProvider.Bind<ITextSelectionRect, TextSelectionRect>().ToSingleton();

        bindingProvider.Bind<ITextRendering, TextRendering>().ToSingleton();
        bindingProvider.Bind<ILinesDecorationRendering, LinesDecorationRendering>().ToSingleton();
        bindingProvider.Bind<IHighlightBracketsRendering, HighlightBracketsRendering>().ToSingleton();
        bindingProvider.Bind<ILineGapRendering, LineGapRendering>().ToSingleton();
        bindingProvider.Bind<IActivatedLineFoldsRendering, ActivatedLineFoldsRendering>().ToSingleton();

        bindingProvider.Bind<ICodeTextBoxRenderingModel, CodeTextBoxRenderingModel>().ToSingleton();
    }
}
