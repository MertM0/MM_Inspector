using System;

namespace MM.Inspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class OnValueChangedAttribute : MMAttribute
    {
        public string Method { get; }

        public OnValueChangedAttribute(string method)
        {
            Method = method;
        }
    }
}
