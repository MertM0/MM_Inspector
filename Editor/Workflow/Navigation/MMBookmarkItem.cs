namespace MM.Inspector.Workflow.Editor
{
    public readonly struct MMBookmarkItem
    {
        public MMBookmarkItem(MMBookmarkEntry entry, int storeIndex, bool available)
        {
            Entry = entry;
            StoreIndex = storeIndex;
            Available = available;
        }

        public MMBookmarkEntry Entry { get; }

        public int StoreIndex { get; }

        public bool Available { get; }
    }
}
