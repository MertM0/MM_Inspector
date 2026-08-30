using System;

namespace MM.Inspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class RequiredAttribute : MMAttribute
    {
        public string Message { get; }

        public RequiredAttribute(string message = null)
        {
            Message = message;
        }
    }
}
