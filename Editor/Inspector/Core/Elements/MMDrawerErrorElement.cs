using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    public sealed class MMDrawerErrorElement : MMElement
    {
        private readonly MMProperty _property;
        private readonly MMElement _inner;
        private readonly List<string> _errors;

        public MMDrawerErrorElement(MMProperty property, MMElement inner, List<string> errors)
        {
            _property = property;
            _inner = inner;
            _errors = errors;

            AddChild(inner);
        }

        public override bool IsVisible => _property.IsVisible;

        protected override float CalculateHeight(float width)
        {
            float total = _inner.GetHeight(width);

            for (int i = 0; i < _errors.Count; i++)
            {
                total += MMMessageElement.GetHeight(_errors[i], width) + EditorGUIUtility.standardVerticalSpacing;
            }

            return total;
        }

        public override void OnGUI(Rect position)
        {
            float y = position.y;

            for (int i = 0; i < _errors.Count; i++)
            {
                float height = MMMessageElement.GetHeight(_errors[i], position.width);

                MMMessageElement.Draw(new Rect(position.x, y, position.width, height), _errors[i], MessageType.Error);

                y += height + EditorGUIUtility.standardVerticalSpacing;
            }

            _inner.OnGUI(new Rect(position.x, y, position.width, position.yMax - y));
        }
    }
}
