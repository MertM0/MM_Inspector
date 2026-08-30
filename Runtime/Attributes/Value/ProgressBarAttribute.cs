using System;

namespace MM.Inspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ProgressBarAttribute : MMRangeAttribute
    {
        public string Label { get; set; }
        public MMColor Color { get; set; }
        public bool Editable { get; set; }

        public ProgressBarAttribute(float min, float max)
            : base(min, max)
        {
        }

        public ProgressBarAttribute(string minMember, string maxMember)
            : base(minMember, maxMember)
        {
        }

        public ProgressBarAttribute(float min, string maxMember)
            : base(min, maxMember)
        {
        }

        public ProgressBarAttribute(string minMember, float max)
            : base(minMember, max)
        {
        }
    }
}
