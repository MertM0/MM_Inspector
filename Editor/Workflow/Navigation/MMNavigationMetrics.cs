using System.Collections.Generic;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public static class MMNavigationMetrics
    {
        private const string Prefix = "MM_Inspector.Workflow.Navigation.";

        public static readonly MMBoolSetting HistoryArrows =
            new MMBoolSetting(Prefix + "HistoryArrows", "History Arrows", true);

        public static readonly MMBoolSetting HideUnavailable =
            new MMBoolSetting(Prefix + "HideUnavailable", "Hide Unavailable", true);

        public static readonly MMIntSetting BarHeight =
            new MMIntSetting(Prefix + "BarHeight", "Bar Height", 26, 16, 32);

        public static readonly MMIntSetting IconSize =
            new MMIntSetting(Prefix + "IconSize", "Icon Size", 26, 14, 32);

        public static readonly MMIntSetting IconSpacing =
            new MMIntSetting(Prefix + "IconSpacing", "Icon Spacing", 2, 0, 8);

        public static readonly MMIntSetting IconPadding =
            new MMIntSetting(Prefix + "IconPadding", "Icon Padding", 2, 0, 8);

        public static readonly MMIntSetting PaddingLeft =
            new MMIntSetting(Prefix + "PaddingLeft", "Padding Left", 5, -10, 20);

        public static readonly MMIntSetting PaddingRight =
            new MMIntSetting(Prefix + "PaddingRight", "Padding Right", 0, -10, 20);

        public static readonly MMIntSetting PaddingTop =
            new MMIntSetting(Prefix + "PaddingTop", "Padding Top", 2, -10, 20);

        public static readonly MMIntSetting PaddingBottom =
            new MMIntSetting(Prefix + "PaddingBottom", "Padding Bottom", 2, -10, 20);

        public static readonly MMIntSetting SectionGap =
            new MMIntSetting(Prefix + "SectionGap", "Section Gap", 7, 0, 16);

        public static readonly IReadOnlyList<MMIntSetting> Sliders = new[]
        {
            BarHeight,
            IconSize,
            IconSpacing,
            IconPadding,
            PaddingLeft,
            PaddingRight,
            PaddingTop,
            PaddingBottom,
            SectionGap
        };

        public static readonly IReadOnlyList<MMBoolSetting> Toggles = new[]
        {
            HistoryArrows,
            HideUnavailable
        };

        public static float Step => IconSize.Value + IconSpacing.Value;

        public static float RowHeight =>
            Mathf.Max(0f, BarHeight.Value + PaddingTop.Value + PaddingBottom.Value);

        public static void Reset()
        {
            for (int i = 0; i < Toggles.Count; i++)
            {
                Toggles[i].Reset();
            }

            for (int i = 0; i < Sliders.Count; i++)
            {
                Sliders[i].Reset();
            }
        }

        public static void Reload()
        {
            for (int i = 0; i < Toggles.Count; i++)
            {
                Toggles[i].Reload();
            }

            for (int i = 0; i < Sliders.Count; i++)
            {
                Sliders[i].Reload();
            }
        }
    }
}
