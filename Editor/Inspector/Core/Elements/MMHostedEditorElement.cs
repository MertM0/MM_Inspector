using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    public sealed class MMHostedEditorElement : MMElement
    {
        private const float MinLabelWidth = 60f;

        private readonly UnityEditor.Editor _editor;

        private float _measured;
        private float _width;

        public MMHostedEditorElement(Object target)
        {
            _editor = UnityEditor.Editor.CreateEditor(target);
            _measured = EditorGUIUtility.singleLineHeight;

            AssemblyReloadEvents.beforeAssemblyReload += Dispose;
        }

        protected override float CalculateHeight(float width)
        {
            return _editor == null ? 0f : _measured;
        }

        public override void OnGUI(Rect position)
        {
            if (_editor == null)
            {
                return;
            }

            if (Event.current.type == EventType.Repaint)
            {
                _width = position.width;
            }

            Rect area = LayoutArea(position);
            float label = EditorGUIUtility.labelWidth;
            bool hierarchy = EditorGUIUtility.hierarchyMode;

            GUIStyle margins = EditorStyles.inspectorDefaultMargins;
            float inset = EditorGUIUtility.currentViewWidth - area.width + margins.padding.left;

            GUILayout.BeginArea(area);

            EditorGUIUtility.hierarchyMode = false;
            EditorGUIUtility.labelWidth = Mathf.Max(MinLabelWidth, label - inset);

            Rect content = EditorGUILayout.BeginVertical(margins);
            _editor.OnInspectorGUI();
            EditorGUILayout.EndVertical();

            EditorGUIUtility.labelWidth = label;
            EditorGUIUtility.hierarchyMode = hierarchy;

            GUILayout.EndArea();

            Measure(content);
        }

        protected override void OnDetach()
        {
            Dispose();
        }

        private void Dispose()
        {
            AssemblyReloadEvents.beforeAssemblyReload -= Dispose;

            if (_editor != null)
            {
                Object.DestroyImmediate(_editor);
            }
        }

        private Rect LayoutArea(Rect position)
        {
            if (position.width > 1f)
            {
                return position;
            }

            float width = _width > 1f ? _width : EditorGUIUtility.currentViewWidth;

            return new Rect(position.x, position.y, width, _measured);
        }

        private void Measure(Rect content)
        {
            if (Event.current.type != EventType.Repaint ||
                content.height <= 0f ||
                Mathf.Approximately(_measured, content.height))
            {
                return;
            }

            _measured = content.height;
            MarkHeightDirty();
        }
    }
}
