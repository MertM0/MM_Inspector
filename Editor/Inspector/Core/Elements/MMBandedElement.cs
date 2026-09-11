using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    public abstract class MMBandedElement : MMContainerElement
    {
        protected const float ArrowInset = 18f;

        private const float MinFieldWidth = 120f;
        private const float MinLabelWidth = ArrowInset + 40f;

        private bool _built;

        protected abstract bool Expanded { get; set; }

        protected abstract bool HasBody { get; }

        protected virtual bool BodyEnabled => true;

        protected abstract MMElement BuildBody();

        protected abstract void DrawRow(Rect label, Rect field);

        public override bool Update()
        {
            bool built = EnsureBody();

            return base.Update() || built;
        }

        protected void InvalidateBody()
        {
            RemoveAllChildren();
            _built = false;
        }

        protected override float CalculateHeight(float width)
        {
            if (!Expanded || !HasBody)
            {
                return MMGroupHeader.Height;
            }

            float body = base.CalculateHeight(width - MMNestedFrame.Inset * 2f);

            if (body <= 0f)
            {
                return MMGroupHeader.Height;
            }

            return MMGroupHeader.Height + body + MMNestedFrame.VerticalPadding - MMNestedFrame.Overlap;
        }

        public override void OnGUI(Rect position)
        {
            Rect band = new Rect(position.x, position.y, position.width, MMGroupHeader.Height);

            MMGroupHeader.DrawBackground(band);
            DrawBand(MMGroupHeader.Inset(band, ArrowInset));

            if (!Expanded || !HasBody || position.yMax <= band.yMax)
            {
                return;
            }

            float top = band.yMax - MMNestedFrame.Overlap;
            Rect frame = new Rect(position.x, top, position.width, position.yMax - top);

            MMNestedFrame.DrawBackground(frame);

            using (new EditorGUI.DisabledScope(!BodyEnabled))
            {
                base.OnGUI(MMNestedFrame.Content(frame));
            }
        }

        private bool EnsureBody()
        {
            if (_built || !Expanded || !HasBody)
            {
                return false;
            }

            _built = true;
            AddChild(BuildBody());

            return true;
        }

        public static float LabelWidth(float rowWidth, float labelWidth)
        {
            float floor = Mathf.Min(MinLabelWidth, rowWidth);
            float preferred = Mathf.Min(labelWidth - ArrowInset, Mathf.Max(0f, rowWidth - MinFieldWidth));

            return Mathf.Clamp(preferred, floor, rowWidth);
        }

        private void DrawBand(Rect row)
        {
            float label = LabelWidth(row.width, EditorGUIUtility.labelWidth);
            float field = Mathf.Max(0f, row.width - label);

            DrawRow(
                new Rect(row.x, row.y, label, row.height),
                new Rect(row.xMax - field, row.y, field, row.height));
        }
    }
}
