using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    public abstract class MMPickerElement : MMElement
    {
        private readonly MMProperty _property;
        private readonly List<MMPickerOption> _options = new List<MMPickerOption>();

        private string[] _labels = Array.Empty<string>();
        private string _error;
        private bool _built;
        private bool _valid;

        protected MMPickerElement(MMProperty property)
        {
            _property = property;
        }

        public override bool IsVisible => _property.IsVisible;

        protected MMProperty Property => _property;

        protected abstract bool TryBuildOptions(List<MMPickerOption> options, out string error);

        public static string ValidateTarget(MMProperty property, MMAttribute attribute)
        {
            return MMPropertyRequirement.Types(property, attribute,
                SerializedPropertyType.Integer, SerializedPropertyType.String);
        }

        protected override float CalculateHeight(float width)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position)
        {
            if (!_built || Event.current.type == EventType.Layout)
            {
                Rebuild();
            }

            if (!_valid)
            {
                MMMessage.Draw(position, _property.Label, _error);
                return;
            }

            SerializedProperty serialized = _property.Serialized;
            bool byName = serialized.propertyType == SerializedPropertyType.String;

            using (new EditorGUI.DisabledScope(!_property.IsEnabled))
            using (new MMMixedValueScope(_property))
            {
                if (byName)
                {
                    DrawByName(position, serialized);
                }
                else
                {
                    DrawById(position, serialized);
                }
            }
        }

        private void Rebuild()
        {
            _built = true;
            _options.Clear();
            _valid = TryBuildOptions(_options, out _error);

            SyncLabels();
        }

        private void DrawByName(Rect position, SerializedProperty serialized)
        {
            string current = serialized.stringValue;
            int selected = IndexOfName(current);

            EditorGUI.BeginChangeCheck();

            int picked = MMPickerPopup.Draw(position, _property.Label, _labels, selected, MMPickerPopup.Missing(current));

            if (EditorGUI.EndChangeCheck() && picked >= 0)
            {
                serialized.stringValue = _options[picked].Name;
            }
        }

        private void DrawById(Rect position, SerializedProperty serialized)
        {
            int current = serialized.intValue;
            int selected = IndexOfId(current);

            EditorGUI.BeginChangeCheck();

            int picked = MMPickerPopup.Draw(position, _property.Label, _labels, selected, MMPickerPopup.Missing(current.ToString()));

            if (EditorGUI.EndChangeCheck() && picked >= 0)
            {
                serialized.intValue = _options[picked].Id;
            }
        }

        private int IndexOfName(string name)
        {
            for (int i = 0; i < _options.Count; i++)
            {
                if (_options[i].Name == name)
                {
                    return i;
                }
            }

            return -1;
        }

        private int IndexOfId(int id)
        {
            for (int i = 0; i < _options.Count; i++)
            {
                if (_options[i].Id == id)
                {
                    return i;
                }
            }

            return -1;
        }

        private void SyncLabels()
        {
            if (_labels.Length != _options.Count)
            {
                _labels = new string[_options.Count];
            }

            for (int i = 0; i < _options.Count; i++)
            {
                _labels[i] = _options[i].Label;
            }
        }
    }
}
