using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public static class MMPlayModeCapture
    {
        private const string ScriptPropertyName = "m_Script";

        public static MMPlayModeSnapshot Of(Object target)
        {
            if (target == null)
            {
                return null;
            }

            MMPlayModeSnapshot snapshot = new MMPlayModeSnapshot
            {
                Id = MMGlobalId.Of(target),
                Json = EditorJsonUtility.ToJson(target)
            };

            CollectReferences(target, snapshot);
            return snapshot;
        }

        private static void CollectReferences(Object target, MMPlayModeSnapshot snapshot)
        {
            using (SerializedObject serialized = new SerializedObject(target))
            {
                SerializedProperty property = serialized.GetIterator();

                while (property.NextVisible(true))
                {
                    if (property.propertyType != SerializedPropertyType.ObjectReference ||
                        property.propertyPath == ScriptPropertyName)
                    {
                        continue;
                    }

                    snapshot.ReferencePaths.Add(property.propertyPath);
                    snapshot.ReferenceIds.Add(MMGlobalId.Of(property.objectReferenceValue));
                }
            }
        }
    }
}
