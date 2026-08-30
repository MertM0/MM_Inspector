namespace MM.Inspector.Editor
{
    internal sealed class HideIfHideProcessor : MMHideProcessor<HideIfAttribute>
    {
        protected override bool IsHidden(MMProperty property, HideIfAttribute attribute)
        {
            bool result = MMConditionEvaluator.Evaluate(property, attribute, out bool resolved);
            return resolved && result;
        }
    }
}
