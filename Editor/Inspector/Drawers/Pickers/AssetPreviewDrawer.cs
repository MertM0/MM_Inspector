using UnityEditor;

namespace MM.Inspector.Editor
{
    internal sealed class AssetPreviewDrawer : MMAttributeDrawer<AssetPreviewAttribute>
    {
        public override int Order => MMDrawerOrder.Decorator;

        protected override string Validate(MMProperty property, AssetPreviewAttribute attribute)
        {
            return MMPropertyRequirement.Types(property, attribute, SerializedPropertyType.ObjectReference);
        }

        protected override MMElement CreateElement(MMProperty property, AssetPreviewAttribute attribute, MMElement next)
        {
            return new AssetPreviewElement(property, next, attribute.Size);
        }
    }
}
