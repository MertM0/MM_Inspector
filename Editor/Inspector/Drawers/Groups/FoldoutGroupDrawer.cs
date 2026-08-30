namespace MM.Inspector.Editor
{
    internal sealed class FoldoutGroupDrawer : MMGroupDrawer<FoldoutGroupAttribute>
    {
        protected override MMElement CreateElement(MMGroupContext context, FoldoutGroupAttribute attribute)
        {
            return new FoldoutGroupElement(context);
        }
    }
}
