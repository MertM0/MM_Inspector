using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    public sealed class MMInlineTreeElement : MMContainerElement
    {
        private readonly SerializedObject _serialized;
        private readonly MMPropertyTree _tree;

        public MMInlineTreeElement(Object target)
        {
            _serialized = new SerializedObject(target);
            _tree = new MMPropertyTree(_serialized);

            AddChild(MMGroupRegistry.BuildElement(MMTypeSchema.Get(target.GetType()).Groups, _tree));
        }

        public override bool Update()
        {
            _tree.Update();

            MMInlineBody.Enter();

            try
            {
                return base.Update();
            }
            finally
            {
                MMInlineBody.Exit();
            }
        }

        public override void OnGUI(Rect position)
        {
            base.OnGUI(position);

            if (_tree.ApplyModifiedProperties())
            {
                MMValidationState.Invalidate();
            }
        }

        protected override void OnDetach()
        {
            _serialized.Dispose();
        }
    }
}
