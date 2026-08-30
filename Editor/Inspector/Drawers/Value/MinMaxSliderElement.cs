using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    internal sealed class MinMaxSliderElement : MMElement
    {
        private const float NumberWidth = 50f;
        private const float Gap = 4f;
        private const float Precision = 100f;

        private readonly MMProperty _property;
        private readonly MMRangeBounds _bounds;

        public MinMaxSliderElement(MMProperty property, MMRangeBounds bounds)
        {
            _property = property;
            _bounds = bounds;
        }

        public override bool IsVisible => _property.IsVisible;

        protected override float CalculateHeight(float width)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position)
        {
            SerializedProperty serialized = _property.Serialized;

            Rect field = EditorGUI.PrefixLabel(position, _property.Label);
            Split(field, out Rect left, out Rect bar, out Rect right);

            float min = _bounds.GetMin(_property);
            float max = _bounds.GetMax(_property);

            using (new EditorGUI.DisabledScope(!_property.IsEnabled))
            using (new MMMixedValueScope(_property))
            {
                if (serialized.propertyType == SerializedPropertyType.Vector2Int)
                {
                    DrawInt(serialized, left, bar, right, Mathf.RoundToInt(min), Mathf.RoundToInt(max));
                }
                else
                {
                    DrawFloat(serialized, left, bar, right, min, max);
                }
            }
        }

        private static void DrawFloat(SerializedProperty serialized, Rect left, Rect bar, Rect right, float min, float max)
        {
            Vector2 value = serialized.vector2Value;

            EditorGUI.BeginChangeCheck();

            float low = Round(EditorGUI.FloatField(left, Round(value.x)));
            float high = Round(EditorGUI.FloatField(right, Round(value.y)));

            EditorGUI.MinMaxSlider(bar, ref low, ref high, min, max);

            if (!EditorGUI.EndChangeCheck())
            {
                return;
            }

            value.x = Round(Mathf.Clamp(low, min, max));
            value.y = Round(Mathf.Clamp(high, value.x, max));

            serialized.vector2Value = value;
        }

        private static void DrawInt(SerializedProperty serialized, Rect left, Rect bar, Rect right, int min, int max)
        {
            Vector2Int value = serialized.vector2IntValue;

            EditorGUI.BeginChangeCheck();

            float low = EditorGUI.IntField(left, value.x);
            float high = EditorGUI.IntField(right, value.y);

            EditorGUI.MinMaxSlider(bar, ref low, ref high, min, max);

            if (!EditorGUI.EndChangeCheck())
            {
                return;
            }

            value.x = Mathf.Clamp(Mathf.RoundToInt(low), min, max);
            value.y = Mathf.Clamp(Mathf.RoundToInt(high), value.x, max);

            serialized.vector2IntValue = value;
        }

        private static void Split(Rect field, out Rect left, out Rect bar, out Rect right)
        {
            left = new Rect(field.x, field.y, NumberWidth, field.height);
            right = new Rect(field.xMax - NumberWidth, field.y, NumberWidth, field.height);
            bar = new Rect(left.xMax + Gap, field.y, field.width - NumberWidth * 2f - Gap * 2f, field.height);
        }

        private static float Round(float value)
        {
            return Mathf.Round(value * Precision) / Precision;
        }
    }
}
