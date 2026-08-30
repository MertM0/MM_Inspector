using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

namespace MM.Inspector.Editor
{
    internal sealed class AnimatorParamElement : MMPickerElement
    {
        private readonly MMValueResolver<Animator> _resolver;
        private readonly AnimatorControllerParameterType _filter;
        private readonly List<MMPickerOption> _cached = new List<MMPickerOption>();

        private RuntimeAnimatorController _cachedController;
        private int _version = -1;

        public AnimatorParamElement(MMProperty property, MMValueResolver<Animator> resolver, AnimatorControllerParameterType filter)
            : base(property)
        {
            _resolver = resolver;
            _filter = filter;
        }

        protected override bool TryBuildOptions(List<MMPickerOption> options, out string error)
        {
            Animator animator = FindAnimator();

            if (animator == null)
            {
                error = "[AnimatorParam] could not find an Animator.";
                return false;
            }

            RuntimeAnimatorController controller = animator.runtimeAnimatorController;

            if (controller == null)
            {
                error = "[AnimatorParam] the Animator has no controller.";
                return false;
            }

            RebuildIfNeeded(animator, controller);

            if (_cached.Count == 0)
            {
                error = "[AnimatorParam] the controller has no matching parameter.";
                return false;
            }

            for (int i = 0; i < _cached.Count; i++)
            {
                options.Add(_cached[i]);
            }

            error = null;
            return true;
        }

        private Animator FindAnimator()
        {
            if (_resolver != null)
            {
                return _resolver.HasError ? null : _resolver.GetValue(Property);
            }

            return Property.Owner is Component component ? component.GetComponent<Animator>() : null;
        }

        private void RebuildIfNeeded(Animator animator, RuntimeAnimatorController controller)
        {
            if (controller == _cachedController && _version == MMEditorDataVersion.Current)
            {
                return;
            }

            _cachedController = controller;
            _version = MMEditorDataVersion.Current;

            _cached.Clear();

            AnimatorControllerParameter[] parameters = GetParameters(animator, controller);

            for (int i = 0; parameters != null && i < parameters.Length; i++)
            {
                AnimatorControllerParameter parameter = parameters[i];

                if (_filter != 0 && parameter.type != _filter)
                {
                    continue;
                }

                _cached.Add(new MMPickerOption(parameter.name, parameter.nameHash));
            }
        }

        private static AnimatorControllerParameter[] GetParameters(Animator animator, RuntimeAnimatorController controller)
        {
            return controller is AnimatorController asset ? asset.parameters : animator.parameters;
        }
    }
}
