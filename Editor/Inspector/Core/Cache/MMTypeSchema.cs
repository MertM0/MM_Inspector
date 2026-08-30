using System;
using System.Collections.Generic;

namespace MM.Inspector.Editor
{
    public sealed class MMTypeSchema
    {
        private static readonly Dictionary<Type, MMTypeSchema> Cache = new Dictionary<Type, MMTypeSchema>();
        private static List<MMTypeProcessor> _processors;

        public Type Type { get; }
        public IReadOnlyList<MMMemberSchema> Members { get; }
        public MMGroupNode Groups { get; }

        private MMTypeSchema(Type type)
        {
            Type = type;

            List<MMMemberSchema> members = new List<MMMemberSchema>();
            foreach (MMTypeProcessor processor in GetProcessors())
            {
                processor.Process(type, members);
            }

            Members = members;
            Groups = MMGroupTreeBuilder.Build(members, ReadGroupSettings(type));
        }

        public static MMTypeSchema Get(Type type)
        {
            if (Cache.TryGetValue(type, out MMTypeSchema cached))
            {
                return cached;
            }

            MMTypeSchema schema = new MMTypeSchema(type);
            Cache[type] = schema;
            return schema;
        }

        public MMMemberSchema Find(string name)
        {
            for (int i = 0; i < Members.Count; i++)
            {
                if (Members[i].Name == name)
                {
                    return Members[i];
                }
            }

            return null;
        }

        private static GroupSettingsAttribute[] ReadGroupSettings(Type type)
        {
            return (GroupSettingsAttribute[])type.GetCustomAttributes(typeof(GroupSettingsAttribute), true);
        }

        private static List<MMTypeProcessor> GetProcessors()
        {
            if (_processors != null)
            {
                return _processors;
            }

            _processors = MMTypeInstances.Of<MMTypeProcessor>();
            _processors.Sort((first, second) => first.Order.CompareTo(second.Order));

            return _processors;
        }
    }
}
