using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    internal sealed class SliderElement : MMElement
    {
        private readonly MMProperty _property;
        private readonly MMRangeBounds _bounds;

        public SliderElement(MMProperty property, MMRangeBounds bounds)
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

            float min = _bounds.GetMin(_property);
            float max = _bounds.GetMax(_property);

            using (new EditorGUI.DisabledScope(!_property.IsEnabled))
            using (new MMMixedValueScope(_property))
            {
                EditorGUI.BeginChangeCheck();

                if (serialized.propertyType == SerializedPropertyType.Integer)
                {
                    int edited = EditorGUI.IntSlider(
                        position, _property.Label, serialized.intValue, Mathf.RoundToInt(min), Mathf.RoundToInt(max));

                    if (EditorGUI.EndChangeCheck())
                    {
                        serialized.intValue = edited;
                    }

                    return;
                }

                float editedFloat = EditorGUI.Slider(position, _property.Label, serialized.floatValue, min, max);

                if (EditorGUI.EndChangeCheck())
                {
                    serialized.floatValue = editedFloat;
                }
            }
        }
    }
}
