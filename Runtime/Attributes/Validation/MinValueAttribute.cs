using System;

namespace MM.Inspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class MinValueAttribute : MMAttribute
    {
        public float Value { get; }
        public string Member { get; }

        public MinValueAttribute(float value)
        {
            Value = value;
        }

        public MinValueAttribute(string member)
        {
            Member = member;
        }
    }
}
