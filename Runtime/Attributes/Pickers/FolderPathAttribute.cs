using System;

namespace MM.Inspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class FolderPathAttribute : MMAttribute
    {
        public bool Absolute { get; set; }
    }
}
