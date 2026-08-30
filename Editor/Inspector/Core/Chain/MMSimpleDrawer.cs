using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    public abstract class MMSimpleDrawer<TAttribute> : MMAttributeDrawer<TAttribute> where TAttribute : MMAttribute
    {
        protected abstract void OnGUI(Rect position, MMProperty property, TAttribute attribute);

        protected virtual float GetHeight(float width, MMProperty property, TAttribute attribute)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        protected sealed override MMElement CreateElement(MMProperty property, TAttribute attribute, MMElement next)
        {
            return new MMDrawerElement(
                property,
                width => GetHeight(width, property, attribute),
                position => OnGUI(position, property, attribute));
        }
    }
}
