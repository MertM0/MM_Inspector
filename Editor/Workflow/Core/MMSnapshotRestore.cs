using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public static class MMSnapshotRestore
    {
        private const string UndoLabel = "Restore Saved Values";
        private const string LostReferenceWarning =
            "A saved object reference no longer exists in edit mode and was cleared.";

        public static bool Apply(MMComponentSnapshot snapshot)
        {
            if (snapshot == null)
            {
                return false;
            }

            return Apply(MMGlobalId.Resolve(snapshot.Id), snapshot);
        }

        public static bool Apply(Object target, MMComponentSnapshot snapshot)
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

        private static void ApplyReferences(Object target, MMComponentSnapshot snapshot)
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

                    property.objectReferenceValue = Resolve(snapshot.ReferenceIds[i]);
                }

                serialized.ApplyModifiedPropertiesWithoutUndo();
            }
        }

        private static Object Resolve(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }

            Object resolved = MMGlobalId.Resolve(id);

            if (resolved == null)
            {
                MMWorkflowLog.WarnOnce(LostReferenceWarning);
            }

            return resolved;
        }
    }
}
