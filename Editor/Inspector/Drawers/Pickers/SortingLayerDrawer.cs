namespace MM.Inspector.Editor
{
    internal sealed class SortingLayerDrawer : MMAttributeDrawer<SortingLayerAttribute>
    {
        protected override string Validate(MMProperty property, SortingLayerAttribute attribute)
        {
            return MMPickerElement.ValidateTarget(property, attribute);
        }

        protected override MMElement CreateElement(MMProperty property, SortingLayerAttribute attribute, MMElement next)
        {
            return new MMCatalogPickerElement(property, () => MMSortingLayerCatalog.Layers);
        }
    }
}
