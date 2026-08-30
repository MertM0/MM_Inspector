using System.Collections.Generic;

namespace MM.Inspector.Editor
{
    public sealed class MMGroupNode
    {
        private readonly List<MMGroupItem> _items = new List<MMGroupItem>();
        private readonly Dictionary<string, MMGroupNode> _childrenByName = new Dictionary<string, MMGroupNode>();

        public string Name { get; }
        public string Path { get; }
        public GroupAttribute Declaration { get; private set; }
        public string Title { get; private set; }
        public bool Expanded { get; private set; }

        public IReadOnlyList<MMGroupItem> Items => _items;

        public MMGroupNode(string name, string path)
        {
            Name = name;
            Path = path;
        }

        public void Apply(GroupSettingsAttribute settings)
        {
            if (!string.IsNullOrEmpty(settings.Title))
            {
                Title = settings.Title;
            }

            Expanded = settings.Expanded;
        }

        public void AddMember(MMMemberSchema member)
        {
            _items.Add(MMGroupItem.FromMember(member));
        }

        public MMGroupNode GetOrCreateChild(string name)
        {
            if (_childrenByName.TryGetValue(name, out MMGroupNode existing))
            {
                return existing;
            }

            string path = string.IsNullOrEmpty(Path) ? name : $"{Path}/{name}";
            MMGroupNode child = new MMGroupNode(name, path);

            _childrenByName[name] = child;
            _items.Add(MMGroupItem.FromGroup(child));

            return child;
        }

        public bool TryDeclare(GroupAttribute attribute)
        {
            if (Declaration == null)
            {
                Declaration = attribute;
                return true;
            }

            return Declaration.GetType() == attribute.GetType();
        }
    }
}
