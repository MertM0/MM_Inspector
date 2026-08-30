using UnityEditor;

namespace MM.Inspector.Editor
{
    internal sealed class ProgressBarDrawer : MMAttributeDrawer<ProgressBarAttribute>
    {
        protected override string Validate(MMProperty property, ProgressBarAttribute attribute)
        {
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
