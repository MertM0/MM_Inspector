using System;

namespace MM.Inspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ResizableTextAreaAttribute : MMAttribute
    {
        public int MinLines { get; set; } = 3;
    }
}
