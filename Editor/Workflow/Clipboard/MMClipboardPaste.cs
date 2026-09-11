using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public static class MMClipboardPaste
    {
        private const string UndoLabel = "Paste Components";

        public static int Into(GameObject[] targets)
        {
            if (targets == null || targets.Length == 0)
            {
                return 0;
            }

            List<Component> sources = Sources();

            if (sources.Count == 0)
            {
                return 0;
            }

            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName(UndoLabel);

            int group = Undo.GetCurrentGroup();
            int pasted = 0;

            for (int i = 0; i < sources.Count; i++)
            {
                MMComponentSnapshot snapshot = MMSnapshotCapture.Of(sources[i]);

                for (int t = 0; t < targets.Length; t++)
                {
                    if (targets[t] != null && Apply(targets[t], sources[i].GetType(), snapshot))
                    {
                        pasted++;
                    }
                }
            }

            Undo.CollapseUndoOperations(group);
            MMMarks.Clipboard.Clear();

            return pasted;
        }

        public static List<Component> Sources()
        {
            List<Component> sources = new List<Component>();
            IReadOnlyList<string> ids = MMMarks.Clipboard.Ids;

            for (int i = 0; i < ids.Count; i++)
            {
                Component source = MMGlobalId.Resolve(ids[i]) as Component;

                if (source != null)
                {
                    sources.Add(source);
                }
            }

            return sources;
        }

        private static bool Apply(GameObject target, Type type, MMComponentSnapshot snapshot)
        {
            Component component = target.GetComponent(type);

            if (component == null)
            {
                component = Undo.AddComponent(target, type);
            }

            return component != null && MMSnapshotRestore.Apply(component, snapshot);
        }
    }
}
