using System;
using System.Collections.Generic;

namespace MM.Inspector.Workflow.Editor
{
    [Serializable]
    public sealed class MMPlayModePayload
    {
        public List<MMPlayModeSnapshot> Snapshots = new List<MMPlayModeSnapshot>();
    }
}
