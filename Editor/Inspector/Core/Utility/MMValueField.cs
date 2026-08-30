using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace MM.Inspector.Editor
{
    public static class MMValueField
    {
        private readonly struct Field
        {
            public readonly int Lines;
            public readonly Func<Rect, GUIContent, object, object> Draw;

            public Field(int lines, Func<Rect, GUIContent, object, object> draw)
            {
                Lines = lines;
                Draw = draw;
            }
        }

        private static readonly Dictionary<Type, Field> Fields = new Dictionary<Type, Field>
        {
            { typeof(int), new Field(1, (rect, label, value) => EditorGUI.IntField(rect, label, Convert.ToInt32(value ?? 0))) },
            { typeof(long), new Field(1, (rect, label, value) => EditorGUI.LongField(rect, label, Convert.ToInt64(value ?? 0L))) },
            { typeof(float), new Field(1, (rect, label, value) => EditorGUI.FloatField(rect, label, Convert.ToSingle(value ?? 0f))) },
            { typeof(double), new Field(1, (rect, label, value) => EditorGUI.DoubleField(rect, label, Convert.ToDouble(value ?? 0d))) },
            { typeof(bool), new Field(1, (rect, label, value) => EditorGUI.Toggle(rect, label, Convert.ToBoolean(value ?? false))) },
            { typeof(string), new Field(1, (rect, label, value) => EditorGUI.TextField(rect, label, value as string ?? string.Empty)) },
            { typeof(Vector2), new Field(1, (rect, label, value) => EditorGUI.Vector2Field(rect, label, value is Vector2 typed ? typed : Vector2.zero)) },
            { typeof(Vector3), new Field(1, (rect, label, value) => EditorGUI.Vector3Field(rect, label, value is Vector3 typed ? typed : Vector3.zero)) },
            { typeof(Vector4), new Field(2, (rect, label, value) => EditorGUI.Vector4Field(rect, label, value is Vector4 typed ? typed : Vector4.zero)) },
            { typeof(Vector2Int), new Field(1, (rect, label, value) => EditorGUI.Vector2IntField(rect, label, value is Vector2Int typed ? typed : Vector2Int.zero)) },
            { typeof(Vector3Int), new Field(1, (rect, label, value) => EditorGUI.Vector3IntField(rect, label, value is Vector3Int typed ? typed : Vector3Int.zero)) },
            { typeof(Color), new Field(1, (rect, label, value) => EditorGUI.ColorField(rect, label, value is Color typed ? typed : Color.white)) },
            { typeof(Rect), new Field(2, (rect, label, value) => EditorGUI.RectField(rect, label, value is Rect typed ? typed : new Rect())) },
            { typeof(Bounds), new Field(2, (rect, label, value) => EditorGUI.BoundsField(rect, label, value is Bounds typed ? typed : new Bounds())) },
            { typeof(AnimationCurve), new Field(1, (rect, label, value) => EditorGUI.CurveField(rect, label, value as AnimationCurve ?? DefaultCurve())) },
            { typeof(LayerMask), new Field(1, DrawLayerMask) }
        };

        public static bool Supports(Type type)
        {
            if (type == null)
            {
                return false;
            }

            return Fields.ContainsKey(type) || type.IsEnum || typeof(UnityEngine.Object).IsAssignableFrom(type);
        }

        public static float GetHeight(Type type)
        {
            int lines = type != null && Fields.TryGetValue(type, out Field field) ? field.Lines : 1;

            return lines * EditorGUIUtility.singleLineHeight + (lines - 1) * EditorGUIUtility.standardVerticalSpacing;
        }

        public static object CreateDefault(Type type)
        {
            if (type == null)
            {
                return null;
            }

            if (type == typeof(string))
            {
                return string.Empty;
            }

            if (type == typeof(AnimationCurve))
            {
                return DefaultCurve();
            }

            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        public static object Draw(Rect position, GUIContent label, Type type, object value)
        {
            if (type == null)
            {
                EditorGUI.LabelField(position, label, MMMessage.Get("Unknown type."));
                return value;
            }

            if (type.IsEnum)
            {
                Enum current = value as Enum ?? (Enum)Enum.ToObject(type, 0);

                return type.IsDefined(typeof(FlagsAttribute), false)
                    ? EditorGUI.EnumFlagsField(position, label, current)
                    : EditorGUI.EnumPopup(position, label, current);
            }

            if (typeof(UnityEngine.Object).IsAssignableFrom(type))
            {
                return EditorGUI.ObjectField(position, label, value as UnityEngine.Object, type, true);
            }

            if (Fields.TryGetValue(type, out Field field))
            {
                return field.Draw(position, label, value);
            }

            EditorGUI.LabelField(position, label, MMMessage.Get(Describe(type, value)));
            return value;
        }

        private static string Describe(Type type, object value)
        {
            return value == null ? type.Name + " is not supported here." : value.ToString();
        }

        private static AnimationCurve DefaultCurve()
        {
            return AnimationCurve.Linear(0f, 0f, 1f, 1f);
        }

        private static object DrawLayerMask(Rect position, GUIContent label, object value)
        {
            LayerMask mask = value is LayerMask typed ? typed : default;

            int concatenated = InternalEditorUtility.LayerMaskToConcatenatedLayersMask(mask);
            int edited = EditorGUI.MaskField(position, label, concatenated, InternalEditorUtility.layers);

            return (LayerMask)InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(edited);
        }
    }
}
