using UnityEngine;

namespace MM.Inspector.Editor
{
    public abstract class MMFramedGroupElement : MMGroupElement
    {
        private readonly string _title;

        protected MMFramedGroupElement(MMGroupContext context)
        {
            _title = context?.Node?.Title;
        }

        protected virtual RectOffset Padding => MMFrame.Padding;

        protected bool HasHeader => !string.IsNullOrEmpty(_title);

        protected abstract float GetContentHeight(float width);

        protected abstract void DrawContent(Rect rect);

        protected float GetStackedHeight(float width)
        {
            return base.CalculateHeight(width);
        }

        protected void DrawStacked(Rect rect)
        {
            base.OnGUI(rect);
        }

        protected override float CalculateHeight(float width)
        {
            RectOffset padding = Padding;
            float content = GetContentHeight(width - padding.horizontal);

            if (content <= 0f)
            {
                return HasHeader ? MMGroupHeader.Height : 0f;
            }

            float total = content + padding.vertical;
            return HasHeader ? total + MMGroupHeader.Height : total;
        }

        public override void OnGUI(Rect position)
        {
            DrawFrame(position);

            float top = position.y;

            if (HasHeader)
            {
                MMGroupHeader.Draw(new Rect(position.x, top, position.width, MMGroupHeader.Height), _title);
                top += MMGroupHeader.Height;
            }

            if (top >= position.yMax)
            {
                return;
            }

            Rect body = new Rect(position.x, top, position.width, position.yMax - top);
            DrawContent(Padding.Remove(body));
        }
    }
}
