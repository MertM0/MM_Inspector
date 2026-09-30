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

            IReadOnlyList<MMComponentSnapshot> snapshots = MMClipboardStore.Snapshots;

            if (snapshots.Count == 0)
            {
                return 0;
            }

            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName(UndoLabel);

            int group = Undo.GetCurrentGroup();
            int pasted = 0;
            int overridden = 0;

            for (int i = 0; i < snapshots.Count; i++)
            {
                Component source = MMGlobalId.Resolve(snapshots[i].Id) as Component;

                if (source == null)
                {
                    continue;
                }

                Type type = source.GetType();

                for (int t = 0; t < targets.Length; t++)
                {
                    if (targets[t] == null)
                    {
                        continue;
                    }

                    Component component = targets[t].GetComponent(type);

                    if (MMAnimationPreview.Overrides(component))
                    {
                        overridden++;
                    }
                    else if (Apply(targets[t], type, component, snapshots[i]))
                    {
                        pasted++;
                    }
                }
            }

            Undo.CollapseUndoOperations(group);

            if (overridden > 0)
            {
                MMWorkflowLog.Warn(OverriddenWarning(overridden));
                return pasted;
            }

            MMClipboardStore.Clear();
            return pasted;
        }

        private static bool Apply(GameObject target, Type type, Component component, MMComponentSnapshot snapshot)
        {
            if (component == null)
            {
                component = Undo.AddComponent(target, type);
            }

            return component != null && MMSnapshotRestore.Apply(component, snapshot);
        }

        private static string OverriddenWarning(int count)
        {
            return count + " component(s) were not pasted because the animation preview drives their values. " +
                   "Turn on recording or leave the preview and paste again; the copy is kept.";
        }
    }
}
