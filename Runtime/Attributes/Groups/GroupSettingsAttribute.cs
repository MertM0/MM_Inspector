using System;

namespace MM.Inspector
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true, Inherited = true)]
    public sealed class GroupSettingsAttribute : MMAttribute
    {
        public string Path { get; }

        public string Title { get; set; }

        public bool Expanded { get; set; }

        public GroupSettingsAttribute(string path)
        {
            Path = path;
        }
    }
}
