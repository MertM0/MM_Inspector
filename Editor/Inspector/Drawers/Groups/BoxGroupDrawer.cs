namespace MM.Inspector.Editor
{
    internal sealed class BoxGroupDrawer : MMGroupDrawer<BoxGroupAttribute>
    {
        protected override MMElement CreateElement(MMGroupContext context, BoxGroupAttribute attribute)
        {
            return new BoxGroupElement(context);
        }
    }
}
