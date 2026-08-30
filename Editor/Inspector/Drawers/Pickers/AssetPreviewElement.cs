using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    internal sealed class AssetPreviewElement : DecoratedElement
    {
        private const float Border = 1f;

        private readonly MMProperty _property;
        private readonly float _size;

        public AssetPreviewElement(MMProperty property, MMElement inner, int size)
            : base(property, inner)
        {
            _property = property;
            _size = Mathf.Max(16, size);
        }

        protected override bool DecorationBelow => true;

        protected override float GetDecorationHeight(float width)
        {
            return Target == null ? 0f : _size + EditorGUIUtility.standardVerticalSpacing;
        }

        protected override void DrawDecoration(Rect rect)
        {
            Object target = Target;
            if (target == null)
            {
                return;
            }

            Texture2D preview = AssetPreview.GetAssetPreview(target);
            if (preview == null)
            {
                preview = AssetPreview.GetMiniThumbnail(target);
            }

            if (preview == null)
            {
                return;
            }

            Rect box = new Rect(
                rect.x + EditorGUIUtility.labelWidth,
                rect.y + EditorGUIUtility.standardVerticalSpacing,
                _size,
                _size);

            EditorGUI.DrawRect(box, MMSkin.Border);

            Rect image = new Rect(box.x + Border, box.y + Border, box.width - Border * 2f, box.height - Border * 2f);
            GUI.DrawTexture(image, preview, ScaleMode.ScaleToFit);
        }

        private Object Target
        {
            get
            {
                SerializedProperty serialized = _property.Serialized;

                if (serialized == null || serialized.propertyType != SerializedPropertyType.ObjectReference)
                {
                    return null;
                }

                return serialized.objectReferenceValue;
            }
        }
    }
}
