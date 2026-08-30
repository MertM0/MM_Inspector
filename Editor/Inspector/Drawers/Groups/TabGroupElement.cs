using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    internal sealed class TabGroupElement : MMHeaderGroupElement
    {
        private static GUIStyle _first;
        private static GUIStyle _middle;
        private static GUIStyle _last;
        private static GUIStyle _only;

        private readonly string _path;
        private readonly int _ownerId;
        private readonly List<string> _names;
        private readonly List<int> _visible = new List<int>();

        public TabGroupElement(MMGroupContext context)
        {
            _path = context.Node.Path;
            _ownerId = context.OwnerId;
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

            EnsureStyles();

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
                    MMUiState.SetTab(_ownerId, _path, visible[i]);
                    MarkHeightDirty();
                }
            }
        }

        private static GUIStyle GetStyle(int index, int count)
        {
            if (count == 1)
            {
                return _only;
            }

            if (index == 0)
            {
                return _first;
            }

            return index == count - 1 ? _last : _middle;
        }

        private static void EnsureStyles()
        {
            if (_middle != null)
            {
                return;
            }

            _first = Centered("Tab first");
            _middle = Centered("Tab middle");
            _last = Centered("Tab last");
            _only = Centered("Tab onlyOne");
        }

        private static GUIStyle Centered(string name)
        {
            GUIStyle source = GUI.skin.FindStyle(name) ?? EditorStyles.miniButtonMid;

            return new GUIStyle(source)
            {
                alignment = TextAnchor.MiddleCenter,
                fixedHeight = 0f,
                stretchHeight = true
            };
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
            int stored = MMUiState.GetTab(_ownerId, _path);
            int index = visible.IndexOf(stored);

            return index >= 0 ? index : 0;
        }
    }
}
