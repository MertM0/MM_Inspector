using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MM.Inspector.Editor
{
    public abstract class MMPickerElement : MMElement
    {
        internal const int NoneIndex = 0;

        private const int EmptyId = 0;

        private readonly MMProperty _property;
        private readonly string _emptyError;
        private readonly string _brokenReference;
        private readonly int _offset;
        private readonly List<MMPickerOption> _options = new List<MMPickerOption>();

        private string[] _labels = Array.Empty<string>();
        private string[] _withMissing;
        private object _source;
        private int _version = -1;
        private string _error;
        private bool _refreshed;

        protected MMPickerElement(MMProperty property, string emptyError = null, bool zeroIdIsNone = false)
        {
            _property = property;
            _emptyError = emptyError;
            _offset = CanBeEmpty(property.Serialized, zeroIdIsNone) ? 1 : 0;
            _brokenReference = MMPickerPopup.Missing(property.ValueType?.Name);
        }

        public override bool IsVisible => _property.IsVisible;

        protected MMProperty Property => _property;

        internal IReadOnlyList<string> Labels => _labels;

        internal string Error => _error;

        protected abstract bool TryGetSource(out object source, out string error);

        protected abstract void Collect(object source, List<MMPickerOption> options);

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
            if (!_refreshed || Event.current.type == EventType.Layout)
            {
                Refresh();
            }

            if (_error != null)
            {
                MMMessage.Draw(position, _property.Label, _error);
                return;
            }

            SerializedProperty serialized = _property.Serialized;

            using (new EditorGUI.DisabledScope(!_property.IsEnabled))
            using (new MMMixedValueScope(_property))
            {
                int selected = Select(serialized, out string missing);

                EditorGUI.BeginChangeCheck();

                int picked = MMPickerPopup.Draw(position, _property.Label, _labels, selected, missing, ref _withMissing);

                if (EditorGUI.EndChangeCheck() && picked >= 0)
                {
                    Write(serialized, picked);
                }
            }
        }

        internal void Refresh()
        {
            _refreshed = true;

            if (!TryGetSource(out object source, out _error))
            {
                return;
            }

            if (!ReferenceEquals(source, _source) || _version != MMEditorDataVersion.Current)
            {
                _source = source;
                _version = MMEditorDataVersion.Current;
                _options.Clear();
                Collect(source, _options);
                SyncLabels();
            }

            if (_options.Count == 0)
            {
                _error = _emptyError;
            }
        }

        internal int Select(SerializedProperty serialized, out string missing)
        {
            missing = null;
            int index;

            switch (serialized.propertyType)
            {
                case SerializedPropertyType.String:
                {
                    string name = serialized.stringValue;

                    if (string.IsNullOrEmpty(name))
                    {
                        return NoneIndex;
                    }

                    index = IndexOfName(name);

                    if (index < 0)
                    {
                        missing = MMPickerPopup.Missing(name);
                    }

                    break;
                }
                case SerializedPropertyType.ObjectReference:
                {
                    Object value = serialized.objectReferenceValue;

                    if (value == null)
                    {
                        if (!MMObjectKey.HasReferenceId(serialized))
                        {
                            return NoneIndex;
                        }

                        missing = _brokenReference;
                        return -1;
                    }

                    index = IndexOfValue(value);

                    if (index < 0)
                    {
                        missing = MMPickerPopup.Missing(value.name);
                    }

                    break;
                }
                default:
                {
                    int id = serialized.intValue;

                    if (_offset > 0 && id == EmptyId)
                    {
                        return NoneIndex;
                    }

                    index = IndexOfId(id);

                    if (index < 0)
                    {
                        missing = MMPickerPopup.Missing(id.ToString());
                    }

                    break;
                }
            }

            return index < 0 ? -1 : index + _offset;
        }

        internal void Write(SerializedProperty serialized, int picked)
        {
            bool none = _offset > 0 && picked == NoneIndex;
            MMPickerOption option = none ? default : _options[picked - _offset];

            switch (serialized.propertyType)
            {
                case SerializedPropertyType.String:
                    serialized.stringValue = none ? string.Empty : option.Name;
                    break;
                case SerializedPropertyType.ObjectReference:
                    serialized.objectReferenceValue = option.Value;
                    break;
                default:
                    serialized.intValue = none ? EmptyId : option.Id;
                    break;
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

        private int IndexOfValue(Object value)
        {
            for (int i = 0; i < _options.Count; i++)
            {
                if (_options[i].Value == value)
                {
                    return i;
                }
            }

            return -1;
        }

        private void SyncLabels()
        {
            int count = _options.Count + _offset;

            if (_labels.Length != count)
            {
                _labels = new string[count];
            }

            if (_offset > 0)
            {
                _labels[NoneIndex] = MMPickerPopup.None;
            }

            for (int i = 0; i < _options.Count; i++)
            {
                _labels[i + _offset] = MMPickerPopup.Escape(_options[i].Label);
            }

            _withMissing = null;
        }

        private static bool CanBeEmpty(SerializedProperty serialized, bool zeroIdIsNone)
        {
            if (serialized == null)
            {
                return false;
            }

            switch (serialized.propertyType)
            {
                case SerializedPropertyType.String:
                case SerializedPropertyType.ObjectReference:
                    return true;
                case SerializedPropertyType.Integer:
                    return zeroIdIsNone;
                default:
                    return false;
            }
        }
    }
}
