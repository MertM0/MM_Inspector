using System;

namespace MM.Inspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class VerticalGroupAttribute : GroupAttribute
    {
        public VerticalGroupAttribute(string path) : base(path)
        {
        }
    }
}
