namespace MM.Inspector.Editor
{
    public abstract class MMValidator
    {
        public abstract MMValidationResult Validate(MMProperty property, MMAttribute attribute);
    }

    public abstract class MMValidator<TAttribute> : MMValidator where TAttribute : MMAttribute
    {
        public sealed override MMValidationResult Validate(MMProperty property, MMAttribute attribute)
        {
            return Validate(property, (TAttribute)attribute);
        }

        protected abstract MMValidationResult Validate(MMProperty property, TAttribute attribute);
    }
}
