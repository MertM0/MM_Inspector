using System;

namespace MM.Inspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method,
        AllowMultiple = false, Inherited = true)]
    public sealed class EndGroupAttribute : MMAttribute
    {
    }
}
