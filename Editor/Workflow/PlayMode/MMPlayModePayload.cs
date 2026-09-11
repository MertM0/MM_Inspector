using System;
using System.Collections.Generic;

namespace MM.Inspector.Workflow.Editor
{
    [Serializable]
    public sealed class MMPlayModePayload
    {
        public List<MMComponentSnapshot> Snapshots = new List<MMComponentSnapshot>();
    }
}
