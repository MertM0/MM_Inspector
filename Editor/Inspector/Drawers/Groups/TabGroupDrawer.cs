namespace MM.Inspector.Editor
{
    internal sealed class TabGroupDrawer : MMGroupDrawer<TabGroupAttribute>
    {
        protected override MMElement CreateElement(MMGroupContext context, TabGroupAttribute attribute)
        {
            return new TabGroupElement(context);
        }
    }
}
