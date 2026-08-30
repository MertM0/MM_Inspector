using System;

namespace MM.Inspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class HideIfAttribute : ConditionAttribute
    {
        public HideIfAttribute(string member) : base(member)
        {
        }

        public HideIfAttribute(string member, object value) : base(member, value)
        {
        }
    }
}
