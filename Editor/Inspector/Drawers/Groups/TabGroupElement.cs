using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    internal sealed class TabGroupElement : MMHeaderGroupElement
    {
        private static readonly MMStyleCache First = Centered("Tab first");
        private static readonly MMStyleCache Middle = Centered("Tab middle");
        private static readonly MMStyleCache Last = Centered("Tab last");
        private static readonly MMStyleCache Only = Centered("Tab onlyOne");

        private readonly string _key;
        private readonly List<string> _names;
        private readonly List<int> _visible = new List<int>();

        public TabGroupElement(MMGroupContext context)
        {
            _key = MMUiState.Key(MMUiState.TabScope, context.Owner, context.Node.Path);
            _names = new List<string>(context.ChildNames);
        }

        protected override bool IsExpanded
        {
            get => true;
            set { }
        }

        protected override float HeaderHeight => MMGroupHeader.Height;

        protected override RectOffset BodyPadding => MMFrame.BodyPadding;

        protected override float BodyIndent => 0f;

        protected override void DrawBackground(Rect rect)
        {
            DrawFrame(rect);
        }

        protected override void DrawHeader(Rect rect)
        {
            List<int> visible = GetVisibleIndices();
            if (visible.Count == 0)
            {
                return;
            }


            int selection = ResolveSelection(visible);
            float width = rect.width / visible.Count;

            for (int i = 0; i < visible.Count; i++)
            {
                Rect tab = new Rect(rect.x + width * i, rect.y, width, rect.height);
                bool selected = i == selection;

                bool picked = GUI.Toggle(
                    tab,
                    selected,
                    MMReflection.ToDisplayName(_names[visible[i]]),
                    GetStyle(i, visible.Count));

                if (picked && !selected)
                {
                    MMUiState.SetTab(_key, visible[i]);
                    MarkHeightDirty();
                }
            }
        }

        private static GUIStyle GetStyle(int index, int count)
        {
            if (count == 1)
            {
                return Only.Style;
            }

            if (index == 0)
            {
                return First.Style;
            }

            return (index == count - 1 ? Last : Middle).Style;
        }

        private static MMStyleCache Centered(string name)
        {
            return new MMStyleCache(() =>
            {
                GUIStyle source = GUI.skin.FindStyle(name) ?? EditorStyles.miniButtonMid;

                return new GUIStyle(source)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fixedHeight = 0f,
                    stretchHeight = true
                };
            });
        }

        protected override float GetBodyHeight(float width)
        {
            MMElement selected = GetSelected();
            return selected?.GetHeight(width) ?? 0f;
        }

        protected override void DrawBody(Rect rect)
        {
            MMElement selected = GetSelected();
            if (selected == null)
            {
                return;
            }

            selected.OnGUI(new Rect(rect.x, rect.y, rect.width, selected.GetHeight(rect.width)));
        }

        private MMElement GetSelected()
        {
            List<int> visible = GetVisibleIndices();
            if (visible.Count == 0)
            {
                return null;
            }

            return Children[visible[ResolveSelection(visible)]];
        }

        private List<int> GetVisibleIndices()
        {
            _visible.Clear();

            for (int i = 0; i < Children.Count; i++)
            {
                if (Children[i].IsVisible)
                {
                    _visible.Add(i);
                }
            }

            return _visible;
        }

        private int ResolveSelection(List<int> visible)
        {
            int stored = MMUiState.GetTab(_key);
            int index = visible.IndexOf(stored);

            return index >= 0 ? index : 0;
        }
    }
}
