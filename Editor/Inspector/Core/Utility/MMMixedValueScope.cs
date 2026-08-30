using System;
using UnityEditor;

namespace MM.Inspector.Editor
{
    public readonly struct MMMixedValueScope : IDisposable
    {
        private readonly bool _previous;

        public MMMixedValueScope(MMProperty property)
        {
            _previous = EditorGUI.showMixedValue;
            EditorGUI.showMixedValue = property != null && property.HasMixedValue;
        }

        public void Dispose()
        {
            EditorGUI.showMixedValue = _previous;
        }
    }
}
