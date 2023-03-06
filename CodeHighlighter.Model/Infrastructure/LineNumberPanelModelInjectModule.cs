using CodeHighlighter.Model;
using DependencyInjection;

namespace CodeHighlighter.Infrastructure;

internal class LineNumberPanelModelInjectModule : InjectModule
{
    private readonly ICodeTextBoxModel? _codeTextBoxModel;

    public LineNumberPanelModelInjectModule(ICodeTextBoxModel? codeTextBoxModel)
    {
        _codeTextBoxModel = codeTextBoxModel;
    }

    public override void Init(IBindingProvider bindingProvider)
    {
        bindingProvider.Bind<ILineNumberGenerator, LineNumberGenerator>().ToSingleton();
        bindingProvider.Bind<IExtendedLineNumberGenerator, ExtendedLineNumberGenerator>().ToSingleton();
        bindingProvider.Bind<ILineGapCollection, LineGapCollection>().ToSingleton();
        bindingProvider.Bind<ILineNumberPanelModel, LineNumberPanelModel>().ToSingleton();

        if (_codeTextBoxModel != null)
        {
            bindingProvider.Bind<ILineFolds>().ToMethod(_ => _codeTextBoxModel.Folds);
        }
        else
        {
            bindingProvider.Bind<ILineFolds, LineFolds>().ToSingleton();
        }
    }
}
