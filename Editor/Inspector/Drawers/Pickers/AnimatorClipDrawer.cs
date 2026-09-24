using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    internal sealed class AnimatorClipDrawer : MMAttributeDrawer<AnimatorClipAttribute>
    {
        protected override string Validate(MMProperty property, AnimatorClipAttribute attribute)
        {
            return MMPropertyRequirement.TypesOrReference(property, attribute, typeof(AnimationClip),
                SerializedPropertyType.String);
        }

        protected override MMElement CreateElement(MMProperty property, AnimatorClipAttribute attribute, MMElement next)
        {
            return new MMAnimatorPickerElement(property, attribute, "the controller has no clip.",
                MMAnimatorCatalog.Clips);
        }
    }
}
