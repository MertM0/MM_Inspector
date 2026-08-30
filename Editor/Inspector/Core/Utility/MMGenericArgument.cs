using System;

namespace MM.Inspector.Editor
{
    internal static class MMGenericArgument
    {
        public static Type Resolve(Type concrete, Type genericBase)
        {
            Type current = concrete;

            while (current != null && current != typeof(object))
            {
                if (current.IsGenericType && current.GetGenericTypeDefinition() == genericBase)
                {
                    return current.GetGenericArguments()[0];
                }

                current = current.BaseType;
            }

            return null;
        }
    }
}
