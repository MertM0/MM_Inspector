namespace MM.Inspector.Editor
{
    internal sealed class DisableIfDisableProcessor : MMDisableProcessor<DisableIfAttribute>
    {
        protected override bool IsDisabled(MMProperty property, DisableIfAttribute attribute)
        {
            bool result = MMConditionEvaluator.Evaluate(property, attribute, out bool resolved);
            return resolved && result;
        }
    }
}
