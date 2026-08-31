using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    public sealed class MMPropertyElement : MMElement
    {
        private const float FoldoutArrowInset = 15f;

        private readonly MMProperty _property;

        public MMPropertyElement(MMProperty property)
        {
            _property = property;
        }

        public override bool IsVisible => _property.IsVisible;

        protected override float CalculateHeight(float width)
        {
            if (_property.Serialized == null)
            {
                return 0f;
            }

            return EditorGUI.GetPropertyHeight(_property.Serialized, _property.Label, includeChildren: true);
        }

        public override void OnGUI(Rect position)
        {
            if (_property.Serialized == null)
            {
                return;
            }

            if (_property.Serialized.hasVisibleChildren && !_property.IsCollection)
            {
                position.xMin += FoldoutArrowInset;
            }

            using (new EditorGUI.DisabledScope(!_property.IsEnabled))
            {
                EditorGUI.PropertyField(position, _property.Serialized, _property.Label, includeChildren: true);
            }
        }
    }
}
