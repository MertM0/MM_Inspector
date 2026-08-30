using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace MM.Inspector.Editor
{
    public sealed class MMListElement : MMElement
    {
        private const float ElementPadding = 2f;
        private const float ElementInset = 4f;
        private const float ArrowInset = 18f;
        private const float BodyOverlap = 1f;

        private readonly MMProperty _property;
        private readonly List<MMElement> _elements = new List<MMElement>();

        private ReorderableList _list;
        private GUIContent _headerLabel;
        private int _cachedSize = -1;
        private float _elementWidth;

        public MMListElement(MMProperty property)
        {
            _property = property;
            Rebuild();
        }

        public override bool IsVisible => _property.IsVisible;

        private bool Expanded
        {
            get => _property.Serialized.isExpanded;
            set => _property.Serialized.isExpanded = value;
        }

        public override bool Update()
        {
            if (_cachedSize == _property.Serialized.arraySize)
            {
                return base.Update();
            }

            Rebuild();
            base.Update();

            return true;
        }

        protected override float CalculateHeight(float width)
        {
            return Expanded ? MMGroupHeader.Height + _list.GetHeight() - BodyOverlap : MMGroupHeader.Height;
        }

        public override void OnGUI(Rect position)
        {
            using (new EditorGUI.DisabledScope(!_property.IsEnabled))
            {
                Rect header = new Rect(position.x, position.y, position.width, MMGroupHeader.Height);

                MMGroupHeader.DrawBackground(header);
                DrawHeader(MMGroupHeader.Inset(header, ArrowInset));

                if (!Expanded)
                {
                    return;
                }

                _list.DoList(new Rect(
                    position.x,
                    header.yMax - BodyOverlap,
                    position.width,
                    position.height - header.height + BodyOverlap));
            }
        }

        private void Rebuild()
        {
            _property.InvalidateChildren();
            _cachedSize = _property.Serialized.arraySize;
            _headerLabel = new GUIContent($"{_property.DisplayName}  ({_cachedSize})");

            _elements.Clear();
            RemoveAllChildren();

            foreach (MMProperty element in _property.Children)
            {
                MMElement built = MMDrawerRegistry.BuildElement(element);

                _elements.Add(built);
                AddChild(built);
            }

            _list = new ReorderableList(
                _property.Serialized.serializedObject,
                _property.Serialized,
                draggable: true,
                displayHeader: false,
                displayAddButton: true,
                displayRemoveButton: true)
            {
                headerHeight = 0f,
                elementHeightCallback = GetElementHeight,
                drawElementCallback = DrawElement,
                onChangedCallback = OnChanged
            };
        }

        private void DrawHeader(Rect rect)
        {
            Expanded = EditorGUI.Foldout(rect, Expanded, _headerLabel, toggleOnLabelClick: true);
        }

        private float GetElementHeight(int index)
        {
            if (index < 0 || index >= _elements.Count)
            {
                return EditorGUIUtility.singleLineHeight;
            }

            float width = _elementWidth > 0f ? _elementWidth : EditorGUIUtility.currentViewWidth;

            return _elements[index].GetHeight(width) + ElementPadding * 2f;
        }

        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            if (index < 0 || index >= _elements.Count)
            {
                return;
            }

            _elementWidth = rect.width - ElementInset;

            Rect inner = new Rect(
                rect.x + ElementInset,
                rect.y + ElementPadding,
                _elementWidth,
                rect.height - ElementPadding * 2f);

            _elements[index].OnGUI(inner);
        }

        private void OnChanged(ReorderableList list)
        {
            _property.Serialized.serializedObject.ApplyModifiedProperties();
            Rebuild();
            MarkHeightDirty();
        }
    }
}
