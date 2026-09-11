using System;

namespace MM.Inspector
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
    public sealed class SearchableAttribute : MMAttribute
    {
    }
}
