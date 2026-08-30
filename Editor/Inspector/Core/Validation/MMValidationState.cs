using UnityEditor;

namespace MM.Inspector.Editor
{
    [InitializeOnLoad]
    public static class MMValidationState
    {
        public static int Version { get; private set; } = 1;

        static MMValidationState()
        {
            Undo.undoRedoPerformed += Invalidate;
        }

        public static void Invalidate()
        {
            Version++;
        }
    }
}
