using System;
using System.Collections.Generic;
using System.Linq;

namespace MM.Inspector.Editor
{
    public static class MMDrawerRegistry
    {
        private static MMHandlerMap<MMDrawer> _drawers;

        public static MMElement BuildElement(MMProperty property)
        {
            List<string> errors = null;
            MMElement element = CreateLeaf(property);

            foreach ((MMAttribute attribute, MMDrawer drawer) in GetChain(property, ref errors))
            {
                element = drawer.CreateElement(property, attribute, element);
            }

            if (MMValidatorRegistry.HasValidators(property))
            {
                element = new MMValidationElement(property, element);
            }

            return errors == null ? element : new MMDrawerErrorElement(property, element, errors);
        }

        public static IReadOnlyList<string> DescribeChain(MMProperty property)
        {
            List<string> errors = null;

            List<string> names = GetChain(property, ref errors)
                .Select(pair => pair.Drawer.GetType().Name)
                .Reverse()
                .ToList();

            names.Add(DescribeLeaf(property));
            return names;
        }

        public static MMDrawer GetDrawer(MMAttribute attribute)
        {
            _drawers ??= new MMHandlerMap<MMDrawer>(typeof(MMAttributeDrawer<>));

            return _drawers.Get(attribute);
        }

        public static bool HasElementDrawers(MMProperty property)
        {
            MMAttribute[] attributes = property.Schema?.Attributes;

            if (attributes == null)
            {
                return false;
            }

            for (int i = 0; i < attributes.Length; i++)
            {
                MMDrawer drawer = GetDrawer(attributes[i]);

                if (drawer != null && drawer.AppliesToCollectionElements)
                {
                    return true;
                }
            }

            return false;
        }

        private static List<(MMAttribute Attribute, MMDrawer Drawer)> GetChain(MMProperty property, ref List<string> errors)
        {
            List<(MMAttribute Attribute, MMDrawer Drawer)> matched = new List<(MMAttribute, MMDrawer)>();

            foreach (MMAttribute attribute in GetAttributes(property))
            {
                MMDrawer drawer = GetDrawer(attribute);
                if (drawer == null || !Targets(drawer, property))
                {
                    continue;
                }

                if (drawer.RequiresSerializedProperty && property.Serialized == null)
                {
                    MMLog.WarnOnce(
                        $"{MMPropertyRequirement.Name(attribute)} on '{property.Name}' needs a serialized field and is ignored.");
                    continue;
                }

                string error = drawer.Validate(property, attribute);
                if (error != null)
                {
                    errors ??= new List<string>();
                    errors.Add(error);
                    continue;
                }

                matched.Add((attribute, drawer));
            }

            WarnOnValueDrawerConflict(property, matched);

            return matched.OrderByDescending(pair => pair.Drawer.Order).ToList();
        }

        private static MMAttribute[] GetAttributes(MMProperty property)
        {
            if (property.Schema != null)
            {
                return property.Schema.Attributes;
            }

            return property.IsCollectionElement
                ? property.Parent.Schema?.Attributes ?? Array.Empty<MMAttribute>()
                : Array.Empty<MMAttribute>();
        }

        private static bool Targets(MMDrawer drawer, MMProperty property)
        {
            if (property.IsCollectionElement)
            {
                return drawer.AppliesToCollectionElements;
            }

            return !property.IsCollection || !drawer.AppliesToCollectionElements;
        }

        private static string DescribeLeaf(MMProperty property)
        {
            switch (property.Kind)
            {
                case MMMemberKind.Method:
                    return nameof(ButtonElement);
                case MMMemberKind.ShownField:
                case MMMemberKind.ShownProperty:
                    return nameof(ShownMemberElement);
                default:
                    if (IsList(property))
                    {
                        return nameof(MMListElement);
                    }

                    return property.HasChildren ? nameof(MMNestedPropertyElement) : nameof(MMPropertyElement);
            }
        }

        private static MMElement CreateLeaf(MMProperty property)
        {
            switch (property.Kind)
            {
                case MMMemberKind.Method:
                    return new ButtonElement(property);
                case MMMemberKind.ShownField:
                case MMMemberKind.ShownProperty:
                    return new ShownMemberElement(property);
                default:
                    if (IsList(property))
                    {
                        return new MMListElement(property);
                    }

                    return property.HasChildren
                        ? new MMNestedPropertyElement(property)
                        : new MMPropertyElement(property);
            }
        }

        private static bool IsList(MMProperty property)
        {
            return property.IsCollection && (property.HasChildren || HasElementDrawers(property));
        }

        private static void WarnOnValueDrawerConflict(MMProperty property, List<(MMAttribute Attribute, MMDrawer Drawer)> matched)
        {
            List<string> valueDrawers = matched
                .Where(pair => pair.Drawer.Order == MMDrawerOrder.Drawer)
                .Select(pair => pair.Attribute.GetType().Name)
                .ToList();

            if (valueDrawers.Count > 1)
            {
                MMLog.WarnOnce($"'{property.Name}' has more than one value drawer ({string.Join(", ", valueDrawers)}). Only the outermost one is used.");
            }
        }
    }
}
