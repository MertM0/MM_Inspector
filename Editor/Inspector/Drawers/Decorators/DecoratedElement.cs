using UnityEngine;

namespace MM.Inspector.Editor
{
    public abstract class DecoratedElement : MMElement
    {
        private readonly MMProperty _property;
        private readonly MMElement _inner;

        protected DecoratedElement(MMProperty property, MMElement inner)
        {
            _property = property;
            _inner = inner;
            AddChild(inner);
        }

        public override bool IsVisible => _property.IsVisible;

        protected virtual bool DecorationBelow => false;

        protected abstract float GetDecorationHeight(float width);

        protected abstract void DrawDecoration(Rect rect);

        protected override float CalculateHeight(float width)
        {
            return GetDecorationHeight(width) + _inner.GetHeight(width);
        }

        public override void OnGUI(Rect position)
        {
            float decoration = GetDecorationHeight(position.width);
            float inner = position.height - decoration;

            if (DecorationBelow)
            {
                _inner.OnGUI(new Rect(position.x, position.y, position.width, inner));

                if (decoration > 0f)
                {
                    DrawDecoration(new Rect(position.x, position.y + inner, position.width, decoration));
                }

                return;
            }

            if (decoration > 0f)
            {
                DrawDecoration(new Rect(position.x, position.y, position.width, decoration));
            }

            _inner.OnGUI(new Rect(position.x, position.y + decoration, position.width, inner));
        }
    }
}
