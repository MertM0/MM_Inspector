using UnityEngine;

namespace MM.Inspector.Samples
{
    public class PickerSample : MonoBehaviour
    {
        [Title("Unity pickers")]
        [Tag]
        public string singleTag = "Untagged";

        [Tag]
        public string[] targetTags = { "Player", "Untagged" };

        [Layer]
        public int layerById;

        [Layer]
        public string layerByName = "Default";

        [Scene]
        public string sceneByName;

        [Scene]
        public int sceneByIndex;

        [SortingLayer]
        public string sortingLayerByName = "Default";

        [Title("Animator")]
        public Animator animator;

        [AnimatorParam(nameof(animator))]
        public string anyParameter;

        [AnimatorParam(nameof(animator), AnimatorControllerParameterType.Trigger)]
        public string triggerParameter;

        [AnimatorState(nameof(animator))]
        public string stateByName;

        [AnimatorState(nameof(animator), Layer = 0)]
        public int stateByHash;

        [AnimatorClip(nameof(animator))]
        public AnimationClip clip;

        [AnimatorClip(nameof(animator))]
        public string clipByName;

        [Title("Assets and paths")]
        [AssetPreview]
        public Sprite icon;

        [AssetPreview(96)]
        public Material previewMaterial;

        [FilePath(Extensions = "json")]
        public string configFile;

        [FolderPath]
        public string outputFolder;
    }
}
