using System.Collections.Generic;
using UnityEngine;

namespace MM.Inspector.Editor
{
    public static class MMSortingLayerCatalog
    {
        private static readonly List<MMPickerOption> Options = new List<MMPickerOption>();

        private static int _version = -1;

        public static IReadOnlyList<MMPickerOption> Layers
        {
            get
            {
                Rebuild();
                return Options;
            }
        }

        private static void Rebuild()
        {
            if (_version == MMEditorDataVersion.Current)
            {
                return;
            }

            _version = MMEditorDataVersion.Current;

            Options.Clear();

            foreach (SortingLayer layer in SortingLayer.layers)
            {
                Options.Add(new MMPickerOption(layer.name, layer.id));
            }
        }
    }
}
