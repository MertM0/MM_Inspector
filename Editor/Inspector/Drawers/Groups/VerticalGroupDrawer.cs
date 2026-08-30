namespace MM.Inspector.Editor
{
    internal sealed class VerticalGroupDrawer : MMGroupDrawer<VerticalGroupAttribute>
    {
        protected override MMElement CreateElement(MMGroupContext context, VerticalGroupAttribute attribute)
        {
            return new VerticalGroupElement(context);
        }
    }
}
