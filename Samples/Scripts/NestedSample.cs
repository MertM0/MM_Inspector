using System;
using System.Collections.Generic;
using UnityEngine;

namespace MM.Inspector.Samples
{
    public class NestedSample : MonoBehaviour
    {
        [Serializable]
        public class Weapon
        {
            public string weaponName = "Sword";

            public bool ranged;

            [ShowIf(nameof(ranged))]
            [Slider(1f, 50f)]
            public float range = 10f;

            [Required]
            public Sprite icon;

            [ProgressBar(0f, 100f, Color = MMColor.Red)]
            public float durability = 80f;
        }

        [Serializable]
        public class Slot
        {
            [HideLabel]
            public string label = "Slot";

            [MinValue(0f)]
            public int cost;
        }

        public Weapon primary = new Weapon();

        public List<Weapon> inventory = new List<Weapon>();

        public Slot[] slots =
        {
            new Slot { label = "Head" },
            new Slot { label = "Chest" }
        };
    }
}
