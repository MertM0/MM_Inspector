using System;
using System.Collections.Generic;
using UnityEditor;

namespace MM.Inspector.Workflow.Editor
{
    public sealed class MMInspectorHost
    {
        private static readonly List<MMInspectorHost> Hosts = new List<MMInspectorHost>();

        private static bool _polling;

        private readonly Func<EditorWindow, bool> _attach;
        private readonly Action<EditorWindow> _refresh;

        private bool _served;

        static MMInspectorHost()
        {
            Selection.selectionChanged += SyncAll;
        }

        public MMInspectorHost(Func<EditorWindow, bool> attach)
        {
            _attach = attach;
            _refresh = Refresh;

            Hosts.Add(this);
            Poll();
        }

        public static void SyncAll()
        {
            for (int i = 0; i < Hosts.Count; i++)
            {
                Hosts[i].Sync();
            }
        }

        public void Sync()
        {
            _served = true;
            MMInspectorWindows.ForEach(_refresh);
        }

        private void Refresh(EditorWindow window)
        {
            if (!_attach(window))
            {
                _served = false;
            }

            window.Repaint();
        }

        private static void Poll()
        {
            if (_polling)
            {
                return;
            }

            _polling = true;
            EditorApplication.update += OnUpdate;
        }

        private static void OnUpdate()
        {
            SyncAll();

            for (int i = 0; i < Hosts.Count; i++)
            {
                if (!Hosts[i]._served)
                {
                    return;
                }
            }

            _polling = false;
            EditorApplication.update -= OnUpdate;
        }
    }
}
