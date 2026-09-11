using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    public sealed class MMSearchElement : MMContainerElement
    {
        private const string CancelStyle = "ToolbarSearchCancelButton";
        private const string CancelEmptyStyle = "ToolbarSearchCancelButtonEmpty";

        private readonly IReadOnlyList<MMProperty> _properties;

        private string _query = string.Empty;

        public MMSearchElement(IReadOnlyList<MMProperty> properties, MMElement body)
        {
            _properties = properties;
            AddChild(body);
        }

        public static MMElement Wrap(Type type, IReadOnlyList<MMProperty> properties, MMElement body)
        {
            if (type == null || properties == null || !type.IsDefined(typeof(SearchableAttribute), true))
            {
                return body;
            }

            return new MMSearchElement(properties, body);
        }

        public static bool Matches(string name, string query)
        {
            return string.IsNullOrEmpty(query) ||
                   (!string.IsNullOrEmpty(name) && name.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        public string Query
        {
            get => _query;
            set
            {
                if (_query == value)
                {
                    return;
                }

                _query = value ?? string.Empty;
                Apply();
            }
        }

        protected override float CalculateHeight(float width)
        {
            return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + base.CalculateHeight(width);
        }

        public override void OnGUI(Rect position)
        {
            Rect field = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

            DrawField(field);

            float top = field.yMax + EditorGUIUtility.standardVerticalSpacing;

            base.OnGUI(new Rect(position.x, top, position.width, position.yMax - top));
        }

        protected override void OnDetach()
        {
            Query = string.Empty;
        }

        private void DrawField(Rect field)
        {
            GUIStyle cancel = GUI.skin.FindStyle(CancelStyle);
            GUIStyle empty = GUI.skin.FindStyle(CancelEmptyStyle);

            if (cancel == null || empty == null || cancel.fixedWidth <= 0f)
            {
                Query = EditorGUI.TextField(field, GUIContent.none, _query, EditorStyles.toolbarSearchField);
                return;
            }

            float width = cancel.fixedWidth;
            float height = cancel.fixedHeight > 0f ? cancel.fixedHeight : field.height;

            Rect text = new Rect(field.x, field.y, field.width - width, field.height);
            Rect button = new Rect(text.xMax, field.y + (field.height - height) * 0.5f, width, height);

            Query = EditorGUI.TextField(text, GUIContent.none, _query, EditorStyles.toolbarSearchField);

            if (string.IsNullOrEmpty(_query))
            {
                if (Event.current.type == EventType.Repaint)
                {
                    empty.Draw(button, GUIContent.none, false, false, false, false);
                }

                return;
            }

            if (GUI.Button(button, GUIContent.none, cancel))
            {
                Query = string.Empty;
                GUIUtility.keyboardControl = 0;
            }
        }

        private void Apply()
        {
            for (int i = 0; i < _properties.Count; i++)
            {
                MMProperty property = _properties[i];

                property.MatchesSearch = Matches(property.DisplayName, _query);
                property.Refresh();
            }
        }
    }
}
