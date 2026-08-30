using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    public abstract class MMElement
    {
        private readonly List<MMElement> _children = new List<MMElement>();
        private bool _attached;
        private float _cachedHeight;
        private float _cachedWidth;
        private bool _heightDirty = true;

        public IReadOnlyList<MMElement> Children => _children;
        public int ChildCount => _children.Count;
        public bool IsAttached => _attached;

        public virtual bool IsVisible => true;

        public void AddChild(MMElement child)
        {
            if (child == null)
            {
                return;
            }

            _children.Add(child);
            _heightDirty = true;

            if (_attached)
            {
                child.Attach();
            }
        }

        public void RemoveAllChildren()
        {
            for (int i = 0; i < _children.Count; i++)
            {
                if (_attached)
                {
                    _children[i].Detach();
                }
            }

            _children.Clear();
            _heightDirty = true;
        }

        public void Attach()
        {
            if (_attached)
            {
                return;
            }

            _attached = true;
            OnAttach();

            for (int i = 0; i < _children.Count; i++)
            {
                _children[i].Attach();
            }
        }

        public void Detach()
        {
            if (!_attached)
            {
                return;
            }

            for (int i = 0; i < _children.Count; i++)
            {
                _children[i].Detach();
            }

            OnDetach();
            _attached = false;
        }

        public virtual bool Update()
        {
            bool dirty = false;

            for (int i = 0; i < _children.Count; i++)
            {
                dirty |= _children[i].Update();
            }

            return dirty;
        }

        public float GetHeight(float width)
        {
            bool reusable = !_heightDirty &&
                            Event.current.type != EventType.Layout &&
                            Mathf.Approximately(_cachedWidth, width);

            if (reusable)
            {
                return _cachedHeight;
            }

            _cachedHeight = CalculateHeight(width);
            _cachedWidth = width;
            _heightDirty = false;

            return _cachedHeight;
        }

        public void MarkHeightDirty()
        {
            _heightDirty = true;
        }

        public virtual void OnGUI(Rect position)
        {
            float spacing = EditorGUIUtility.standardVerticalSpacing;
            float y = position.y;
            bool first = true;

            for (int i = 0; i < _children.Count; i++)
            {
                MMElement child = _children[i];
                if (!child.IsVisible)
                {
                    continue;
                }

                if (!first)
                {
                    y += spacing;
                }

                float height = child.GetHeight(position.width);
                child.OnGUI(new Rect(position.x, y, position.width, height));
                y += height;
                first = false;
            }
        }

        protected virtual float CalculateHeight(float width)
        {
            float spacing = EditorGUIUtility.standardVerticalSpacing;
            float total = 0f;
            bool first = true;

            for (int i = 0; i < _children.Count; i++)
            {
                MMElement child = _children[i];
                if (!child.IsVisible)
                {
                    continue;
                }

                if (!first)
                {
                    total += spacing;
                }

                total += child.GetHeight(width);
                first = false;
            }

            return total;
        }

        protected virtual void OnAttach()
        {
        }

        protected virtual void OnDetach()
        {
        }
    }
}
