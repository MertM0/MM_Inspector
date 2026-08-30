namespace MM.Inspector.Editor
{
    public abstract class MMGroupDrawer
    {
        public abstract MMElement CreateElement(MMGroupContext context, GroupAttribute attribute);
    }

    public abstract class MMGroupDrawer<TAttribute> : MMGroupDrawer where TAttribute : GroupAttribute
    {
        public sealed override MMElement CreateElement(MMGroupContext context, GroupAttribute attribute)
        {
            return CreateElement(context, (TAttribute)attribute);
        }

        protected abstract MMElement CreateElement(MMGroupContext context, TAttribute attribute);
    }
}
