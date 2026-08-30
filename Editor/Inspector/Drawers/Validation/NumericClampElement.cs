using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    internal sealed class NumericClampElement : MMElement
    {
        private readonly MMProperty _property;
        private readonly MMElement _inner;
        private readonly MMBound _min;
        private readonly MMBound _max;

        public NumericClampElement(MMProperty property, MMElement inner, MMBound min, MMBound max)
        {
            _property = property;
            _inner = inner;
            _min = min;
            _max = max;

            AddChild(inner);
        }

        public override bool IsVisible => _property.IsVisible;

        protected override float CalculateHeight(float width)
        {
            return _inner.GetHeight(width);
        }

        public override void OnGUI(Rect position)
        {
            EditorGUI.BeginChangeCheck();

            _inner.OnGUI(position);

            if (EditorGUI.EndChangeCheck())
            {
                Clamp();
            }
        }

        private void Clamp()
        {
            SerializedProperty serialized = _property.Serialized;
            if (serialized == null)
            {
                return;
            }

            switch (serialized.propertyType)
            {
                case SerializedPropertyType.Integer:
                    ClampInt(serialized);
                    break;

                case SerializedPropertyType.Float:
                    ClampFloat(serialized);
                    break;
            }
        }

        private void ClampInt(SerializedProperty serialized)
        {
            int value = serialized.intValue;

            if (_min != null)
            {
                value = Mathf.Max(value, Mathf.CeilToInt(_min.GetValue(_property)));
            }

            if (_max != null)
            {
                value = Mathf.Min(value, Mathf.FloorToInt(_max.GetValue(_property)));
            }

            if (value != serialized.intValue)
            {
                serialized.intValue = value;
            }
        }

        private void ClampFloat(SerializedProperty serialized)
        {
            float value = serialized.floatValue;

            if (_min != null)
            {
                value = Mathf.Max(value, _min.GetValue(_property));
            }

            if (_max != null)
            {
                value = Mathf.Min(value, _max.GetValue(_property));
            }

            if (!Mathf.Approximately(value, serialized.floatValue))
            {
                serialized.floatValue = value;
            }
        }
    }
}
