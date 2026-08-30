namespace MM.Inspector.Editor
{
    internal sealed class ReadOnlyDisableProcessor : MMDisableProcessor<ReadOnlyAttribute>
    {
        protected override bool IsDisabled(MMProperty property, ReadOnlyAttribute attribute)
        {
            return true;
        }
    }
}
