using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public sealed class MMObjectMarks
    {
        private const char Separator = '\n';

        private readonly string _key;
        private readonly string _warning;

        private List<string> _ids;
        private IReadOnlyList<string> _view;
        private HashSet<MMObjectId> _live;

        public MMObjectMarks(string key, string warning)
        {
            _key = key;
            _warning = warning;

            EditorApplication.hierarchyChanged += InvalidateLive;
        }

        public event System.Action Changed;

        public IReadOnlyList<string> Ids
        {
            get
            {
                Stored();
                return _view;
            }
        }

        public int Count => Stored().Count;

        public bool Contains(Object target)
        {
            return target != null && Live.Contains(MMObjectId.Of(target));
        }

        public void Toggle(Object target)
        {
            if (target == null)
            {
                return;
            }

            string id = MMGlobalId.Of(target);

            if (string.IsNullOrEmpty(id))
            {
                MMWorkflowLog.WarnOnce(_warning);
                return;
            }

            List<string> ids = Stored();
            MMObjectId live = MMObjectId.Of(target);

            if (ids.Remove(id))
            {
                Live.Remove(live);
            }
            else
            {
                ids.Add(id);
                Live.Add(live);
            }

            Save();
        }

        public void Clear()
        {
            List<string> ids = Stored();

            if (ids.Count == 0)
            {
                return;
            }

            ids.Clear();
            InvalidateLive();
            Save();
        }

        private HashSet<MMObjectId> Live
        {
            get
            {
                if (_live != null)
                {
                    return _live;
                }

                _live = new HashSet<MMObjectId>();

                List<string> ids = Stored();

                for (int i = 0; i < ids.Count; i++)
                {
                    Object resolved = MMGlobalId.Resolve(ids[i]);

                    if (resolved != null)
                    {
                        _live.Add(MMObjectId.Of(resolved));
                    }
                }

                return _live;
            }
        }

        private List<string> Stored()
        {
            if (_ids != null)
            {
                return _ids;
            }

            _ids = new List<string>();
            _view = _ids.AsReadOnly();

            string stored = SessionState.GetString(_key, string.Empty);

            if (string.IsNullOrEmpty(stored))
            {
                return _ids;
            }

            string[] entries = stored.Split(Separator);

            for (int i = 0; i < entries.Length; i++)
            {
                if (!string.IsNullOrEmpty(entries[i]))
                {
                    _ids.Add(entries[i]);
                }
            }

            return _ids;
        }

        private void InvalidateLive()
        {
            _live = null;
        }

        private void Save()
        {
            SessionState.SetString(_key, string.Join(Separator.ToString(), _ids));
            Changed?.Invoke();
        }
    }
}
