using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public static class MMStripLayout
    {
        public static Rect IconRect(int index, float scroll, float height)
        {
            float size = MMNavigationMetrics.IconSize.Value;

            return new Rect(
                Mathf.Round(index * MMNavigationMetrics.Step - scroll),
                Mathf.Round((height - size) * 0.5f),
                size,
                size);
        }

        public static Rect ImageRect(Rect box)
        {
            float inset = MMNavigationMetrics.IconPadding.Value * 2f;
            float size = Mathf.Max(1f, Mathf.Min(box.width, box.height) - inset);

            return new Rect(
                box.x + (box.width - size) * 0.5f,
                box.y + (box.height - size) * 0.5f,
                size,
                size);
        }

        public static int IndexAt(float localX, float scroll, int count)
        {
            float step = MMNavigationMetrics.Step;

            if (step <= 0f)
            {
                return -1;
            }

            float content = localX + scroll;
            int index = Mathf.FloorToInt(content / step);

            if (index < 0 || index >= count)
            {
                return -1;
            }

            return content - index * step > MMNavigationMetrics.IconSize.Value ? -1 : index;
        }

        public static float ClampScroll(float scroll, int count, float width)
        {
            float max = Mathf.Max(0f, count * MMNavigationMetrics.Step - width);
            return Mathf.Round(Mathf.Clamp(scroll, 0f, max));
        }

        public static float DropLineX(int slot, float scroll, float width, float thickness)
        {
            float gap = MMNavigationMetrics.IconSpacing.Value;
            float center = slot * MMNavigationMetrics.Step - scroll - gap * 0.5f;
            return Mathf.Clamp(Mathf.Round(center - thickness * 0.5f), 0f, Mathf.Max(0f, width - thickness));
        }

        public static float ScrollTo(float scroll, int index, float width)
        {
            float step = MMNavigationMetrics.Step;
            float target = index * step;

            if (target < scroll)
            {
                return target;
            }

            return target + step > scroll + width ? target + step - width : scroll;
        }
    }
}
