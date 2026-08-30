using System;

namespace MM.Inspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class SeparatorAttribute : MMAttribute
    {
        public float Space { get; }

        public SeparatorAttribute(float space = 8f)
        {
            Space = space;
        }
    }
}
