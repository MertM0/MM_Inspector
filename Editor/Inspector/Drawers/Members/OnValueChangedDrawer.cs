namespace MM.Inspector.Editor
{
    internal sealed class OnValueChangedDrawer : MMAttributeDrawer<OnValueChangedAttribute>
    {
        public override int Order => MMDrawerOrder.Inspector;

        protected override MMElement CreateElement(MMProperty property, OnValueChangedAttribute attribute, MMElement next)
        {
            MMActionResolver resolver = MMActionResolver.Create(property.OwnerType, attribute.Method);
            return new OnValueChangedElement(property, next, resolver);
        }
    }
}
