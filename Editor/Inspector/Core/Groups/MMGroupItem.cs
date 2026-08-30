namespace MM.Inspector.Editor
{
    public readonly struct MMGroupItem
    {
        public readonly MMMemberSchema Member;
        public readonly MMGroupNode Group;

        private MMGroupItem(MMMemberSchema member, MMGroupNode group)
        {
            Member = member;
            Group = group;
        }

        public bool IsGroup => Group != null;

        public static MMGroupItem FromMember(MMMemberSchema member) => new MMGroupItem(member, null);

        public static MMGroupItem FromGroup(MMGroupNode group) => new MMGroupItem(null, group);
    }
}
