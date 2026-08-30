using UnityEditor;

namespace MM.Inspector.Editor
{
    internal sealed class RequiredValidator : MMValidator<RequiredAttribute>
    {
        protected override MMValidationResult Validate(MMProperty property, RequiredAttribute attribute)
        {
            SerializedProperty serialized = property.Serialized;
            if (serialized == null)
            {
                return MMValidationResult.Valid;
            }

            if (!IsEmpty(serialized))
            {
                return MMValidationResult.Valid;
            }

            string message = string.IsNullOrEmpty(attribute.Message)
                ? $"{property.DisplayName} is required."
                : attribute.Message;

            return MMValidationResult.Error(message);
        }

        private static bool IsEmpty(SerializedProperty serialized)
        {
            switch (serialized.propertyType)
            {
                case SerializedPropertyType.ObjectReference:
                    return serialized.objectReferenceValue == null;
                case SerializedPropertyType.String:
                    return string.IsNullOrEmpty(serialized.stringValue);
                case SerializedPropertyType.ExposedReference:
                    return serialized.exposedReferenceValue == null;
                case SerializedPropertyType.ManagedReference:
                    return serialized.managedReferenceValue == null;
                default:
                    return false;
            }
        }
    }
}
