using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public static class MMBookmarkDrag
    {
        private const float Threshold = 4f;

        private static int _index = -1;
        private static float _grab;
        private static float _origin;
        private static bool _active;
        private static int _slot = -1;

        public static bool Active => _active;

        public static int Index => _index;

        public static int Slot => _slot;

        public static float Grab => _grab;

        public static bool Pressed => _index >= 0;

        public static int SlotAt(float contentX, float step, int count)
        {
            if (step <= 0f || count <= 0)
            {
                return 0;
            }

            return Mathf.Clamp(Mathf.RoundToInt(contentX / step), 0, count);
        }

        public static void Press(int index, float contentX, float iconX)
        {
            _index = index;
            _grab = contentX - iconX;
            _origin = contentX;
            _active = false;
            _slot = -1;
        }

        public static void Move(float contentX, float step, int count)
        {
            if (_index < 0)
            {
                return;
            }

            if (!_active && Mathf.Abs(contentX - _origin) < Threshold)
            {
                return;
            }

            _active = true;
            _slot = SlotAt(contentX, step, count);
        }

        public static int Release()
        {
            int slot = _active ? _slot : -1;
            Cancel();
            return slot;
        }

        public static void Cancel()
        {
            _index = -1;
            _slot = -1;
            _active = false;
        }
    }
}
