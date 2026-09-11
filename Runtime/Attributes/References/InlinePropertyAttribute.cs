using System;

namespace MM.Inspector
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class InlinePropertyAttribute : MMAttribute
    {
    }
}
