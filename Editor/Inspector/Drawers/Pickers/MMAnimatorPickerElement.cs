using System;
using System.Collections.Generic;
using UnityEngine;

namespace MM.Inspector.Editor
{
    internal sealed class MMAnimatorPickerElement : MMPickerElement
    {
        private readonly MMValueResolver<Animator> _resolver;
        private readonly Action<RuntimeAnimatorController, List<MMPickerOption>> _collect;
        private readonly string _noAnimator;
        private readonly string _noController;

        public MMAnimatorPickerElement(MMProperty property, MMAnimatorAttribute attribute, string emptyMessage,
            Action<RuntimeAnimatorController, List<MMPickerOption>> collect)
            : base(property, MMPropertyRequirement.Name(attribute) + " " + emptyMessage, zeroIdIsNone: true)
        {
            _collect = collect;
            _resolver = string.IsNullOrEmpty(attribute.AnimatorMember)
                ? null
                : MMValueResolver<Animator>.Create(property.MemberOwnerType, attribute.AnimatorMember);

            string name = MMPropertyRequirement.Name(attribute);
            _noAnimator = name + " could not find an Animator.";
            _noController = name + " the Animator has no controller.";
        }

        protected override bool TryGetSource(out object source, out string error)
        {
            Animator animator = FindAnimator();
            RuntimeAnimatorController controller = animator == null ? null : animator.runtimeAnimatorController;

            source = controller;
            error = animator == null ? _noAnimator : controller == null ? _noController : null;
            return error == null;
        }

        protected override void Collect(object source, List<MMPickerOption> options)
        {
            _collect((RuntimeAnimatorController)source, options);
        }

        private Animator FindAnimator()
        {
            if (_resolver != null)
            {
                return _resolver.HasError ? null : _resolver.GetValue(Property);
            }

            return Property.MemberOwner is Component component ? component.GetComponent<Animator>() : null;
        }
    }
}
