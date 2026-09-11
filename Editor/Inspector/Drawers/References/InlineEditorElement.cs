using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    public sealed class InlineEditorElement : MMBandedElement
    {
        private readonly MMProperty _property;

        private Object _current;

        public InlineEditorElement(MMProperty property)
        {
            _property = property;
            _current = Reference;
        }

        public override bool IsVisible => _property.IsVisible;

        protected override bool HasBody => _current != null;

        protected override bool Expanded
        {
            get => _property.Serialized.isExpanded;
            set => _property.Serialized.isExpanded = value;
        }

        private Object Reference => _property.Serialized?.objectReferenceValue;

        public override bool Update()
        {
            if (Reference == _current)
            {
                return base.Update();
            }

            _current = Reference;
            InvalidateBody();
            base.Update();

            return true;
        }

        protected override MMElement BuildBody()
        {
            return MMInlineBody.For(_current);
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
                EditorGUI.PropertyField(field, _property.Serialized, GUIContent.none);
            }
        }
    }
}
