using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    [Serializable]
    public sealed class MMSnapshotPayload
    {
        public List<MMComponentSnapshot> Snapshots = new List<MMComponentSnapshot>();

        public static MMSnapshotPayload Load(string key)
        {
            string stored = SessionState.GetString(key, string.Empty);

            if (string.IsNullOrEmpty(stored))
            {
                return new MMSnapshotPayload();
            }

            MMSnapshotPayload payload;

            try
            {
                payload = JsonUtility.FromJson<MMSnapshotPayload>(stored);
            }
            catch (Exception)
            {
                payload = null;
            }

            return payload ?? new MMSnapshotPayload();
        }

        public void Save(string key)
        {
            SessionState.SetString(key, JsonUtility.ToJson(this));
        }
    }
}
