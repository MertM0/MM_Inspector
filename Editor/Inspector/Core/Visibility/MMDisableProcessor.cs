namespace MM.Inspector.Editor
{
    public abstract class MMDisableProcessor
    {
        public abstract bool IsDisabled(MMProperty property, MMAttribute attribute);
    }

    public abstract class MMDisableProcessor<TAttribute> : MMDisableProcessor where TAttribute : MMAttribute
    {
        public sealed override bool IsDisabled(MMProperty property, MMAttribute attribute)
        {
            return IsDisabled(property, (TAttribute)attribute);
        }

        protected abstract bool IsDisabled(MMProperty property, TAttribute attribute);
    }
}
