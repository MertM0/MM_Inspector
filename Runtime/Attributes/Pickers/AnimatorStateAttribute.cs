using System;

namespace MM.Inspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class AnimatorStateAttribute : MMAnimatorAttribute
    {
        public const int AllLayers = -1;

        public int Layer { get; set; } = AllLayers;

        public AnimatorStateAttribute()
            : base(null)
        {
        }

        public AnimatorStateAttribute(string animatorMember)
            : base(animatorMember)
        {
        }
    }
}
