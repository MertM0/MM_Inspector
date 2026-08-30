namespace MM.Inspector.Editor
{
    internal sealed class TitleDrawer : MMAttributeDrawer<TitleAttribute>
    {
        public override int Order => MMDrawerOrder.Decorator;

        public override bool RequiresSerializedProperty => false;

        protected override MMElement CreateElement(MMProperty property, TitleAttribute attribute, MMElement next)
        {
            return new TitleElement(property, next, Resolve(property, attribute.Text), attribute.Line);
        }

        private static string Resolve(MMProperty property, string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            MMValueResolver<string> resolver = MMValueResolver<string>.Create(property.OwnerType, text);

            return resolver.HasError ? text : resolver.GetValue(property);
        }
    }
}
