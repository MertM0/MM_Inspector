using System;

namespace MM.Inspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public sealed class ValidateInputAttribute : MMAttribute
    {
        public string Member { get; }
        public string Message { get; }

        public ValidateInputAttribute(string member, string message = null)
        {
            Member = member;
            Message = message;
        }
    }
}
