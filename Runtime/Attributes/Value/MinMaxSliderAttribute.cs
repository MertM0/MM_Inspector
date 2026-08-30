using System;

namespace MM.Inspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class MinMaxSliderAttribute : MMRangeAttribute
    {
        public MinMaxSliderAttribute(float min, float max)
            : base(min, max)
        {
        }

        public MinMaxSliderAttribute(string minMember, string maxMember)
            : base(minMember, maxMember)
        {
        }

        public MinMaxSliderAttribute(float min, string maxMember)
            : base(min, maxMember)
        {
        }

        public MinMaxSliderAttribute(string minMember, float max)
            : base(minMember, max)
        {
        }
    }
}
