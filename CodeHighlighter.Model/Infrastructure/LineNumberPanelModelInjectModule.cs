using CodeHighlighter.Ancillary;
using CodeHighlighter.Core;
using CodeHighlighter.Model;
using DependencyInjection;

namespace CodeHighlighter.Infrastructure;

internal class LineNumberPanelModelInjectModule : InjectModule
{
    private readonly ICodeTextBox? _codeTextBoxModel;

    public LineNumberPanelModelInjectModule(ICodeTextBox? codeTextBoxModel)
    {
        _codeTextBoxModel = codeTextBoxModel;
    }

    public override void Init(IBindingProvider bindingProvider)
    {
        bindingProvider.Bind<ILineNumberGenerator, LineNumberGenerator>().ToSingleton();
        bindingProvider.Bind<IExtendedLineNumberGenerator, ExtendedLineNumberGenerator>().ToSingleton();
        bindingProvider.Bind<ILineGapCollection, LineGapCollection>().ToSingleton();
        bindingProvider.Bind<ILineNumberPanel, LineNumberPanel>().ToSingleton();

        if (_codeTextBoxModel is not null)
        {
            bindingProvider.Bind<ILineFolds>().ToMethod(_ => _codeTextBoxModel.Folds);
        }
        else
        {
            bindingProvider.Bind<ILineFolds, LineFolds>().ToSingleton();
        }
    }
}
