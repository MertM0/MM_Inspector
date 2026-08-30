using System;

namespace MM.Inspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class DropdownAttribute : MMAttribute
    {
        public string Source { get; }

        public DropdownAttribute(string source)
        {
            Source = source;
        }
    }
}
