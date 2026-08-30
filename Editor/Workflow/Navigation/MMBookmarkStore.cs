using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public static class MMBookmarkStore
    {
        private const string KeyPrefix = "MM_Inspector.Workflow.Bookmarks.";

        private static List<MMBookmarkEntry> _entries;

        public static event System.Action Changed;

        public static string Key => KeyPrefix + Hash(Application.dataPath);

        public static IReadOnlyList<MMBookmarkEntry> Entries
        {
            get
            {
                Load();
                return _entries;
            }
        }

        private static int IndexOf(string id)
        {
            Load();

            for (int i = 0; i < _entries.Count; i++)
            {
                if (_entries[i].Id == id)
                {
                    return i;
                }
            }

            return -1;
        }

        public static void Add(Object target)
        {
            if (target != null)
            {
                Add(MMBookmarkResolver.IdOf(target), target.name);
            }
        }

        public static void Add(string id, string label)
        {
            if (string.IsNullOrEmpty(id) || IndexOf(id) >= 0)
            {
                return;
            }

            _entries.Add(new MMBookmarkEntry { Id = id, Label = label });
            Save();
        }

        public static void Remove(string id)
        {
            int index = IndexOf(id);

            if (index < 0)
            {
                return;
            }

            _entries.RemoveAt(index);
            Save();
        }

        public static void Insert(Object target, int slot)
        {
            if (target != null)
            {
                Insert(MMBookmarkResolver.IdOf(target), target.name, slot);
            }
        }

        public static void Insert(string id, string label, int slot)
        {
            if (string.IsNullOrEmpty(id))
            {
                return;
            }

            int existing = IndexOf(id);

            if (existing >= 0)
            {
                MoveTo(existing, slot);
                return;
            }

            _entries.Insert(Mathf.Clamp(slot, 0, _entries.Count), new MMBookmarkEntry { Id = id, Label = label });
            Save();
        }

        public static void MoveTo(int from, int slot)
        {
            Load();

            int to = TargetIndex(from, slot, _entries.Count);

            if (to >= 0)
            {
                Move(from, to);
            }
        }

        public static int TargetIndex(int from, int slot, int count)
        {
            if (count <= 0 || from < 0 || from >= count)
            {
                return -1;
            }

            int target = Mathf.Clamp(slot > from ? slot - 1 : slot, 0, count - 1);
            return target == from ? -1 : target;
        }

        private static void Move(int from, int to)
        {
            Load();

            if (from < 0 || from >= _entries.Count || to < 0 || to >= _entries.Count || from == to)
            {
                return;
            }

            MMBookmarkEntry entry = _entries[from];
            _entries.RemoveAt(from);
            _entries.Insert(to, entry);
            Save();
        }

        public static void SetLabel(string id, string label)
        {
            Load();

            for (int i = 0; i < _entries.Count; i++)
            {
                if (_entries[i].Id != id)
                {
                    continue;
                }

                _entries[i].Label = string.IsNullOrEmpty(label) ? null : label;
                Save();
                return;
            }
        }

        public static string Serialize(List<MMBookmarkEntry> entries)
        {
            MMBookmarkPayload payload = new MMBookmarkPayload();
            payload.Entries = entries ?? new List<MMBookmarkEntry>();
            return JsonUtility.ToJson(payload);
        }

        public static List<MMBookmarkEntry> Deserialize(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return new List<MMBookmarkEntry>();
            }

            MMBookmarkPayload payload;

            try
            {
                payload = JsonUtility.FromJson<MMBookmarkPayload>(json);
            }
            catch (System.Exception)
            {
                payload = null;
            }

            if (payload == null || payload.Entries == null)
            {
                return new List<MMBookmarkEntry>();
            }

            return payload.Entries;
        }

        public static string Hash(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "0";
            }

            unchecked
            {
                uint hash = 2166136261u;

                for (int i = 0; i < value.Length; i++)
                {
                    hash ^= value[i];
                    hash *= 16777619u;
                }

                return hash.ToString("x8");
            }
        }

        private static void Load()
        {
            if (_entries != null)
            {
                return;
            }

            _entries = Deserialize(EditorPrefs.GetString(Key, string.Empty));
        }

        private static void Save()
        {
            EditorPrefs.SetString(Key, Serialize(_entries));

            if (Changed != null)
            {
                Changed();
            }
        }
    }
}
