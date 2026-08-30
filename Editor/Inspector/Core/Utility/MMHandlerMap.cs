using System;
using System.Collections.Generic;

namespace MM.Inspector.Editor
{
    internal sealed class MMHandlerMap<TBase> where TBase : class
    {
        private readonly Dictionary<Type, TBase> _handlers = new Dictionary<Type, TBase>();

        public MMHandlerMap(Type genericBase)
        {
            foreach (TBase handler in MMTypeInstances.Of<TBase>())
            {
                Type attributeType = MMGenericArgument.Resolve(handler.GetType(), genericBase);

                if (attributeType == null || _handlers.ContainsKey(attributeType))
                {
                    continue;
                }

                _handlers[attributeType] = handler;
            }
        }

        public TBase Get(MMAttribute attribute)
        {
            return attribute != null && _handlers.TryGetValue(attribute.GetType(), out TBase handler) ? handler : null;
        }
    }
}
