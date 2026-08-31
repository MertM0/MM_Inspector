using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    internal sealed class ProgressBarElement : MMElement
    {
        private const float Border = 1f;
        private const string MixedText = "—";

        private static GUIStyle _textStyle;
        private static GUIStyle _fieldStyle;

        private readonly MMProperty _property;
        private readonly MMRangeBounds _bounds;
        private readonly string _label;
        private readonly MMColor _color;
        private readonly bool _editable;
        private readonly string _controlName;

        private bool _editing;
        private bool _focusRequested;
        private float _valueBeforeEdit;

        public ProgressBarElement(MMProperty property, MMRangeBounds bounds, string label, MMColor color, bool editable)
        {
            _property = property;
            _bounds = bounds;
            _label = label;
            _color = color;
            _editable = editable;
            _controlName = "MMProgressBar:" +
                           (property.Serialized?.serializedObject?.targetObject?.GetInstanceID() ?? 0) + ":" +
                           property.Serialized?.propertyPath;
        }

        public override bool IsVisible => _property.IsVisible;

        protected override float CalculateHeight(float width)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position)
        {
            SerializedProperty serialized = _property.Serialized;

            TryGetValue(out float value);

            float min = _bounds.GetMin(_property);
            float max = _bounds.GetMax(_property);
            bool interactive = _editable && _property.IsEnabled && serialized != null;

            if (!interactive && _editing)
            {
                EndEdit();
            }

            Rect bar = EditorGUI.PrefixLabel(position, _property.Label);
            int controlId = GUIUtility.GetControlID(FocusType.Passive, bar);

            using (new EditorGUI.DisabledScope(!_property.IsEnabled))
            using (new MMMixedValueScope(_property))
            {
                DrawTrack(bar, value, min, max);

                if (_editing)
                {
                    DrawField(bar, serialized, min, max);
                    return;
                }

                DrawText(bar, value, max);

                if (interactive)
                {
                    HandleMouse(controlId, bar, serialized, min, max);
                }
            }
        }

        private void DrawTrack(Rect bar, float value, float min, float max)
        {
            EditorGUI.DrawRect(bar, MMSkin.Border);

            Rect inner = new Rect(bar.x + Border, bar.y + Border, bar.width - Border * 2f, bar.height - Border * 2f);
            EditorGUI.DrawRect(inner, MMSkin.Track);

            float fill = _property.HasMixedValue || Mathf.Approximately(max, min)
                ? 0f
                : Mathf.Clamp01((value - min) / (max - min));
            if (fill <= 0f)
            {
                return;
            }

            EditorGUI.DrawRect(new Rect(inner.x, inner.y, inner.width * fill, inner.height), MMColorPalette.Get(_color, MMSkin.Accent));
        }

        private void DrawText(Rect bar, float value, float max)
        {
            if (_property.HasMixedValue)
            {
                EditorGUI.LabelField(bar, MixedText, TextStyle);
                return;
            }

            string text = string.IsNullOrEmpty(_label) ? $"{value:0.##} / {max:0.##}" : _label;
            EditorGUI.LabelField(bar, text, TextStyle);
        }

        private void DrawField(Rect bar, SerializedProperty serialized, float min, float max)
        {
            Event current = Event.current;
            bool keyDown = current.type == EventType.KeyDown;
            bool cancel = keyDown && current.keyCode == KeyCode.Escape;
            bool commit = keyDown && (current.keyCode == KeyCode.Return || current.keyCode == KeyCode.KeypadEnter);

            Rect field = new Rect(bar.x + Border, bar.y + Border, bar.width - Border * 2f, bar.height - Border * 2f);

            GUI.SetNextControlName(_controlName);

            EditorGUI.BeginChangeCheck();

            if (serialized.propertyType == SerializedPropertyType.Integer)
            {
                int edited = EditorGUI.IntField(field, serialized.intValue, FieldStyle);

                if (EditorGUI.EndChangeCheck())
                {
                    serialized.intValue = Mathf.Clamp(edited, Mathf.RoundToInt(min), Mathf.RoundToInt(max));
                }
            }
            else
            {
                float edited = EditorGUI.FloatField(field, serialized.floatValue, FieldStyle);

                if (EditorGUI.EndChangeCheck())
                {
                    serialized.floatValue = Mathf.Clamp(edited, min, max);
                }
            }

            if (_focusRequested)
            {
                EditorGUI.FocusTextInControl(_controlName);
                _focusRequested = false;
                return;
            }

            if (cancel)
            {
                Write(serialized, _valueBeforeEdit, min, max);
                EndEdit();
                return;
            }

            if (commit)
            {
                EndEdit();
                return;
            }

            if (current.type == EventType.Repaint && GUI.GetNameOfFocusedControl() != _controlName)
            {
                EndEdit();
            }
        }

        private void HandleMouse(int controlId, Rect bar, SerializedProperty serialized, float min, float max)
        {
            Event current = Event.current;

            switch (current.GetTypeForControl(controlId))
            {
                case EventType.MouseDown:
                    if (current.button != 0 || !bar.Contains(current.mousePosition))
                    {
                        break;
                    }

                    if (current.clickCount >= 2)
                    {
                        BeginEdit();
                    }
                    else
                    {
                        GUIUtility.hotControl = controlId;
                        GUIUtility.keyboardControl = 0;
                        Write(serialized, ValueAt(bar, current.mousePosition.x, min, max), min, max);
                    }

                    current.Use();
                    break;

                case EventType.MouseDrag:
                    if (GUIUtility.hotControl != controlId)
                    {
                        break;
                    }

                    Write(serialized, ValueAt(bar, current.mousePosition.x, min, max), min, max);
                    current.Use();
                    break;

                case EventType.MouseUp:
                    if (GUIUtility.hotControl != controlId)
                    {
                        break;
                    }

                    GUIUtility.hotControl = 0;
                    current.Use();
                    break;
            }
        }

        private void BeginEdit()
        {
            TryGetValue(out _valueBeforeEdit);
            _editing = true;
            _focusRequested = true;
            GUIUtility.hotControl = 0;
        }

        private void EndEdit()
        {
            _editing = false;
            _focusRequested = false;
            GUIUtility.keyboardControl = 0;
        }

        private static void Write(SerializedProperty serialized, float value, float min, float max)
        {
            float clamped = Mathf.Clamp(value, min, max);

            if (serialized.propertyType == SerializedPropertyType.Integer)
            {
                serialized.intValue = Mathf.RoundToInt(clamped);
            }
            else
            {
                serialized.floatValue = clamped;
            }

            GUI.changed = true;
        }

        private static float ValueAt(Rect bar, float mouseX, float min, float max)
        {
            float inner = Mathf.Max(1f, bar.width - Border * 2f);
            float normalized = Mathf.Clamp01((mouseX - bar.x - Border) / inner);

            return Mathf.Lerp(min, max, normalized);
        }

        private bool TryGetValue(out float value)
        {
            SerializedProperty serialized = _property.Serialized;

            if (serialized == null)
            {
                switch (_property.GetValue())
                {
                    case int shownInt:
                        value = shownInt;
                        return true;
                    case float shownFloat:
                        value = shownFloat;
                        return true;
                    default:
                        value = 0f;
                        return false;
                }
            }

            switch (serialized.propertyType)
            {
                case SerializedPropertyType.Integer:
                    value = serialized.intValue;
                    return true;
                case SerializedPropertyType.Float:
                    value = serialized.floatValue;
                    return true;
                default:
                    value = 0f;
                    return false;
            }
        }

        private static GUIStyle TextStyle
        {
            get
            {
                if (_textStyle == null)
                {
                    _textStyle = new GUIStyle(EditorStyles.miniBoldLabel)
                    {
                        alignment = TextAnchor.MiddleCenter
                    };

                    _textStyle.normal.textColor = MMSkin.Text;
                }

                return _textStyle;
            }
        }

        private static GUIStyle FieldStyle
        {
            get
            {
                if (_fieldStyle == null)
                {
                    _fieldStyle = new GUIStyle(EditorStyles.numberField)
                    {
                        alignment = TextAnchor.MiddleCenter
                    };
                }

                return _fieldStyle;
            }
        }
    }
}
