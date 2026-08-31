using System;
using System.Collections.Generic;

namespace MM.Inspector.Editor
{
    public static class MMGroupRegistry
    {
        private static MMHandlerMap<MMGroupDrawer> _drawers;

        public static MMElement BuildElement(MMGroupNode node, MMPropertyTree tree)
        {
            return BuildElement(node, tree.Find, OwnerId(tree));
        }

        public static MMElement BuildElement(MMGroupNode node, Func<string, MMProperty> lookup, int ownerId)
        {
            List<MMElement> children = new List<MMElement>();
            List<string> names = new List<string>();

            foreach (MMGroupItem item in node.Items)
            {
                if (item.IsGroup)
                {
                    children.Add(BuildElement(item.Group, lookup, ownerId));
                    names.Add(item.Group.Name);
                    continue;
                }

                MMProperty property = lookup(item.Member.Name);
                if (property == null)
                {
                    continue;
                }

                children.Add(MMDrawerRegistry.BuildElement(property));
                names.Add(item.Member.Name);
            }

            MMGroupContext context = new MMGroupContext(node, ownerId, children, names);
            MMElement element = CreateElement(context);

            foreach (MMElement child in children)
            {
                element.AddChild(child);
            }

            return element;
        }

        private static int OwnerId(MMPropertyTree tree)
        {
            UnityEngine.Object target = tree.SerializedObject.targetObject;

            return target == null ? 0 : target.GetInstanceID();
        }

        private static MMElement CreateElement(MMGroupContext context)
        {
            GroupAttribute declaration = context.Node.Declaration;
            if (declaration == null)
            {
                return new MMGroupElement();
            }

            _drawers ??= new MMHandlerMap<MMGroupDrawer>(typeof(MMGroupDrawer<>));

            MMGroupDrawer drawer = _drawers.Get(declaration);

            return drawer == null ? new MMGroupElement() : drawer.CreateElement(context, declaration);
        }
    }
}
