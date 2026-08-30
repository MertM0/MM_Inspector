using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    public static class MMReflection
    {
        public const BindingFlags MemberFlags =
            BindingFlags.Instance | BindingFlags.Static |
            BindingFlags.Public | BindingFlags.NonPublic |
            BindingFlags.DeclaredOnly;

        private const int MaxNestingDepth = 7;

        private static readonly Dictionary<Type, bool> HasAttributeCache = new Dictionary<Type, bool>();
        private static readonly Dictionary<Assembly, bool> AttributeReachCache = new Dictionary<Assembly, bool>();

        private static Dictionary<string, Assembly> _loadedAssemblies;

        public static IEnumerable<FieldInfo> GetAllFields(Type type)
        {
            foreach (Type level in GetTypeChain(type))
            {
                foreach (FieldInfo field in level.GetFields(MemberFlags))
                {
                    yield return field;
                }
            }
        }

        public static IEnumerable<PropertyInfo> GetAllProperties(Type type)
        {
            foreach (Type level in GetTypeChain(type))
            {
                foreach (PropertyInfo property in level.GetProperties(MemberFlags))
                {
                    yield return property;
                }
            }
        }

        public static IEnumerable<MethodInfo> GetAllMethods(Type type)
        {
            foreach (Type level in GetTypeChain(type))
            {
                foreach (MethodInfo method in level.GetMethods(MemberFlags))
                {
                    yield return method;
                }
            }
        }

        public static bool IsUnitySerialized(FieldInfo field)
        {
            if (field.IsStatic || field.IsLiteral || field.IsInitOnly)
            {
                return false;
            }

            if (field.IsDefined(typeof(NonSerializedAttribute), false))
            {
                return false;
            }

            return field.IsPublic || field.IsDefined(typeof(SerializeField), true);
        }

        public static bool HasAnyMMAttribute(Type type)
        {
            if (type == null)
            {
                return false;
            }

            if (HasAttributeCache.TryGetValue(type, out bool cached))
            {
                return cached;
            }

            bool found = CanDeclareMMAttribute(type) &&
                         ScanForMMAttribute(type, new HashSet<Type> { type }, 0);

            HasAttributeCache[type] = found;
            return found;
        }

        public static string ToDisplayName(string memberName)
        {
            if (string.IsNullOrEmpty(memberName))
            {
                return string.Empty;
            }

            return ObjectNames.NicifyVariableName(memberName);
        }

        private static bool ScanForMMAttribute(Type type, HashSet<Type> visited, int depth)
        {
            foreach (FieldInfo field in GetAllFields(type))
            {
                if (field.IsDefined(typeof(MMAttribute), true))
                {
                    return true;
                }
            }

            foreach (PropertyInfo property in GetAllProperties(type))
            {
                if (property.IsDefined(typeof(MMAttribute), true))
                {
                    return true;
                }
            }

            foreach (MethodInfo method in GetAllMethods(type))
            {
                if (method.IsDefined(typeof(MMAttribute), true))
                {
                    return true;
                }
            }

            return depth < MaxNestingDepth && ScanNested(type, visited, depth);
        }

        private static bool ScanNested(Type type, HashSet<Type> visited, int depth)
        {
            foreach (FieldInfo field in GetAllFields(type))
            {
                if (!IsUnitySerialized(field))
                {
                    continue;
                }

                Type nested = NestedCandidate(field.FieldType);

                if (nested == null || !visited.Add(nested))
                {
                    continue;
                }

                if (ScanForMMAttribute(nested, visited, depth + 1))
                {
                    return true;
                }
            }

            return false;
        }

        private static Type NestedCandidate(Type fieldType)
        {
            Type candidate = MMCollection.GetElementType(fieldType) ?? fieldType;

            if (candidate == null || candidate.IsPrimitive || candidate.IsEnum || candidate == typeof(string))
            {
                return null;
            }

            if (typeof(UnityEngine.Object).IsAssignableFrom(candidate))
            {
                return null;
            }

            if (!candidate.IsDefined(typeof(SerializableAttribute), false))
            {
                return null;
            }

            return CanDeclareMMAttribute(candidate) ? candidate : null;
        }

        private static bool CanDeclareMMAttribute(Type type)
        {
            return CanReachAttributes(type.Assembly);
        }

        private static bool CanReachAttributes(Assembly assembly)
        {
            if (assembly == null)
            {
                return false;
            }

            if (AttributeReachCache.TryGetValue(assembly, out bool cached))
            {
                return cached;
            }

            AttributeReachCache[assembly] = false;

            bool reaches = assembly == typeof(MMAttribute).Assembly;

            if (!reaches)
            {
                foreach (AssemblyName reference in assembly.GetReferencedAssemblies())
                {
                    if (CanReachAttributes(FindLoaded(reference.Name)))
                    {
                        reaches = true;
                        break;
                    }
                }
            }

            AttributeReachCache[assembly] = reaches;
            return reaches;
        }

        private static Assembly FindLoaded(string name)
        {
            if (_loadedAssemblies == null)
            {
                _loadedAssemblies = new Dictionary<string, Assembly>();

                foreach (Assembly loaded in AppDomain.CurrentDomain.GetAssemblies())
                {
                    _loadedAssemblies[loaded.GetName().Name] = loaded;
                }
            }

            return _loadedAssemblies.TryGetValue(name, out Assembly assembly) ? assembly : null;
        }

        private static IEnumerable<Type> GetTypeChain(Type type)
        {
            List<Type> chain = new List<Type>();

            while (type != null && type != typeof(object) && type != typeof(MonoBehaviour) && type != typeof(ScriptableObject))
            {
                chain.Add(type);
                type = type.BaseType;
            }

            chain.Reverse();
            return chain;
        }
    }
}
