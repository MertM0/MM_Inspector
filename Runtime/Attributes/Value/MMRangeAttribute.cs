using System;

namespace MM.Inspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class MMRangeAttribute : MMAttribute
    {
        public float Min { get; }
        public float Max { get; }
        public string MinMember { get; }
        public string MaxMember { get; }

        protected MMRangeAttribute(float min, float max)
        {
            Min = min;
            Max = max;
        }

        protected MMRangeAttribute(string minMember, string maxMember)
        {
            MinMember = minMember;
            MaxMember = maxMember;
        }

        protected MMRangeAttribute(float min, string maxMember)
        {
            Min = min;
            MaxMember = maxMember;
        }

        protected MMRangeAttribute(string minMember, float max)
        {
            MinMember = minMember;
            Max = max;
        }
    }
}
