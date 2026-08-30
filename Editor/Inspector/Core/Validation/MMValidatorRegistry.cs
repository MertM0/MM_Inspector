using System.Collections.Generic;

namespace MM.Inspector.Editor
{
    public static class MMValidatorRegistry
    {
        private static MMHandlerMap<MMValidator> _validators;

        public static MMValidator GetValidator(MMAttribute attribute)
        {
            _validators ??= new MMHandlerMap<MMValidator>(typeof(MMValidator<>));

            return _validators.Get(attribute);
        }

        public static bool HasValidators(MMProperty property)
        {
            return property.Schema != null && property.Schema.Validators.Length > 0;
        }

        public static void Collect(MMProperty property, List<MMValidationResult> results)
        {
            results.Clear();

            MMMemberSchema schema = property.Schema;
            if (schema == null)
            {
                return;
            }

            for (int i = 0; i < schema.Validators.Length; i++)
            {
                (MMAttribute attribute, MMValidator validator) = schema.Validators[i];

                MMValidationResult result = validator.Validate(property, attribute);
                if (!result.IsValid)
                {
                    results.Add(result);
                }
            }
        }
    }
}
