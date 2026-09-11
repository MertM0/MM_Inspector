using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    public static class MMInlineBody
    {
        private const int MaxDepth = 3;
        private const string DepthMessage = "Inline editors stop nesting here.";

        private static int _depth;

        public static MMElement For(Object target)
        {
            if (target == null)
            {
                return null;
            }

            if (_depth >= MaxDepth)
            {
                return new MMMessageElement(DepthMessage, MessageType.Info);
            }

            return MMReflection.HasAnyMMAttribute(target.GetType())
                ? new MMInlineTreeElement(target)
                : (MMElement)new MMHostedEditorElement(target);
        }

        public static void Enter()
        {
            _depth++;
        }

        public static void Exit()
        {
            _depth--;
        }
    }
}
