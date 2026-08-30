using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    public class MMGroupElement : MMElement
    {
        public override bool IsVisible
        {
            get
            {
                for (int i = 0; i < Children.Count; i++)
                {
                    if (Children[i].IsVisible)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        protected static void DrawFrame(Rect rect)
        {
            MMFrame.Draw(rect);
        }
    }
}
