using UnityEngine;

namespace MM.Inspector.Editor
{
    internal sealed class BoxGroupElement : MMFramedGroupElement
    {
        public BoxGroupElement(MMGroupContext context) : base(context)
        {
        }

        protected override float GetContentHeight(float width)
        {
            return GetStackedHeight(width);
        }

        protected override void DrawContent(Rect rect)
        {
            DrawStacked(rect);
        }
    }
}
