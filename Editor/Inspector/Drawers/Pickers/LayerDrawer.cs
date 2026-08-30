using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    internal sealed class LayerDrawer : MMSimpleDrawer<LayerAttribute>
    {
        protected override string Validate(MMProperty property, LayerAttribute attribute)
        {
            return MMPropertyRequirement.Types(property, attribute,
                SerializedPropertyType.Integer, SerializedPropertyType.String);
        }

        protected override void OnGUI(Rect position, MMProperty property, LayerAttribute attribute)
        {
            SerializedProperty serialized = property.Serialized;

            EditorGUI.BeginChangeCheck();

            if (serialized.propertyType == SerializedPropertyType.Integer)
            {
                int picked = EditorGUI.LayerField(position, property.Label, serialized.intValue);

                if (EditorGUI.EndChangeCheck())
                {
                    serialized.intValue = picked;
                }

                return;
            }

            int current = LayerMask.NameToLayer(serialized.stringValue);
            int layer = EditorGUI.LayerField(position, property.Label, Mathf.Max(0, current));

            if (EditorGUI.EndChangeCheck())
            {
                serialized.stringValue = LayerMask.LayerToName(layer);
            }
        }
    }
}
