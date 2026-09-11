using System.Collections.Generic;
using UnityEditor;

namespace MM.Inspector.Editor
{
    public static class MMNestedBody
    {
        public static MMElement For(MMProperty property)
        {
            Dictionary<string, MMProperty> byName = new Dictionary<string, MMProperty>();

            foreach (MMProperty child in property.Children)
            {
                byName[child.Name] = child;
            }

            MMElement body = MMGroupRegistry.BuildElement(
                MMTypeSchema.Get(property.ValueType).Groups,
                name => byName.TryGetValue(name, out MMProperty found) ? found : null,
                Owner(property));

            return MMSearchElement.Wrap(property.ValueType, property.Children, body);
        }

        private static MMObjectKey Owner(MMProperty property)
        {
            SerializedProperty serialized = property.Serialized;

            return new MMObjectKey(serialized.serializedObject.targetObject, serialized.propertyPath);
        }
    }
}
