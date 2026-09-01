using System.Collections.Generic;

namespace MM.Inspector.Editor
{
    public sealed class MMGroupContext
    {
        public MMGroupNode Node { get; }
        public MMObjectKey Owner { get; }
        public IReadOnlyList<MMElement> Children { get; }
        public IReadOnlyList<string> ChildNames { get; }

        public MMGroupContext(MMGroupNode node, MMObjectKey owner, IReadOnlyList<MMElement> children, IReadOnlyList<string> childNames)
        {
            Node = node;
            Owner = owner;
            Children = children;
            ChildNames = childNames;
        }
    }
}
