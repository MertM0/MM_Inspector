using System.Collections.Generic;

namespace MM.Inspector.Editor
{
    public static class MMGroupTreeBuilder
    {
        public static MMGroupNode Build(IReadOnlyList<MMMemberSchema> members, IReadOnlyList<GroupSettingsAttribute> settings = null)
        {
            MMGroupNode root = new MMGroupNode(string.Empty, string.Empty);

            if (members == null)
            {
                return root;
            }

            for (int i = 0; i < members.Count; i++)
            {
                MMMemberSchema member = members[i];
                List<GroupAttribute> groups = member.GetAttributes<GroupAttribute>();

                if (groups.Count == 0)
                {
                    root.AddMember(member);
                    continue;
                }

                GroupAttribute deepest = groups[0];

                for (int g = 0; g < groups.Count; g++)
                {
                    Declare(root, groups[g]);

                    if (Depth(groups[g].EffectivePath) > Depth(deepest.EffectivePath))
                    {
                        deepest = groups[g];
                    }
                }

                Descend(root, deepest.EffectivePath).AddMember(member);
            }

            ApplySettings(root, settings);
            return root;
        }

        private static void ApplySettings(MMGroupNode root, IReadOnlyList<GroupSettingsAttribute> settings)
        {
            if (settings == null)
            {
                return;
            }

            for (int i = 0; i < settings.Count; i++)
            {
                GroupSettingsAttribute setting = settings[i];

                Descend(root, setting.Path).Apply(setting);
            }
        }

        private static int Depth(string path)
        {
            return string.IsNullOrEmpty(path) ? 0 : path.Split('/').Length;
        }

        private static void Declare(MMGroupNode root, GroupAttribute group)
        {
            MMGroupNode target = Descend(root, group.DeclarationPath);

            if (!target.TryDeclare(group))
            {
                MMLog.WarnOnce(
                    $"Group '{group.DeclarationPath}' is declared as both " +
                    $"{target.Declaration.GetType().Name} and {group.GetType().Name}. The first one is used.");
            }
        }

        private static MMGroupNode Descend(MMGroupNode root, string path)
        {
            MMGroupNode current = root;

            if (string.IsNullOrEmpty(path))
            {
                return current;
            }

            foreach (string segment in path.Split('/'))
            {
                if (!string.IsNullOrEmpty(segment))
                {
                    current = current.GetOrCreateChild(segment);
                }
            }

            return current;
        }
    }
}
