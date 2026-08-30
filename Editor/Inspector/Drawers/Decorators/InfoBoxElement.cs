using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    internal sealed class InfoBoxElement : DecoratedElement
    {
        private const float BottomSpace = 2f;

        private readonly MMProperty _property;
        private readonly string _message;
        private readonly MessageType _type;
        private readonly MMValueResolver<bool> _visibleIf;

        public InfoBoxElement(MMProperty property, MMElement inner, string message, InfoBoxType type, string visibleIf)
            : base(property, inner)
        {
            _property = property;
            _message = message;
            _type = ToMessageType(type);

            if (!string.IsNullOrEmpty(visibleIf))
            {
                _visibleIf = MMValueResolver<bool>.Create(property.OwnerType, visibleIf);
            }
        }

        protected override float GetDecorationHeight(float width)
        {
            return ShouldShow ? MMMessageElement.GetHeight(_message, width) + BottomSpace : 0f;
        }

        protected override void DrawDecoration(Rect rect)
        {
            MMMessageElement.Draw(
                new Rect(rect.x, rect.y, rect.width, rect.height - BottomSpace), _message, _type);
        }

        private bool ShouldShow
        {
            get
            {
                if (_visibleIf == null || _visibleIf.HasError)
                {
                    return true;
                }

                return _visibleIf.GetValue(_property);
            }
        }

        private static MessageType ToMessageType(InfoBoxType type)
        {
            switch (type)
            {
                case InfoBoxType.Warning:
                    return MessageType.Warning;
                case InfoBoxType.Error:
                    return MessageType.Error;
                default:
                    return MessageType.Info;
            }
        }
    }
}
