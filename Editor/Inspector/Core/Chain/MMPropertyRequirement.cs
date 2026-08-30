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

            if (serialized == null)
            {
                return null;
            }

            for (int i = 0; i < accepted.Length; i++)
            {
                if (serialized.propertyType == accepted[i])
                {
                    return null;
                }
            }

            return Name(attribute) + " needs " + Describe(accepted) + ".";
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
