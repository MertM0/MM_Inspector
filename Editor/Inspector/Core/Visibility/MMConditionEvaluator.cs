using System;

namespace MM.Inspector.Editor
{
    public static class MMConditionEvaluator
    {
        public static bool Evaluate(MMProperty property, ConditionAttribute attribute, out bool resolved)
        {
            resolved = true;

            Type ownerType = property.OwnerType;
            if (ownerType == null)
            {
                resolved = false;
                return false;
            }

            MMValueResolver<object> resolver = MMValueResolver<object>.Create(ownerType, attribute.Member);
            if (resolver.HasError)
            {
                resolved = false;
                return false;
            }

            object value = resolver.GetValue(property.Owner);

            if (attribute.HasValue)
            {
                return Equals(value, attribute.Value);
            }

            return value is bool flag && flag;
        }
    }
}
