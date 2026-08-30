namespace MM.Inspector.Editor
{
    internal sealed class ShowDrawerChainDrawer : MMAttributeDrawer<ShowDrawerChainAttribute>
    {
        public override int Order => MMDrawerOrder.System;

        public override bool RequiresSerializedProperty => false;

        protected override MMElement CreateElement(MMProperty property, ShowDrawerChainAttribute attribute, MMElement next)
        {
            return new DrawerChainElement(property, next);
        }
    }
}
