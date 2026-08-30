using System;

namespace MM.Inspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class ShowIfAttribute : ConditionAttribute
    {
        public ShowIfAttribute(string member) : base(member)
        {
        }

        public ShowIfAttribute(string member, object value) : base(member, value)
        {
        }
    }
}
