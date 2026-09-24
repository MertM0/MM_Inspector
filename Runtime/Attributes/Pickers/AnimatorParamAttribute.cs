using System;
using UnityEngine;

namespace MM.Inspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class AnimatorParamAttribute : MMAnimatorAttribute
    {
        public AnimatorControllerParameterType ParameterType { get; }

        public AnimatorParamAttribute()
            : base(null)
        {
        }

        public AnimatorParamAttribute(string animatorMember)
            : base(animatorMember)
        {
        }

        public AnimatorParamAttribute(AnimatorControllerParameterType parameterType)
            : base(null)
        {
            ParameterType = parameterType;
        }

        public AnimatorParamAttribute(string animatorMember, AnimatorControllerParameterType parameterType)
            : base(animatorMember)
        {
            ParameterType = parameterType;
        }
    }
}
