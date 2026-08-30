using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MM.Inspector.Editor
{
    public sealed class MMMemberSchema
    {
        public string Name { get; }
        public string DisplayName { get; }
        public MMMemberKind Kind { get; }
        public MemberInfo Member { get; }
        public Type DeclaringType { get; }
        public Type ValueType { get; }
        public MMAttribute[] Attributes { get; }
        public int Order { get; }

        public (MMAttribute Attribute, MMHideProcessor Processor)[] Hides { get; }
        public (MMAttribute Attribute, MMDisableProcessor Processor)[] Disables { get; }
        public (MMAttribute Attribute, MMValidator Validator)[] Validators { get; }

        public MMMemberSchema(MemberInfo member, MMMemberKind kind)
        {
            Member = member;
            Kind = kind;
            Name = member.Name;
            DeclaringType = member.DeclaringType;
            ValueType = ResolveValueType(member);
            Attributes = member.GetCustomAttributes(typeof(MMAttribute), true).Cast<MMAttribute>().ToArray();

            PropertyOrderAttribute orderAttribute = GetAttribute<PropertyOrderAttribute>();
            Order = orderAttribute?.Order ?? 0;

            DisplayName = MMReflection.ToDisplayName(Name);

            Hides = Bind(Attributes, MMVisibilityRegistry.GetHideProcessor);
            Disables = Bind(Attributes, MMVisibilityRegistry.GetDisableProcessor);
            Validators = Bind(Attributes, MMValidatorRegistry.GetValidator);
        }

        private static (MMAttribute Attribute, THandler Handler)[] Bind<THandler>(
            MMAttribute[] attributes, Func<MMAttribute, THandler> lookup) where THandler : class
        {
            List<(MMAttribute, THandler)> bound = null;

            for (int i = 0; i < attributes.Length; i++)
            {
                THandler handler = lookup(attributes[i]);
                if (handler == null)
                {
                    continue;
                }

                bound ??= new List<(MMAttribute, THandler)>();
                bound.Add((attributes[i], handler));
            }

            return bound == null
                ? Array.Empty<(MMAttribute, THandler)>()
                : bound.ToArray();
        }

        public T GetAttribute<T>() where T : MMAttribute
        {
            for (int i = 0; i < Attributes.Length; i++)
            {
                if (Attributes[i] is T match)
                {
                    return match;
                }
            }

            return null;
        }

        public List<T> GetAttributes<T>() where T : MMAttribute
        {
            List<T> matches = new List<T>();

            for (int i = 0; i < Attributes.Length; i++)
            {
                if (Attributes[i] is T match)
                {
                    matches.Add(match);
                }
            }

            return matches;
        }

        public bool HasAttribute<T>() where T : MMAttribute
        {
            return GetAttribute<T>() != null;
        }

        public object GetValue(object owner)
        {
            switch (Member)
            {
                case FieldInfo field:
                    return field.IsStatic ? field.GetValue(null) : owner == null ? null : field.GetValue(owner);
                case PropertyInfo property:
                    if (!property.CanRead)
                    {
                        return null;
                    }

                    MethodInfo getter = property.GetGetMethod(true);
                    return getter.IsStatic ? property.GetValue(null) : owner == null ? null : property.GetValue(owner);
                default:
                    return null;
            }
        }

        private static Type ResolveValueType(MemberInfo member)
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
    }
}
