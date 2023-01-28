using CodeHighlighter.Model;
using DependencyInjection;

namespace CodeHighlighter.Infrastructure;

internal class LineNumberPanelModelInjectModule : InjectModule
{
    public override void Init(IBindingProvider bindingProvider)
    {
        bindingProvider.Bind<ILineNumberGenerator, LineNumberGenerator>();
        bindingProvider.Bind<IExtendedLineNumberGenerator, ExtendedLineNumberGenerator>();
        bindingProvider.Bind<ILineNumberGapCollection, LineNumberGapCollection>();

        bindingProvider.Bind<ILineNumberPanelModel, LineNumberPanelModel>();
    }
}
