using System;

namespace MM.Inspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class DisableIfAttribute : ConditionAttribute
    {
        public DisableIfAttribute(string member) : base(member)
        {
        }

        public DisableIfAttribute(string member, object value) : base(member, value)
        {
        }
    }
}
