using System;
using System.Collections.Generic;

namespace MM.Inspector.Editor
{
    internal sealed class MMCatalogPickerElement : MMPickerElement
    {
        private readonly Func<IReadOnlyList<MMPickerOption>> _catalog;

        public MMCatalogPickerElement(
            MMProperty property, Func<IReadOnlyList<MMPickerOption>> catalog, string emptyError = null)
            : base(property, emptyError)
        {
            _catalog = catalog;
        }

        protected override bool TryGetSource(out object source, out string error)
        {
            source = _catalog();
            error = null;
            return true;
        }

        protected override void Collect(object source, List<MMPickerOption> options)
        {
            IReadOnlyList<MMPickerOption> catalog = (IReadOnlyList<MMPickerOption>)source;

            for (int i = 0; i < catalog.Count; i++)
            {
                options.Add(catalog[i]);
            }
        }
    }
}
