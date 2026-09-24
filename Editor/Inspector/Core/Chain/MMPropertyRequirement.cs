using System;
using System.Text;
using UnityEditor;

namespace MM.Inspector.Editor
{
    public static class MMPropertyRequirement
    {
        private const string AttributeSuffix = "Attribute";

        public static string Types(MMProperty property, MMAttribute attribute, params SerializedPropertyType[] accepted)
        {
            SerializedProperty serialized = property.Serialized;

            return serialized == null || Accepts(serialized.propertyType, accepted)
                ? null
                : Name(attribute) + " needs " + Describe(accepted) + ".";
        }

        public static string TypesOrReference(MMProperty property, MMAttribute attribute, Type referenceType,
            params SerializedPropertyType[] accepted)
        {
            SerializedProperty serialized = property.Serialized;

            if (serialized == null)
            {
                return null;
            }

            bool accepts = serialized.propertyType == SerializedPropertyType.ObjectReference
                ? property.ValueType != null && referenceType.IsAssignableFrom(property.ValueType)
                : Accepts(serialized.propertyType, accepted);

            return accepts
                ? null
                : Name(attribute) + " needs " + Describe(accepted) + " or " + Article(referenceType.Name) + " reference.";
        }

        private static bool Accepts(SerializedPropertyType type, SerializedPropertyType[] accepted)
        {
            return Array.IndexOf(accepted, type) >= 0;
        }

        public static string Name(MMAttribute attribute)
        {
            string name = attribute.GetType().Name;

            return "[" + (name.EndsWith(AttributeSuffix)
                ? name.Substring(0, name.Length - AttributeSuffix.Length)
                : name) + "]";
        }

        private static string Describe(SerializedPropertyType[] accepted)
        {
            StringBuilder builder = new StringBuilder(Article(accepted[0].ToString()));

            for (int i = 1; i < accepted.Length; i++)
            {
                builder.Append(i == accepted.Length - 1 ? " or " : ", ");
                builder.Append(accepted[i]);
            }

            return builder.Append(" field").ToString();
        }

        private static string Article(string typeName)
        {
            return ("AEIOU".IndexOf(typeName[0]) >= 0 ? "an " : "a ") + typeName;
        }
    }
}
