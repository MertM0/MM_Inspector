using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    public sealed class InlinePropertyElement : MMContainerElement
    {
        private readonly MMProperty _property;

        public InlinePropertyElement(MMProperty property)
        {
            _property = property;
            AddChild(MMNestedBody.For(property));
        }

        public override bool IsVisible => _property.IsVisible;

        private bool HasLabel => !string.IsNullOrEmpty(_property.Label.text);

        protected override float CalculateHeight(float width)
        {
            float body = base.CalculateHeight(width);

            if (!HasLabel)
            {
                return body;
            }

            return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + body;
        }

        public override void OnGUI(Rect position)
        {
            Rect body = position;

            if (HasLabel)
            {
                Rect label = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(label, _property.Label);

                body.yMin = label.yMax + EditorGUIUtility.standardVerticalSpacing;
            }

            using (new EditorGUI.DisabledScope(!_property.IsEnabled))
            {
                base.OnGUI(body);
            }
        }
    }
}
