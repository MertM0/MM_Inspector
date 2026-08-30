namespace MM.Inspector.Editor
{
    public abstract class MMDrawer
    {
        public virtual int Order => MMDrawerOrder.Drawer;

        public virtual bool RequiresSerializedProperty => true;

        public virtual bool AppliesToCollectionElements => Order == MMDrawerOrder.Drawer;

        public virtual string Validate(MMProperty property, MMAttribute attribute)
        {
            return null;
        }

        public abstract MMElement CreateElement(MMProperty property, MMAttribute attribute, MMElement next);
    }
}
