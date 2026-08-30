using UnityEditor;

namespace MM.Inspector.Editor
{
    internal sealed class FolderPathDrawer : MMAttributeDrawer<FolderPathAttribute>
    {
        protected override string Validate(MMProperty property, FolderPathAttribute attribute)
        {
            return MMPropertyRequirement.Types(property, attribute, SerializedPropertyType.String);
        }

        protected override MMElement CreateElement(MMProperty property, FolderPathAttribute attribute, MMElement next)
        {
            return new FolderPathElement(property, attribute.Absolute);
        }
    }
}
