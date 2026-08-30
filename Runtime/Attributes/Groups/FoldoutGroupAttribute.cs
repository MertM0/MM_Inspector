using System;

namespace MM.Inspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class FoldoutGroupAttribute : GroupAttribute
    {
        public FoldoutGroupAttribute(string path) : base(path)
        {
        }
    }
}
