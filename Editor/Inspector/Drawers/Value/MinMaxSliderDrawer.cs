using UnityEditor;

namespace MM.Inspector.Editor
{
    internal sealed class MinMaxSliderDrawer : MMAttributeDrawer<MinMaxSliderAttribute>
    {
        protected override string Validate(MMProperty property, MinMaxSliderAttribute attribute)
        {
            return MMPropertyRequirement.Types(property, attribute,
                SerializedPropertyType.Vector2, SerializedPropertyType.Vector2Int);
        }

        protected override MMElement CreateElement(MMProperty property, MinMaxSliderAttribute attribute, MMElement next)
        {
            return new MinMaxSliderElement(property, new MMRangeBounds(attribute, property.OwnerType));
        }
    }
}
