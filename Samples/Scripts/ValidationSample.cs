using UnityEngine;

namespace MM.Inspector.Samples
{
    public class ValidationSample : MonoBehaviour
    {
        [Title("Required")]
        [Required]
        public GameObject target;

        [Required("Pick a material before playing.")]
        public Material material;

        [Title("Custom rules")]
        [ValidateInput(nameof(HasName), "Name cannot be empty.")]
        public string characterName = "Hero";

        [ValidateInput(nameof(IsEven), "Wave count must be even.")]
        public int waveCount = 4;

        [Title("Clamping")]
        [MinValue(0f)]
        [MaxValue(100f)]
        public int percentage = 50;

        public float floor = 10f;

        [MinValue(nameof(floor))]
        public float aboveFloor = 25f;

        private bool HasName => !string.IsNullOrEmpty(characterName);

        private bool IsEven => waveCount % 2 == 0;
    }
}
