using System.Collections.Generic;
using UnityEngine;

namespace MM.Inspector.Samples
{
    public class ValueSample : MonoBehaviour
    {
        [Title("Sliders")]
        [Slider(0f, 10f)]
        public float volume = 3f;

        [Slider(0, 10)]
        public int quality = 4;

        public int lowerBound;
        public int upperBound = 100;

        [Slider(nameof(lowerBound), nameof(upperBound))]
        public float dynamicRange = 25f;

        [MinMaxSlider(0f, 100f)]
        public Vector2 spawnDelay = new Vector2(20f, 80f);

        [MinMaxSlider(0, 20)]
        public Vector2Int enemyCount = new Vector2Int(3, 12);

        [Slider(0f, 1f)]
        public List<float> mixLevels = new List<float> { 0.2f, 0.8f };

        [Title("Progress bars")]
        [ProgressBar(0f, 100f)]
        public float health = 65f;

        [ProgressBar(0f, 100f, Color = MMColor.Green, Editable = true)]
        public float stamina = 40f;

        public int magazineSize = 30;

        [ProgressBar(0f, nameof(magazineSize), Color = MMColor.Orange, Editable = true, Label = "Ammo")]
        public int ammo = 12;

        [Title("Dropdowns")]
        [Dropdown(nameof(WeaponNames))]
        public string weapon = "Sword";

        [Dropdown(nameof(Difficulties))]
        public int difficulty = 3;

        [Title("Text and curves")]
        [ResizableTextArea]
        public string notes = "The text area grows with its content.";

        [CurveRange(0f, 0f, 1f, 1f)]
        public AnimationCurve fade = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        [Title("Labels")]
        [LabelText("Renamed Field")]
        public int renamed = 1;

        [HideLabel]
        public string withoutLabel = "No label";

        private string[] WeaponNames => new[] { "Sword", "Bow", "Staff" };

        private DropdownList<int> Difficulties => new DropdownList<int>
        {
            { "Easy", 1 },
            { "Normal", 3 },
            { "Hard", 5 }
        };
    }
}
