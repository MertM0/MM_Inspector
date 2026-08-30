using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    [InitializeOnLoad]
    public static class MMHeaderItemBridge
    {
        private const string FieldName = "s_EditorHeaderItemsMethods";
        private const string DelegateName = "HeaderItemDelegate";
        private const string MethodName = "OnGUI";

        private static readonly List<MMHeaderItem> _items = new List<MMHeaderItem>();
        private static readonly FieldInfo _field;
        private static readonly Type _delegateType;

        private static IList _registered;

        static MMHeaderItemBridge()
        {
            _field = typeof(EditorGUIUtility).GetField(FieldName, BindingFlags.NonPublic | BindingFlags.Static);
            _delegateType = typeof(EditorGUIUtility).GetNestedType(DelegateName,
                BindingFlags.NonPublic | BindingFlags.Public);

            if (!Available)
            {
                Debug.LogWarning("[MM_Inspector] Component header icons are not available in this Unity version.");
                return;
            }

            Collect();

            if (_items.Count == 0)
            {
                return;
            }

            EditorApplication.update += EnsureRegistered;
        }

        public static bool Available => _field != null && _delegateType != null;

        public static IReadOnlyList<MMHeaderItem> Items => _items;

        private static void Collect()
        {
            _items.AddRange(MMWorkflowTypes.Sorted<MMHeaderItem>(item => item.Order));
        }

        private static void EnsureRegistered()
        {
            IList list = _field.GetValue(null) as IList;

            if (list == null || ReferenceEquals(list, _registered))
            {
                return;
            }

            for (int i = 0; i < _items.Count; i++)
            {
                if (IsRegistered(list, _items[i]))
                {
                    continue;
                }

                Delegate handler = CreateHandler(_items[i]);

                if (handler != null)
                {
                    list.Add(handler);
                }
            }

            _registered = list;
        }

        private static bool IsRegistered(IList list, MMHeaderItem item)
        {
            for (int i = 0; i < list.Count; i++)
            {
                Delegate entry = list[i] as Delegate;

                if (entry != null && ReferenceEquals(entry.Target, item))
                {
                    return true;
                }
            }

            return false;
        }

        private static Delegate CreateHandler(MMHeaderItem item)
        {
            try
            {
                return Delegate.CreateDelegate(_delegateType, item, MethodName);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
