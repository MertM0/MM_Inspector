using System;

namespace MM.Inspector
{
    public enum InfoBoxType
    {
        Info,
        Warning,
        Error
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class InfoBoxAttribute : MMAttribute
    {
        public string Message { get; }
        public InfoBoxType Type { get; }
        public string VisibleIf { get; set; }

        public InfoBoxAttribute(string message, InfoBoxType type = InfoBoxType.Info)
        {
            Message = message;
            Type = type;
        }
    }
}
