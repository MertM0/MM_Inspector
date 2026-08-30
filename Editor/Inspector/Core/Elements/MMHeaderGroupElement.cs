using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    public abstract class MMHeaderGroupElement : MMGroupElement
    {
        protected abstract bool IsExpanded { get; set; }

        protected virtual float HeaderHeight => EditorGUIUtility.singleLineHeight;

        protected virtual RectOffset HeaderPadding => MMFrame.NoPadding;

        protected virtual RectOffset BodyPadding => MMFrame.NoPadding;

        protected virtual float BodyIndent => 12f;

        protected abstract void DrawHeader(Rect rect);

        protected virtual void DrawBackground(Rect rect)
        {
        }

        protected virtual float GetBodyHeight(float width)
        {
            return base.CalculateHeight(width);
        }

        protected virtual void DrawBody(Rect rect)
        {
            base.OnGUI(rect);
        }

        protected override float CalculateHeight(float width)
        {
            float total = HeaderPadding.vertical + HeaderHeight;

            if (!IsExpanded)
            {
                return total;
            }

            RectOffset bodyPadding = BodyPadding;
            float body = GetBodyHeight(width - bodyPadding.horizontal - BodyIndent);

            if (body > 0f)
            {
                total += bodyPadding.vertical + body;
            }

            return total;
        }

        public override void OnGUI(Rect position)
        {
            DrawBackground(position);

            RectOffset headerPadding = HeaderPadding;
            Rect headerArea = new Rect(
                position.x,
                position.y,
                position.width,
                headerPadding.vertical + HeaderHeight);

            DrawHeader(headerPadding.Remove(headerArea));

            if (!IsExpanded || headerArea.yMax >= position.yMax)
            {
                return;
            }

            Rect bodyArea = new Rect(
                position.x,
                headerArea.yMax,
                position.width,
                position.yMax - headerArea.yMax);

            Rect body = BodyPadding.Remove(bodyArea);
            float indent = BodyIndent;

            DrawBody(new Rect(body.x + indent, body.y, body.width - indent, body.height));
        }
    }
}
