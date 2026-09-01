using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    internal sealed class ButtonElement : MMElement
    {
        private const float VerticalPadding = 2f;
        private const float ArrowWidth = 13f;
        private const float ArrowGap = 4f;

        private static GUIStyle _arrowStyle;

        private readonly MMProperty _property;
        private readonly MMActionResolver _resolver;
        private readonly GUIContent _label;
        private readonly ParameterInfo[] _parameters;
        private readonly GUIContent[] _parameterLabels;
        private readonly object[] _arguments;
        private readonly string _key;

        private readonly bool _hasUnsupportedParameter;

        private bool _expanded;

        public ButtonElement(MMProperty property)
        {
            _property = property;

            ButtonAttribute attribute = property.Schema?.GetAttribute<ButtonAttribute>();
            string text = string.IsNullOrEmpty(attribute?.Label) ? property.DisplayName : attribute.Label;
            _label = new GUIContent(text);

            MethodInfo method = property.Schema?.Member as MethodInfo;
            _resolver = MMActionResolver.FromMethod(method);

            _parameters = method?.GetParameters() ?? new ParameterInfo[0];
            _parameterLabels = new GUIContent[_parameters.Length];
            _arguments = new object[_parameters.Length];

            for (int i = 0; i < _parameters.Length; i++)
            {
                ParameterInfo parameter = _parameters[i];

                _parameterLabels[i] = new GUIContent(MMReflection.ToDisplayName(parameter.Name));
                _arguments[i] = parameter.HasDefaultValue
                    ? parameter.DefaultValue
                    : MMValueField.CreateDefault(parameter.ParameterType);

                _hasUnsupportedParameter |= !MMValueField.Supports(parameter.ParameterType);
            }

            _key = MMUiState.Key(
                MMUiState.ButtonScope,
                new MMObjectKey(property.Owner as UnityEngine.Object),
                property.Name);

            _expanded = _parameters.Length == 0 || MMUiState.GetExpanded(_key, true);
        }

        public override bool IsVisible => _property.IsVisible;

        protected override float CalculateHeight(float width)
        {
            float line = EditorGUIUtility.singleLineHeight;

            if (_parameters.Length == 0)
            {
                return line + VerticalPadding;
            }

            float content = line + ArgumentsHeight();

            return content + MMFrame.Padding.vertical + VerticalPadding;
        }

        public override void OnGUI(Rect position)
        {
            float line = EditorGUIUtility.singleLineHeight;

            if (_parameters.Length == 0)
            {
                DrawInvoke(new Rect(position.x, position.y + VerticalPadding * 0.5f, position.width, line));
                return;
            }

            Rect frame = new Rect(
                position.x,
                position.y + VerticalPadding * 0.5f,
                position.width,
                position.height - VerticalPadding);

            MMFrame.Draw(frame);

            Rect content = MMFrame.Padding.Remove(frame);
            Rect row = new Rect(content.x, content.y, content.width, line);
            float offset = ArrowWidth + ArrowGap;
            Rect arrow = new Rect(row.x, row.y + (line - ArrowWidth) * 0.5f, ArrowWidth, ArrowWidth);
            Rect button = new Rect(row.x + offset, row.y, row.width - offset, line);

            DrawInvoke(button);
            DrawArrow(arrow);

            if (!_expanded)
            {
                return;
            }

            DrawArguments(new Rect(content.x, row.yMax, content.width, ArgumentsHeight()));
        }

        private void DrawInvoke(Rect rect)
        {
            using (new EditorGUI.DisabledScope(!_property.IsEnabled || _hasUnsupportedParameter))
            {
                if (!GUI.Button(rect, _label))
                {
                    return;
                }

                _property.Modify(_label.text,
                    () => _resolver.Invoke(_property.Owner, _parameters.Length == 0 ? null : _arguments));
            }
        }

        private void DrawArrow(Rect rect)
        {
            bool expanded = GUI.Toggle(rect, _expanded, GUIContent.none, ArrowStyle);

            if (expanded == _expanded)
            {
                return;
            }

            _expanded = expanded;
            MMUiState.SetExpanded(_key, expanded);
        }

        private static GUIStyle ArrowStyle
        {
            get
            {
                if (_arrowStyle == null)
                {
                    _arrowStyle = GUI.skin.FindStyle("IN Foldout") ?? new GUIStyle(EditorStyles.foldout);
                }

                return _arrowStyle;
            }
        }

        private void DrawArguments(Rect rect)
        {
            float spacing = EditorGUIUtility.standardVerticalSpacing;
            float y = rect.y + spacing;

            using (new EditorGUI.DisabledScope(!_property.IsEnabled))
            {
                for (int i = 0; i < _parameters.Length; i++)
                {
                    Type type = _parameters[i].ParameterType;
                    float height = MMValueField.GetHeight(type);

                    _arguments[i] = MMValueField.Draw(
                        new Rect(rect.x, y, rect.width, height), _parameterLabels[i], type, _arguments[i]);

                    y += height + spacing;
                }
            }
        }

        private float ArgumentsHeight()
        {
            if (!_expanded)
            {
                return 0f;
            }

            float total = 0f;

            for (int i = 0; i < _parameters.Length; i++)
            {
                total += MMValueField.GetHeight(_parameters[i].ParameterType) + EditorGUIUtility.standardVerticalSpacing;
            }

            return total;
        }
    }
}
