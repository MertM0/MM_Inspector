using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    public sealed class MMNestedPropertyElement : MMContainerElement
    {
        private const float IndentWidth = 15f;
        private const float ArrowInset = 15f;

        private readonly MMProperty _property;

        public MMNestedPropertyElement(MMProperty property)
        {
            _property = property;
            AddChild(BuildBody(property));
        }

        private static MMElement BuildBody(MMProperty property)
        {
            Dictionary<string, MMProperty> byName = new Dictionary<string, MMProperty>();

            foreach (MMProperty child in property.Children)
            {
                byName[child.Name] = child;
            }

            return MMGroupRegistry.BuildElement(
                MMTypeSchema.Get(property.ValueType).Groups,
                name => byName.TryGetValue(name, out MMProperty found) ? found : null,
                Owner(property));
        }

        private static MMObjectKey Owner(MMProperty property)
        {
            SerializedProperty serialized = property.Serialized;

            return new MMObjectKey(serialized.serializedObject.targetObject, serialized.propertyPath);
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
