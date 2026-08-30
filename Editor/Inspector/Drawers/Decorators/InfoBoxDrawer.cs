namespace MM.Inspector.Editor
{
    internal sealed class InfoBoxDrawer : MMAttributeDrawer<InfoBoxAttribute>
    {
        public override int Order => MMDrawerOrder.Decorator;

        public override bool RequiresSerializedProperty => false;

        protected override MMElement CreateElement(MMProperty property, InfoBoxAttribute attribute, MMElement next)
        {
            return new InfoBoxElement(property, next, attribute.Message, attribute.Type, attribute.VisibleIf);
        }
    }
}
