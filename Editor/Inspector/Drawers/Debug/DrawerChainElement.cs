using UnityEditor;

namespace MM.Inspector.Editor
{
    internal sealed class DrawerChainElement : MMContainerElement
    {
        private readonly MMProperty _property;

        public DrawerChainElement(MMProperty property, MMElement next)
        {
            _property = property;

            string chain = string.Join("  ->  ", MMDrawerRegistry.DescribeChain(property));
            AddChild(new MMMessageElement($"{property.Name}:  {chain}", MessageType.None));
            AddChild(next);
        }

        public override bool IsVisible => _property.IsVisible;
    }
}
