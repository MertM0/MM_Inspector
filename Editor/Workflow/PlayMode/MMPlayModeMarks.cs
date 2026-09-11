using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public static class MMPlayModeMarks
    {
        private const string StateKey = "MM_Inspector.Workflow.PlayModeMarks";
        private const char Separator = '\n';
        private const string RuntimeWarning =
            "Objects created at run time cannot be saved; they have no counterpart in edit mode.";

        private static List<string> _ids;
        private static HashSet<MMObjectId> _live;

        public static event System.Action Changed;

        public static IReadOnlyList<string> Ids
        {
            get
            {
                Load();
                return _ids;
            }
        }

        public static int Count => Ids.Count;

        public static bool Contains(Object target)
        {
            if (target == null)
            {
                return false;
            }

            Load();
            return _live.Contains(MMObjectId.Of(target));
        }

        public static void Toggle(Object target)
        {
            if (target == null)
            {
                return;
            }

            string id = MMGlobalId.Of(target);

            if (string.IsNullOrEmpty(id))
            {
                MMWorkflowLog.WarnOnce(RuntimeWarning);
                return;
            }

            Load();

            MMObjectId live = MMObjectId.Of(target);

            if (_ids.Remove(id))
            {
                _live.Remove(live);
            }
            else
            {
                _ids.Add(id);
                _live.Add(live);
            }

            Save();
        }

        public static void Clear()
        {
            Load();

            if (_ids.Count == 0)
            {
                return;
            }

            _ids.Clear();
            _live.Clear();
            Save();
        }

        private static void Load()
        {
            if (_ids != null)
            {
                return;
            }

            _ids = new List<string>();
            _live = new HashSet<MMObjectId>();

            string stored = SessionState.GetString(StateKey, string.Empty);

            if (string.IsNullOrEmpty(stored))
            {
                return;
            }

            string[] entries = stored.Split(Separator);

            for (int i = 0; i < entries.Length; i++)
            {
                if (string.IsNullOrEmpty(entries[i]))
                {
                    continue;
                }

                _ids.Add(entries[i]);

                Object resolved = MMGlobalId.Resolve(entries[i]);

                if (resolved != null)
                {
                    _live.Add(MMObjectId.Of(resolved));
                }
            }
        }

        private static void Save()
        {
            SessionState.SetString(StateKey, string.Join(Separator.ToString(), _ids));
            Changed?.Invoke();
        }
    }
}
