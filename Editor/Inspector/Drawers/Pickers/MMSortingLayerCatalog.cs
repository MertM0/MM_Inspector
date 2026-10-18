using System.Collections.Generic;
using UnityEngine;

namespace MM.Inspector.Editor
{
    public static class MMSortingLayerCatalog
    {
        private static List<MMPickerOption> _options = new List<MMPickerOption>();

        public static IReadOnlyList<MMPickerOption> Layers
        {
            get
            {
                SortingLayer[] layers = SortingLayer.layers;

                if (!Matches(layers))
                {
                    _options = Build(layers);
                }

                return _options;
            }
        }

        private static bool Matches(SortingLayer[] layers)
        {
            if (layers.Length != _options.Count)
            {
                return false;
            }

            for (int i = 0; i < layers.Length; i++)
            {
                if (layers[i].id != _options[i].Id || layers[i].name != _options[i].Name)
                {
                    return false;
                }
            }

            return true;
        }

        private static List<MMPickerOption> Build(SortingLayer[] layers)
        {
            List<MMPickerOption> options = new List<MMPickerOption>(layers.Length);

            foreach (SortingLayer layer in layers)
            {
                options.Add(new MMPickerOption(layer.name, layer.id));
            }

            return options;
        }
    }
}
