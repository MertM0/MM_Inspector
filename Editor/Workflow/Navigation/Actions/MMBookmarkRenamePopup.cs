using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public sealed class MMBookmarkRenamePopup : PopupWindowContent
    {
        private const string ControlName = "MMBookmarkRename";
        private const float Width = 220f;
        private const float Height = 24f;
        private const float Padding = 3f;

        private readonly string _id;

        private string _label;
        private bool _focused;
        private bool _cancelled;

        public MMBookmarkRenamePopup(MMBookmarkEntry entry)
        {
            _id = entry.Id;
            _label = entry.Label;
        }

        public override Vector2 GetWindowSize()
        {
            return new Vector2(Width, Height);
        }

        public override void OnGUI(Rect rect)
        {
            if (HandleKeys())
            {
                return;
            }

            Rect field = new Rect(Padding, Padding, rect.width - Padding * 2f, EditorGUIUtility.singleLineHeight);

            GUI.SetNextControlName(ControlName);
            _label = EditorGUI.TextField(field, _label);

            if (_focused)
            {
                return;
            }

            _focused = true;
            EditorGUI.FocusTextInControl(ControlName);
        }

        public override void OnClose()
        {
            if (_cancelled)
            {
                return;
            }

            MMBookmarkStore.SetLabel(_id, _label);
        }

        private bool HandleKeys()
        {
            Event current = Event.current;

            if (current.type != EventType.KeyDown)
            {
                return false;
            }

            if (current.keyCode == KeyCode.Return || current.keyCode == KeyCode.KeypadEnter)
            {
                current.Use();
                editorWindow.Close();
                return true;
            }

            if (current.keyCode != KeyCode.Escape)
            {
                return false;
            }

            _cancelled = true;
            current.Use();
            editorWindow.Close();
            return true;
        }
    }
}
