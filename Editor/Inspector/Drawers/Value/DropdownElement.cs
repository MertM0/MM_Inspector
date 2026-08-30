using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    internal sealed class DropdownElement : MMElement
    {
        private readonly MMProperty _property;
        private readonly MMValueResolver<object> _resolver;
        private readonly string _source;
        private readonly List<string> _labels = new List<string>();
        private readonly List<object> _values = new List<object>();

        private string[] _options = Array.Empty<string>();
        private bool _collected;
        private Type _pairType;
        private PropertyInfo _keyProperty;
        private PropertyInfo _valueProperty;

        public DropdownElement(MMProperty property, MMValueResolver<object> resolver, string source)
        {
            _property = property;
            _resolver = resolver;
            _source = source;
        }

        public override bool IsVisible => _property.IsVisible;

        protected override float CalculateHeight(float width)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position)
        {
            if (_resolver.HasError)
            {
                MMMessage.Draw(position, _property.Label, _resolver.ErrorMessage);
                return;
            }

            if (!_collected || Event.current.type == EventType.Layout)
            {
                _collected = Collect(_resolver.GetValue(_property));
            }

            if (!_collected)
            {
                MMMessage.Draw(position, _property.Label, "[Dropdown] '" + _source + "' must return a collection.");
                return;
            }

            SerializedProperty serialized = _property.Serialized;
            object current = Read(serialized);
            int selected = IndexOf(current);

            using (new EditorGUI.DisabledScope(!_property.IsEnabled))
            using (new MMMixedValueScope(_property))
            {
                EditorGUI.BeginChangeCheck();

                int picked = MMPickerPopup.Draw(position, _property.Label, _options, selected, MMPickerPopup.Missing(Describe(current)));

                if (EditorGUI.EndChangeCheck() && picked >= 0)
                {
                    Write(serialized, _values[picked]);
                }
            }
        }

        private bool Collect(object source)
        {
            if (!(source is IEnumerable items))
            {
                return false;
            }

            _labels.Clear();
            _values.Clear();

            foreach (object item in items)
            {
                if (TryReadPair(item, out string label, out object value))
                {
                    _labels.Add(label);
                    _values.Add(value);
                    continue;
                }

                _labels.Add(Describe(item));
                _values.Add(item);
            }

            SyncOptions();
            return true;
        }

        private void SyncOptions()
        {
            if (_options.Length != _labels.Count)
            {
                _options = new string[_labels.Count];
            }

            for (int i = 0; i < _labels.Count; i++)
            {
                _options[i] = _labels[i];
            }
        }

        private bool TryReadPair(object item, out string label, out object value)
        {
            label = null;
            value = null;

            if (item == null)
            {
                return false;
            }

            EnsurePairAccessors(item.GetType());

            if (_keyProperty == null || _valueProperty == null)
            {
                return false;
            }

            label = _keyProperty.GetValue(item) as string;
            value = _valueProperty.GetValue(item);

            return true;
        }

        private void EnsurePairAccessors(Type itemType)
        {
            if (itemType == _pairType)
            {
                return;
            }

            _pairType = itemType;
            _keyProperty = null;
            _valueProperty = null;

            if (!itemType.IsGenericType || itemType.GetGenericTypeDefinition() != typeof(KeyValuePair<,>))
            {
                return;
            }

            if (itemType.GetGenericArguments()[0] != typeof(string))
            {
                return;
            }

            _keyProperty = itemType.GetProperty("Key");
            _valueProperty = itemType.GetProperty("Value");
        }

        private int IndexOf(object current)
        {
            for (int i = 0; i < _values.Count; i++)
            {
                if (AreEqual(_values[i], current))
                {
                    return i;
                }
            }

            return -1;
        }

        private static bool AreEqual(object candidate, object current)
        {
            if (candidate is UnityEngine.Object || current is UnityEngine.Object)
            {
                return (candidate as UnityEngine.Object) == (current as UnityEngine.Object);
            }

            if (candidate == null || current == null)
            {
                return candidate == null && current == null;
            }

            if (candidate.GetType() != current.GetType() && candidate is IConvertible && current is IConvertible)
            {
                try
                {
                    return Convert.ToDouble(candidate).Equals(Convert.ToDouble(current));
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return candidate.Equals(current);
        }

        private static string Describe(object value)
        {
            if (value == null)
            {
                return "None";
            }

            return value is UnityEngine.Object unityObject && unityObject == null ? "None" : value.ToString();
        }

        private static object Read(SerializedProperty serialized)
        {
            switch (serialized.propertyType)
            {
                case SerializedPropertyType.String:
                    return serialized.stringValue;
                case SerializedPropertyType.Integer:
                case SerializedPropertyType.Enum:
                    return serialized.intValue;
                case SerializedPropertyType.Float:
                    return serialized.floatValue;
                case SerializedPropertyType.Boolean:
                    return serialized.boolValue;
                case SerializedPropertyType.ObjectReference:
                    return serialized.objectReferenceValue;
                default:
                    return serialized.boxedValue;
            }
        }

        private static void Write(SerializedProperty serialized, object value)
        {
            switch (serialized.propertyType)
            {
                case SerializedPropertyType.String:
                    serialized.stringValue = value as string ?? string.Empty;
                    return;
                case SerializedPropertyType.Integer:
                case SerializedPropertyType.Enum:
                    serialized.intValue = Convert.ToInt32(value);
                    return;
                case SerializedPropertyType.Float:
                    serialized.floatValue = Convert.ToSingle(value);
                    return;
                case SerializedPropertyType.Boolean:
                    serialized.boolValue = Convert.ToBoolean(value);
                    return;
                case SerializedPropertyType.ObjectReference:
                    serialized.objectReferenceValue = value as UnityEngine.Object;
                    return;
                default:
                    serialized.boxedValue = value;
                    return;
            }
        }
    }
}
