using UnityEditor;

namespace MM.Inspector.Editor
{
    internal sealed class FilePathDrawer : MMAttributeDrawer<FilePathAttribute>
    {
        protected override string Validate(MMProperty property, FilePathAttribute attribute)
        {
            return MMPropertyRequirement.Types(property, attribute, SerializedPropertyType.String);
        }

        protected override MMElement CreateElement(MMProperty property, FilePathAttribute attribute, MMElement next)
        {
            return new FilePathElement(property, attribute.Absolute, attribute.Extensions);
        }
    }
}
