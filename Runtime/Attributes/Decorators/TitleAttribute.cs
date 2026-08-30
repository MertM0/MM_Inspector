using System;

namespace MM.Inspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class TitleAttribute : MMAttribute
    {
        public string Text { get; }
        public bool Line { get; set; } = true;

        public TitleAttribute(string text = null)
        {
            Text = text;
        }
    }
}
