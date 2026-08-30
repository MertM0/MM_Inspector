using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UnityEngine.Object), true, isFallback = true)]
    public class MMInspectorEditor : UnityEditor.Editor
    {
        private const string ScriptPropertyName = "m_Script";
        private const string HideScriptFieldKey = "MM_Inspector.Workflow.HideScriptField";

        private bool _engineEnabled;
        private float _width;
        private MMPropertyTree _tree;
        private MMContainerElement _root;

        protected virtual void OnEnable()
        {
            if (target == null)
            {
                return;
            }

            _engineEnabled = MMReflection.HasAnyMMAttribute(target.GetType());
            if (!_engineEnabled)
            {
                return;
            }

            _tree = new MMPropertyTree(serializedObject);
            _root = new MMContainerElement();

            MMProperty script = _tree.Find(ScriptPropertyName);
            if (script != null && !EditorPrefs.GetBool(HideScriptFieldKey, false))
            {
                _root.AddChild(new MMPropertyElement(script));
            }

            MMTypeSchema schema = MMTypeSchema.Get(target.GetType());
            _root.AddChild(MMGroupRegistry.BuildElement(schema.Groups, _tree));

            _root.Attach();

            MMValidationState.Invalidate();
        }

        protected virtual void OnDisable()
        {
            _root?.Detach();
            _root = null;
            _tree = null;
        }

        public override void OnInspectorGUI()
        {
            if (!_engineEnabled || _root == null)
            {
                DrawDefaultInspector();
                return;
            }

            _tree.Update();
            _root.Update();

            float width = _width > 0f ? _width : EditorGUIUtility.currentViewWidth;
            float height = _root.GetHeight(width);

            Rect area = GUILayoutUtility.GetRect(0f, height);

            if (Event.current.type == EventType.Repaint && !Mathf.Approximately(_width, area.width))
            {
                _width = area.width;
                Repaint();
            }

            _root.OnGUI(area);

            if (_tree.ApplyModifiedProperties())
            {
                MMValidationState.Invalidate();
            }
        }
    }
}
