namespace MM.Inspector.Editor
{
    internal sealed class HorizontalGroupDrawer : MMGroupDrawer<HorizontalGroupAttribute>
    {
        protected override MMElement CreateElement(MMGroupContext context, HorizontalGroupAttribute attribute)
        {
            return new HorizontalGroupElement(context);
        }
    }
}
