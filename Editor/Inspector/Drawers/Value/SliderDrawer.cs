using UnityEditor;

namespace MM.Inspector.Editor
{
    internal sealed class SliderDrawer : MMAttributeDrawer<SliderAttribute>
    {
        protected override string Validate(MMProperty property, SliderAttribute attribute)
        {
            return MMPropertyRequirement.Types(property, attribute,
                SerializedPropertyType.Integer, SerializedPropertyType.Float);
        }

        protected override MMElement CreateElement(MMProperty property, SliderAttribute attribute, MMElement next)
        {
            return new SliderElement(property, new MMRangeBounds(attribute, property.OwnerType));
        }
    }
}
