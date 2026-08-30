using UnityEditor;

namespace MM.Inspector.Editor
{
    internal sealed class MinValueDrawer : MMAttributeDrawer<MinValueAttribute>
    {
        public override int Order => MMDrawerOrder.Validator;

        protected override string Validate(MMProperty property, MinValueAttribute attribute)
        {
            return MMPropertyRequirement.Types(property, attribute,
                SerializedPropertyType.Integer, SerializedPropertyType.Float);
        }

        protected override MMElement CreateElement(MMProperty property, MinValueAttribute attribute, MMElement next)
        {
            return new NumericClampElement(
                property, next, new MMBound(attribute.Value, attribute.Member, property.OwnerType), null);
        }
    }
}
