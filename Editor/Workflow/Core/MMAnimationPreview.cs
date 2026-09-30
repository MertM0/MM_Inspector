using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public static class MMAnimationPreview
    {
        public static bool Overrides(Object target)
        {
            return target != null && AnimationMode.InAnimationMode() && !IsRecording() && HasAnimatedProperty(target);
        }

        private static bool IsRecording()
        {
            AnimationWindow[] windows = Resources.FindObjectsOfTypeAll<AnimationWindow>();

            for (int i = 0; i < windows.Length; i++)
            {
                if (windows[i].recording)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool HasAnimatedProperty(Object target)
        {
            using (SerializedObject serialized = new SerializedObject(target))
            {
                SerializedProperty property = serialized.GetIterator();

                while (property.Next(true))
                {
                    if (AnimationMode.IsPropertyAnimated(target, property.propertyPath))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
