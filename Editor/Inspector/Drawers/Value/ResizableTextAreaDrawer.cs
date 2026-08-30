using UnityEditor;

namespace MM.Inspector.Editor
{
    internal sealed class ResizableTextAreaDrawer : MMAttributeDrawer<ResizableTextAreaAttribute>
    {
        protected override string Validate(MMProperty property, ResizableTextAreaAttribute attribute)
        {
            return MMPropertyRequirement.Types(property, attribute, SerializedPropertyType.String);
        }

        protected override MMElement CreateElement(MMProperty property, ResizableTextAreaAttribute attribute, MMElement next)
        {
            return new ResizableTextAreaElement(property, attribute.MinLines);
        }
    }
}
