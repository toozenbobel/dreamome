using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Common.Helpers;

[ExcludeFromCodeCoverage]
public class DateTimeProviderContext : IDisposable
{
    internal readonly Func<DateTime> ContextDateTimeNow;
    private static readonly ThreadLocal<Stack> ThreadScopeStack = new(() => new Stack());

    public DateTimeProviderContext(Func<DateTime> contextDateTimeNow)
    {
        ContextDateTimeNow = contextDateTimeNow;
        ThreadScopeStack.Value?.Push(this);
    }

    public static DateTimeProviderContext? Current
    {
        get
        {
            if (ThreadScopeStack.Value is { Count: 0 })
                return null;

            return ThreadScopeStack.Value?.Peek() as DateTimeProviderContext;
        }
    }

    public void Dispose()
    {
        ThreadScopeStack.Value?.Pop();
    }
}