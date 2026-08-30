using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    internal sealed class CurveRangeDrawer : MMSimpleDrawer<CurveRangeAttribute>
    {
        private static readonly Color CurveColor = new Color(0.4f, 0.85f, 0.35f);

        protected override string Validate(MMProperty property, CurveRangeAttribute attribute)
        {
            return MMPropertyRequirement.Types(property, attribute, SerializedPropertyType.AnimationCurve);
        }

        protected override void OnGUI(Rect position, MMProperty property, CurveRangeAttribute attribute)
        {
            SerializedProperty serialized = property.Serialized;
            Rect ranges = Rect.MinMaxRect(attribute.MinX, attribute.MinY, attribute.MaxX, attribute.MaxY);

            EditorGUI.BeginChangeCheck();

            AnimationCurve edited = EditorGUI.CurveField(
                position, property.Label, serialized.animationCurveValue, CurveColor, ranges);

            if (EditorGUI.EndChangeCheck())
            {
                serialized.animationCurveValue = edited;
            }
        }
    }
}
