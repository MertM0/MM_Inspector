using System;
using System.Collections.Generic;
using UnityEditor;

namespace MM.Inspector.Editor
{
    public static class MMManagedTypes
    {
        private const string NoneLabel = "None";

        private static readonly Dictionary<Type, Type[]> TypeCacheByBase = new Dictionary<Type, Type[]>();
        private static readonly Dictionary<Type, string[]> NameCacheByBase = new Dictionary<Type, string[]>();

        public static Type[] Of(Type baseType)
        {
            if (baseType == null)
            {
                return Array.Empty<Type>();
            }

            if (TypeCacheByBase.TryGetValue(baseType, out Type[] cached))
            {
                return cached;
            }

            List<Type> types = new List<Type> { null };

            foreach (Type candidate in TypeCache.GetTypesDerivedFrom(baseType))
            {
                if (IsAssignable(candidate))
                {
                    types.Add(candidate);
                }
            }

            Type[] result = types.ToArray();
            TypeCacheByBase[baseType] = result;

            return result;
        }

        public static string[] NamesOf(Type baseType)
        {
            if (baseType == null)
            {
                return Array.Empty<string>();
            }

            if (NameCacheByBase.TryGetValue(baseType, out string[] cached))
            {
                return cached;
            }

            Type[] types = Of(baseType);
            string[] names = new string[types.Length];

            names[0] = NoneLabel;

            for (int i = 1; i < types.Length; i++)
            {
                names[i] = MMReflection.ToDisplayName(types[i].Name);
            }

            NameCacheByBase[baseType] = names;

            return names;
        }

        public static int IndexOf(Type baseType, Type type)
        {
            if (type == null)
            {
                return 0;
            }

            Type[] types = Of(baseType);

            for (int i = 1; i < types.Length; i++)
            {
                if (types[i] == type)
                {
                    return i;
                }
            }

            return -1;
        }

        public static bool TryCreate(Type type, out object instance)
        {
            instance = null;

            if (type == null)
            {
                return true;
            }

            try
            {
                instance = Activator.CreateInstance(type);
                return true;
            }
            catch (Exception exception)
            {
                MMLog.WarnOnce($"'{type.Name}' cannot be assigned to a [SerializeReference] field: {exception.Message}");
                return false;
            }
        }

        private static bool IsAssignable(Type type)
        {
            return !type.IsAbstract &&
                   !type.IsInterface &&
                   !type.IsValueType &&
                   !type.IsGenericTypeDefinition &&
                   !MMReflection.IsUnityObject(type) &&
                   type.GetConstructor(Type.EmptyTypes) != null &&
                   type.IsDefined(typeof(SerializableAttribute), false);
        }
    }
}
