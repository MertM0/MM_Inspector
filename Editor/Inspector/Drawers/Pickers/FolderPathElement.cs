using UnityEditor;

namespace MM.Inspector.Editor
{
    internal sealed class FolderPathElement : PathElement
    {
        public FolderPathElement(MMProperty property, bool absolute)
            : base(property, absolute)
        {
        }

        protected override string OpenPanel(string startPath)
        {
            return EditorUtility.OpenFolderPanel("Select Folder", ResolveDirectory(startPath), string.Empty);
        }
    }
}
