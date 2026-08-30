namespace MM.Inspector.Editor
{
    internal sealed class DropdownDrawer : MMAttributeDrawer<DropdownAttribute>
    {
        protected override MMElement CreateElement(MMProperty property, DropdownAttribute attribute, MMElement next)
        {
            MMValueResolver<object> resolver = MMValueResolver<object>.Create(property.OwnerType, attribute.Source);
            return new DropdownElement(property, resolver, attribute.Source);
        }
    }
}
