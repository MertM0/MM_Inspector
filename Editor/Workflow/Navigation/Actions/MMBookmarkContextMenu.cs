using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public static class MMBookmarkContextMenu
    {
        private static List<MMBookmarkAction> _actions;

        public static IReadOnlyList<MMBookmarkAction> Actions
        {
            get
            {
                Build();
                return _actions;
            }
        }

        public static void Show(MMBookmarkEntry entry, Rect anchor)
        {
            Build();

            GenericMenu menu = new GenericMenu();

            for (int i = 0; i < _actions.Count; i++)
            {
                MMBookmarkAction action = _actions[i];
                GUIContent label = new GUIContent(action.Label);

                if (!action.IsEnabled(entry))
                {
                    menu.AddDisabledItem(label);
                    continue;
                }

                menu.AddItem(label, false, delegate { action.Execute(entry, anchor); });
            }

            menu.ShowAsContext();
        }

        private static void Build()
        {
            _actions ??= MMWorkflowTypes.Sorted<MMBookmarkAction>(action => action.Order);
        }
    }
}
