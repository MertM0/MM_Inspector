using System;

namespace MM.Inspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class HorizontalGroupAttribute : GroupAttribute
    {
        public HorizontalGroupAttribute(string path) : base(path)
        {
        }
    }
}
