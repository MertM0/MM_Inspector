using UnityEngine;

namespace MM.Inspector.Editor
{
    public readonly struct MMObjectKey
    {
        private const string None = "0";

        private readonly string _value;

        public MMObjectKey(Object owner) : this(owner, null)
        {
        }

        public MMObjectKey(Object owner, string path)
        {
            string id = Identify(owner);

            _value = string.IsNullOrEmpty(path) ? id : id + "/" + path;
        }

        public override string ToString()
        {
            return _value ?? None;
        }

        private static string Identify(Object target)
        {
            if (target == null)
            {
                return None;
            }

#if UNITY_6000_4_OR_NEWER
            return EntityId.ToULong(target.GetEntityId()).ToString();
#else
            return target.GetInstanceID().ToString();
#endif
        }
    }
}
