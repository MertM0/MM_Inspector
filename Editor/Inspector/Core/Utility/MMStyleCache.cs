using System;
using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    public sealed class MMStyleCache
    {
        private static bool _pro;
        private static int _skin;

        private readonly Func<GUIStyle> _build;

        private GUIStyle _style;
        private int _version = -1;

        public MMStyleCache(Func<GUIStyle> build)
        {
            _build = build;
        }

        public static int Skin
        {
            get
            {
                if (_pro != EditorGUIUtility.isProSkin)
                {
                    _pro = EditorGUIUtility.isProSkin;
                    _skin++;
                }

                return _skin;
            }
        }

        public GUIStyle Style
        {
            get
            {
                if (_style != null && _version == Skin)
                {
                    return _style;
                }

                _version = Skin;
                _style = _build();

                return _style;
            }
        }
    }
}
