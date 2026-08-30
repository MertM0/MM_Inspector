namespace MM.Inspector.Editor
{
    internal sealed class EnableIfDisableProcessor : MMDisableProcessor<EnableIfAttribute>
    {
        protected override bool IsDisabled(MMProperty property, EnableIfAttribute attribute)
        {
            bool result = MMConditionEvaluator.Evaluate(property, attribute, out bool resolved);
            return resolved && !result;
        }
    }
}
