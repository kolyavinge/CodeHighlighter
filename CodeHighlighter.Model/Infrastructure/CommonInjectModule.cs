﻿using CodeHighlighter.Model;
using DependencyInjection;

namespace CodeHighlighter.Infrastructure;

internal class CommonInjectModule : InjectModule
{
    public override void Init(IBindingProvider bindingProvider)
    {
        bindingProvider.Bind<IText, Text>().ToSingleton();
        bindingProvider.Bind<ITextCursor, TextCursor>().ToSingleton();
        bindingProvider.Bind<ITokens, Tokens>().ToSingleton();
        bindingProvider.Bind<ITokensColors, TokensColors>().ToSingleton();
        bindingProvider.Bind<ITextMeasures, TextMeasures>().ToSingleton();
        bindingProvider.Bind<IHistoryInternal, History>().ToSingleton();
        bindingProvider.Bind<ILinesDecorationCollection, LinesDecorationCollection>().ToSingleton();
        bindingProvider.Bind<ITextSelectionInternal, TextSelection>().ToSingleton();
        bindingProvider.Bind<ITextSelector, TextSelector>().ToSingleton();
        bindingProvider.Bind<IViewportContext, DummyViewportContext>().ToSingleton();
        bindingProvider.Bind<IViewportInternal, Viewport>().ToSingleton();
        bindingProvider.Bind<IBracketsHighlighter, BracketsHighlighter>().ToSingleton();
        bindingProvider.Bind<ITextEvents, TextEvents>().ToSingleton();
        bindingProvider.Bind<ICodeTextBoxModel, CodeTextBoxModel>().ToSingleton();
    }
}
