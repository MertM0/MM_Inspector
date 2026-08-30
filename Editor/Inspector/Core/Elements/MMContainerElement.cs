using System.Collections.Generic;

namespace MM.Inspector.Editor
{
    public class MMContainerElement : MMElement
    {
        public MMContainerElement()
        {
        }

        public MMContainerElement(IEnumerable<MMProperty> properties)
        {
            AddProperties(properties);
        }

        public void AddProperties(IEnumerable<MMProperty> properties)
        {
            if (properties == null)
            {
                return;
            }

            foreach (MMProperty property in properties)
            {
                MMElement element = CreateElementFor(property);
                if (element != null)
                {
                    AddChild(element);
                }
            }
        }

        protected virtual MMElement CreateElementFor(MMProperty property)
        {
            return MMDrawerRegistry.BuildElement(property);
        }
    }
}
