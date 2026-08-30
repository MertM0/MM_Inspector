using System;
using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    public sealed class MMDrawerElement : MMElement
    {
        private readonly MMProperty _property;
        private readonly Func<float, float> _height;
        private readonly Action<Rect> _draw;

        public MMDrawerElement(MMProperty property, Func<float, float> height, Action<Rect> draw)
        {
            _property = property;
            _height = height;
            _draw = draw;
        }

        public override bool IsVisible => _property.IsVisible;

        protected override float CalculateHeight(float width)
        {
            return _height(width);
        }

        public override void OnGUI(Rect position)
        {
            using (new EditorGUI.DisabledScope(!_property.IsEnabled))
            using (new MMMixedValueScope(_property))
            {
                _draw(position);
            }
        }
    }
}
