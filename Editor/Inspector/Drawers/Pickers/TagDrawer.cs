using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    internal sealed class TagDrawer : MMSimpleDrawer<TagAttribute>
    {
        private const string Untagged = "Untagged";

        protected override string Validate(MMProperty property, TagAttribute attribute)
        {
            return MMPropertyRequirement.Types(property, attribute, SerializedPropertyType.String);
        }

        protected override void OnGUI(Rect position, MMProperty property, TagAttribute attribute)
        {
            SerializedProperty serialized = property.Serialized;
            string current = string.IsNullOrEmpty(serialized.stringValue) ? Untagged : serialized.stringValue;

            EditorGUI.BeginChangeCheck();

            string picked = EditorGUI.TagField(position, property.Label, current);

            if (EditorGUI.EndChangeCheck())
            {
                serialized.stringValue = picked;
            }
        }
    }
}
