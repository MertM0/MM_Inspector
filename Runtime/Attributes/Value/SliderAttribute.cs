using System;

namespace MM.Inspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class SliderAttribute : MMRangeAttribute
    {
        public SliderAttribute(float min, float max)
            : base(min, max)
        {
        }

        public SliderAttribute(string minMember, string maxMember)
            : base(minMember, maxMember)
        {
        }

        public SliderAttribute(float min, string maxMember)
            : base(min, maxMember)
        {
        }

        public SliderAttribute(string minMember, float max)
            : base(minMember, max)
        {
        }
    }
}
