using System;
using System.Reflection;

namespace MM.Inspector.Editor
{
    public sealed class MMActionResolver
    {
        private readonly MethodInfo _method;

        public bool HasError { get; }
        public string ErrorMessage { get; }

        private MMActionResolver(MethodInfo method)
        {
            _method = method;
        }

        private MMActionResolver(string error)
        {
            HasError = true;
            ErrorMessage = error;
            MMLog.WarnOnce(error);
        }

        public static MMActionResolver FromMethod(MethodInfo method)
        {
            return method == null ? new MMActionResolver("Missing method.") : new MMActionResolver(method);
        }

        public static MMActionResolver Create(Type ownerType, string methodName)
        {
            if (string.IsNullOrEmpty(methodName))
            {
                return new MMActionResolver("Empty method name.");
            }

            MemberInfo member = MMMemberLookup.Find(ownerType, methodName);
            if (member is not MethodInfo method)
            {
                return new MMActionResolver($"'{methodName}' not found on {ownerType.Name}. Expected a parameterless method.");
            }

            return new MMActionResolver(method);
        }

        public void Invoke(MMProperty property)
        {
            Invoke(property?.Owner);
        }

        public void Invoke(object owner)
        {
            Invoke(owner, null);
        }

        public void Invoke(object owner, object[] arguments)
        {
            if (HasError)
            {
                return;
            }

            _method.Invoke(_method.IsStatic ? null : owner, arguments);
        }
    }
}
