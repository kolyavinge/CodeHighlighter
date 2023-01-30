using CodeHighlighter.Model;
using DependencyInjection;

namespace CodeHighlighter.Infrastructure;

internal class LineNumberPanelModelInjectModule : InjectModule
{
    public override void Init(IBindingProvider bindingProvider)
    {
        bindingProvider.Bind<ILineNumberGenerator, LineNumberGenerator>().ToSingleton();
        bindingProvider.Bind<IExtendedLineNumberGenerator, ExtendedLineNumberGenerator>().ToSingleton();
        bindingProvider.Bind<ILineGapCollection, LineGapCollection>().ToSingleton();

        bindingProvider.Bind<ILineNumberPanelModel, LineNumberPanelModel>().ToSingleton();
    }
}
