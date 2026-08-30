namespace MM.Inspector.Editor
{
    internal sealed class SeparatorDrawer : MMAttributeDrawer<SeparatorAttribute>
    {
        public override int Order => MMDrawerOrder.Decorator;

        public override bool RequiresSerializedProperty => false;

        protected override MMElement CreateElement(MMProperty property, SeparatorAttribute attribute, MMElement next)
        {
            return new SeparatorElement(property, next, attribute.Space);
        }
    }
}
