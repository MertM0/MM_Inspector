using UnityEngine;

namespace MM.Inspector.Editor
{
    internal sealed class SeparatorElement : DecoratedElement
    {
        private readonly float _space;

        public SeparatorElement(MMProperty property, MMElement inner, float space)
            : base(property, inner)
        {
            _space = space;
        }

        protected override float GetDecorationHeight(float width)
        {
            return _space;
        }

        protected override void DrawDecoration(Rect rect)
        {
        }
    }
}
