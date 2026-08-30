namespace MM.Inspector.Workflow.Editor
{
    public sealed class MMPlayModeSnapshot
    {
        public MMPlayModeSnapshot(int instanceId, string id, string json)
        {
            InstanceId = instanceId;
            Id = id;
            Json = json;
        }

        public int InstanceId { get; }

        public string Id { get; }

        public string Json { get; }
    }
}
