using UnityEditor;

namespace MM.Inspector.Editor
{
    internal sealed class InlinePropertyDrawer : MMAttributeDrawer<InlinePropertyAttribute>
    {
        protected override string Validate(MMProperty property, InlinePropertyAttribute attribute)
        {
            return IsEmbeddedType(property.Serialized)
                ? null
                : MMPropertyRequirement.Name(attribute) + " needs a serializable type with members.";
        }

        private static bool IsEmbeddedType(SerializedProperty serialized)
        {
            return serialized != null &&
                   serialized.propertyType == SerializedPropertyType.Generic &&
                   !serialized.isArray &&
                   serialized.hasVisibleChildren;
        }

        protected override MMElement CreateElement(MMProperty property, InlinePropertyAttribute attribute, MMElement next)
        {
            return new InlinePropertyElement(property);
        }
    }
}
