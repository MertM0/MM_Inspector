namespace MM.Inspector.Editor
{
    public abstract class MMAttributeDrawer<TAttribute> : MMDrawer where TAttribute : MMAttribute
    {
        public sealed override string Validate(MMProperty property, MMAttribute attribute)
        {
            return Validate(property, (TAttribute)attribute);
        }

        public sealed override MMElement CreateElement(MMProperty property, MMAttribute attribute, MMElement next)
        {
            return CreateElement(property, (TAttribute)attribute, next);
        }

        protected virtual string Validate(MMProperty property, TAttribute attribute)
        {
            return null;
        }

        protected abstract MMElement CreateElement(MMProperty property, TAttribute attribute, MMElement next);
    }
}
