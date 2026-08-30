using System;

namespace MM.Inspector.Editor
{
    public sealed class MMBound
    {
        private readonly float _literal;
        private readonly MMValueResolver<float> _resolver;

        public MMBound(float literal, string member, Type ownerType)
        {
            _literal = literal;

            if (!string.IsNullOrEmpty(member))
            {
                _resolver = MMValueResolver<float>.Create(ownerType, member);
            }
        }

        public float GetValue(MMProperty property)
        {
            return _resolver == null || _resolver.HasError ? _literal : _resolver.GetValue(property);
        }
    }
}
