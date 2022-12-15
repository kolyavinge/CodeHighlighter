using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CodeHighlighter.Infrastructure;
using CodeHighlighter.InputActions;

namespace CodeHighlighter.Tests.Model;

internal class TestInputActionsFactory : IInputActionsFactory
{
    private readonly Dictionary<Type, object> _actions = new();

    public TestInputActionsFactory()
    {
        var actionTypes = Assembly
            .GetAssembly(typeof(IInputActionsFactory))
            .GetTypes()
            .Where(type => type.IsClass && type.GetCustomAttribute<InputActionAttribute>() != null);

        foreach (var actionType in actionTypes)
        {
            _actions.Add(actionType.GetInterfaces().First(), Activator.CreateInstance(actionType));
        }
    }

    public TInputAction Get<TInputAction>()
    {
        return (TInputAction)_actions[typeof(TInputAction)];
    }
}
