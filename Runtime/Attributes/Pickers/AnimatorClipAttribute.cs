using System;

namespace MM.Inspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class AnimatorClipAttribute : MMAnimatorAttribute
    {
        public AnimatorClipAttribute()
            : base(null)
        {
        }

        public AnimatorClipAttribute(string animatorMember)
            : base(animatorMember)
        {
        }
    }
}
