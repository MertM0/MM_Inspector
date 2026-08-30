namespace MM.Inspector.Editor
{
    internal sealed class SceneDrawer : MMAttributeDrawer<SceneAttribute>
    {
        private const string EmptyError = "[Scene] found no enabled scene in Build Settings.";

        protected override string Validate(MMProperty property, SceneAttribute attribute)
        {
            return MMPickerElement.ValidateTarget(property, attribute);
        }

        protected override MMElement CreateElement(MMProperty property, SceneAttribute attribute, MMElement next)
        {
            return new MMCatalogPickerElement(property, () => MMSceneCatalog.Scenes, EmptyError);
        }
    }
}
