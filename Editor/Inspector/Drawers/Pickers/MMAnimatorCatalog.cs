using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

namespace MM.Inspector.Editor
{
    public static class MMAnimatorCatalog
    {
        private static readonly HashSet<AnimationClip> _seenClips = new HashSet<AnimationClip>();

        public static AnimatorController Source(RuntimeAnimatorController controller)
        {
            while (controller is AnimatorOverrideController overrides)
            {
                controller = overrides.runtimeAnimatorController;
            }

            return controller as AnimatorController;
        }

        public static void Parameters(RuntimeAnimatorController controller, AnimatorControllerParameterType filter,
            List<MMPickerOption> options)
        {
            AnimatorController source = Source(controller);

            if (source == null)
            {
                return;
            }

            AnimatorControllerParameter[] parameters = source.parameters;

            for (int i = 0; i < parameters.Length; i++)
            {
                AnimatorControllerParameter parameter = parameters[i];

                if (filter == 0 || parameter.type == filter)
                {
                    options.Add(new MMPickerOption(parameter.name, parameter.nameHash));
                }
            }
        }

        public static void States(RuntimeAnimatorController controller, int layer, List<MMPickerOption> options)
        {
            AnimatorController source = Source(controller);

            if (source == null)
            {
                return;
            }

            AnimatorControllerLayer[] layers = source.layers;
            bool prefixLayer = layer == AnimatorStateAttribute.AllLayers && layers.Length > 1;

            for (int i = 0; i < layers.Length; i++)
            {
                if ((layer != AnimatorStateAttribute.AllLayers && i != layer) || layers[i].syncedLayerIndex >= 0)
                {
                    continue;
                }

                string prefix = prefixLayer ? layers[i].name + MMPickerPopup.PathSeparator : string.Empty;
                CollectStates(layers[i].stateMachine, prefix, options);
            }
        }

        public static void Clips(RuntimeAnimatorController controller, List<MMPickerOption> options)
        {
            if (controller == null)
            {
                return;
            }

            AnimationClip[] clips = controller.animationClips;

            for (int i = 0; i < clips.Length; i++)
            {
                AnimationClip clip = clips[i];

                if (clip != null && _seenClips.Add(clip))
                {
                    options.Add(new MMPickerOption(clip.name, clip.name, 0, clip));
                }
            }

            _seenClips.Clear();
        }

        private static void CollectStates(AnimatorStateMachine machine, string prefix, List<MMPickerOption> options)
        {
            if (machine == null)
            {
                return;
            }

            ChildAnimatorState[] states = machine.states;

            for (int i = 0; i < states.Length; i++)
            {
                AnimatorState state = states[i].state;
                options.Add(new MMPickerOption(prefix + state.name, state.name, state.nameHash));
            }

            ChildAnimatorStateMachine[] machines = machine.stateMachines;

            for (int i = 0; i < machines.Length; i++)
            {
                AnimatorStateMachine child = machines[i].stateMachine;
                CollectStates(child, prefix + child.name + MMPickerPopup.PathSeparator, options);
            }
        }
    }
}
