using System;
using System.Collections.Generic;
using UnityEditor;

namespace MM.Inspector.Editor
{
    public sealed class MMPropertyTree
    {
        private const string ScriptPropertyName = "m_Script";

        private readonly SerializedObject _serializedObject;
        private readonly List<MMProperty> _root = new List<MMProperty>();
        private readonly Dictionary<string, MMProperty> _byName = new Dictionary<string, MMProperty>();

        public IReadOnlyList<MMProperty> Root => _root;
        public SerializedObject SerializedObject => _serializedObject;

        public MMPropertyTree(SerializedObject serializedObject)
        {
            _serializedObject = serializedObject;
            Build();
        }

        public void Update()
        {
            _serializedObject.UpdateIfRequiredOrScript();

            for (int i = 0; i < _root.Count; i++)
            {
                _root[i].Refresh();
            }
        }

        public MMProperty Find(string name)
        {
            return _byName.TryGetValue(name, out MMProperty property) ? property : null;
        }

        public bool ApplyModifiedProperties()
        {
            return _serializedObject.ApplyModifiedProperties();
        }

        private void Build()
        {
            UnityEngine.Object target = _serializedObject.targetObject;
            if (target == null)
            {
                return;
            }

            Type targetType = target.GetType();
            MMTypeSchema schema = MMTypeSchema.Get(targetType);

            SerializedProperty script = _serializedObject.FindProperty(ScriptPropertyName);
            if (script != null)
            {
                MMProperty scriptNode = new MMProperty(null, script.Copy(), target, null, forcedDisabled: true);
                _root.Add(scriptNode);
                _byName[ScriptPropertyName] = scriptNode;
            }

            foreach (MMMemberSchema member in schema.Members)
            {
                SerializedProperty serialized = null;

                if (member.Kind == MMMemberKind.SerializedField)
                {
                    serialized = _serializedObject.FindProperty(member.Name);
                    if (serialized == null)
                    {
                        continue;
                    }
                }

                MMProperty node = new MMProperty(member, serialized, target, null);
                _root.Add(node);
                _byName[member.Name] = node;
            }
        }
    }
}
