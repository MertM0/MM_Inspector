using System;
using UnityEngine;

namespace MM.Inspector.Samples
{
    public class MemberSample : MonoBehaviour
    {
        [PropertyOrder(-10)]
        public string drawnFirst = "PropertyOrder moves this above everything else";

        [Title("Buttons")]
        public int score;

        [Button]
        public void ResetScore()
        {
            score = 0;
        }

        [Button("Add Points")]
        public void AddPoints(int amount, bool doubled)
        {
            score += doubled ? amount * 2 : amount;
        }

        [Title("Shown without serializing")]
        [ShowInInspector]
        [NonSerialized]
        public float runtimeOnly = 3.14f;

        [ShowInInspector]
        public int DoubledScore => score * 2;

        [ShowInInspector]
        [ProgressBar(0f, 100f, Color = MMColor.Green, Label = "Score")]
        public int ScoreBar => score;

        [Title("Change callbacks")]
        [OnValueChanged(nameof(OnRadiusChanged))]
        public float radius = 1f;

        [ShowInInspector]
        public float Circumference { get; private set; }

        [Separator]
        [InfoBox("ShowDrawerChain prints the drawers wrapping a field.", InfoBoxType.Warning)]
        [ShowDrawerChain]
        [Slider(0f, 10f)]
        public float inspected = 4f;

        private void OnRadiusChanged()
        {
            Circumference = 2f * Mathf.PI * radius;
        }
    }
}
