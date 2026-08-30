namespace MM.Inspector.Editor
{
    internal sealed class ValidateInputValidator : MMValidator<ValidateInputAttribute>
    {
        protected override MMValidationResult Validate(MMProperty property, ValidateInputAttribute attribute)
        {
            System.Type ownerType = property.OwnerType;
            if (ownerType == null)
            {
                return MMValidationResult.Valid;
            }

            MMValueResolver<bool> resolver = MMValueResolver<bool>.Create(ownerType, attribute.Member);
            if (resolver.HasError)
            {
                return MMValidationResult.Warning(resolver.ErrorMessage);
            }

            if (resolver.GetValue(property.Owner))
            {
                return MMValidationResult.Valid;
            }

            string message = string.IsNullOrEmpty(attribute.Message)
                ? $"{property.DisplayName} is not valid."
                : attribute.Message;

            return MMValidationResult.Error(message);
        }
    }
}
