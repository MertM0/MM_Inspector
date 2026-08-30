using System;

namespace MM.Inspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class FilePathAttribute : MMAttribute
    {
        public string Extensions { get; set; }
        public bool Absolute { get; set; }
    }
}
