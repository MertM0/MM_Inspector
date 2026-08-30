using UnityEngine;

namespace MM.Inspector.Samples
{
    public class ConditionalSample : MonoBehaviour
    {
        public enum MovementMode
        {
            Ground,
            Flying
        }

        [Title("Visibility")]
        public bool showOptional;

        [ShowIf(nameof(showOptional))]
        public float optionalValue = 1f;

        [HideIf(nameof(showOptional))]
        public string hiddenWhenShown = "Visible while the toggle is off";

        public MovementMode mode;

        [ShowIf(nameof(mode), MovementMode.Flying)]
        public float altitude = 20f;

        public bool advanced;

        [ShowIf(nameof(advanced))]
        [HideIf(nameof(showOptional))]
        public string needsBoth = "Advanced on, Show Optional off";

        [Separator]
        [Title("Enabled state")]
        public bool unlocked = true;

        [EnableIf(nameof(unlocked))]
        public int editableWhenUnlocked = 5;

        [DisableIf(nameof(unlocked))]
        public int editableWhenLocked = 7;

        [ReadOnly]
        public float alwaysReadOnly = 42f;

        [InfoBox("Conditions accept a member name, and optionally the value it must equal.")]
        public int documented = 1;
    }
}
