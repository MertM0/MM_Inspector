using UnityEditor;

namespace MM.Inspector.Editor
{
    internal sealed class MaxValueDrawer : MMAttributeDrawer<MaxValueAttribute>
    {
        public override int Order => MMDrawerOrder.Validator;

        protected override string Validate(MMProperty property, MaxValueAttribute attribute)
        {
            return MMPropertyRequirement.Types(property, attribute,
                SerializedPropertyType.Integer, SerializedPropertyType.Float);
        }

        protected override MMElement CreateElement(MMProperty property, MaxValueAttribute attribute, MMElement next)
        {
            return new NumericClampElement(
                property, next, null, new MMBound(attribute.Value, attribute.Member, property.OwnerType));
        }
    }
}
