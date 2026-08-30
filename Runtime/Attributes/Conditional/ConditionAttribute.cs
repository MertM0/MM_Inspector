namespace MM.Inspector
{
    public abstract class ConditionAttribute : MMAttribute
    {
        public string Member { get; }
        public object Value { get; }
        public bool HasValue { get; }

        protected ConditionAttribute(string member)
        {
            Member = member;
            Value = null;
            HasValue = false;
        }

        protected ConditionAttribute(string member, object value)
        {
            Member = member;
            Value = value;
            HasValue = true;
        }
    }
}
