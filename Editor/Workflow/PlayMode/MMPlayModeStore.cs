using System;
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

        public static int Count => Load().Snapshots.Count;

        public static void Clear()
        {
            SessionState.EraseString(StateKey);
        }

        public static void Capture()
        {
            MMPlayModePayload payload = new MMPlayModePayload();
            IReadOnlyList<string> ids = MMMarks.PlayMode.Ids;

            for (int i = 0; i < ids.Count; i++)
            {
                MMComponentSnapshot snapshot = MMSnapshotCapture.Of(MMGlobalId.Resolve(ids[i]));

                if (snapshot != null)
                {
                    payload.Snapshots.Add(snapshot);
                }
            }

            SessionState.SetString(StateKey, JsonUtility.ToJson(payload));
        }

        public static int Restore()
        {
            MMPlayModePayload payload = Load();
            int restored = 0;

            for (int i = 0; i < payload.Snapshots.Count; i++)
            {
                if (MMSnapshotRestore.Apply(payload.Snapshots[i]))
                {
                    restored++;
                }
            }

            Clear();
            return restored;
        }

        private static MMPlayModePayload Load()
        {
            string stored = SessionState.GetString(StateKey, string.Empty);

            if (string.IsNullOrEmpty(stored))
            {
                return new MMPlayModePayload();
            }

            MMPlayModePayload payload;

            try
            {
                payload = JsonUtility.FromJson<MMPlayModePayload>(stored);
            }
            catch (Exception)
            {
                payload = null;
            }

            return payload ?? new MMPlayModePayload();
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
