using System;
using System.Collections.Generic;
using System.Reflection;

namespace MM.Inspector.Editor
{
    public static class MMMemberLookup
    {
        private const BindingFlags Flags =
            BindingFlags.Instance | BindingFlags.Static |
            BindingFlags.Public | BindingFlags.NonPublic |
            BindingFlags.DeclaredOnly;

        private static readonly Dictionary<(Type, string), MemberInfo> Cache = new Dictionary<(Type, string), MemberInfo>();

        public static MemberInfo Find(Type type, string name)
        {
            if (type == null || string.IsNullOrEmpty(name))
            {
                return null;
            }

            (Type, string) key = (type, name);
            if (Cache.TryGetValue(key, out MemberInfo cached))
            {
                return cached;
            }

            MemberInfo found = Search(type, name);
            Cache[key] = found;
            return found;
        }

        public static Type GetValueType(MemberInfo member)
        {
            switch (member)
            {
                case FieldInfo field:
                    return field.FieldType;
                case PropertyInfo property:
                    return property.PropertyType;
                case MethodInfo method:
                    return method.ReturnType;
                default:
                    return null;
            }
        }

        public static bool IsStatic(MemberInfo member)
        {
            switch (member)
            {
                case FieldInfo field:
                    return field.IsStatic;
                case PropertyInfo property:
                    return property.GetGetMethod(true)?.IsStatic ?? false;
                case MethodInfo method:
                    return method.IsStatic;
                default:
                    return false;
            }
        }

        public static object Read(MemberInfo member, object owner)
        {
            object target = IsStatic(member) ? null : owner;

            switch (member)
            {
                case FieldInfo field:
                    return field.GetValue(target);
                case PropertyInfo property:
                    return property.GetValue(target);
                case MethodInfo method:
                    return method.Invoke(target, null);
                default:
                    return null;
            }
        }

        private static MemberInfo Search(Type type, string name)
        {
            while (type != null && type != typeof(object))
            {
                FieldInfo field = type.GetField(name, Flags);
                if (field != null)
                {
                    return field;
                }

                PropertyInfo property = type.GetProperty(name, Flags);
                if (property != null && property.CanRead && property.GetIndexParameters().Length == 0)
                {
                    return property;
                }

                MethodInfo method = type.GetMethod(name, Flags, null, Type.EmptyTypes, null);
                if (method != null)
                {
                    return method;
                }

                type = type.BaseType;
            }

            return null;
        }
    }
}
