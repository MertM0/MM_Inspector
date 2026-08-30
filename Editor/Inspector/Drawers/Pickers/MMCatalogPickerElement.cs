using System;
using System.Collections.Generic;

namespace MM.Inspector.Editor
{
    internal sealed class MMCatalogPickerElement : MMPickerElement
    {
        private readonly Func<IReadOnlyList<MMPickerOption>> _catalog;
        private readonly string _emptyError;

        public MMCatalogPickerElement(
            MMProperty property, Func<IReadOnlyList<MMPickerOption>> catalog, string emptyError = null)
            : base(property)
        {
            _catalog = catalog;
            _emptyError = emptyError;
        }

        protected override bool TryBuildOptions(List<MMPickerOption> options, out string error)
        {
            IReadOnlyList<MMPickerOption> catalog = _catalog();

            if (catalog.Count == 0 && !string.IsNullOrEmpty(_emptyError))
            {
                error = _emptyError;
                return false;
            }

            for (int i = 0; i < catalog.Count; i++)
            {
                options.Add(catalog[i]);
            }

            error = null;
            return true;
        }
    }
}
