using System;
using System.Collections.Generic;

namespace MM.Inspector.Editor
{
    internal static class MMCollection
    {
        public static Type GetElementType(Type collectionType)
        {
            if (collectionType == null)
            {
                return null;
            }

            if (collectionType.IsArray)
            {
                return collectionType.GetElementType();
            }

            if (collectionType.IsGenericType && collectionType.GetGenericTypeDefinition() == typeof(List<>))
            {
                return collectionType.GetGenericArguments()[0];
            }

            return null;
        }
    }
}
