namespace MM.Inspector.Workflow.Editor
{
    public sealed class MMPlayModeSnapshot
    {
        public MMPlayModeSnapshot(MMObjectId owner, string globalId, string json)
        {
            Owner = owner;
            GlobalId = globalId;
            Json = json;
        }

        public MMObjectId Owner { get; }

        public string GlobalId { get; }

        public string Json { get; }
    }
}
