using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    public sealed class MMProperty
    {
        private readonly Type _valueTypeOverride;
        private readonly bool _forcedDisabled;
        private List<MMProperty> _children;
        private GUIContent _label;

        public MMMemberSchema Schema { get; }
        public SerializedProperty Serialized { get; }
        public MMProperty Parent { get; }
        public object Owner { get; }
        public string Name { get; }
        public string DisplayName { get; }

        public bool IsVisible { get; private set; } = true;
        public bool IsEnabled { get; private set; } = true;

        public MMMemberKind Kind => Schema?.Kind ?? MMMemberKind.SerializedField;
        public Type ValueType => _valueTypeOverride ?? Schema?.ValueType;
        public Type OwnerType => Owner?.GetType() ?? Schema?.DeclaringType;

        public MMProperty(
            MMMemberSchema schema,
            SerializedProperty serialized,
            object owner,
            MMProperty parent,
            bool forcedDisabled = false,
            Type valueTypeOverride = null)
        {
            Schema = schema;
            Serialized = serialized;
            Owner = owner;
            Parent = parent;
            _forcedDisabled = forcedDisabled;
            _valueTypeOverride = valueTypeOverride;

            Name = schema?.Name ?? serialized?.name ?? string.Empty;
            DisplayName = schema?.DisplayName ?? serialized?.displayName ?? Name;
        }

        public IReadOnlyList<MMProperty> Children => _children ??= BuildChildren();

        public GUIContent Label => _label ??= BuildLabel();

        public bool HasMixedValue => Serialized != null && Serialized.hasMultipleDifferentValues;

        public bool IsCollection =>
            Serialized != null &&
            Serialized.isArray &&
            Serialized.propertyType != SerializedPropertyType.String;

        public Type ElementType => MMCollection.GetElementType(ValueType);

        public bool IsCollectionElement => Schema == null && Parent != null && Parent.IsCollection;

        public bool HasChildren
        {
            get
            {
                if (Serialized == null)
                {
                    return false;
                }

                return IsCollection
                    ? MMReflection.HasAnyMMAttribute(ElementType)
                    : MMReflection.HasAnyMMAttribute(ValueType);
            }
        }

        public void InvalidateChildren()
        {
            _children = null;
        }

        public void Refresh()
        {
            IsVisible = MMVisibility.IsVisible(this);
            IsEnabled = !_forcedDisabled && MMVisibility.IsEnabled(this);

            if (_children == null)
            {
                return;
            }

            for (int i = 0; i < _children.Count; i++)
            {
                _children[i].Refresh();
            }
        }

        public void Modify(string label, Action change)
        {
            UnityEngine.Object unityObject = Owner as UnityEngine.Object;

            Serialized?.serializedObject?.ApplyModifiedProperties();

            if (unityObject != null)
            {
                Undo.RecordObject(unityObject, label);
            }

            change();

            if (unityObject != null)
            {
                EditorUtility.SetDirty(unityObject);
            }

            MMValidationState.Invalidate();
        }

        public object GetValue()
        {
            return _valueTypeOverride != null ? Owner : Schema?.GetValue(Owner);
        }

        private GUIContent BuildLabel()
        {
            if (Schema == null)
            {
                return new GUIContent(DisplayName);
            }

            if (Schema.HasAttribute<HideLabelAttribute>())
            {
                return GUIContent.none;
            }

            LabelTextAttribute labelText = Schema.GetAttribute<LabelTextAttribute>();
            if (labelText == null)
            {
                return new GUIContent(DisplayName);
            }

            MMValueResolver<string> resolver = MMValueResolver<string>.Create(OwnerType, labelText.Text);

            return new GUIContent(resolver.HasError ? DisplayName : resolver.GetValue(Owner));
        }

        private List<MMProperty> BuildChildren()
        {
            return IsCollection ? BuildElements() : BuildMembers();
        }

        private List<MMProperty> BuildElements()
        {
            List<MMProperty> children = new List<MMProperty>();

            Type elementType = ElementType;
            if (elementType == null)
            {
                return children;
            }

            IList list = Schema?.GetValue(Owner) as IList;

            for (int i = 0; i < Serialized.arraySize; i++)
            {
                object elementOwner = list != null && i < list.Count ? list[i] : null;

                MMProperty node = new MMProperty(
                    null,
                    Serialized.GetArrayElementAtIndex(i),
                    elementOwner,
                    this,
                    valueTypeOverride: elementType);

                node.Refresh();
                children.Add(node);
            }

            return children;
        }

        private List<MMProperty> BuildMembers()
        {
            List<MMProperty> children = new List<MMProperty>();

            object childOwner = GetValue();
            if (childOwner == null)
            {
                return children;
            }

            MMTypeSchema schema = MMTypeSchema.Get(ValueType);

            foreach (MMMemberSchema member in schema.Members)
            {
                SerializedProperty child = member.Kind == MMMemberKind.SerializedField
                    ? Serialized.FindPropertyRelative(member.Name)
                    : null;

                if (member.Kind == MMMemberKind.SerializedField && child == null)
                {
                    continue;
                }

                MMProperty node = new MMProperty(member, child, childOwner, this);
                node.Refresh();
                children.Add(node);
            }

            return children;
        }
    }
}
