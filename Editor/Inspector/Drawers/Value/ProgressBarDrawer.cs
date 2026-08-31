using UnityEditor;

namespace MM.Inspector.Editor
{
    internal sealed class ProgressBarDrawer : MMAttributeDrawer<ProgressBarAttribute>
    {
        public override bool RequiresSerializedProperty => false;

        protected override string Validate(MMProperty property, ProgressBarAttribute attribute)
        {
            if (property.Serialized == null)
            {
                return property.ValueType == typeof(int) || property.ValueType == typeof(float)
                    ? null
                    : MMPropertyRequirement.Name(attribute) + " needs an int or float member.";
            }

            return MMPropertyRequirement.Types(property, attribute,
                SerializedPropertyType.Integer, SerializedPropertyType.Float);
        }

        protected override MMElement CreateElement(MMProperty property, ProgressBarAttribute attribute, MMElement next)
        {
            MMRangeBounds bounds = new MMRangeBounds(attribute, property.OwnerType);
            return new ProgressBarElement(property, bounds, attribute.Label, attribute.Color, attribute.Editable);
        }
    }
}
