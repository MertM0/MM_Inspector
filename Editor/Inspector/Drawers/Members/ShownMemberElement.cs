using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    internal sealed class ShownMemberElement : MMElement
    {
        private readonly MMProperty _property;
        private readonly GUIContent _label;

        public ShownMemberElement(MMProperty property)
        {
            _property = property;
            _label = new GUIContent(property.DisplayName);
        }

        public override bool IsVisible => _property.IsVisible;

        protected override float CalculateHeight(float width)
        {
            return MMValueField.GetHeight(_property.ValueType);
        }

        public override void OnGUI(Rect position)
        {
            using (new EditorGUI.DisabledScope(true))
            {
                MMValueField.Draw(position, _label, _property.ValueType, _property.GetValue());
            }
        }
    }
}
