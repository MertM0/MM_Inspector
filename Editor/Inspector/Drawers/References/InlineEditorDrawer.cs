using UnityEditor;

namespace MM.Inspector.Editor
{
    internal sealed class InlineEditorDrawer : MMAttributeDrawer<InlineEditorAttribute>
    {
        protected override string Validate(MMProperty property, InlineEditorAttribute attribute)
        {
            return MMPropertyRequirement.Types(property, attribute, SerializedPropertyType.ObjectReference);
        }

        protected override MMElement CreateElement(MMProperty property, InlineEditorAttribute attribute, MMElement next)
        {
            return new InlineEditorElement(property);
        }
    }
}
