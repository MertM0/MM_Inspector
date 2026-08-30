using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    public sealed class MMNestedPropertyElement : MMContainerElement
    {
        private const float IndentWidth = 15f;
        private const float ArrowInset = 6f;

        private readonly MMProperty _property;

        public MMNestedPropertyElement(MMProperty property)
        {
            _property = property;
            AddProperties(property.Children);
        }

        public override bool IsVisible => _property.IsVisible;

        protected override float CalculateHeight(float width)
        {
            float header = EditorGUIUtility.singleLineHeight;

            if (!_property.Serialized.isExpanded)
            {
                return header;
            }

            float children = base.CalculateHeight(width - IndentWidth);
            return children <= 0f
                ? header
                : header + EditorGUIUtility.standardVerticalSpacing + children;
        }

        public override void OnGUI(Rect position)
        {
            Rect header = new Rect(position.x + ArrowInset, position.y, position.width - ArrowInset, EditorGUIUtility.singleLineHeight);

            _property.Serialized.isExpanded = EditorGUI.Foldout(
                header,
                _property.Serialized.isExpanded,
                _property.DisplayName,
                toggleOnLabelClick: true);

            if (!_property.Serialized.isExpanded)
            {
                return;
            }

            float top = header.yMax + EditorGUIUtility.standardVerticalSpacing;
            Rect body = new Rect(
                position.x + IndentWidth,
                top,
                position.width - IndentWidth,
                position.yMax - top);

            using (new EditorGUI.DisabledScope(!_property.IsEnabled))
            {
                base.OnGUI(body);
            }
        }
    }
}
