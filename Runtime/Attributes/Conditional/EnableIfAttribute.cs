using System;

namespace MM.Inspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class EnableIfAttribute : ConditionAttribute
    {
        public EnableIfAttribute(string member) : base(member)
        {
        }

        public EnableIfAttribute(string member, object value) : base(member, value)
        {
        }
    }
}
