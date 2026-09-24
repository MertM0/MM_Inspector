namespace MM.Inspector.Editor
{
    internal sealed class AnimatorStateDrawer : MMAttributeDrawer<AnimatorStateAttribute>
    {
        protected override string Validate(MMProperty property, AnimatorStateAttribute attribute)
        {
            return MMPickerElement.ValidateTarget(property, attribute);
        }

        protected override MMElement CreateElement(MMProperty property, AnimatorStateAttribute attribute, MMElement next)
        {
            int layer = attribute.Layer;

            return new MMAnimatorPickerElement(property, attribute, "the controller has no matching state.",
                (controller, options) => MMAnimatorCatalog.States(controller, layer, options));
        }
    }
}
