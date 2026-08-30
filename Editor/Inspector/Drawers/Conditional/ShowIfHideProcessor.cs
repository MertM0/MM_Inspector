namespace MM.Inspector.Editor
{
    internal sealed class ShowIfHideProcessor : MMHideProcessor<ShowIfAttribute>
    {
        protected override bool IsHidden(MMProperty property, ShowIfAttribute attribute)
        {
            bool result = MMConditionEvaluator.Evaluate(property, attribute, out bool resolved);
            return resolved && !result;
        }
    }
}
