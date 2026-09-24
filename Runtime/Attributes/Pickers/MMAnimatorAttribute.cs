namespace MM.Inspector
{
    public abstract class MMAnimatorAttribute : MMAttribute
    {
        public string AnimatorMember { get; }

        protected MMAnimatorAttribute(string animatorMember)
        {
            AnimatorMember = animatorMember;
        }
    }
}
