using System;
using System.Collections.Generic;
using System.Reflection;

namespace MM.Inspector.Editor
{
    public sealed class MMValueResolver<T>
    {
        private const char MemberPrefix = '$';

        private static readonly Dictionary<(Type, string), MMValueResolver<T>> Cache =
            new Dictionary<(Type, string), MMValueResolver<T>>();

        private readonly MemberInfo _member;
        private readonly T _literal;
        private readonly bool _isLiteral;

        public bool HasError { get; }
        public string ErrorMessage { get; }

        private MMValueResolver(T literal)
        {
            _literal = literal;
            _isLiteral = true;
        }

        private MMValueResolver(MemberInfo member)
        {
            _member = member;
        }

        private MMValueResolver(string error)
        {
            HasError = true;
            ErrorMessage = error;
            MMLog.WarnOnce(error);
        }

        public static MMValueResolver<T> Create(Type ownerType, string source)
        {
            (Type, string) key = (ownerType, source);

            if (Cache.TryGetValue(key, out MMValueResolver<T> cached))
            {
                return cached;
            }

            MMValueResolver<T> created = Build(ownerType, source);
            Cache[key] = created;

            return created;
        }

        private static MMValueResolver<T> Build(Type ownerType, string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return new MMValueResolver<T>($"Empty source for {typeof(T).Name} value.");
            }

            bool explicitMember = source[0] == MemberPrefix;

            if (typeof(T) == typeof(string) && !explicitMember)
            {
                return new MMValueResolver<T>((T)(object)source);
            }

            string memberName = explicitMember ? source.Substring(1) : source;

            MemberInfo member = MMMemberLookup.Find(ownerType, memberName);
            if (member == null)
            {
                return new MMValueResolver<T>($"'{memberName}' not found on {ownerType.Name}. Expected a field, property or parameterless method.");
            }

            Type valueType = MMMemberLookup.GetValueType(member);
            if (!IsCompatible(valueType))
            {
                return new MMValueResolver<T>($"'{memberName}' on {ownerType.Name} is {valueType.Name}, which cannot be used as {typeof(T).Name}.");
            }

            return new MMValueResolver<T>(member);
        }

        public T GetValue(MMProperty property)
        {
            return GetValue(property?.Owner);
        }

        public T GetValue(object owner)
        {
            if (HasError)
            {
                return default;
            }

            if (_isLiteral)
            {
                return _literal;
            }

            object raw = MMMemberLookup.Read(_member, owner);
            return Convert(raw);
        }

        private static bool IsCompatible(Type valueType)
        {
            if (valueType == null || valueType == typeof(void))
            {
                return false;
            }

            if (typeof(T).IsAssignableFrom(valueType))
            {
                return true;
            }

            return typeof(IConvertible).IsAssignableFrom(valueType) && typeof(IConvertible).IsAssignableFrom(typeof(T));
        }

        private static T Convert(object raw)
        {
            if (raw == null)
            {
                return default;
            }

            if (raw is T typed)
            {
                return typed;
            }

            try
            {
                return (T)System.Convert.ChangeType(raw, typeof(T));
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}
