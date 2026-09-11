using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public static class MMPlayModeRestore
    {
        private const string UndoLabel = "Restore Play Mode Values";

        public static bool Apply(MMPlayModeSnapshot snapshot)
        {
            if (snapshot == null)
            {
                return false;
            }

            return Apply(MMGlobalId.Resolve(snapshot.Id), snapshot);
        }

        public static bool Apply(Object target, MMPlayModeSnapshot snapshot)
        {
            if (target == null || snapshot == null || string.IsNullOrEmpty(snapshot.Json))
            {
                return false;
            }

            Undo.RecordObject(target, UndoLabel);
            EditorJsonUtility.FromJsonOverwrite(snapshot.Json, target);
            ApplyReferences(target, snapshot);

            if (PrefabUtility.IsPartOfPrefabInstance(target))
            {
                PrefabUtility.RecordPrefabInstancePropertyModifications(target);
            }

            EditorUtility.SetDirty(target);
            return true;
        }

        private static void ApplyReferences(Object target, MMPlayModeSnapshot snapshot)
        {
            if (snapshot.ReferencePaths.Count == 0)
            {
                return;
            }

            using (SerializedObject serialized = new SerializedObject(target))
            {
                for (int i = 0; i < snapshot.ReferencePaths.Count; i++)
                {
                    SerializedProperty property = serialized.FindProperty(snapshot.ReferencePaths[i]);

                    if (property == null || property.propertyType != SerializedPropertyType.ObjectReference)
                    {
                        continue;
                    }

                    property.objectReferenceValue = MMGlobalId.Resolve(snapshot.ReferenceIds[i]);
                }

                serialized.ApplyModifiedPropertiesWithoutUndo();
            }
        }
    }
}
