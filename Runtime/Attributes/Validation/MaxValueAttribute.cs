using System;

namespace MM.Inspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class MaxValueAttribute : MMAttribute
    {
        public float Value { get; }
        public string Member { get; }

        public MaxValueAttribute(float value)
        {
            Value = value;
        }

        public MaxValueAttribute(string member)
        {
            Member = member;
        }
    }
}
