using CodeHighlighter.InputActions;
using CodeHighlighter.Model;
using DependencyInjection;

namespace CodeHighlighter.Infrastructure;

internal class CodeTextBoxInjectModule : InjectModule
{
    public override void Init(IBindingProvider bindingProvider)
    {
        bindingProvider.Bind<IText, Text>().ToSingleton();
        bindingProvider.Bind<ITextCursor, TextCursor>().ToSingleton();
        bindingProvider.Bind<ITextCursorPositionCorrector, TextCursorPositionCorrector>().ToSingleton();
        bindingProvider.Bind<ITextCursorAbsolutePosition, TextCursorAbsolutePosition>().ToSingleton();
        bindingProvider.Bind<ITokens, Tokens>().ToSingleton();
        bindingProvider.Bind<ITokensColors, TokensColors>().ToSingleton();
        bindingProvider.Bind<ITextMeasuresInternal, TextMeasures>().ToSingleton();
        bindingProvider.Bind<IHistoryInternal, History>().ToSingleton();
        bindingProvider.Bind<ILinesDecorationCollection, LinesDecorationCollection>().ToSingleton();
        bindingProvider.Bind<ITextSelectionInternal, TextSelection>().ToSingleton();
        bindingProvider.Bind<ITextSelector, TextSelector>().ToSingleton();
        bindingProvider.Bind<IEditTextResultToLinesChangeConverter, EditTextResultToLinesChangeConverter>().ToSingleton();
        bindingProvider.Bind<ITextLinesChangingLogic, TextLinesChangingLogic>().ToSingleton();
        bindingProvider.Bind<ITextChangedEventArgsFactory, TextChangedEventArgsFactory>().ToSingleton();
        // IViewportContext не нужно добавлять в контейнер
        bindingProvider.Bind<IViewportInternal, Viewport>().ToSingleton();
        bindingProvider.Bind<IBracketsHighlighter, BracketsHighlighter>().ToSingleton();
        bindingProvider.Bind<ITextEventsInternal, TextEvents>().ToSingleton();
        bindingProvider.Bind<ITextMeasuresEvents, TextMeasuresEvents>().ToSingleton();
        bindingProvider.Bind<ILineGapCollection, LineGapCollection>().ToSingleton();
        bindingProvider.Bind<ILineFolds, LineFolds>().ToSingleton();
        bindingProvider.Bind<ILineFoldsUpdater, LineFoldsUpdater>().ToSingleton();
        bindingProvider.Bind<ILineNumberGenerator, LineNumberGenerator>().ToSingleton();
        bindingProvider.Bind<IExtendedLineNumberGenerator, ExtendedLineNumberGenerator>().ToSingleton();
        bindingProvider.Bind<IVerticalScrollBarMaximumValueStrategy, DefaultVerticalScrollBarMaximumValueStrategy>().ToSingleton();
        bindingProvider.Bind<IHorizontalScrollBarMaximumValueStrategy, DefaultHorizontalScrollBarMaximumValueStrategy>().ToSingleton();
        bindingProvider.Bind<IViewportVerticalOffsetUpdater, ViewportVerticalOffsetUpdater>().ToSingleton();
        bindingProvider.Bind<IViewportCursorPositionCorrector, ViewportCursorPositionCorrector>().ToSingleton();
        bindingProvider.Bind<IPageScroller, PageScroller>().ToSingleton();
        bindingProvider.Bind<ICodeTextBoxModelAdditionalInfo, CodeTextBoxModelAdditionalInfo>().ToSingleton();
        bindingProvider.Bind<IInputActionsFactory, InputActionsFactory>().ToSingleton();
        bindingProvider.Bind<IInputActionContext, InputActionContext>().ToSingleton();
        bindingProvider.Bind<IHistoryActionsFactory, HistoryActionsFactory>().ToSingleton();
        bindingProvider.Bind<ICodeTextBoxModel, CodeTextBoxModel>().ToSingleton();
    }
}
