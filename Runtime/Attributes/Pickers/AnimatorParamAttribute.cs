using System;
using UnityEngine;

namespace MM.Inspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class AnimatorParamAttribute : MMAttribute
    {
        public string AnimatorMember { get; }
        public AnimatorControllerParameterType ParameterType { get; }

        public AnimatorParamAttribute()
        {
        }

        public AnimatorParamAttribute(string animatorMember)
        {
            AnimatorMember = animatorMember;
        }

        public AnimatorParamAttribute(AnimatorControllerParameterType parameterType)
        {
            ParameterType = parameterType;
        }

        public AnimatorParamAttribute(string animatorMember, AnimatorControllerParameterType parameterType)
        {
            AnimatorMember = animatorMember;
            ParameterType = parameterType;
        }
    }
}
