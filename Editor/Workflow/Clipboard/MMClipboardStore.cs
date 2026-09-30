using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public static class MMClipboardStore
    {
        private const string StateKey = "MM_Inspector.Workflow.ClipboardSnapshots";

        public static IReadOnlyList<MMComponentSnapshot> Snapshots => MMSnapshotPayload.Load(StateKey).Snapshots;

        public static void Toggle(Object target)
        {
            MMMarks.Clipboard.Toggle(target);

            string id = MMGlobalId.Of(target);

            if (string.IsNullOrEmpty(id))
            {
                return;
            }

            MMSnapshotPayload payload = MMSnapshotPayload.Load(StateKey);
            payload.Snapshots.RemoveAll(snapshot => snapshot.Id == id);

            if (MMMarks.Clipboard.Contains(target))
            {
                payload.Snapshots.Add(MMSnapshotCapture.Of(target));
            }

            payload.Save(StateKey);
        }

        public static void Clear()
        {
            SessionState.EraseString(StateKey);
            MMMarks.Clipboard.Clear();
        }
    }
}
