using System;
using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    public sealed class ManagedReferenceElement : MMBandedElement
    {
        private const string MissingPrefix = "Missing: ";

        private readonly MMProperty _property;

        private long _typeId;
        private string[] _options;
        private int _selected;
        private bool _missing;
        private bool _hasValue;

        public ManagedReferenceElement(MMProperty property)
        {
            _property = property;
            Rebuild();
        }

        public override bool IsVisible => _property.IsVisible;

        protected override bool HasBody => _hasValue;

        protected override bool BodyEnabled => _property.IsEnabled;

        protected override bool Expanded
        {
            get => _property.Serialized.isExpanded;
            set => _property.Serialized.isExpanded = value;
        }

        public override bool Update()
        {
            if (_typeId == _property.Serialized.managedReferenceId)
            {
                return base.Update();
            }

            Rebuild();
            base.Update();

            return true;
        }

        protected override MMElement BuildBody()
        {
            return MMNestedBody.For(_property);
        }

        protected override void DrawRow(Rect label, Rect field)
        {
            if (HasBody)
            {
                Expanded = EditorGUI.Foldout(label, Expanded, _property.Label, toggleOnLabelClick: true);
            }
            else
            {
                EditorGUI.LabelField(label, _property.Label);
            }

            using (new EditorGUI.DisabledScope(!_property.IsEnabled))
            {
                int picked = EditorGUI.Popup(field, _selected, _options);

                if (picked != _selected)
                {
                    Assign(picked);
                }
            }
        }

        private void Assign(int index)
        {
            if (_missing)
            {
                index--;
            }

            Type[] types = MMManagedTypes.Of(_property.DeclaredType);

            if (index < 0 || index >= types.Length || !MMManagedTypes.TryCreate(types[index], out object instance))
            {
                return;
            }

            _property.Serialized.managedReferenceValue = instance;
            _property.Serialized.serializedObject.ApplyModifiedProperties();

            Rebuild();
        }

        private void Rebuild()
        {
            InvalidateBody();

            _typeId = _property.Serialized.managedReferenceId;
            _property.InvalidateChildren();

            Type declared = _property.DeclaredType;
            Type current = _property.Serialized.managedReferenceValue?.GetType();

            _hasValue = current != null;
            _selected = MMManagedTypes.IndexOf(declared, current);
            _options = MMManagedTypes.NamesOf(declared);
            _missing = _selected < 0;

            if (_missing)
            {
                _options = WithMissing(_options, _property.Serialized.managedReferenceFullTypename);
                _selected = 0;
            }
        }

        private static string[] WithMissing(string[] options, string typeName)
        {
            string[] merged = new string[options.Length + 1];

            merged[0] = MissingPrefix + (string.IsNullOrEmpty(typeName) ? "?" : typeName);

            for (int i = 0; i < options.Length; i++)
            {
                merged[i + 1] = options[i];
            }

            return merged;
        }
    }
}
