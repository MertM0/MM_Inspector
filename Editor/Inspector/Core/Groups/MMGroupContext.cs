using System.Collections.Generic;

namespace MM.Inspector.Editor
{
    public sealed class MMGroupContext
    {
        public MMGroupNode Node { get; }
        public int OwnerId { get; }
        public IReadOnlyList<MMElement> Children { get; }
        public IReadOnlyList<string> ChildNames { get; }

        public MMGroupContext(MMGroupNode node, int ownerId, IReadOnlyList<MMElement> children, IReadOnlyList<string> childNames)
        {
            Node = node;
            OwnerId = ownerId;
            Children = children;
            ChildNames = childNames;
        }
    }
}
