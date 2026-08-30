using System;

namespace MM.Inspector.Editor
{
    public sealed class MMRangeBounds
    {
        private readonly MMBound _min;
        private readonly MMBound _max;

        public MMRangeBounds(MMRangeAttribute attribute, Type ownerType)
        {
            _min = new MMBound(attribute.Min, attribute.MinMember, ownerType);
            _max = new MMBound(attribute.Max, attribute.MaxMember, ownerType);
        }

        public float GetMin(MMProperty property)
        {
            return _min.GetValue(property);
        }

        public float GetMax(MMProperty property)
        {
            return _max.GetValue(property);
        }
    }
}
