using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    public sealed class MMMessageElement : MMElement
    {
        private const float MinHeight = 32f;
        private const float Padding = 8f;

        private readonly string _text;
        private readonly MessageType _type;

        public MMMessageElement(string text, MessageType type = MessageType.Info)
        {
            _text = text;
            _type = type;
        }

        public static float GetHeight(string text, float width)
        {
            float content = EditorStyles.helpBox.CalcHeight(MMMessage.Get(text), width - Padding * 2f);

            return Mathf.Max(MinHeight, content + Padding);
        }

        public static void Draw(Rect position, string text, MessageType type)
        {
            EditorGUI.HelpBox(position, text, type);
        }

        protected override float CalculateHeight(float width)
        {
            return GetHeight(_text, width);
        }

        public override void OnGUI(Rect position)
        {
            Draw(position, _text, _type);
        }
    }
}
