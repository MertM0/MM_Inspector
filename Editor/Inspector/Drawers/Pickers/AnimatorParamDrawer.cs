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
            MMValueResolver<UnityEngine.Animator> resolver = string.IsNullOrEmpty(attribute.AnimatorMember)
                ? null
                : MMValueResolver<UnityEngine.Animator>.Create(property.OwnerType, attribute.AnimatorMember);

            return new AnimatorParamElement(property, resolver, attribute.ParameterType);
        }
    }
}
