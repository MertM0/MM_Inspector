using System;
using System.IO;
using UnityEngine;

namespace MM.Inspector.Editor
{
    public static class MMPathUtility
    {
        private static string _projectRoot;

        public static string ProjectRoot
        {
            get
            {
                if (_projectRoot != null)
                {
                    return _projectRoot;
                }

                DirectoryInfo root = Directory.GetParent(Application.dataPath);
                _projectRoot = root == null ? string.Empty : Normalize(root.FullName);

                return _projectRoot;
            }
        }

        public static string ToProjectRelative(string absolute)
        {
            if (string.IsNullOrEmpty(absolute))
            {
                return absolute;
            }

            string normalized = Normalize(absolute);
            string root = ProjectRoot;

            if (string.IsNullOrEmpty(root) || !normalized.StartsWith(root + "/", StringComparison.OrdinalIgnoreCase))
            {
                MMLog.WarnOnce("'" + normalized + "' is outside the project folder, the absolute path is stored.");
                return normalized;
            }

            return normalized.Substring(root.Length + 1);
        }

        public static string ToAbsolute(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }

            string normalized = Normalize(path);

            return Path.IsPathRooted(normalized) ? normalized : ProjectRoot + "/" + normalized;
        }

        public static string Normalize(string path)
        {
            return string.IsNullOrEmpty(path) ? path : path.Replace('\\', '/').TrimEnd('/');
        }
    }
}
