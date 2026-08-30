using System;

namespace MM.Inspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class TabGroupAttribute : GroupAttribute
    {
        public string TabName { get; }

        public override string EffectivePath => $"{Path}/{TabName}";

        public TabGroupAttribute(string path, string tabName) : base(path)
        {
            TabName = tabName;
        }
    }
}
