using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    internal sealed class OnValueChangedElement : MMElement
    {
        private readonly MMProperty _property;
        private readonly MMElement _inner;
        private readonly MMActionResolver _resolver;

        public OnValueChangedElement(MMProperty property, MMElement inner, MMActionResolver resolver)
        {
            _property = property;
            _inner = inner;
            _resolver = resolver;

            AddChild(inner);
        }

        public override bool IsVisible => _property.IsVisible;

        protected override float CalculateHeight(float width)
        {
            return _inner.GetHeight(width);
        }

        public override void OnGUI(Rect position)
        {
            EditorGUI.BeginChangeCheck();

            _inner.OnGUI(position);

            if (!EditorGUI.EndChangeCheck() || _resolver.HasError)
            {
                return;
            }

            _property.Modify(_property.DisplayName, () => _resolver.Invoke(_property));
        }
    }
}
