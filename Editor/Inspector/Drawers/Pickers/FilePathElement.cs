using UnityEditor;

namespace MM.Inspector.Editor
{
    internal sealed class FilePathElement : PathElement
    {
        private readonly string _extensions;

        public FilePathElement(MMProperty property, bool absolute, string extensions)
            : base(property, absolute)
        {
            _extensions = extensions;
        }

        protected override string OpenPanel(string startPath)
        {
            string directory = ResolveDirectory(startPath);

            if (string.IsNullOrEmpty(_extensions))
            {
                return EditorUtility.OpenFilePanel("Select File", directory, string.Empty);
            }

            return EditorUtility.OpenFilePanelWithFilters("Select File", directory, new[] { "Files", _extensions });
        }
    }
}
