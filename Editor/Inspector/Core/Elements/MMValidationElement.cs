using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    public sealed class MMValidationElement : MMElement
    {
        private readonly MMProperty _property;
        private readonly MMElement _inner;
        private readonly List<MMValidationResult> _results = new List<MMValidationResult>();

        private int _version = -1;

        public MMValidationElement(MMProperty property, MMElement inner)
        {
            _property = property;
            _inner = inner;
            AddChild(inner);
        }

        public override bool IsVisible => _property.IsVisible;

        public override bool Update()
        {
            bool dirty = base.Update();

            if (_version == MMValidationState.Version)
            {
                return dirty;
            }

            _version = MMValidationState.Version;
            MMValidatorRegistry.Collect(_property, _results);
            MarkHeightDirty();

            return true;
        }

        protected override float CalculateHeight(float width)
        {
            float total = _inner.GetHeight(width);

            for (int i = 0; i < _results.Count; i++)
            {
                total += EditorGUIUtility.standardVerticalSpacing + MMMessageElement.GetHeight(_results[i].Message, width);
            }

            return total;
        }

        public override void OnGUI(Rect position)
        {
            float innerHeight = _inner.GetHeight(position.width);
            _inner.OnGUI(new Rect(position.x, position.y, position.width, innerHeight));

            float y = position.y + innerHeight;

            for (int i = 0; i < _results.Count; i++)
            {
                y += EditorGUIUtility.standardVerticalSpacing;

                float height = MMMessageElement.GetHeight(_results[i].Message, position.width);

                MMMessageElement.Draw(
                    new Rect(position.x, y, position.width, height),
                    _results[i].Message,
                    ToMessageType(_results[i].Severity));

                y += height;
            }
        }

        private static MessageType ToMessageType(MMValidationSeverity severity)
        {
            switch (severity)
            {
                case MMValidationSeverity.Error:
                    return MessageType.Error;
                case MMValidationSeverity.Warning:
                    return MessageType.Warning;
                default:
                    return MessageType.Info;
            }
        }
    }
}
