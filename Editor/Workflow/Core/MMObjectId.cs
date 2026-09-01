using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public readonly struct MMObjectId : System.IEquatable<MMObjectId>
    {
        public static MMObjectId None => default;

#if UNITY_6000_4_OR_NEWER
        private readonly EntityId _value;

        private MMObjectId(EntityId value)
        {
            _value = value;
        }

        public static MMObjectId Of(Object target)
        {
            return target == null ? default : new MMObjectId(target.GetEntityId());
        }

        public static MMObjectId FromRaw(ulong raw)
        {
            return new MMObjectId(EntityId.FromULong(raw));
        }

        public ulong Raw => EntityId.ToULong(_value);

        public Object Resolve()
        {
            return EditorUtility.EntityIdToObject(_value);
        }
#else
        private readonly int _value;

        private MMObjectId(int value)
        {
            _value = value;
        }

        public static MMObjectId Of(Object target)
        {
            return target == null ? default : new MMObjectId(target.GetInstanceID());
        }

        public static MMObjectId FromRaw(ulong raw)
        {
            return new MMObjectId(unchecked((int)raw));
        }

        public ulong Raw => (uint)_value;

        public Object Resolve()
        {
#pragma warning disable 618
            return EditorUtility.InstanceIDToObject(_value);
#pragma warning restore 618
        }
#endif

        public bool Equals(MMObjectId other)
        {
            return _value.Equals(other._value);
        }

        public override bool Equals(object obj)
        {
            return obj is MMObjectId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public static bool operator ==(MMObjectId left, MMObjectId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MMObjectId left, MMObjectId right)
        {
            return !left.Equals(right);
        }
    }
}
