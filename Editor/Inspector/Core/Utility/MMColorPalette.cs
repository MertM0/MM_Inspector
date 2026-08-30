using UnityEngine;

namespace MM.Inspector.Editor
{
    public static class MMColorPalette
    {
        public static Color Get(MMColor color, Color fallback)
        {
            switch (color)
            {
                case MMColor.Red:
                    return new Color(0.898f, 0.282f, 0.302f);
                case MMColor.Green:
                    return new Color(0.298f, 0.686f, 0.314f);
                case MMColor.Blue:
                    return new Color(0.184f, 0.502f, 0.929f);
                case MMColor.Yellow:
                    return new Color(0.949f, 0.788f, 0.298f);
                case MMColor.Orange:
                    return new Color(0.949f, 0.600f, 0.290f);
                case MMColor.Violet:
                    return new Color(0.608f, 0.318f, 0.878f);
                case MMColor.Cyan:
                    return new Color(0.337f, 0.800f, 0.949f);
                case MMColor.Magenta:
                    return new Color(0.886f, 0.294f, 0.753f);
                case MMColor.Gray:
                    return new Color(0.510f, 0.510f, 0.510f);
                case MMColor.White:
                    return new Color(0.900f, 0.900f, 0.900f);
                default:
                    return fallback;
            }
        }
    }
}
