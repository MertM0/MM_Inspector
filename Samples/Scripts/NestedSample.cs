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

        [Serializable]
        public class Bounds
        {
            public float min;

            public float max = 10f;
        }

        [InlineProperty]
        public Bounds spawnRange = new Bounds();

        [InlineProperty]
        [HideLabel]
        public Bounds falloff = new Bounds();

        public abstract class Effect
        {
        }

        [Serializable]
        public class DamageEffect : Effect
        {
            [MinValue(0f)]
            public int amount = 10;

            public bool critical;

            [ShowIf(nameof(critical))]
            [Slider(1f, 4f)]
            public float multiplier = 2f;
        }

        [Serializable]
        public class HealEffect : Effect
        {
            [BoxGroup("Healing")]
            [ProgressBar(0f, 100f, Color = MMColor.Green)]
            public float power = 40f;

            [BoxGroup("Healing")]
            public bool overTime;
        }

        [SerializeReference]
        public Effect onHit;

        [InlineEditor]
        public ValueSample tuning;

        [InlineEditor]
        public Transform anchor;

        public Weapon primary = new Weapon();

        public List<Weapon> inventory = new List<Weapon>();

        public Slot[] slots =
        {
            new Slot { label = "Head" },
            new Slot { label = "Chest" }
        };
    }
}
