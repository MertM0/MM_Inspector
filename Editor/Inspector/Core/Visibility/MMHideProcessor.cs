namespace MM.Inspector.Editor
{
    public abstract class MMHideProcessor
    {
        public abstract bool IsHidden(MMProperty property, MMAttribute attribute);
    }

    public abstract class MMHideProcessor<TAttribute> : MMHideProcessor where TAttribute : MMAttribute
    {
        public sealed override bool IsHidden(MMProperty property, MMAttribute attribute)
        {
            return IsHidden(property, (TAttribute)attribute);
        }

        protected abstract bool IsHidden(MMProperty property, TAttribute attribute);
    }
}
