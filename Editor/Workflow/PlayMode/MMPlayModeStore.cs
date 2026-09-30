using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    [InitializeOnLoad]
    public static class MMPlayModeStore
    {
        private const string StateKey = "MM_Inspector.Workflow.PlayModeSnapshots";

        static MMPlayModeStore()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        public static int Count => MMSnapshotPayload.Load(StateKey).Snapshots.Count;

        public static void Clear()
        {
            SessionState.EraseString(StateKey);
        }

        public static void Capture()
        {
            MMSnapshotPayload payload = new MMSnapshotPayload();
            IReadOnlyList<string> ids = MMMarks.PlayMode.Ids;

            for (int i = 0; i < ids.Count; i++)
            {
                MMComponentSnapshot snapshot = MMSnapshotCapture.Of(MMGlobalId.Resolve(ids[i]));

                if (snapshot != null)
                {
                    payload.Snapshots.Add(snapshot);
                }
            }

            payload.Save(StateKey);
        }

        public static int Restore()
        {
            MMSnapshotPayload payload = MMSnapshotPayload.Load(StateKey);
            int restored = 0;

            try
            {
                for (int i = 0; i < payload.Snapshots.Count; i++)
                {
                    if (MMSnapshotRestore.Apply(payload.Snapshots[i]))
                    {
                        restored++;
                    }
                }
            }
            finally
            {
                MMFlattenedIds.Release();
            }

            Clear();
            MMMarks.PlayMode.Clear();
            return restored;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange change)
        {
            if (change == PlayModeStateChange.ExitingPlayMode)
            {
                Capture();
                return;
            }

            if (change != PlayModeStateChange.EnteredEditMode)
            {
                return;
            }

            int restored = Restore();

            if (restored > 0)
            {
                Debug.Log("[MM_Inspector] Restored play mode values on " + restored + " component(s).");
            }
        }
    }
}
