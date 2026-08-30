using System;

namespace MM.Inspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class BoxGroupAttribute : GroupAttribute
    {
        public BoxGroupAttribute(string path) : base(path)
        {
        }
    }
}
