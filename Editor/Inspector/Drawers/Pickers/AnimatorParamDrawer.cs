using UnityEngine;

namespace MM.Inspector.Editor
{
    internal sealed class AnimatorParamDrawer : MMAttributeDrawer<AnimatorParamAttribute>
    {
        protected override string Validate(MMProperty property, AnimatorParamAttribute attribute)
        {
            return MMPickerElement.ValidateTarget(property, attribute);
        }

        protected override MMElement CreateElement(MMProperty property, AnimatorParamAttribute attribute, MMElement next)
        {
            AnimatorControllerParameterType filter = attribute.ParameterType;

            return new MMAnimatorPickerElement(property, attribute, "the controller has no matching parameter.",
                (controller, options) => MMAnimatorCatalog.Parameters(controller, filter, options));
        }
    }
}
